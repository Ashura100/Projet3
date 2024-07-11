using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class UiManager : MonoBehaviour
{
    Pendu pendu;
    [SerializeField]
    UIDocument uIDocument;
    [SerializeField]
    GameObject startUi;
    [SerializeField]
    GameObject gameUi;
    [SerializeField]
    GameObject settingsUi;
    [SerializeField]
    GameObject categoryUi;
    [SerializeField]
    GameObject classUi;
    [SerializeField]
    GameObject winUi;
    [SerializeField]
    GameObject loseUi;

    VisualElement root;
    VisualElement startMenu;
    Button screen;
    Button settings;
    Button category;
    Button classement;

    // Start is called before the first frame update
    void Awake()
    {
        pendu = GetComponent<Pendu>();
        root = uIDocument.rootVisualElement;

        startMenu = root.Q<VisualElement>("StartMenu");
        screen = root.Q<Button>("TouchToPlay");
        settings = root.Q<Button>("Settings");
        category = root.Q<Button>("Category");
        classement = root.Q<Button>("Classement");

        List<Button> buttons = new List<Button> { screen, settings, category, classement };
        foreach (var button in buttons)
        {
            button.clickable.clicked += () => OnButtonTouch(button);
        }
    }

    void OnButtonTouch(Button button)
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
        // Use DoTween for a slide transition
        fromScreen.transform.DOMoveX(-Screen.width, 0.5f).OnComplete(() => fromScreen.SetActive(false));
        toScreen.transform.position = new Vector3(Screen.width, toScreen.transform.position.y, toScreen.transform.position.z);
        toScreen.SetActive(true);
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
