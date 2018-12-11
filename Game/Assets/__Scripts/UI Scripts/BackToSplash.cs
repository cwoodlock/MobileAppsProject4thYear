using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToSplash : MonoBehaviour {

    public string SceneToLoad;

    public void Ok()
    {
        //https://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager.html
        SceneManager.LoadScene(SceneToLoad);
    }
}
