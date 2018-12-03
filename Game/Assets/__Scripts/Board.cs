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

                //Crate dots, it now generates the board without matches
                int dotToUse = Random.Range(0, dots.Length);
                int maxIterations = 0;
                while (MatchesAt(i, j, dots[dotToUse]))
                {
                    dotToUse = Random.Range(0, dots.Length);
                    maxIterations++;
                    //Debug.Log(maxIterations);
                }

                GameObject dot = Instantiate(dots[dotToUse], tempPosition, Quaternion.identity);
                dot.transform.parent = this.transform; //Put in hirearchy
                dot.name = "( " + i + ", " + j + " )";

                //Add dots to array
                allDots[i, j] = dot;
            }
        }
    }

    //Generating board without matches
    private bool MatchesAt(int column, int row, GameObject piece)
    {
        if (column > 1 && row > 1)
        {
            if (allDots[column - 1, row].tag == piece.tag && allDots[column - 2, row].tag == piece.tag)
            {
                return true;
            }
            if (allDots[column, row - 1].tag == piece.tag && allDots[column, row - 2].tag == piece.tag)
            {
                return true;
            }

        }
        else if (column <= 1 || row <= 1)
        {
            if (row > 1)
            {
                if (allDots[column, row - 1].tag == piece.tag && allDots[column, row - 2].tag == piece.tag)
                {
                    return true;
                }
            }
            if (column > 1)
            {
                if (allDots[column - 1, row].tag == piece.tag && allDots[column - 2, row].tag == piece.tag)
                {
                    return true;
                }
            }
        }

        return false;
    }

    //This will destroy matched dots
    private void DestroyMatchesAt(int column, int row)
    {
        if (allDots[column, row].GetComponent<Dot>().isMatched)
        {
            Destroy(allDots[column, row]);
            allDots[column, row] = null;
        }
    }

    public void DestroyMatches()
    {
        //Cycle through all the pieces on the board
        for(int i =0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                //if there is an object in array
                if(allDots[i,j] != null)
                {
                    DestroyMatchesAt(i, j);
                }
            }
        }
        //find if anything needs to be decreased when there is a match
        StartCoroutine(DecreaseRowCo());
    }

    //Coroutine to drop the dots 
    private IEnumerator DecreaseRowCo()
    {
        int nullCount = 0;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if(allDots[i,j] == null)
                {
                    //increase null counts
                    nullCount++;
                } else if(nullCount > 0)
                {
                    allDots[i, j].GetComponent<Dot>().row -= nullCount;
                    //fix dot position
                    allDots[i, j] = null;
                }
                
            }
            //reset the nullcount
            nullCount = 0;
        }
        //pause to refil the board
        yield return new WaitForSeconds(.4f);
        StartCoroutine(FillBoardCo());
    }

    //Refill the board after dots are removed and dots are dropped down
    private void RefillBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if(allDots[i, j] == null)
                {
                    //Checks through all of the pieces and checks if null 
                    Vector2 tempPosition = new Vector2(i, j);
                    int dotToUse = Random.Range(0, dots.Length);
                    GameObject piece = Instantiate(dots[dotToUse], tempPosition, Quaternion.identity);
                    allDots[i, j] = piece;
                }
            }
        }
    }

    //Check if there are any current matches on the board 
    private bool MatchesOnBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if(allDots[i, j] != null)
                {
                    if(allDots[i, j].GetComponent<Dot>().isMatched)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    //CoRoutine to use refillboard and matchesonboard to fill the board constantly
    private IEnumerator FillBoardCo()
    {
        RefillBoard();
        //pause to refill board
        yield return new WaitForSeconds(.5f);

        while (MatchesOnBoard())
        {
            yield return new WaitForSeconds(.5f);
            DestroyMatches();
        }
    }
}
