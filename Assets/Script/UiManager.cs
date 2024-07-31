using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

namespace Hangman
{
    public class UiManager : MonoBehaviour
    {
        public static UiManager Instance;

        [SerializeField] public GameManager gameManager;

        [SerializeField] public GameObject createOrSignUi;
        [SerializeField] public GameObject createAccountUi;
        [SerializeField] public GameObject signUpUi;
        [SerializeField] public GameObject startUi;
        [SerializeField] public GameObject gameUi;
        [SerializeField] public GameObject settingsUi;
        [SerializeField] public GameObject categoryUi;
        [SerializeField] public GameObject classUi;
        [SerializeField] public GameObject winUi;
        [SerializeField] public GameObject loseUi;
        [SerializeField] public GameObject pauseUi;

        public GameObject currentScreen;

        private float validateSlide;
        private float easeTimeSeconds = 1.2f;

        private void Awake()
        {
            if (Instance == null)
            {
                // Recherche de l'instance existante dans la scène
                Instance = this;
            }
        }
        // Start is called before the first frame update
        void Start()
        {
            currentScreen = createOrSignUi;//écran visible en premier

            if (currentScreen == createOrSignUi)
            {
                AudioManager.Instance.CreateAccountTheme();//joue le thème de l'écran de création de compte
            }

            if (currentScreen == startUi)
            {
                AudioManager.Instance.PlayTheme();//joue le thème principal
            }

            if (GameManager.Instance.gameWon > 0)
            {
                ChangeScreen(currentScreen, gameUi, 0); //0 pour une transition instantannée
                AudioManager.Instance.PlayGameTheme();//joue le theme de jeu
            }

        }

        //switch les différent UI en fonction de l'UI actif
        public void OnButtonTouch(Button button)
        {
            switch (button.name)
            {
                case "Create":
                    ChangeScreen(currentScreen, createAccountUi);
                    break;
                case "Sign":
                    ChangeScreen(currentScreen, signUpUi);
                    break;
                case "TouchToPlay":
                    DOTween.To(() => validateSlide, x => validateSlide = x, -110, easeTimeSeconds).SetEase(Ease.OutBounce).OnComplete(() => { ChangeScreen(currentScreen, gameUi); });
                    break;
                case "Settings":
                    ChangeScreen(currentScreen, settingsUi);
                    break;
                case "Category":
                    ChangeScreen(currentScreen, categoryUi);
                    break;
                case "Classement":
                    ChangeScreen(currentScreen, classUi);
                    break;
            }
            AudioManager.Instance.PlayGameClickSound();
        }

        //animation de changement d'écran/UI (3ème arguments optionnel est egale à 0.5f par défauts )
        public void ChangeScreen(GameObject fromScreen, GameObject toScreen, float speed = 0.5f)
        {

            fromScreen.transform.DOMoveX(-Screen.width, speed).OnComplete(() => {
                fromScreen.SetActive(false);
                toScreen.SetActive(true);
                toScreen.transform.position = new Vector3(Screen.width, toScreen.transform.position.y, toScreen.transform.position.z);
                toScreen.transform.DOMoveX(0, speed);
                currentScreen = toScreen;

                AudioManager.Instance.StopCurrentSound();
                if (currentScreen == startUi)
                {
                    AudioManager.Instance.PlayTheme();
                }
                else if (currentScreen == gameUi)
                {
                    AudioManager.Instance.PlayGameTheme();
                }
            });

        }

        //fonction UI menu pause
        public void PauseMenu()
        {
            ChangeScreen(currentScreen, pauseUi);
        }

        //fonction UI retour menu
        public void GoBackToMenu()
        {
            // Determine which screen is currently active and switch to startUi
            ChangeScreen(currentScreen, startUi);
        }

        //fonction UI retour compte menu
        public void ReturnToAccountMenu()
        {
            ChangeScreen(currentScreen, createOrSignUi);
        }

        //fonction UI victoire
        public void OnWin()
        {
            ChangeScreen(currentScreen, winUi);
        }

        //fonction UI défaite
        public void OnLose()
        {
            ChangeScreen(currentScreen, loseUi);
        }
    }

}