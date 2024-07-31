using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Hangman
{
    public class GameManager : MonoBehaviour
    {
        // Instance statique du GameManager
        public static GameManager Instance;

        // Autres membres de la classe GameManager
        [SerializeField]
        public Pendu currentGame;
        public string currentCategory = "10"; // Devient privée
        public int score;
        public int gameWon;
        public bool isPaused;

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
        }

        //recharge l'application
        public void Restart()
        {
            SceneManager.LoadScene(0);
        }

        //met sur pause en faisait apparaitre l'UI pause
        public void Paused()
        {
            isPaused = true;
            UiManager.Instance.PauseMenu();
            Debug.Log("Pause");
        }
#if UNITY_EDITOR
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
#endif
    }
}
