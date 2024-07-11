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
    Button goBack;
    Button bonusRemoveWrongLetter;
    Button bonusShowCorrectLetter;

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
        goBack = root.Q<Button>("Return");

        bonusRemoveWrongLetter = root.Q<Button>("CancelLetter");
        bonusShowCorrectLetter = root.Q<Button>("AddLetter");

        List<Button> alphaButton = root.Query<Button>("AlphaButton").ToList();
        foreach (Button button in alphaButton)
        {
            string letter = button.text;
            button.clicked += () => OnLetterTouch(letter);
        }

        bonusRemoveWrongLetter.clicked += RemoveWrongLetter;
        bonusShowCorrectLetter.clicked += ShowCorrectLetter;

        goBack.clickable.clicked += GoBack;

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
                    gameManager.Ui.OnWin();
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
                gameManager.Ui.OnLose();
                IsWon = false;
            }
            // Traitez le cas o� la lettre choisie est incorrecte
            // Ajoutez ici votre logique pour les lettres incorrectes
        }
    }

    void RemoveWrongLetter()
    {
        char[] guessedWordArray = guessedWord.ToCharArray();
        for (int i = 0; i < guessedWordArray.Length; i++)
        {
            if (guessedWordArray[i] == '-')
            {
                // Trouver le bouton correspondant � cette lettre
                List<Button> alphaButtons = root.Query<Button>("AlphaButton").ToList();
                foreach (Button button in alphaButtons)
                {
                    if (button.text == targetWord[i].ToString())
                    {
                        // Changer la couleur du texte du bouton pour indiquer qu'il contient la bonne lettre
                        button.style.color = Color.green;
                    }
                }

                // Remplacer la lettre incorrecte par la lettre correcte dans guessedWord
                guessedWordArray[i] = targetWord[i];
                guessedWord = new string(guessedWordArray);
                break;
            }
        }
    }

    void ShowCorrectLetter()
    {
        // Find a correct letter in targetWord that hasn't been guessed yet and reveal it
        for (int i = 0; i < targetWord.Length; i++)
        {
            if (guessedWord[i] == '-')
            {
                guessedWord = guessedWord.Substring(0, i) + targetWord[i] + guessedWord.Substring(i + 1);
                UpdateWordLabel();
                break;
            }
        }
    }

    void CheckCategorie()
    {

    }

    void UpdateWordLabel()
    {
        // Met � jour le label avec le mot devin� jusqu'� pr�sent
        wordLabel.text = guessedWord;
    }

    void GoBack()
    {
        if (gameManager.Ui != null)
        {
            gameManager.Ui.GoBackToMenu();
        }
        else
        {
            Debug.LogError("UiManager not found!");
        }
    }

    bool IsCorrectLetter(string letter)
    {
        // V�rifie si la lettre choisie est pr�sente dans le mot cible
        return targetWord.Contains(letter);
    }
}
