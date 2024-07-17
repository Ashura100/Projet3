using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class UiManager : MonoBehaviour
{
    private static UiManager instance;

    // Ajoutez cette propriété pour accéder à l'instance unique de UiManager
    public static UiManager Instance
    {
        get
        {
            if (instance == null)
            {
                // Recherche de l'instance existante dans la scène
                instance = FindObjectOfType<UiManager>();

                // Si aucune instance n'est trouvée, créez une nouvelle instance
                if (instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(UiManager).Name);
                    instance = singletonObject.AddComponent<UiManager>();
                }
            }
            return instance;
        }
    }


    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject startUi;
    [SerializeField] GameObject gameUi;
    [SerializeField] GameObject settingsUi;
    [SerializeField] GameObject categoryUi;
    [SerializeField] GameObject classUi;
    [SerializeField] GameObject winUi;
    [SerializeField] GameObject loseUi;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void OnButtonTouch(Button button)
    {
        switch (button.name)
        {
            case "TouchToPlay":
                ChangeScreen(startUi, gameUi);
                break;
            case "Settings":
                ChangeScreen(startUi, settingsUi);
                break;
            case "Category":
                ChangeScreen(startUi, categoryUi);
                break;
            case "Classement":
                ChangeScreen(startUi, classUi);
                break;
        }
    }

    public void ChangeScreen(GameObject fromScreen, GameObject toScreen)
    {
        fromScreen.transform.DOMoveX(-Screen.width, 0.5f).OnComplete(() => fromScreen.SetActive(false)); toScreen.SetActive(true);
        // Ensure toScreen is positioned off-screen to the right and then animate it into view
        toScreen.transform.position = new Vector3(Screen.width, toScreen.transform.position.y, toScreen.transform.position.z);
        toScreen.transform.DOMoveX(0, 0.5f);
    }

    public void GoBackToMenu()
    {
        // Determine which screen is currently active and switch to startUi
        if (gameUi.activeSelf)
        {
            ChangeScreen(gameUi, startUi);
        }
        else if (settingsUi.activeSelf)
        {
            ChangeScreen(settingsUi, startUi);
        }
        else if (categoryUi.activeSelf)
        {
            ChangeScreen(categoryUi, startUi);
        }
        else if (classUi.activeSelf)
        {
            ChangeScreen(classUi, startUi);
        }
        else if (winUi.activeSelf)
        {
            ChangeScreen(winUi, gameUi);
        }
        else if (loseUi.activeSelf)
        {
            ChangeScreen(loseUi, startUi);
        }

        return;
    }


    public void OnWin()
    {
        ChangeScreen(gameUi, winUi);
    }

    public void OnLose()
    {
        ChangeScreen(gameUi, loseUi);
    }
}
