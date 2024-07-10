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
    TextField information;
    Button turnBack;

    public string targetWord = "HELLO";
    private string guessedWord;
    public const string CATEGORIE = "https://trouve-mot.fr/api/categorie";

    public bool IsWon;
    public int score;
    public int lifeMax;

    public Pendu(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    void Awake()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        wordLabel = root.Q<Label>("Word");
        indiceText = root.Q<TextField>("Indice");
        information = root.Q<TextField>("Information");
        turnBack = root.Q<Button>("Return");

        List<Button> bonusButtons = root.Query<Button>("BonusButton").ToList();
        foreach (Button button in bonusButtons)
        {
            button.clickable.clickedWithEventInfo += OnGameButtonTouch;
        }
        List<Button> alphaButton = root.Query<Button>("AlphaButton").ToList();
        foreach (Button button in alphaButton)
        {
            string letter = button.text;
            button.clicked += () => OnLetterTouch(letter);
        }

        turnBack.clickable.clicked += OnReturnTouch;

        guessedWord = new string('-', targetWord.Length);
        UpdateWordLabel();

        lifeMax = targetWord.Length; // Nombre de vies bas� sur la longueur du mot
        score = 0; // Initialiser le score � 0
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

    void OnLetterTouch(string chosenLetter)
    {
        // Convertir la lettre choisie en majuscule pour la comparer avec targetWord
        chosenLetter = chosenLetter.ToUpper();

        if (IsCorrectLetter(chosenLetter))
        {
            information.value = $"Correct letter: {chosenLetter}";

            // Mettre � jour guessedWord avec la lettre correcte
            char[] guessedWordArray = guessedWord.ToCharArray();

            // V�rifier si au moins une lettre a �t� trouv�e
            bool letterGuessed = false;

            // Assurer que guessedWordArray a une taille correspondant � targetWord
            if (guessedWordArray.Length != targetWord.Length)
            {
                guessedWordArray = new char[targetWord.Length]; // Initialiser avec la taille correcte
                for (int i = 0; i < guessedWordArray.Length; i++)
                {
                    guessedWordArray[i] = '_'; // Initialiser avec des caract�res par d�faut (par exemple '_')
                }
            }

            for (int i = 0; i < targetWord.Length; i++)
            {
                if (char.ToUpper(targetWord[i]) == chosenLetter[0])
                {
                    // V�rifier que l'indice i est dans les limites de guessedWordArray
                    if (i < guessedWordArray.Length)
                    {
                        guessedWordArray[i] = chosenLetter[0];
                        letterGuessed = true;
                        score++;
                    }
                    else
                    {
                        Debug.LogError($"Index {i} is out of bounds for guessedWordArray of length {guessedWordArray.Length}");
                    }
                }
            }

            // Mettre � jour guessedWord uniquement si une lettre a �t� trouv�e
            if (letterGuessed)
            {
                guessedWord = new string(guessedWordArray);
                UpdateWordLabel();

                // V�rifier si le mot a �t� devin�
                if (guessedWord.Equals(targetWord))
                {
                    information.value = "F�licitations ! Vous avez devin� le mot !";
                    IsWon = true;
                    // Ajoutez ici votre logique pour la victoire
                }
            }
        }
        else
        {
            information.value = $"Incorrect letter: {chosenLetter}";
            lifeMax--;

            if (lifeMax <= 0)
            {
                information.value = "Game Over ! Vous avez perdu.";
                IsWon = false;
            }
            // Traitez le cas o� la lettre choisie est incorrecte
            // Ajoutez ici votre logique pour les lettres incorrectes
        }
    }

    void OnReturnTouch()
    {
        gameManager.Ui.isPlaying = false;
        gameManager.Ui.ChangeScreen();
    }

    public void CheckCategorie()
    {

    }

    void UpdateWordLabel()
    {
        // Met � jour le label avec le mot devin� jusqu'� pr�sent
        wordLabel.text = guessedWord;
    }

    bool IsCorrectLetter(string letter)
    {
        // V�rifie si la lettre choisie est pr�sente dans le mot cible
        return targetWord.Contains(letter);
    }
}
