using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Instance statique du GameManager
    public static GameManager Instance;

    // Autres membres de la classe GameManager
    [SerializeField]
    public Test currentGame;
    public string currentCategory = "10"; // Devient privée
    public int score = 0;
    public int gameWon;

    // Propriété publique pour accéder à currentCategory
    public string CurrentCategory
    {
        get { return currentCategory; }
        set { currentCategory = value; }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            // Recherche de l'instance existante dans la scène
            Instance = this;
        }
        DontDestroyOnLoad(Instance);
    }
    void Start()
    {
        gameWon = 0;
        score = 0;
    }
    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    public void Paused()
    {

    }

    /*public void Exit()
    {
        if (Application.isEditor)
        {
            UnityEditor.EditorApplication.isPlaying = false;

        }
        else
        {
            Application.Quit();
        }
    }*/
}
