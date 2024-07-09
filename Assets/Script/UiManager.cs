using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class UiManager : MonoBehaviour
{
    private static UiManager instance;
    [SerializeField]
    UIDocument uIDocument;
    [SerializeField]
    GameObject starttUi;
    [SerializeField]
    GameObject gameUi;
    [SerializeField]
    GameObject settingsUi;
    [SerializeField]
    GameObject categoryUi;
    [SerializeField]
    GameObject classUi;

    VisualElement root;
    VisualElement startMenu;
    Button screen;
    Button settings;
    Button category;
    Button classement;

    public bool isPlaying;
    public bool isSettings;
    public bool isCategory;
    public bool isClass;

    // Start is called before the first frame update
    void Awake()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        startMenu = root.Q<VisualElement>("StartMenu");
        screen = root.Q<Button>("TouchToPlay");
        settings = root.Q<Button>("Settings");
        category = root.Q<Button>("Category");
        classement = root.Q<Button>("Classement");

        screen.clickable.clicked += OnScreenTouch;
        settings.clickable.clicked += OnSettingsTouch;
        category.clickable.clicked += OnCategoryTouch;
        classement.clickable.clicked += OnClassementTouch;
    }

    void OnScreenTouch()
    {
        ChangeScreen();
        isPlaying = true;
    }

    void OnSettingsTouch()
    {
        ChangeScreen();
        isSettings = true;
    }

    void OnCategoryTouch()
    {
        ChangeScreen();
        isCategory = true;
    }

    void OnClassementTouch()
    {
        ChangeScreen();
        isClass = true;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void ChangeSprite()
    {

    }

    public void ChangeScreen()
    {
        if (isPlaying == true)
        {
            starttUi.SetActive(false);
            gameUi.SetActive(true);
        }
        else
        {
            starttUi.SetActive(true);
            gameUi.SetActive(false);
        }

        /*if (isSettings == true)
        {
            starttUi.SetActive(false);
            settingsUi.SetActive(true);
        }
        else
        {
            starttUi.SetActive(true);
            settingsUi.SetActive(false);
        }

        if (isCategory == true)
        {
            starttUi.SetActive(false);
            categoryUi.SetActive(true);
        }
        else
        {
            starttUi.SetActive(true);
            categoryUi.SetActive(false);
        }

        if (isClass == true)
        {
            starttUi.SetActive(false);
            classUi.SetActive(true);
        }
        else
        {
            starttUi.SetActive(true);
            classUi.SetActive(false);
        }*/
    }

    void OnWin()
    {

    }

    void OnLose()
    {

    }

}
