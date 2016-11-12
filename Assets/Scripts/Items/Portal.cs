using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class Portal : MonoBehaviour {
    public GameObject Player;
    public bool InDark;
    public float ShineSpeed = 0.8f;
    public float Tolerance = 0.1f;
    public AudioClip OutSound;
    public AudioClip InSound;
    public float SoundFactor = 1f;

    private AudioSource _audioSource;
    private Renderer _renderer;
    private float _defaultShineLocation;

    void Start() {
        this._renderer = this.GetComponent<Renderer>();
        this._defaultShineLocation 
            = this._renderer.material.GetFloat("_ShineLocation");
        this._renderer.material.SetFloat("_ShineWidth", this.computeShineWidth(
            this._defaultShineLocation));

        if (this.InDark) {
            this.buildAudioSource(this.OutSound);
        } else {
            this.buildAudioSource(this.InSound);
        }
    }

    void Update() {
        float oldShineLocation 
            = this._renderer.material.GetFloat("_ShineLocation");

        // you have to reset the animation to its default state if the player
        // is in the dark world
        if (!this.InDark 
            || Math.Abs(oldShineLocation - this._defaultShineLocation) 
            > this.Tolerance) {
            this.setShininess(oldShineLocation);
        }

        this.updateAudioVolume();
    }

    void OnTriggerEnter2D(Collider2D collider2D) {
        if (this.checkIfOther(collider2D)) {
            this.handlePortalPhysics(collider2D);
            return;
        }

        if (!this.InDark || !this.checkIfPlayer(collider2D)) return;
        this.InDark = false;
        Portal.SetLayerRecursively(
            this.Player, LayerMask.NameToLayer("Light"));
        Portal.SetSortingLayerRecursively(
            this.Player, "MiddlegroundLight");

        this.updateAudioClip(this.InSound);
    }

    void OnTriggerExit2D(Collider2D collider2D) {
        if (this.InDark || !this.checkIfPlayer(collider2D)) return;
        this.InDark = true;
        Portal.SetLayerRecursively(this.Player, LayerMask.NameToLayer("Dark"));
        Portal.SetSortingLayerRecursively(
            this.Player, "MiddlegroundDark");

        this.updateAudioClip(this.OutSound);
    }

    private bool checkIfPlayer(Collider2D collider2D) {
        return (collider2D.tag == "Player" && collider2D is CircleCollider2D);
    }

    public static void SetLayerRecursively(
        GameObject gameObject, int newLayer) {
        gameObject.layer = newLayer;

        foreach (Transform child 
            in gameObject.GetComponentInChildren<Transform>()) {
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }

    public static void SetSortingLayerRecursively(
        GameObject gameObject, string newSortingLayer) {
        SpriteRenderer spriteRenderer 
            = gameObject.GetComponent<SpriteRenderer>();

        if (spriteRenderer != null) {
            spriteRenderer.sortingLayerName = newSortingLayer;
        }

        foreach (Transform child
            in gameObject.GetComponentInChildren<Transform>()) {
            SetSortingLayerRecursively(child.gameObject, newSortingLayer);
        }
    }

    private void setShininess(float oldShineLocation) {
        float newShineLocation 
            = oldShineLocation + (this.ShineSpeed * Time.deltaTime);
        newShineLocation %= 1;
        this._renderer.material.SetFloat("_ShineLocation", newShineLocation);
        this._renderer.material.SetFloat(
            "_ShineWidth", this.computeShineWidth(newShineLocation));
    }

    private float computeShineWidth(float shineLocation) {
        float shineWidth = 2*shineLocation;
        const float middle = 0.5f;

        if (shineWidth > 2* middle) {
            shineWidth = this.computeShineWidth(2*middle - shineLocation);
        }

        return shineWidth;
    }

    private void updateAudioVolume() {
        if (!this.InDark) return;

        Vector2 distance 
            = this.Player.transform.position - this.transform.position;
        this._audioSource.volume = 1/distance.sqrMagnitude;
        this._audioSource.volume *= this.SoundFactor;
    }

    private void updateAudioClip(AudioClip audioClip) {
        this._audioSource.clip = audioClip;
        this._audioSource.Play();
    }

    private void buildAudioSource(AudioClip audioClip) {
        this._audioSource 
            = SoundManager.Instance.gameObject.AddComponent<AudioSource>();
        this._audioSource.loop = true;
        this._audioSource.clip = audioClip;
    }


    private bool checkIfOther(Collider2D collider2D) {
        return collider2D.tag != "Player";
    }

    private void handlePortalPhysics(Collider2D collider2D) {
        if (collider2D is PolygonCollider2D && collider2D.tag == "Ground") {
            this.computeColliders((PolygonCollider2D) collider2D);
        }
    }

    private void computeColliders(PolygonCollider2D polygonCollider2D) {
        List<Vector2> intersectionPoints 
            = this.computeIntersectionPoints(polygonCollider2D);
        this.createArcColliders2D(intersectionPoints, polygonCollider2D);
    }

    private List<Vector2> computeIntersectionPoints(
        PolygonCollider2D polygonCollider2D) {
        Vector2[] points = polygonCollider2D.GetPath(0);
        int size = points.Length;
        List<Vector2> intersectionPoints = new List<Vector2>();
        List<Vector2> edgeIntersections;
        CircleCollider2D circle = this.GetComponent <CircleCollider2D>();

        if (size < 2) return intersectionPoints;

        for (int i = 1; i < size; i++) {
            edgeIntersections 
                = this.computeEdgeIntersections(
                    polygonCollider2D.transform.TransformPoint(points[i]),
                    polygonCollider2D.transform.TransformPoint(points[i - 1]),
                    circle);

            if (edgeIntersections == null || edgeIntersections.Count <= 0) {
                continue;
            }

            intersectionPoints.AddRange(edgeIntersections);
        }

        edgeIntersections
                = this.computeEdgeIntersections(
                    polygonCollider2D.transform.TransformPoint(points[size-1]),
                    polygonCollider2D.transform.TransformPoint(points[0]),
                    circle);

        if (edgeIntersections != null && edgeIntersections.Count > 0) {
            intersectionPoints.AddRange(edgeIntersections);
        }        

        return intersectionPoints;
    }

    private List<Vector2> computeEdgeIntersections(
        Vector2 p1, Vector2 p2, CircleCollider2D circle) {

        // both points in circle
        bool isP1In = circle.OverlapPoint(p1);
        bool isP2In = circle.OverlapPoint(p2);

        if (isP1In && isP2In) return null;

        RaycastHit2D hit;
        List<Vector2> edgeIntersections = new List<Vector2>();

        if (isP1In && !isP2In) {
            hit = Physics2D.Raycast(p2, p1 - p2, 
                Mathf.Infinity, 1 << LayerMask.NameToLayer("Portal"));
            if (hit.collider != null) edgeIntersections.Add(hit.point);
        } else if (isP2In && !isP1In) {
            hit = Physics2D.Raycast(p1, p2 - p1, 
                Mathf.Infinity, 1 << LayerMask.NameToLayer("Portal"));
            if (hit.collider != null) edgeIntersections.Add(hit.point);
        } else {
            float magnitude = (p2 - p1).magnitude;
            hit = Physics2D.Raycast(p1, p2 - p1, 
                magnitude, 1 << LayerMask.NameToLayer("Portal"));

            if (hit.collider != null) {
                edgeIntersections.Add(hit.point);
                hit = Physics2D.Raycast(p2, p1 - p2, 
                    magnitude, 1 << LayerMask.NameToLayer("Portal"));
                if (hit.collider != null) edgeIntersections.Add(hit.point);
            }
        }

        return edgeIntersections;
    }

    private void createArcColliders2D(List<Vector2> intersectionPoints, 
        PolygonCollider2D polygonCollider2D) {
        List<float> angles = new List<float>();
        int size = intersectionPoints.Count;
        CircleCollider2D circle = this.GetComponent<CircleCollider2D>();
        Vector2 center 
            = new Vector2(circle.transform.position.x + circle.offset.x,
            circle.transform.position.y + circle.offset.y);
        float radius = circle.radius;
        Vector2 point;

        for (int i = 0; i < size; i++) {
            point = intersectionPoints[i];
            angles.Add(((Mathf.Atan2(point.y - center.y,point.x - center.x) 
                * Mathf.Rad2Deg )+360)%360);

        }
        angles.Sort();

        size = angles.Count;

        float searchAngle = (angles[0] + angles[1] / 2) % 360;
        Vector2 searchPoint = new Vector2(
            Mathf.Cos(searchAngle) * radius + center.x,
            Mathf.Sin(searchAngle) * radius + center.y);
        bool searchState = polygonCollider2D.OverlapPoint(searchPoint);

        for (int i = 0; i < size; i++) {
            if (searchState) {
                GameObject newObjectCollider = new GameObject();
                newObjectCollider.transform.parent = this.gameObject.transform;
                newObjectCollider.name = "Portal Collider";
                newObjectCollider.tag = "Portal Collider";
                newObjectCollider.transform.localScale = new Vector3(1, 1, 1);
                ArcCollider2D newArc
                    = newObjectCollider.AddComponent<ArcCollider2D>();
                newArc.offsetRotation = (int) Mathf.Round(angles[i]);
                newArc.radius = radius;

                if (i == size - 1) {
                    newArc.totalAngle = (int) Mathf.Round(
                        angles[0] + 360 - angles[size-1]);
                } else { 
                    newArc.totalAngle = (int) Mathf.Round(
                    angles[i + 1] - angles[i]);
                }

                
                newArc.pizzaSlice = false;
                searchState = false;
                newObjectCollider.transform.localPosition = Vector3.zero;
                newArc.GetComponent<EdgeCollider2D>().offset = Vector2.zero;
            } else {
                searchState = true;
            }
        }
    }
}