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

namespace Hangman.Pendu
{
    public class Pendu : MonoBehaviour
    {
        [SerializeField] private GameManager gameManager;
        [SerializeField] public UIDocument uIDocument;
        [SerializeField] private Image Img;

        private VisualElement root;
        VisualElement spriteContainer;
        private Label wordLabel;
        private TextField scoreText;
        public TextField indiceText;
        private TextField information;
        private Button goBack;
        private Button pause;
        private Button bonusRemoveWrongLetter;
        private Button bonusShowCorrectLetter;

        private List<string> chosenLetters;
        public string targetWord;
        private string guessedWord;
        public const string CATEGORIE = "https://trouve-mot.fr/api/categorie/";

        public bool IsWon;
        int scoreThreshold = 2; // Exemple : 5 points requis pour r�v�ler une lettre
        public int lifeMax;

        public List<Sprite> spritesList;

        private void OnEnable()
        {
            Reset();
        }

        public void Reset()
        {
            root = uIDocument.rootVisualElement;

            spriteContainer = root.Q<VisualElement>("GameContainer");
            wordLabel = root.Q<Label>("Word");
            scoreText = root.Q<TextField>("Score");
            indiceText = root.Q<TextField>("Indice");
            information = root.Q<TextField>("Information");
            goBack = root.Q<Button>("Return");
            pause = root.Q<Button>("PauseButton");
            bonusRemoveWrongLetter = root.Q<Button>("CancelLetter");
            bonusShowCorrectLetter = root.Q<Button>("AddLetter");

            chosenLetters = new List<string>();

            bonusRemoveWrongLetter.clicked += RemoveWrongLetter;
            bonusShowCorrectLetter.clicked += ShowCorrectLetter;
            pause.clicked += PauseClicked;
            goBack.clicked += GoBack;

            var alphaButtons = root.Query<Button>("AlphaButton").ToList();
            foreach (var button in alphaButtons)
            {
                var letter = button.text;
                button.clicked += () => OnLetterTouch(letter);
            }

            wordLabel.AddToClassList("word-label");
            guessedWord = new string('_', targetWord.Length);
            UpdateWordLabel();

            lifeMax = 10; // Nombre de vies bas� sur un nombre fixe pour les jeux de pendu
                          // Rechercher la d�finition du mot cible � l'aide de l'API de dictionnaire
            StartCoroutine(GetWordDefinition());
        }

        private void PauseClicked()
        {
            gameManager.Paused();
        }

        private void Update()
        {
            scoreText.value = $"{GameManager.Instance.score}";
        }

        public void OnLetterTouch(string chosenLetter)
        {
            AudioManager.Instance.PlayClickSound();
            chosenLetter = chosenLetter.ToUpper();

            if (chosenLetters.Contains(chosenLetter))
            {
                information.value = $"Vous avez d�j� choisi la lettre: {chosenLetter}";
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
                        GameManager.Instance.score++;
                    }
                }

                if (letterGuessed)
                {
                    guessedWord = new string(guessedWordArray);
                    UpdateWordLabel();

                    if (guessedWord.Equals(targetWord))
                    {
                        information.value = "F�licitations ! Vous avez devin� le mot !";
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
                UpdateHangmanSprite();

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
            AudioManager.Instance.PlayClickSound();
            var wrongLetterButtons = new List<Button>();

            if (GameManager.Instance.score < scoreThreshold)
            {
                information.value = $"Score insuffisant pour supprimer une lettre. Score actuel : {GameManager.Instance.score}";
                return;
            }

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

            GameManager.Instance.score -= scoreThreshold;
        }

        private void ShowCorrectLetter()
        {
            AudioManager.Instance.PlayClickSound();

            // V�rifier si le score atteint le seuil requis
            if (GameManager.Instance.score < scoreThreshold)
            {
                information.value = $"Score insuffisant pour r�v�ler une lettre. Score actuel : {GameManager.Instance.score}";
                return;
            }

            // V�rifier si le mot a d�j� �t� trouv�
            if (guessedWord.Equals(targetWord))
            {
                information.value = "Le mot est trouv� !";
                UiManager.Instance.OnWin();
                IsWon = true;
                GameManager.Instance.gameWon++;
                return;
            }

            // R�v�ler une lettre correcte
            for (int i = 0; i < targetWord.Length; i++)
            {
                if (guessedWord[i] == '_')
                {
                    guessedWord = guessedWord.Substring(0, i) + targetWord[i] + guessedWord.Substring(i + 1);
                    UpdateWordLabel();
                    break;
                }
            }

            // D�duire des points pour l'utilisation de la fonctionnalit�
            // Par exemple, d�duire 5 points pour utiliser cette fonctionnalit�
            GameManager.Instance.score -= scoreThreshold;
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
                    indiceText.value = "Erreur de r�cup�ration de la d�finition.";
                }
                else if (request.responseCode == 404)
                {
                    Debug.LogError("Mot non trouv� dans le dictionnaire.");
                    indiceText.value = "D�finition non trouv�e.";
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

        private void UpdateHangmanSprite()
        {
            Debug.Log($"spritesList is null: {spritesList == null}");
            Debug.Log($"spriteContainer is null: {spriteContainer == null}");

            if (spritesList == null || spritesList.Count == 0)
            {
                Debug.LogError("La liste de sprites du pendu est vide ou non d�finie.");
                return;
            }

            if (spriteContainer == null)
            {
                Debug.LogError("Le conteneur des sprites est non d�fini.");
                return;
            }

            // Clear existing sprites
            spriteContainer.Clear();

            int spriteIndex = Mathf.Max(0, spritesList.Count - lifeMax - 1);

            if (spriteIndex >= 0 && spriteIndex < spritesList.Count)
            {
                var image = new Image();
                image.sprite = spritesList[spriteIndex];
                spriteContainer.Add(image);
            }
            else
            {
                Debug.LogWarning("Index du sprite hors des limites.");
            }
        }

        private void GoBack()
        {
            AudioManager.Instance.PlayClickSound();
            UiManager.Instance.GoBackToMenu();
        }

        private bool IsCorrectLetter(string letter)
        {
            return targetWord.Contains(letter);
        }

        private string RemoveAccents(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            // Normaliser le texte en Forme D (d�compose les caract�res accentu�s)
            var normalizedText = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            // Parcourir chaque caract�re
            foreach (var ch in normalizedText)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(ch);

                // Ajouter le caract�re s'il n'est pas une marque diacritique
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(ch);
                }
            }

            // Re-normaliser en Forme C
            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
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
}