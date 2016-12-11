using UnityEngine;
using System.Collections;

public class LightWell : MonoBehaviour {
    public Vector2 Offset = new Vector2(0, -8);
    public bool activated = true;
    public float maxdistance = 100;
    public Collider2D portalCollider;
    public int PlayerSpeed = 10;
    [Range(1, 10)]
    public int RayNumber = 6;
    [Range(1, 5)]
    public float width = 3;
    public float Angle { get { return (this.transform.rotation.eulerAngles.z + 90) * Mathf.Deg2Rad; } }
    private LineRenderer[][] line;
    public Material material;
    private float yLight = 0;
    private Animator _lightAnimator;
    private Animator _darkAnimator;
    public LevelManager levelManager;
    public AudioClip EatSound;
    public AudioClip BeamSound;

    private AudioSource _eatSource;
    private AudioSource _beamSource;

    // Use this for initialization
    void Start() {
        _darkAnimator = this.GetComponent<Animator>();
        _lightAnimator = this.transform.FindChild("GoodLightWell").GetComponent<Animator>();
        yLight = this.transform.FindChild("GoodLightWell").transform.position.y-1.5f;
        line = new LineRenderer[RayNumber][];
        for (int i = 0; i < RayNumber; i++)
        {

            line[i] = new LineRenderer[2];
            for(int j = 0; j < 2; j++)
            {
                GameObject newgameobject = new GameObject();
                newgameobject.name = "LightBeam";
                newgameobject.transform.parent = this.transform;
                line[i][j] = newgameobject.AddComponent<LineRenderer>();
                line[i][j].SetVertexCount(2);
                line[i][j].SetColors(Color.white, new Color(255, 255, 255, 0.0f));
                line[i][j].material = material;
                
            }
            line[i][0].sortingLayerName = "ShadowLightDark";
            line[i][1].sortingLayerName = "ShadowLightLight";
        }
        _darkAnimator.SetBool("Active", activated);
        _lightAnimator.SetBool("Active", !activated);
        this.buildAudioSources();
    }
	public void ToggleBeam()
    {
        activated = !activated;
        {
            for (int i = 0; i < RayNumber; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    line[i][j].enabled=activated;

                }
            }
        }
        _darkAnimator.SetBool("Active", activated);
        _lightAnimator.SetBool("Active", !activated);
    }
	// Update is called once per frame
	void Update () {
        if (portalCollider == null)
        {
            portalCollider = GameObject.FindWithTag("Portal").GetComponent<CircleCollider2D>();

        }
            Collider2D coll = this.GetComponent<Collider2D>();

            Vector2 direction = new Vector2(Mathf.Cos(Angle), Mathf.Sin(Angle));
            RaycastHit2D[][] hits = new RaycastHit2D[RayNumber][];
            Vector2 delta = new Vector2(Mathf.Sin(Angle) * (width / (RayNumber - 1)),-Mathf.Cos(Angle) * (width / (RayNumber - 1)));
            Vector2 currentPosition = this.transform.position;
            Vector2 RealOffset = new Vector2(Mathf.Cos(Angle) * this.Offset.y+ Mathf.Sin(Angle) * this.Offset.x, Mathf.Cos(Angle) * this.Offset.x + Mathf.Sin(Angle) * this.Offset.y);
            currentPosition += RealOffset;
            Vector2[] positions = new Vector2[RayNumber];
            if (RayNumber != 1) {
                currentPosition -= new Vector2(Mathf.Sin(Angle) * (width / 2),- Mathf.Cos(Angle) * (width / 2));
            }
            for (int j = 0; j < RayNumber; j++)
            {
                positions[j] = currentPosition;
                line[j][0].SetPosition(0, new Vector3(currentPosition.x, currentPosition.y,-1));
                line[j][1].SetPosition(0, new Vector3(currentPosition.x, currentPosition.y, -1));
            if (activated ^ (portalCollider.OverlapPoint(new Vector2(currentPosition.x,yLight))))
                {
                    hits[j] = Physics2D.RaycastAll(currentPosition, direction, maxdistance);
                }
                else
                {
                    line[j][0].SetPosition(1, new Vector3(currentPosition.x, currentPosition.y, -1));
                    line[j][1].SetPosition(1, new Vector3(currentPosition.x, currentPosition.y, -1));
                }
                currentPosition += delta;
            }

            int i = 0;
            foreach (RaycastHit2D[] hitarray in hits)
            {
                if (hitarray != null)
                {
                Collider2D tempLight = null;
                Collider2D tempDark = null;
                bool hasObstacle = false;
                foreach (RaycastHit2D hit in hitarray)
                {

                    if (!hasObstacle)
                    {
                        if (hit && hit.collider == coll)
                        {
                        }
                        else
                        {
                            GameObject otherObject = hit.collider.gameObject;
                            if (otherObject.CompareTag("Player"))
                            {
                                otherObject.GetComponent<PlayerControllerScript>().InitBeam(PlayerSpeed * direction);
                                otherObject.GetComponent<PlayerControllerScript>()._doubleJump = false;

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
                            else if (otherObject.CompareTag("Enemy"))
                            {
                                otherObject.GetComponent<EnemyScript>().Death();
                            }
                            else if (otherObject.layer == LayerMask.NameToLayer("Portal"))// A DEBUGGER CA NE MARCHE PAS
                            {

                                if ((tempDark != null && tempDark.OverlapPoint(hit.point)) || (tempLight != null && tempLight.OverlapPoint(hit.point)))
                                {
                                    hasObstacle = true;
                                }
                            }
                            else if (otherObject.layer == LayerMask.NameToLayer("ShadowLightDark"))
                            {
                                otherObject.GetComponent<ShadowLightPlatform>().ApllyForce(direction * PlayerSpeed);
                                hasObstacle = true;
                            }
                            if (hasObstacle)
                            {
                                Vector3 Offset = new Vector3(hit.point.x, hit.point.y, -1);
                                line[i][0].SetPosition(1, Offset);
                                line[i][1].SetPosition(1, Offset);
                            }

                        }
                    }
                }
                if (!hasObstacle)
                {
                    Vector3 Offset = new Vector3(positions[i].x, positions[i].y, 0) + new Vector3(Mathf.Cos(Angle) * maxdistance, Mathf.Sin(Angle) * maxdistance, -1);
                    line[i][0].SetPosition(1, Offset);
                    line[i][1].SetPosition(1, Offset);
                }
                i++;
            }
            }
	}
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag != "Player") return;
        _darkAnimator.SetBool("Eats", true);
        _lightAnimator.SetBool("Eats", true);

        SoundManager.Instance.PlayFx(this._eatSource);
        levelManager.OopsPlayerIsDead();
    }

    private void updateAudio() {
        if (this.activated) {
            this._beamSource.volume = 1f;
        } else {
            this._beamSource.volume = 0f;
        }
    }

    private void buildAudioSources() {
        this._eatSource
            = Sound.BuildFxSource(this.gameObject, this.EatSound, false, 0f);
        this._eatSource = SoundManager.Instance.AddFxSource(
                    this._eatSource, "LightWellEats" + this.GetInstanceID());
        this._eatSource.volume = 1f;
        this._eatSource.dopplerLevel = 0f;

        this._beamSource
            = Sound.BuildFxSource(this.gameObject, this.BeamSound, true, 1f);
        this._beamSource = SoundManager.Instance.AddFxSource(
                    this._beamSource, "LightWellBeam" + this.GetInstanceID());
        this._beamSource.volume = 1f;
        this._beamSource.dopplerLevel = 0f;
        SoundManager.Instance.PlayFx(this._beamSource);
    }
}
