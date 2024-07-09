using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.UIElements;
using static System.Net.WebRequestMethods;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;


public class Pendu : MonoBehaviour
{
    [SerializeField]
    GameManager gameManager;
    [SerializeField]
    UIDocument uIDocument;
    VisualElement root;
    Label wordLabel;
    TextField indiceText;
    TextField infoamtion;
    Button turnBack;

    public string word;
    public const string CATEGORIE = "https://trouve-mot.fr/api/categorie";
    public List<char> playLetterList = new List<char>();

    public bool IsWon = false;
    public int score = 0;
    int counter = 0;
    int lifeMax = 10;

    public Pendu(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    void Awake()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        wordLabel = root.Q<Label>("Word");
        indiceText = root.Q<TextField>("Indice");
        infoamtion = root.Q<TextField>("Information");
        turnBack = root.Q<Button>("Return");

        List<Button> bonusButtons = root.Query<Button>("BonusButton").ToList();
        foreach (Button button in bonusButtons)
        {
            button.clickable.clickedWithEventInfo += OnGameButtonTouch;
        }
        List<Button> alphaButton = root.Query<Button>("AlphaButton").ToList();
        foreach (Button button in alphaButton)
        {
            button.clickable.clickedWithEventInfo += OnLetterTouch;
        }

        turnBack.clickable.clicked += OnReturnTouch;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnGameButtonTouch(EventBase eventBase)
    {
        Button button = (Button)eventBase.target;
        Debug.Log("click" + button.text);
    }

    void OnLetterTouch(EventBase evt)
    {
        Button button = (Button)evt.target;
        wordLabel.text = button.text;
        CheckFindWord();
    }

    void OnReturnTouch()
    {
        gameManager.Ui.isPlaying = false;
        gameManager.Ui.ChangeScreen();
    }

    public void CheckCategorie()
    {

    }

    public void CheckUserLetter(Button buttonText)
    {
        if (!IsInputValid(buttonText))
        {
            return;
        }

        infoamtion.value = $"il vous reste {lifeMax - counter} essaies";
        infoamtion.value = "veuillez saisir une lettre";
        infoamtion.value = $"veuillez saisir une lettre, il vous reste {lifeMax - counter} essaies";
        //buttonText = OnLetterTouch();
        char letter = buttonText.text[0];
    }

    public void CheckFindWord()
    {

    }

    bool IsInputValid(Button button)
    {
        if (//OnLetterTouch() || !Char.IsLetter(button.text[0]))
        {
            wordLabel.text = "veuillez saisir une lettre";
            return false;
        }
        return true;
    }
}
