using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour {

    public Text levelText;
    public int level;
    public GameObject confirmPanel;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ConfirmPanel()
    {
        confirmPanel.SetActive(true);
    }
}
