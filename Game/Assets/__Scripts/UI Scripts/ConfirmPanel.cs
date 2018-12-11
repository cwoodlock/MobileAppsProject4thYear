using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ConfirmPanel : MonoBehaviour {

    public string levelToLoad;
    public int level;

    public void Cancel()
    {
        this.gameObject.SetActive(false);
    }

    public void Play()
    {
        //used to set the level to load
        PlayerPrefs.SetInt("Current level", level -1);
        SceneManager.LoadScene("Main");
    }
}
