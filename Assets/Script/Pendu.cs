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

namespace Hangman
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
        int scoreThreshold = 2; // Exemple : 5 points requis pour révéler une lettre
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

            var alphaButtons = root.Query<Button>("AlphaButton");
            foreach (var button in alphaButtons.ToList())
            {
                var letter = button.text;
                button.clicked += () => OnLetterTouch(letter);
            }

            wordLabel.AddToClassList("word-label");
            guessedWord = new string('_', targetWord.Length);
            UpdateWordLabel();

            lifeMax = 10; // Nombre de vies basé sur un nombre fixe pour les jeux de pendu
                          // Rechercher la définition du mot cible à l'aide de l'API de dictionnaire
            StartCoroutine(GetWordDefinition());
        }

        //fait appel à la fonction pause pendant le jeu
        private void PauseClicked()
        {
            gameManager.Paused();
        }

        //met à jour le score
        private void Update()
        {
            scoreText.value = $"{GameManager.Instance.score}";
        }

        //fonction principal du jeu
        public void OnLetterTouch(string chosenLetter)
        {
            AudioManager.Instance.PlayClickSound();//joue le son de click pour chacune des lettres
            chosenLetter = chosenLetter.ToUpper();

            if (chosenLetters.Contains(chosenLetter))
            {
                information.value = $"Vous avez déjà choisi la lettre: {chosenLetter}";//informe si la lettre est déjà choisi
                return;
            }

            chosenLetters.Add(chosenLetter);

            foreach (var button in root.Query<Button>("AlphaButton").ToList())
            {
                if (button.text == chosenLetter)
                {
                    button.SetEnabled(false);//désactive la lettre qui a déjà été utilisée
                    break;
                }
            }

            if (IsCorrectLetter(chosenLetter))
            {
                information.value = $"Correct letter: {chosenLetter}";//informe que la lettre est juste
                bool letterGuessed = false;

                var guessedWordArray = guessedWord.ToCharArray();
                for (int i = 0; i < targetWord.Length; i++)
                {
                    if (RemoveAccents(targetWord[i].ToString().ToUpper()) == chosenLetter)
                    {
                        guessedWordArray[i] = targetWord[i];
                        letterGuessed = true;
                        GameManager.Instance.score++; //ajoute des points et les sauvegardes si la lettre est juste
                    }
                }

                if (letterGuessed)
                {
                    guessedWord = new string(guessedWordArray);
                    UpdateWordLabel();

                    if (guessedWord.Equals(targetWord))
                    {
                        information.value = "Félicitations ! Vous avez deviné le mot !";//informe de la victoire
                        UiManager.Instance.OnWin();//lance UI victoire
                        IsWon = true;
                        GameManager.Instance.gameWon++;//prend en compte la partie gagnée pour ramener à l'UI de jeu
                    }
                }
            }
            else
            {
                information.value = $"Incorrect letter: {chosenLetter}";//informe que la lettre est mauvaise
                lifeMax--;//diminue les vie
                UpdateHangmanSprite();//fonction appelé pour mettre à jour les sprite

                if (lifeMax <= 0)
                {
                    information.value = "Game Over ! Vous avez perdu.";//partie perdu
                    UiManager.Instance.OnLose();//active l'écran de défaite
                    IsWon = false;
                }
            }
        }

        //bonus qui désactive les mauvaise lettres, activiable seulement si on a un score supérieur à 2 et diminue le score de 2 en 2
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

        //bonus donnant des lettres du mots à trouver , activiable seulement si on a un score supérieur à 2 et diminue le score de 2 en 2
        private void ShowCorrectLetter()
        {
            AudioManager.Instance.PlayClickSound();

            // Vérifier si le score atteint le seuil requis
            if (GameManager.Instance.score < scoreThreshold)
            {
                information.value = $"Score insuffisant pour révéler une lettre. Score actuel : {GameManager.Instance.score}";
                return;
            }

            // Vérifier si le mot a déjà été trouvé
            if (guessedWord.Equals(targetWord))
            {
                information.value = "Le mot est trouvé !";
                UiManager.Instance.OnWin();
                IsWon = true;
                GameManager.Instance.gameWon++;
                return;
            }

            // Révéler une lettre correcte
            for (int i = 0; i < targetWord.Length; i++)
            {
                if (guessedWord[i] == '_')
                {
                    guessedWord = guessedWord.Substring(0, i) + targetWord[i] + guessedWord.Substring(i + 1);
                    UpdateWordLabel();
                    break;
                }
            }

            // Déduire des points pour l'utilisation de la fonctionnalité
            // Par exemple, déduire 5 points pour utiliser cette fonctionnalité
            GameManager.Instance.score -= scoreThreshold;
        }

        //coroutine qui défini le mot à trouver en fonction de la catégorie choisi
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

        //fonction qui met à jour les lettres visible dans le label du mot à trouver
        private void UpdateWordLabel()
        {
            wordLabel.text = guessedWord;
        }

        //fonction qui met à jour les sprites en fonction des erreurs et nombre de vie
        private void UpdateHangmanSprite()
        {
            Debug.Log($"spritesList is null: {spritesList == null}");
            Debug.Log($"spriteContainer is null: {spriteContainer == null}");

            if (spritesList == null || spritesList.Count == 0)
            {
                Debug.LogError("La liste de sprites du pendu est vide ou non définie.");
                return;
            }

            if (spriteContainer == null)
            {
                Debug.LogError("Le conteneur des sprites est non défini.");
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

        //bouton retour menu
        private void GoBack()
        {
            AudioManager.Instance.PlayClickSound();
            UiManager.Instance.GoBackToMenu();
        }

        private bool IsCorrectLetter(string letter)
        {
            return targetWord.Contains(letter);
        }

        //fonction qui transforme les lettres accentués en lettre
        private string RemoveAccents(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                return text;
            }

            // Normaliser le texte en Forme D (décompose les caractères accentués)
            var normalizedText = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            // Parcourir chaque caractère
            foreach (var ch in normalizedText)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(ch);

                // Ajouter le caractère s'il n'est pas une marque diacritique
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