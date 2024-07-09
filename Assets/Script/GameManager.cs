using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Pendu currentGame;
    public UiManager Ui;
    void Start()
    {
        currentGame = new Pendu(this);
    }
    public void Restart()
    {
        currentGame = new Pendu(this);
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
