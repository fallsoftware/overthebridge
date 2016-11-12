using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

public class PortalPhysics : MonoBehaviour {
    public string ColliderName = "Portal Collider";

	void Start () {
	
	}
	
	void Update () {
	
	}

    private bool checkIfOther(Collider2D collider2D) {
        return collider2D.tag != "Player";
    }

    void OnTriggerEnter2D(Collider2D collider2D) {
        if (this.checkIfOther(collider2D)) {
            this.handlePortalPhysics(collider2D);
            return;
        }
    }

    private void handlePortalPhysics(Collider2D collider2D) {
        if (collider2D is PolygonCollider2D && collider2D.tag == "Ground") {
            this.computeColliders((PolygonCollider2D)collider2D);
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
        CircleCollider2D circle = this.GetComponent<CircleCollider2D>();

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
                    polygonCollider2D.transform.TransformPoint(points[size - 1]),
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
            angles.Add(((Mathf.Atan2(point.y - center.y, point.x - center.x)
                * Mathf.Rad2Deg) + 360) % 360);

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
                newObjectCollider.name = this.ColliderName;
                newObjectCollider.tag = this.ColliderName;
                newObjectCollider.transform.localScale = new Vector3(1, 1, 1);
                ArcCollider2D newArc
                    = newObjectCollider.AddComponent<ArcCollider2D>();
                newArc.offsetRotation = (int)Mathf.Round(angles[i]);
                newArc.radius = radius;

                if (i == size - 1) {
                    newArc.totalAngle = (int)Mathf.Round(
                        angles[0] + 360 - angles[size - 1]);
                } else {
                    newArc.totalAngle = (int)Mathf.Round(
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

    public void DestroyColliders() {
        foreach (Transform child in this.gameObject.transform) {
            if (child.tag == this.ColliderName) {
                Destroy(child.gameObject);
            }
        }
    }

    public void ComputeColliders(bool computeColliders) {
        CircleCollider2D circle = this.GetComponent<CircleCollider2D>();

        circle.enabled = computeColliders;
    }
}
