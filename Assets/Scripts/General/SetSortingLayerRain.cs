using UnityEngine;
using System.Collections;

public class SetSortingLayerRain : MonoBehaviour {
    public ParticleSystem[] ParticleSystems;
    public string SortingLayer = "UI";

	void Start () {
	    int size = this.ParticleSystems.Length;

	    for (int i = 0; i < size; i++) {
	        this.ParticleSystems[i].GetComponent<Renderer>().sortingLayerName
	            = this.SortingLayer;
	    }
	}

	void Update () {
	
	}
}
