using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this will be used during refilling the board so that moves canot be made while it is refilling
public enum GameState
{
    wait,
    move
}

public class Board : MonoBehaviour {

    //variables
    public int width; //width of board
    public int height; //height of board
    public GameObject tilePrefab;
    private BackgroundTile[,] allTiles;
    public GameObject[] dots;
    public GameObject[,] allDots;
    public int offSet; //To be used to spawn in dots to slide into place
    public GameState currentState = GameState.move;
    public GameObject destroyEffect; //Used to import the particle effect to destroy the dots

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
                Vector2 tempPosition = new Vector2(i, j  + offSet);
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
                //set row and column of the dot here
                dot.GetComponent<Dot>().row = j;
                dot.GetComponent<Dot>().column = i;
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
            GameObject particle = Instantiate(destroyEffect, allDots[column, row].transform.position,Quaternion.identity);
            //Destroy the particle after .5f
            Destroy(particle, .5f);
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
                    Vector2 tempPosition = new Vector2(i, j + offSet);
                    int dotToUse = Random.Range(0, dots.Length);
                    GameObject piece = Instantiate(dots[dotToUse], tempPosition, Quaternion.identity);
                    allDots[i, j] = piece;
                    //Pieces will now slide in 
                    piece.GetComponent<Dot>().row = j;
                    piece.GetComponent<Dot>().column = i;
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

        //wait after we make some movements
        yield return new WaitForSeconds(.5f);

        //Check for deadlock
        if (IsDeadlocked())
        {
            ShuffleBoard();
            Debug.Log("Deadlocked!");
        }
        currentState = GameState.move;
    }

    //Starting to detect Deadlock
    private void SwitchPieces(int column, int row, Vector2 direction)
    {
        //Take the second piece and save it in a holder
        GameObject holder = allDots[column + (int)direction.x, row + (int)direction.y] as GameObject;
        //switchimh the first dot to be the second position
        allDots[column + (int)direction.x, row + (int)direction.y] = allDots[column, row];
        //Set the first dot to be the second dot
        allDots[column, row] = holder;
    }

    private bool CheckForMatches()
    {
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                //check if empty space 
                if(allDots[i,j] != null)
                {
                    //Make sure that one and two to the right are in the board
                    if (i < width - 2)
                    {
                        //check right and two dots to the right exist
                        if (allDots[i + +1, j] != null && allDots[i + 2, j] != null)
                        {
                            if (allDots[i + 1, j].tag == allDots[i, j].tag
                                && allDots[i + 2, j].tag == allDots[i, j].tag)
                            {
                                return true;
                            }
                        }
                    }
                    //make sure that one and two up are in the board
                    if (j < height - 2)
                    {
                        //Check up if dots exist and have same tags
                        if (allDots[i, j + 1] != null && allDots[i, j + 2] != null)
                        {
                            if (allDots[i, j + 1].tag == allDots[i, j].tag
                                && allDots[i, j + 2].tag == allDots[i, j].tag)
                            {
                                return true;
                            }
                        }
                    }
                }
            }
        }
        return false;
    }

    //Switch and check the pieces
    public bool SwitchAndCheck(int column, int row, Vector2 direction)
    {
        //switch piece up or down
        SwitchPieces(column, row, direction);
        //check for matches
        if (CheckForMatches())
        {
            //if it matches switch them back
            SwitchPieces(column, row, direction);
            return true;
        }
        SwitchPieces(column, row, direction);
        return false;
    }

    //Go through every piece on board switch right and check and then switch up and check
    private bool IsDeadlocked()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (allDots[i, j] != null)
                {
                    if (i < width - 1)
                    {
                        if (SwitchAndCheck(i, j, Vector2.right))
                        {
                            return false;
                        }
                    }
                    if (j < height - 1)
                    {
                        if (SwitchAndCheck(i, j, Vector2.up))
                        {
                            return false;
                        }
                    }
                }
            }
        }
        return true;
    }

    //Function to Shuffle the board if it is deadlocked
    private void ShuffleBoard()
    {
        //Create a list of game objects
        List<GameObject> newBoard = new List<GameObject>();

        //Add every piece to this list
        for(int i = 0; i < width; i++)
        {
            for (int j = 0; j < width; j++)
            {
                if(allDots[i, j] != null)
                {
                    newBoard.Add(allDots[i, j]);
                }
            }
        }

        //For every spot on the board 
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < width; j++)
            {
                //Pick a random number 
                int pieceToUse = Random.Range(0, newBoard.Count);
                int maxIterations = 0;

                while (MatchesAt(i, j, newBoard[pieceToUse]) && maxIterations < 100)
                { 
                
                    pieceToUse = Random.Range(0, newBoard.Count);
                    maxIterations++;
                    Debug.Log(maxIterations);
                }
                maxIterations = 0;
                //Make a container for the oiece
                Dot piece = newBoard[pieceToUse].GetComponent<Dot>();
                //Assign the colum and row to piece
                piece.column = i;
                piece.row = j;
                //Fill in the dots array with new piece
                allDots[i, j] = newBoard[pieceToUse];
                //Remove it from the list
                newBoard.Remove(newBoard[pieceToUse]);
            }
        }
        //Check if it still deadlocked
        if (IsDeadlocked())
        {
            ShuffleBoard(); //This could cause an infinite loop however it is unlikely
        }
    }
}
