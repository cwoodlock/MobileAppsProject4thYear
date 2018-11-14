using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundTile : MonoBehaviour {

    public GameObject[] dots;

	// Use this for initialization
	void Start () {
        Initialise();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Initialise()
    {
        int dotToUse = Random.Range(0, dots.Length);
        GameObject dot = Instantiate(dots[dotToUse], transform.position, Quaternion.identity);
        dot.transform.parent = this.transform; //Put in hirearchy
        dot.name = this.gameObject.name;
    }
}
