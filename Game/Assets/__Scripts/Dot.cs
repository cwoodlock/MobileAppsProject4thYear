using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dot : MonoBehaviour {

    private Vector2 firstTouchPosition;
    private Vector2 finalTouchPosition;
    public float swipeAngle = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
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
        Debug.Log(swipeAngle);
    }
}
