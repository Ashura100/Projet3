using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using System;
using UnityEngine.UIElements;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Text;
using System.Linq;

public class Test : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private UIDocument uIDocument;

    private VisualElement root;
    private Label wordLabel;
    private TextField indiceText;
    private TextField information;
    private Button goBack;
    private Button bonusRemoveWrongLetter;
    private Button bonusShowCorrectLetter;

    private List<string> chosenLetters;
    public string targetWord;
    private string guessedWord;
    public const string CATEGORIE = "https://trouve-mot.fr/api/categorie/";

    public bool IsWon;
    public int score;
    public int lifeMax;

    private void OnEnable()
    {
        Reset();
    }

    public void Reset()
    {
        root = uIDocument.rootVisualElement;

        wordLabel = root.Q<Label>("Word");
        indiceText = root.Q<TextField>("Indice");
        information = root.Q<TextField>("Information");
        goBack = root.Q<Button>("Return");
        bonusRemoveWrongLetter = root.Q<Button>("CancelLetter");
        bonusShowCorrectLetter = root.Q<Button>("AddLetter");

        chosenLetters = new List<string>();

        bonusRemoveWrongLetter.clicked += RemoveWrongLetter;
        bonusShowCorrectLetter.clicked += ShowCorrectLetter;
        goBack.clicked += GoBack;

        var alphaButtons = root.Query<Button>("AlphaButton").ToList();
        foreach (var button in alphaButtons)
        {
            var letter = button.text;
            button.clicked += () => OnLetterTouch(letter);
        }

        guessedWord = new string('_', targetWord.Length);
        UpdateWordLabel();

        lifeMax = 11; // Nombre de vies basé sur un nombre fixe pour les jeux de pendu
        score = 0; // Initialiser le score à 0

        // Rechercher la définition du mot cible à l'aide de l'API de dictionnaire
        StartCoroutine(GetWordDefinition());
    }

    private void OnLetterTouch(string chosenLetter)
    {
        chosenLetter = chosenLetter.ToUpper();

        if (chosenLetters.Contains(chosenLetter))
        {
            information.value = $"Vous avez déjà choisi la lettre: {chosenLetter}";
            return;
        }

        chosenLetters.Add(chosenLetter);

        foreach (var button in root.Query<Button>("AlphaButton").ToList())
        {
            if (button.text == chosenLetter)
            {
                button.SetEnabled(false);
                break;
            }
        }

        if (IsCorrectLetter(chosenLetter))
        {
            information.value = $"Correct letter: {chosenLetter}";
            bool letterGuessed = false;

            var guessedWordArray = guessedWord.ToCharArray();
            for (int i = 0; i < targetWord.Length; i++)
            {
                if (RemoveAccents(targetWord[i].ToString().ToUpper()) == chosenLetter)
                {
                    guessedWordArray[i] = targetWord[i];
                    letterGuessed = true;
                    score++;
                }
            }

            if (letterGuessed)
            {
                guessedWord = new string(guessedWordArray);
                UpdateWordLabel();

                if (guessedWord.Equals(targetWord))
                {
                    information.value = "Félicitations ! Vous avez deviné le mot !";
                    UiManager.Instance.OnWin();
                    IsWon = true;
                    GameManager.Instance.gameWon++;
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
                UiManager.Instance.OnLose();
                IsWon = false;
            }
        }
    }

    private void RemoveWrongLetter()
    {
        var wrongLetterButtons = new List<Button>();

        foreach (var button in root.Query<Button>("AlphaButton").ToList())
        {
            var letter = RemoveAccents(button.text.ToUpper());
            if (!targetWord.Any(c => RemoveAccents(c.ToString().ToUpper()) == letter))
            {
                wrongLetterButtons.Add(button);
            }
        }

        if (wrongLetterButtons.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, wrongLetterButtons.Count);
            var buttonToDisable = wrongLetterButtons[randomIndex];
            buttonToDisable.SetEnabled(false);

            chosenLetters.Add(buttonToDisable.text.ToUpper());
        }
    }

    private void ShowCorrectLetter()
    {
        for (int i = 0; i < targetWord.Length; i++)
        {
            if (guessedWord[i] == '_')
            {
                guessedWord = guessedWord.Substring(0, i) + targetWord[i] + guessedWord.Substring(i + 1);
                UpdateWordLabel();
                break;
            }
        }
    }

    private IEnumerator GetWordDefinition()
    {
        string requestUrl = CATEGORIE + GameManager.Instance.CurrentCategory;

        using (UnityWebRequest request = UnityWebRequest.Get(requestUrl))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error: " + request.error);
                indiceText.value = "Erreur de récupération de la définition.";
            }
            else if (request.responseCode == 404)
            {
                Debug.LogError("Mot non trouvé dans le dictionnaire.");
                indiceText.value = "Définition non trouvée.";
            }
            else if (request.result == UnityWebRequest.Result.Success)
            {
                var response = request.downloadHandler.text;
                targetWord = ParseWord(response);

                guessedWord = new string('_', targetWord.Length);
                UpdateWordLabel();
            }
            else
            {
                Debug.LogError("Unexpected error: " + request.responseCode);
                indiceText.value = "Erreur inattendue.";
            }
        }
    }

    private string ParseWord(string json)
    {
        var jArray = JArray.Parse(json);
        foreach (var item in jArray)
        {
            return item["name"]?.ToString().ToUpper();
        }
        return null;
    }

    private void UpdateWordLabel()
    {
        wordLabel.text = guessedWord;
    }

    private void GoBack()
    {
        UiManager.Instance.GoBackToMenu();
    }

    private bool IsCorrectLetter(string letter)
    {
        return targetWord.Contains(letter);
    }

    private string RemoveAccents(string text)
    {
        return string.Concat(text.Normalize(NormalizationForm.FormD)
                                .Where(ch => char.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark))
                      .Normalize(NormalizationForm.FormC);
    }

    [System.Serializable]
    public class DictionaryApiResponse
    {
        public Meaning[] meanings;
    }

    [System.Serializable]
    public class Meaning
    {
        public Definition[] definitions;
    }

    [System.Serializable]
    public class Definition
    {
        public string definition;
    }

    [System.Serializable]
    public class CategoryWord
    {
        public string categorie;
        public string name;
    }

    [System.Serializable]
    public class CategoryResponse
    {
        public CategoryWord[] words; 
    }
}