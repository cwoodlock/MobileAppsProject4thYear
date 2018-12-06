using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "World", menuName = "Level")]
public class Level : ScriptableObject {

    [Header("Board Dimentions")]
    public int width;
    public int height;

    [Header("Available Dots")]
    public GameObject[] dots;

    [Header("Score Goals")]
    public int[] scoreGoals;
}
