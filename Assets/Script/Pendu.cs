using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System.Globalization;
using System.Linq;
using System.Text;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;

namespace Hangman
{
    public class Pendu : MonoBehaviour
    {
        public static Pendu Instance;

        [SerializeField] UiManager uiManager;

        [Header("Game Data")]
        public string targetWord;
        private string guessedWord;
        public List<Sprite> spritesList;

        private List<string> chosenLetters = new List<string>();
        private List<char> wrongLetters = new List<char>();

        public int lifeMax = 10;
        private int errors = 0;

        public bool IsWon = false;

        public const string CATEGORIE = "https://trouve-mot.fr/api/categorie/";

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        private void Start()
        {
            ResetGame();
        }



        // ------------------------------------------------------------
        // ---------------------- GAME RESET --------------------------
        // ------------------------------------------------------------
        public void ResetGame()
        {
            chosenLetters.Clear();
            wrongLetters.Clear();
            errors = 0;
            IsWon = false;

            StartCoroutine(GetWordDefinition());
        }

        // ------------------------------------------------------------
        // ------------------- GET WORD FROM API -----------------------
        // ------------------------------------------------------------
        private IEnumerator GetWordDefinition()
        {
            string url = CATEGORIE + GameManager.Instance.CurrentCategory;

            using (UnityWebRequest request = UnityWebRequest.Get(url))
            {
                yield return request.SendWebRequest();

                if (request.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError("API Error: " + request.error);
                    uiManager.UpdateWord("Erreur API");
                    yield break;
                }

                var json = request.downloadHandler.text;
                targetWord = ParseWord(json).ToUpper();

                guessedWord = new string('_', targetWord.Length);

                uiManager.UpdateWord(GetDisplayWord());
                uiManager.UpdateWrongLetters("");
                uiManager.UpdateHangmanImage(0);
            }
        }

        private string ParseWord(string json)
        {
            var jArray = JArray.Parse(json);
            foreach (var item in jArray)
                return item["name"]?.ToString();

            return "ERREUR";
        }

        // ------------------------------------------------------------
        // ------------------- PUBLIC GETTERS --------------------------
        // ------------------------------------------------------------
        public string GetDisplayWord()
        {
            return string.Join(" ", guessedWord.ToCharArray());
        }

        public string GetWrongLetters()
        {
            return string.Join(" ", wrongLetters);
        }

        public int GetErrorCount() => errors;

        public Sprite GetHangmanSprite(int errorIndex)
        {
            int index = Mathf.Clamp(errorIndex, 0, spritesList.Count - 1);
            return spritesList[index];
        }

        // ------------------------------------------------------------
        // ---------------- LETTER TOUCH MAIN LOGIC --------------------
        // ------------------------------------------------------------
        public void OnLetterTouch(string letter)
        {
            letter = RemoveAccents(letter.ToUpper());

            if (chosenLetters.Contains(letter))
                return;

            chosenLetters.Add(letter);

            if (IsCorrectLetter(letter))
                RevealLetter(letter);
            else
                WrongLetter(letter);

            RefreshUI();
        }

        // ------------------------------------------------------------
        // ---------------------- LETTER CHECK -------------------------
        // ------------------------------------------------------------
        private bool IsCorrectLetter(string letter)
        {
            return targetWord.Any(c => RemoveAccents(c.ToString().ToUpper()) == letter);
        }

        // ------------------------------------------------------------
        // ---------------------- CORRECT LETTER -----------------------
        // ------------------------------------------------------------
        private void RevealLetter(string letter)
        {
            var chars = guessedWord.ToCharArray();

            for (int i = 0; i < targetWord.Length; i++)
            {
                if (RemoveAccents(targetWord[i].ToString().ToUpper()) == letter)
                {
                    chars[i] = targetWord[i];
                    GameManager.Instance.score++;
                }
            }

            guessedWord = new string(chars);

            if (guessedWord == targetWord)
            {
                IsWon = true;
                GameManager.Instance.SwitchScreen(ScreenType.WinUI);
                GameManager.Instance.gameWon++;
            }
        }

        // ------------------------------------------------------------
        // ------------------------ WRONG LETTER -----------------------
        // ------------------------------------------------------------
        private void WrongLetter(string letter)
        {
            wrongLetters.Add(letter[0]);
            errors++;
            lifeMax--;

            if (lifeMax <= 0)
            {
                IsWon = false;
                GameManager.Instance.SwitchScreen(ScreenType.LoseUI);
            }
        }

        // ------------------------------------------------------------
        // ----------------------- BONUS: REMOVE WRONG -----------------
        // ------------------------------------------------------------
        public void RemoveWrongLetter()
        {
            if (GameManager.Instance.score < 2)
                return;

            var wrongButtons = chosenLetters
                .Where(l => !IsCorrectLetter(l))
                .ToList();

            if (wrongButtons.Count == 0)
                return;

            string chosen = wrongButtons[Random.Range(0, wrongButtons.Count)];

            wrongLetters.Remove(chosen[0]);
            GameManager.Instance.score -= 2;

            RefreshUI();
        }

        // ------------------------------------------------------------
        // ----------------------- BONUS: SHOW CORRECT -----------------
        // ------------------------------------------------------------
        public void ShowCorrectLetter()
        {
            if (GameManager.Instance.score < 2)
                return;

            for (int i = 0; i < targetWord.Length; i++)
            {
                if (guessedWord[i] == '_')
                {
                    guessedWord = guessedWord.Substring(0, i) +
                                  targetWord[i] +
                                  guessedWord.Substring(i + 1);

                    break;
                }
            }

            GameManager.Instance.score -= 2;
            RefreshUI();
        }

        // ------------------------------------------------------------
        // ---------------------- REFRESH UI ---------------------------
        // ------------------------------------------------------------
        private void RefreshUI()
        {
            uiManager.UpdateWord(GetDisplayWord());
            uiManager.UpdateWrongLetters(GetWrongLetters());
            uiManager.UpdateHangmanImage(errors);
        }

        // ------------------------------------------------------------
        // -------------------- REMOVE ACCENTS -------------------------
        // ------------------------------------------------------------
        private string RemoveAccents(string text)
        {
            var norm = text.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();

            foreach (var c in norm)
            {
                var cat = CharUnicodeInfo.GetUnicodeCategory(c);
                if (cat != UnicodeCategory.NonSpacingMark)
                    sb.Append(c);
            }

            return sb.ToString().Normalize(NormalizationForm.FormC);
        }
    }
}
