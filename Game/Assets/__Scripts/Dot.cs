using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : MonoBehaviour
{

    //variables
    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;
    private GameObject otherDot;
    private Board board;
    private Vector2 tempPosition;

    public float swipeAngle = 0;
    public int column;
    public int row;
    public int targetX;
    public int targetY;
    public bool isMatched = false;
    public int previousColumn;
    public int previousRow;
    public float swipeResist = 1f;


    // Use this for initialization
    void Start()
    {
        //get a handle on the board
        board = FindObjectOfType<Board>();

        //targetX = (int)transform.position.x;
        //targetY = (int)transform.position.y;
       // row = targetY;
       // column = targetX;

       // previousRow = row;
       // previousColumn = column;

    }

    // Update is called once per frame
    void Update()        
    {
        //Try and find matches
        FindMatches();

        //Check matches
        if (isMatched)
        {
            SpriteRenderer mySprite = GetComponent<SpriteRenderer>();
            mySprite.color = new Color(0f, 0f, 0f, 0.2f); //change transperity of the dot
        }

        targetX = column;
        targetY = row;
        //*******Change Left and right************
        if (Mathf.Abs(targetX - transform.position.x) > .1)
        {
            //move towards the target
            tempPosition = new Vector2(targetX, transform.position.y);
            //https://docs.unity3d.com/ScriptReference/Vector2.Lerp.html
            transform.position = Vector2.Lerp(transform.position, tempPosition, .6f);
            if(board.allDots[column, row] != this.gameObject)
            {
                //set colum,row as this object
                board.allDots[column, row] = this.gameObject;
            }
        }
        else
        {
            //Directly set the position
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = tempPosition;
            
        }
        //*********Change Up and down ***********
        if (Mathf.Abs(targetY - transform.position.y) > .1)
        {
            //move towards the target
            tempPosition = new Vector2(transform.position.x, targetY);
            //https://docs.unity3d.com/ScriptReference/Vector2.Lerp.html
            transform.position = Vector2.Lerp(transform.position, tempPosition, .6f);
            if (board.allDots[column, row] != this.gameObject)
            {
                //set colum,row as this object
                board.allDots[column, row] = this.gameObject;
            }
        }
        else
        {
            //Directly set the position
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = tempPosition;
            
        }
    }

    //Get positon of the mouse when pressed 
    private void OnMouseDown()
    {
        //https://docs.unity3d.com/ScriptReference/Camera.ViewportToWorldPoint.html
        //Can only move if its in the move state
        if(board.currentState == GameState.move)
        {
            firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        //Debug.Log(firstTouchPosition);
    }

    //get final position of the mouse when it is release
    private void OnMouseUp()
    {
        if (board.currentState == GameState.move)
        {
            finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log(firstTouchPosition);
            CalculateAngle();
        }
        
    }

    //get the angle between the presses
    void CalculateAngle()
    {
        if (Mathf.Abs(finalTouchPosition.y - firstTouchPosition.y) > swipeResist || Mathf.Abs(finalTouchPosition.x - firstTouchPosition.x) > swipeResist)
        {
            swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, finalTouchPosition.x - firstTouchPosition.x) * 180 / Mathf.PI;
            MovePieces();
            board.currentState = GameState.wait;

        }
        else
        {
            board.currentState = GameState.move;

        }
    }

    void MovePieces()
        //Send damien this code
    {
        if ((swipeAngle > 135 && swipeAngle <= -135) && column > 0)
        {
            Debug.Log("Swipe left");
            //left swipe
            otherDot = board.allDots[column - 1, row]; //get dot thats to the right
            previousRow = row;
            previousColumn = column;
            otherDot.GetComponent<Dot>().column += 1; //get dot script for that dot and change the column
            column -= 1; //increase selected dot
        }
        if (swipeAngle > -45 && swipeAngle <= 45 && column < board.width -1)
        {
            Debug.Log("Swipe right");
            //Right swipe
            otherDot = board.allDots[column + 1, row]; //get dot thats to the right
            previousRow = row;
            previousColumn = column;
            otherDot.GetComponent<Dot>().column -= 1; //get dot script for that dot and change the column
            column += 1; //increase selected dot

        }
        if (swipeAngle > 45 && swipeAngle <= 135 && row < board.height -1)
        {
            Debug.Log("Swipe up");
            //up swipe
            otherDot = board.allDots[column, row+1]; //get dot thats to the right
            previousRow = row;
            previousColumn = column;
            otherDot.GetComponent<Dot>().row -= 1; //get dot script for that dot and change the column
            row += 1; //increase selected dot
        }

        if ((swipeAngle < -45 && swipeAngle >= -135) && row > 0)
        {
            Debug.Log("Swipe down");
            //down swipe
            otherDot = board.allDots[column, row-1]; //get dot thats to the right
            previousRow = row;
            previousColumn = column;
            otherDot.GetComponent<Dot>().row += 1; //get dot script for that dot and change the column
            row -= 1; //increase selected dot
        }

        //check to see if there are matches
        StartCoroutine(CheckMoveCo());
    }

    void FindMatches()
    {
        if(column > 0 && column < board.width - 1) //check left and right
        {
            GameObject leftDot1 = board.allDots[column - 1, row];
            GameObject rightDot1 = board.allDots[column + 1, row];
            if (leftDot1 != null && rightDot1 != null)
            {
                //https://docs.unity3d.com/Manual/Tags.html
                if (leftDot1.tag == this.gameObject.tag && rightDot1.tag == this.gameObject.tag)
                {
                    leftDot1.GetComponent<Dot>().isMatched = true;
                    rightDot1.GetComponent<Dot>().isMatched = true;
                    isMatched = true;
                }
            }
        }

        if (row > 0 && row < board.height - 1) //check up and down
        {
            GameObject upDot1 = board.allDots[column , row +1];
            GameObject downDot1 = board.allDots[column , row -1];

            if (upDot1 != null && downDot1 != null)
            {
                //https://docs.unity3d.com/Manual/Tags.html
                if (upDot1.tag == this.gameObject.tag && downDot1.tag == this.gameObject.tag)
                {
                    upDot1.GetComponent<Dot>().isMatched = true;
                    downDot1.GetComponent<Dot>().isMatched = true;
                    isMatched = true;
                }
            }
        }
    }
    //coroutine requires a return statement https://docs.unity3d.com/Manual/Coroutines.html
    public IEnumerator CheckMoveCo()
    {
        yield return new WaitForSeconds(.5f);
        if(otherDot != null) //check if pieces matched
        {
            if(!isMatched && !otherDot.GetComponent<Dot>().isMatched)
            {
                //change dot back when its not matched
                otherDot.GetComponent<Dot>().row = row;
                otherDot.GetComponent<Dot>().column = column;

                row = previousRow;
                column = previousColumn;

                yield return new WaitForSeconds(.5f);
                board.currentState = GameState.move;
            }
            else
            {
                board.DestroyMatches();
            }
            otherDot = null;
        } 
    }
}
