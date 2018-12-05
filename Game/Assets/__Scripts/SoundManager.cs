using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    //Variables
    public AudioSource[] destroyNoise;

    public void PlayRandomDestroyNoise()
    {
        //Choose random num
        int clipToPlay = Random.Range(0, destroyNoise.Length);

        //Play that clip
        destroyNoise[clipToPlay].Play();
    }

	
}
