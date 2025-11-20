using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Hangman
{
    public enum ScreenType
    {
        /*CreateOrSignUI, CreateAccountUI, SignUpUI,*/ StartUI, GameUI, SettingsUI, CategoryUI, ClassUI, WinUI, LoseUI, PauseUI
    }
    public class GameManager : MonoBehaviour
    {
        // Instance statique du GameManager
        public static GameManager Instance;

        [SerializeField] public UiManager uiManager;

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
            SwitchScreen(ScreenType.StartUI);
            uiManager.SetupButtons();
        }

        public void SwitchScreen(ScreenType screenType)
        {
            HideAllScreens();

            // Affiche l'écran correspondant
            switch (screenType)
            {
                case ScreenType.StartUI:
                    uiManager.startUiRoot.style.display = DisplayStyle.Flex;
                    break;
                /*case ScreenType.CreateAccountUI:
                    uiManager.createAccountUiRoot.style.display = DisplayStyle.Flex;
                    break;
                case ScreenType.CreateOrSignUI:
                    uiManager.createOrSignUiRoot.style.display = DisplayStyle.Flex;
                    break;
                case ScreenType.SignUpUI:
                    uiManager.signUpUiRoot.style.display = DisplayStyle.Flex;
                    break;*/
                case ScreenType.GameUI:
                    uiManager.gameUiRoot.style.display = DisplayStyle.Flex;
                    break;
                case ScreenType.SettingsUI:
                    uiManager.settingsUiRoot.style.display = DisplayStyle.Flex;
                    break;
                case ScreenType.CategoryUI:
                    uiManager.categoryUiRoot.style.display = DisplayStyle.Flex;
                    break;
                case ScreenType.ClassUI:
                    uiManager.classUiRoot.style.display = DisplayStyle.Flex;
                    break;
                case ScreenType.WinUI:
                    uiManager.winUiRoot.style.display = DisplayStyle.Flex;
                    break;
                case ScreenType.LoseUI:
                    uiManager.loseUiRoot.style.display = DisplayStyle.Flex;
                    break;
                case ScreenType.PauseUI:
                    uiManager.pauseUiRoot.style.display = DisplayStyle.Flex;
                    break;
                default:
                    Debug.LogWarning("Unknown screen type: " + screenType);
                    break;
            }
        }

        private void HideAllScreens()
        {
            uiManager.startUiRoot.style.display = DisplayStyle.None;
            /* uiManager.createAccountUiRoot.style.display = DisplayStyle.None;
            uiManager.createOrSignUiRoot.style.display = DisplayStyle.None;
            uiManager.signUpUiRoot.style.display = DisplayStyle.None;*/
            uiManager.gameUiRoot.style.display = DisplayStyle.None;
            uiManager.settingsUiRoot.style.display = DisplayStyle.None;
            uiManager.categoryUiRoot.style.display = DisplayStyle.None;
            uiManager.classUiRoot.style.display = DisplayStyle.None;
            uiManager.winUiRoot.style.display = DisplayStyle.None;
            uiManager.loseUiRoot.style.display = DisplayStyle.None;
            uiManager.pauseUiRoot.style.display = DisplayStyle.None;
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
