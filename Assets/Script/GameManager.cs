using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Instance statique du GameManager
    private static GameManager instance;

    // Propriété statique pour accéder à l'instance unique
    public static GameManager Instance
    {
        get
        {
            // Retourne l'instance existante s'il y en a une, sinon en crée une nouvelle
            if (instance == null)
            {
                instance = FindObjectOfType<GameManager>(); // Recherche GameManager dans la scène
                if (instance == null)
                {
                    GameObject obj = new GameObject("GameManager");
                    instance = obj.AddComponent<GameManager>();
                }
            }
            return instance;
        }
    }

    // Autres membres de la classe GameManager
    public Test currentGame;
    public string currentCategory = "10"; // Devient privée

    // Propriété publique pour accéder à currentCategory
    public string CurrentCategory
    {
        get { return currentCategory; }
        set { currentCategory = value; }
    }
    void Start()
    {
        
    }
    public void Restart()
    {
        SceneManager.LoadScene(0);
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
