using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {

    //variables
    public int width; //width of board
    public int height; //height of board
    public GameObject tilePrefab;
    private BackgroundTile[,] allTiles;
    public GameObject[] dots;
    public GameObject[,] allDots;

    // Use this for initialization
    void Start () {
        allTiles = new BackgroundTile[width, height]; //size of the grid
        allDots = new GameObject[width, height];
        SetUp();
    }
	
	private void SetUp()
    {
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                //Adapted from https://gamedev.stackexchange.com/questions/128398/why-does-the-unity-manual-add-quaternion-identity-to-an-instantiated-object
                Vector2 tempPosition = new Vector2(i, j);
                GameObject backgroundTile = Instantiate(tilePrefab, tempPosition, Quaternion.identity) as GameObject;
                backgroundTile.transform.parent = this.transform; //This puts the grid pieces in the board hireacrchy
                backgroundTile.name = "( " + i + ", " + j + " )"; //This names the grid locations

                //Crate dots
                int dotToUse = Random.Range(0, dots.Length);
                GameObject dot = Instantiate(dots[dotToUse], tempPosition, Quaternion.identity);
                dot.transform.parent = this.transform; //Put in hirearchy
                dot.name = "( " + i + ", " + j + " )";

                //Add dots to array
                allDots[i, j] = dot;
            }
        }
    }
}
