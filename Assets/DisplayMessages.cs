using UnityEngine;
using System.Collections;

public class DisplayMessages : MonoBehaviour {
    public GameObject Messages;
    public string darkMessage ="";
    public string lightMessage ="";
    public GameObject AnchorMessage;
    private bool _displayed=false;
	// Use this for initialization
	void Start () {
	
	}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!_displayed && collision.gameObject.name == "Player")
        {
            GameObject newGameObject = Instantiate(Messages);
            newGameObject.transform.SetParent(this.transform);
            newGameObject.transform.position = AnchorMessage.transform.position;
            newGameObject.GetComponent<TextMesh>().text = darkMessage;
            newGameObject.transform.FindChild("MessageLight").GetComponent<TextMesh>().text = lightMessage;
            _displayed = true;
        }
    }
    // Update is called once per frame
    void Update () {
	
	}
}
