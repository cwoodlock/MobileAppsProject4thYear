using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum GameType
{
    Moves,
    Time
}

[System.Serializable]
public class EndGameRequirement
{
    public GameType gameType;
    public int counterValue;
}

public class EndGameManager : MonoBehaviour {

    public GameObject movesLabel;
    public GameObject timeLabel;
    public Text counter;
    public int currentCounterValue;
    public EndGameRequirement requirements;

    private float timerSeconds;
    private Board board;

    // Use this for initialization
    void Start() {
        board = FindObjectOfType<Board>();
        SetupGame();
    }

    void SetupGame()
    {
        currentCounterValue = requirements.counterValue;
        if (requirements.gameType == GameType.Moves)
        {
            movesLabel.SetActive(true);
            timeLabel.SetActive(false);
        } else
        {
            timerSeconds = 1;
            movesLabel.SetActive(false);
            timeLabel.SetActive(true);
        }
        counter.text = "" + currentCounterValue;
    }

    public void DecreaseCounterValue() {

        if (board.currentState != GameState.pause)
        {
            currentCounterValue--;
            counter.text = "" + currentCounterValue;
            if (currentCounterValue <= 0)
            {
                board.currentState = GameState.lose;
                Debug.Log("You lose");
                currentCounterValue = 0;
                counter.text = "" + currentCounterValue;
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		if(requirements.gameType == GameType.Time)
        {
            timerSeconds -= Time.deltaTime;
            if(timerSeconds <= 0)
            {
                DecreaseCounterValue();
                timerSeconds = 1;
            }
        }

	}
}
