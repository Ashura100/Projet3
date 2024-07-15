using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;

public class GameManager : MonoBehaviour
{
    public test currentGame;
    public UiManager Ui;
    void Start()
    {
        currentGame = new test(this);
    }
    public void Restart()
    {
        currentGame = new test(this);
    }

    public void Paused()
    {

    }

    public void Exit()
    {
        if (Application.isEditor)
        {
            UnityEditor.EditorApplication.isPlaying = false;

        }
        else
        {
            Application.Quit();
        }
    }
}
