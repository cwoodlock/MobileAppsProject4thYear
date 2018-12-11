using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ConfirmPanel : MonoBehaviour {

    public string levelToLoad;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void Cancel()
    {
        this.gameObject.SetActive(false);
    }

    public void Play()
    {
        SceneManager.LoadScene(levelToLoad);
    }
}
