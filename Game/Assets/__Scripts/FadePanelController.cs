using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadePanelController : MonoBehaviour {

    //References to animators
    public Animator panelAnim;
    public Animator gameInfoAnim;

	//Method for ok button
    public void OK()
    {
        if (panelAnim != null && gameInfoAnim != null)
        {
            panelAnim.SetBool("Out", true);
            gameInfoAnim.SetBool("Out", true);
            StartCoroutine(GameStartCo());
        }
    }

    //Set the game state
    IEnumerator GameStartCo()
    {
        yield return new WaitForSeconds(1f);
        Board board = FindObjectOfType<Board>();
        board.currentState = GameState.move;
    }
}
