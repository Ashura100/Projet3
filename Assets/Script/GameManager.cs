using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Pendu currentGame;
    public UiManager currentUi;
    void Start()
    {
        
    }
    public void Restart()
    {
        
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
