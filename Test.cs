using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using System;
using UnityEngine.UIElements;
using Newtonsoft.Json.Linq;
using System.Globalization;
using System.Text;

public class Test : MonoBehaviour
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

    public string targetWord;
    private string guessedWord;
    public const string CATEGORIE = "https://trouve-mot.fr/api/categorie/";

    public bool IsWon;
    public int score;
    public int lifeMax;

    public static String RemoveDiacritics(this String s)
    {
        String normalizedString = s.Normalize(NormalizationForm.FormD);
        StringBuilder stringBuilder = new StringBuilder();

        for (int i = 0; i < normalizedString.Length; i++)
        {
            Char c = normalizedString[i];
            if (CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
            {
                stringBuilder.Append(c);
            }
        }

        return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
    }

    void Awake()
    {
        root = uIDocument.rootVisualElement;

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
        goBack.clicked += GoBack;

        guessedWord = new string('_', targetWord.Length);
        UpdateWordLabel();

        lifeMax = 6; // Nombre de vies basé sur un nombre fixe pour les jeux de pendu
        score = 0; // Initialiser le score à 0

        // Rechercher la définition du mot cible à l'aide de l'API de dictionnaire
        StartCoroutine(GetWordDefinition());
    }

    public Test(GameManager gameManager)
    {
        this.gameManager = gameManager;
    }

    void Start()
    {

    }

    void Update()
    {

    }

    void OnLetterTouch(string chosenLetter)
    {
        chosenLetter = chosenLetter.ToUpper();

        if (IsCorrectLetter(chosenLetter))
        {
            information.value = $"Correct letter: {chosenLetter}";

            char[] guessedWordArray = guessedWord.ToCharArray();
            bool letterGuessed = false;

            for (int i = 0; i < targetWord.Length; i++)
            {
                if (char.ToUpper(targetWord[i]) == chosenLetter[0])
                {
                    guessedWordArray[i] = chosenLetter[0];
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
                    gameManager.Ui.OnWin();
                    IsWon = true;
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
        }
    }

    void RemoveWrongLetter()
    {
        char[] guessedWordArray = guessedWord.ToCharArray();
        for (int i = 0; i < guessedWordArray.Length; i++)
        {
            if (guessedWordArray[i] == '_')
            {
                List<Button> alphaButtons = root.Query<Button>("AlphaButton").ToList();
                foreach (Button button in alphaButtons)
                {
                    if (button.text == targetWord[i].ToString())
                    {
                        button.style.color = Color.green;
                    }
                }

                guessedWordArray[i] = targetWord[i];
                guessedWord = new string(guessedWordArray);
                UpdateWordLabel();
                break;
            }
        }
    }

    void ShowCorrectLetter()
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

    IEnumerator GetWordDefinition()
    {
        string requestUrl = CATEGORIE + "10";

        Debug.Log("Sending request to: " + requestUrl);

        using (UnityWebRequest request = UnityWebRequest.Get(requestUrl))
        {
            yield return request.SendWebRequest();

            Debug.Log("Request completed with status: " + request.result);
            Debug.Log("Response code: " + request.responseCode);
            Debug.Log("Response headers: " + request.GetResponseHeaders());

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
                Debug.Log("Response: " + request.downloadHandler.text);
                var response = request.downloadHandler.text;
                targetWord = ParseWord(response);

                // Initialiser guessedWord après avoir défini targetWord
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

    string ParseWord(string json)
    {
        JArray jArray = JArray.Parse(json);
        foreach (JObject item in jArray)
        {
            return item.GetValue("name").ToString().ToUpper();
        }
        return null;
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

    void UpdateWordLabel()
    {
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
        return targetWord.Contains(letter);
    }
}