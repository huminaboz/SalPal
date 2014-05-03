using UnityEngine;
using System.Collections;

public class GUIManager : MonoBehaviour {

    //Variable for each of the GUIText objects on the stage
    public GUIText gameOverText, instructionsText, titleText;
	
	// Use this for initialization
	void Start () 
    {
        //Inform the event manager to call GameStart when the game begins
        GameEventManager.GameStart += GameStart;
        GameEventManager.GameOver += GameOver;
        gameOverText.enabled = false;
	}   
	
	// Update is called once per frame
	void Update () 
    {
        if (Input.GetButtonDown("Jump"))
        {
            GameEventManager.TriggerGameStart();
        }
	}

    private void GameStart()
    {
        gameOverText.enabled = false;
        instructionsText.enabled = false;
        titleText.enabled = false;
        enabled = false;
    }

    private void GameOver()
    {
        gameOverText.enabled = true;
        instructionsText.enabled = true;
        enabled = true;
    }
} //Class
