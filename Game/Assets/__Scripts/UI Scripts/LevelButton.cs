using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour {

    public Text levelText;
    public int level;
    public GameObject confirmPanel;

    [Header("Active Stuff")]
    public bool isActive;
    public Sprite activeSprite; //Available levels
    public Sprite lockedSprite; //Locked levels

    private Image buttonImage;
    private Button myButton;

	// Use this for initialization
	void Start () {
        buttonImage = GetComponent<Image>();
        myButton = GetComponent<Button>();
        ShowLevel();
        DecideSprite();
	}

    void DecideSprite()
    {
        if (isActive)
        {
            buttonImage.sprite = activeSprite;
            myButton.enabled = true;
            levelText.enabled = true;
        }else
        {
            buttonImage.sprite = lockedSprite;
            myButton.enabled = false;
            levelText.enabled = false;
        }
    }
	
    void ShowLevel()
    {
        levelText.text = "" + level;
    }

	// Update is called once per frame
	void Update () {
		
	}

    public void ConfirmPanel(int level)
    {
        confirmPanel.GetComponent<ConfirmPanel>().level = level;
        confirmPanel.SetActive(true);
    }
}
