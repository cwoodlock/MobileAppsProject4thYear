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


    // Use this for initialization
    void Start()
    {
        board = FindObjectOfType<Board>();
        targetX = (int)transform.position.x;
        targetY = (int)transform.position.y;
        row = targetY;
        column = targetX;
    }

    // Update is called once per frame
    void Update()
    {
        targetX = column;
        targetY = row;
        //*******Change Left and right************
        if (Mathf.Abs(targetX - transform.position.x) > .1)
        {
            //move towards the target
            tempPosition = new Vector2(targetX, transform.position.y);
            //https://docs.unity3d.com/ScriptReference/Vector2.Lerp.html
            transform.position = Vector2.Lerp(transform.position, tempPosition, .4f);

        }
        else
        {
            //Directly set the position
            tempPosition = new Vector2(targetX, transform.position.y);
            transform.position = tempPosition;
            board.allDots[column, row] = this.gameObject;
        }
        //*********Change Up and down ***********
        if (Mathf.Abs(targetY - transform.position.y) > .1)
        {
            //move towards the target
            tempPosition = new Vector2(transform.position.x, targetY);
            //https://docs.unity3d.com/ScriptReference/Vector2.Lerp.html
            transform.position = Vector2.Lerp(transform.position, tempPosition, .4f);

        }
        else
        {
            //Directly set the position
            tempPosition = new Vector2(transform.position.x, targetY);
            transform.position = tempPosition;
            board.allDots[column, row] = this.gameObject;
        }
    }

    //Get positon of the mouse when pressed 
    private void OnMouseDown()
    {
        //https://docs.unity3d.com/ScriptReference/Camera.ViewportToWorldPoint.html
        firstTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Debug.Log(firstTouchPosition);
    }

    //get final position of the mouse when it is release
    private void OnMouseUp()
    {
        finalTouchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //Debug.Log(firstTouchPosition);
        CalculateAngle();
    }

    //get the angle between the presses
    void CalculateAngle()
    {
        swipeAngle = Mathf.Atan2(finalTouchPosition.y - firstTouchPosition.y, firstTouchPosition.x - firstTouchPosition.x) * 180 / Mathf.PI; //returns value in radians so must multiple by 180/PI
        //Debug.Log(swipeAngle);
        MovePieces();
    }

    void MovePieces()
    {
        if (swipeAngle > -45 && swipeAngle <= 45 && column < board.width)
        {
            //Right swipe
            otherDot = board.allDots[column + 1, row]; //get dot thats to the right
            otherDot.GetComponent<Dot>().column -= 1; //get dot scrit for that dot and change the column
            column += 1; //increase selected dot

        }
        else if (swipeAngle > 45 && swipeAngle <= 135 && row < board.height)
        {
            //up swipe
            otherDot = board.allDots[column, row+1]; //get dot thats to the right
            otherDot.GetComponent<Dot>().row -= 1; //get dot scrit for that dot and change the column
            row += 1; //increase selected dot
        }
        else if ((swipeAngle > 135 || swipeAngle <= -135 ) && column > 0)
        {
            //left swipe
            otherDot = board.allDots[column - 1, row]; //get dot thats to the right
            otherDot.GetComponent<Dot>().column += 1; //get dot scrit for that dot and change the column
            column -= 1; //increase selected dot
        }
        else if (swipeAngle > -45 && swipeAngle >= -135 && row > 0)
        {
            //Left swipe
            otherDot = board.allDots[column, row-1]; //get dot thats to the right
            otherDot.GetComponent<Dot>().row += 1; //get dot scrit for that dot and change the column
            row -= 1; //increase selected dot
        }
    }
}
