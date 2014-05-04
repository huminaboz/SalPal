using UnityEngine;
using System.Collections;

public static class GameEventManager
{

    public delegate void GameEvent();
    public static event GameEvent GameStart, GameOver;

    //These functions will be called when we want to move to this gamestate
    public static void TriggerGameStart()
    {
        //Careful to only call an event if someone is subscribed to it - null otherwise and will cause an error
        //if (GameStart != null)
        //{
            GameStart();
        //}
    }

    public static void TriggerGameOver()
    {
        //if (GameOver != null)
        //{
            GameOver();
        //}
    }
} //Class
