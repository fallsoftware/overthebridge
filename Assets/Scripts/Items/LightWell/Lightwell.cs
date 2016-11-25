using UnityEngine;
using System.Collections;

public class lightwell : MonoBehaviour {

    public bool activated = true;
    public float maxdistance = 100;
    public Collider2D portalCollider;
    public int PlayerSpeed = 10;
    [Range(1, 10)]
    public int RayNumber = 1;
    [Range(1, 5)]
    public float width = 1;
    public float Angle { get { return this.transform.rotation.eulerAngles.z * Mathf.Deg2Rad; } }
    private LineRenderer[] line;
    public Material material;
	// Use this for initialization
	void Start () {
        material = new Material(Shader.Find("Particles/Additive"));
        line = new LineRenderer[RayNumber];
        for (int i = 0; i < RayNumber; i++)
        {
            GameObject newgameobject = new GameObject();
            newgameobject.transform.parent = this.transform;
            line[i] = newgameobject.AddComponent<LineRenderer>();
            line[i].SetVertexCount(2);
            line[i].SetColors(Color.white, new Color(255, 255, 255, 0.0f));
            line[i].material = material;
            line[i].sortingLayerName = "ShadowLight";
        }

    }
	
	// Update is called once per frame
	void Update () {
        if (portalCollider == null)
        {
            portalCollider = GameObject.FindWithTag("Portal").GetComponent<CircleCollider2D>();

        }
        
        if (activated)
        {
            Collider2D coll = this.GetComponent<Collider2D>();

            Vector2 direction = new Vector2(Mathf.Cos(Angle), Mathf.Sin(Angle));
            RaycastHit2D[][] hits = new RaycastHit2D[RayNumber][];
            Vector2 delta = new Vector2(Mathf.Sin(Angle) * (width / (RayNumber - 1)),Mathf.Cos(Angle) * (width / (RayNumber - 1)));
            Vector2 currentPosition = this.transform.position;
            Vector2[] positions = new Vector2[RayNumber];
            if (RayNumber != 1) {
                currentPosition -= new Vector2(Mathf.Sin(Angle) * (width / 2), Mathf.Cos(Angle) * (width / 2));
            }
            for (int j = 0; j < RayNumber; j++)
            {
                positions[j] = currentPosition;
                line[j].SetPosition(0, new Vector3(currentPosition.x, currentPosition.y,-1));
                hits[j] = Physics2D.RaycastAll(currentPosition, direction, maxdistance);
                currentPosition += delta;
            }
            Debug.logger.Log(hits.Length);
            int i = 0;
            foreach (RaycastHit2D[] hitarray in hits)
            {
                Collider2D tempLight = null;
                Collider2D tempDark = null;
                bool hasObstacle = false;
                foreach (RaycastHit2D hit in hitarray)
                {

                    if (!hasObstacle) {
                        if (hit && hit.collider == coll)
                        {
                        }
                        else
                        {
                            GameObject otherObject = hit.collider.gameObject;
                            if (otherObject.CompareTag("Player"))
                            {
                                otherObject.GetComponent<Rigidbody2D>().velocity = direction * PlayerSpeed;
                                Debug.logger.Log(otherObject.GetComponent<Rigidbody2D>().velocity);
                            }
                            else if (otherObject.layer == LayerMask.NameToLayer("Dark") && otherObject.CompareTag("Ground"))
                            {
                                if (portalCollider.OverlapPoint(hit.point))
                                {
                                    tempDark = hit.collider;
                                }
                                else
                                {
                                    hasObstacle = true;

                                }

                            }
                            else if (otherObject.layer == LayerMask.NameToLayer("Light") && otherObject.CompareTag("Ground"))
                            {
                                if (portalCollider.OverlapPoint(hit.point))
                                {
                                    hasObstacle = true;
                                }
                                else
                                {
                                    tempLight = hit.collider;
                                }
                            }
                            else if (otherObject.layer == LayerMask.NameToLayer("Portal"))// A DEBUGGER CA NE MARCHE PAS
                            {

                                if ((tempDark != null && tempDark.OverlapPoint(hit.point)) || (tempLight != null && tempLight.OverlapPoint(hit.point)))
                                {
                                    Debug.logger.Log(tempDark);

                                    hasObstacle = true;
                                }
                            }
                            else if (otherObject.layer == LayerMask.NameToLayer("ShadowLight"))
                            {
                                otherObject.GetComponent<ShadowLightPlatform>().ApllyForce(direction * PlayerSpeed);
                                hasObstacle = true;
                            }
                            if (hasObstacle)
                            {
                                line[i].SetPosition(1, new Vector3(hit.point.x, hit.point.y, -1));
                            }

                        }
                    }
                }
                if (!hasObstacle)
                {
                    line[i].SetPosition(1, new Vector3(positions[i].x, positions[i].y,0) + new Vector3(Mathf.Cos(Angle) * maxdistance, Mathf.Sin(Angle) * maxdistance,-1));
                }
                i++;
            }
        }
	}
}
