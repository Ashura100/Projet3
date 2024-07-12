using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using System;
using UnityEngine.UIElements;

public class test : MonoBehaviour
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
    public const string DICTIONARY_API_URL = "https://api.dictionaryapi.dev/api/v2/entries/en/";

    public bool IsWon;
    public int score;
    public int lifeMax;

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

        lifeMax = 6; // Nombre de vies bas� sur un nombre fixe pour les jeux de pendu
        score = 0; // Initialiser le score � 0

        // Rechercher la d�finition du mot cible � l'aide de l'API de dictionnaire
        StartCoroutine(GetWordDefinition(targetWord));
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
                    information.value = "F�licitations ! Vous avez devin� le mot !";
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

    IEnumerator GetWordDefinition(string word)
    {
        string formattedWord = word.ToLower(); // Formater le mot en minuscules pour l'API
        string requestUrl = DICTIONARY_API_URL + formattedWord;

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
                indiceText.value = "Erreur de r�cup�ration de la d�finition.";
            }
            else if (request.responseCode == 404)
            {
                Debug.LogError("Mot non trouv� dans le dictionnaire.");
                indiceText.value = "D�finition non trouv�e.";
            }
            else if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Response: " + request.downloadHandler.text);
                var response = request.downloadHandler.text;
                var definition = ParseDefinition(response);
                indiceText.value = definition;
            }
            else
            {
                Debug.LogError("Unexpected error: " + request.responseCode);
                indiceText.value = "Erreur inattendue.";
            }
        }
    }

    string ParseDefinition(string json)
    {
        Debug.Log("Parsing response: " + json);

        // Parsing simple, bas� sur la structure JSON de l'API de dictionnaire
        var jsonResponse = JsonUtility.FromJson<DictionaryApiResponse[]>(json);
        if (jsonResponse != null && jsonResponse.Length > 0)
        {
            var meanings = jsonResponse[0].meanings;
            if (meanings != null && meanings.Length > 0)
            {
                return meanings[0].definitions[0].definition;
            }
        }
        return "D�finition non trouv�e.";
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