using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class UiManager : MonoBehaviour
{
    public static UiManager Instance;

    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject startUi;
    [SerializeField] GameObject gameUi;
    [SerializeField] GameObject settingsUi;
    [SerializeField] GameObject categoryUi;
    [SerializeField] GameObject classUi;
    [SerializeField] GameObject winUi;
    [SerializeField] GameObject loseUi;

    public GameObject currentScreen;

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
        currentScreen = startUi;

        if(GameManager.Instance.gameWon > 0)
        {
            ChangeScreen(currentScreen, gameUi, 0); //0 pour une transition instantannée
        }
        
    }

    public void OnButtonTouch(Button button)
    {
        switch (button.name)
        {
            case "TouchToPlay":
                ChangeScreen(currentScreen, gameUi);
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
    }

    //3ème arguments optionnel est egale à 0.5f par défauts 
    public void ChangeScreen(GameObject fromScreen, GameObject toScreen, float speed = 0.5f)
    {
        
        fromScreen.transform.DOMoveX(-Screen.width, speed).OnComplete(() => { 
            fromScreen.SetActive(false); 
            toScreen.SetActive(true); 
            toScreen.transform.position = new Vector3(Screen.width, toScreen.transform.position.y, toScreen.transform.position.z);
            toScreen.transform.DOMoveX(0, speed);
            currentScreen = toScreen;
        });
        
    }

    public void GoBackToMenu()
    {
        // Determine which screen is currently active and switch to startUi
        ChangeScreen(currentScreen, startUi);
    }


    public void OnWin()
    {
        ChangeScreen(currentScreen, winUi);
    }

    public void OnLose()
    {
        ChangeScreen(currentScreen, loseUi);
    }
}
