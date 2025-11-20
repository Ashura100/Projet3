using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Hangman
{
    public class UiManager : MonoBehaviour
    {
        [SerializeField] Category category;

        /*[SerializeField] UIDocument createOrSignUi;
        [SerializeField] UIDocument createAccountUi;
        [SerializeField] UIDocument signUpUi;*/
        [SerializeField] UIDocument startUi;
        [SerializeField] UIDocument gameUi;
        [SerializeField] UIDocument settingsUi;
        [SerializeField] UIDocument categoryUi;
        [SerializeField] UIDocument classUi;
        [SerializeField] UIDocument winUi;
        [SerializeField] UIDocument loseUi;
        [SerializeField] UIDocument pauseUi;

        public VisualElement createOrSignUiRoot { get; private set; }
        public VisualElement createAccountUiRoot { get; private set; }
        public VisualElement signUpUiRoot { get; private set; }
        public VisualElement startUiRoot { get; private set; }
        public VisualElement gameUiRoot { get; private set; }
        public VisualElement settingsUiRoot { get; private set; }
        public VisualElement categoryUiRoot { get; private set; }
        public VisualElement classUiRoot { get; private set; }
        public VisualElement winUiRoot { get; private set; }
        public VisualElement loseUiRoot { get; private set; }
        public VisualElement pauseUiRoot { get; private set; }

        VisualElement hangmanImage;

        Label wordLabel, wrongLettersLabel;

        TextField username, emailAdress, password;

        Button createAccountButton, signUpButton;

        Slider musicSlider, sfxSlider;
        Toggle fullscreenToggle;

        private void Awake()
        {
            startUiRoot = startUi.rootVisualElement;
            /*createAccountUiRoot = createAccountUi.rootVisualElement;
            createOrSignUiRoot = createOrSignUi.rootVisualElement;
            signUpUiRoot = signUpUi.rootVisualElement;*/
            gameUiRoot = gameUi.rootVisualElement;
            settingsUiRoot = settingsUi.rootVisualElement;
            categoryUiRoot = categoryUi.rootVisualElement;
            classUiRoot = classUi.rootVisualElement;
            winUiRoot = winUi.rootVisualElement;
            loseUiRoot = loseUi.rootVisualElement;
            pauseUiRoot = pauseUi.rootVisualElement;
        }
        // Start is called before the first frame update
        void Start()
        {
            SetupButtons();
        }

        public void SetupButtons()
        {
            startUiRoot.Q<Button>("TouchToPlay").clicked += () =>
            {
                AudioManager.Instance.PlayClickSound();
                GameManager.Instance.SwitchScreen(ScreenType.GameUI);
                Pendu.Instance.ResetGame();
            };

            startUiRoot.Q<Button>("Category").clicked += () =>
            {
                AudioManager.Instance.PlayClickSound();
                GameManager.Instance.SwitchScreen(ScreenType.CategoryUI);
            };

            startUiRoot.Q<Button>("Settings").clicked += () =>
            {
                AudioManager.Instance.PlayClickSound();
                GameManager.Instance.SwitchScreen(ScreenType.SettingsUI);
            };

            startUiRoot.Q<Button>("Classement").clicked += () =>
            {
                AudioManager.Instance.PlayClickSound();
                GameManager.Instance.SwitchScreen(ScreenType.GameUI);
            };

            // ---------- BOUTON RETOUR ----------
            gameUiRoot.Q<Button>("Return").clicked += () =>
            {
                AudioManager.Instance.PlayClickSound();
                GameManager.Instance.SwitchScreen(ScreenType.StartUI);
            };

            // ---------- BOUTON PAUSE ----------
            gameUiRoot.Q<Button>("PauseButton").clicked += () =>
            {
                AudioManager.Instance.PlayClickSound();
                GameManager.Instance.Paused();
                GameManager.Instance.SwitchScreen(ScreenType.PauseUI);
            };

            // ---------- BOUTONS BONUS ----------
            gameUiRoot.Q<Button>("CancelLetter").clicked += () =>
            {
                AudioManager.Instance.PlayClickSound();
                Pendu.Instance.RemoveWrongLetter();
            };

            gameUiRoot.Q<Button>("AddLetter").clicked += () =>
            {
                AudioManager.Instance.PlayClickSound();
                Pendu.Instance.ShowCorrectLetter();
            };

            // ---------- BOUTONS ALPHABET ----------
            var alphaButtons = gameUiRoot.Query<Button>("AlphaButton");
            foreach (var button in alphaButtons.ToList())
            {
                string letter = button.text;
                button.clicked += () =>
                {
                    AudioManager.Instance.PlayClickSound();
                    Pendu.Instance.OnLetterTouch(letter);
                };
            }

            // Bouton retour
            categoryUiRoot.Q<Button>("Return").clicked += () =>
            {
                AudioManager.Instance.PlayClickSound();
                GameManager.Instance.SwitchScreen(ScreenType.StartUI);
            };

            // Boutons de catégories
            categoryUiRoot.Q<Button>("CorpsButton").clicked += () => category.SelectCategory("6");
            categoryUiRoot.Q<Button>("ArtButton").clicked += () => category.SelectCategory("10");
            categoryUiRoot.Q<Button>("AnimalButton").clicked += () => category.SelectCategory("19");
            categoryUiRoot.Q<Button>("ArmyButton").clicked += () => category.SelectCategory("26");

            settingsUiRoot.Q<Button>("Return").clicked += () =>
            {
                AudioManager.Instance.PlayClickSound();
                GameManager.Instance.SwitchScreen(ScreenType.StartUI);
            };

            /*createAccountUiRoot.Q<Button>("Return").clicked += () => GameManager.Instance.SwitchScreen(ScreenType.CreateOrSignUI) ;

            createOrSignUiRoot.Q<Button>("Create").clicked += () =>
            {
                AudioManager.Instance.PlayGameClickSound();
                GameManager.Instance.SwitchScreen(ScreenType.CreateAccountUI);
            };

            createOrSignUiRoot.Q<Button>("Sign").clicked += () =>
            {
                AudioManager.Instance.PlayGameClickSound();
                GameManager.Instance.SwitchScreen(ScreenType.SignUpUI);
            };*/

            winUiRoot.Q<Button>("Continue").clicked += () =>
            {
                AudioManager.Instance.PlayClickSound();
                GameManager.Instance.SwitchScreen(ScreenType.GameUI);
                Pendu.Instance.ResetGame();
            };

            loseUiRoot.Q<Button>("Return").clicked += () =>
            {
                AudioManager.Instance.PlayClickSound();
                GameManager.Instance.SwitchScreen(ScreenType.StartUI);
            };
        }

        public void UpdateWord(string content)
        {
            wordLabel = gameUiRoot.Q<Label>("Word");

            if (wordLabel != null)
                wordLabel.text = content;
        }

        public void UpdateWrongLetters(string content)
        {
            wrongLettersLabel = gameUiRoot.Q<Label>("WrongLetters");

            if (wrongLettersLabel != null)
                wrongLettersLabel.text = "Fautes : " + content;
        }

        public void UpdateHangmanImage(int errors)
        {
            hangmanImage = gameUiRoot.Q<VisualElement>("GameContainer");

            hangmanImage.style.backgroundImage =
                new StyleBackground(Pendu.Instance.GetHangmanSprite(errors));
        }

        public void SetupSettingsUI(Settings settings)
        {
            musicSlider = settingsUiRoot.Q<Slider>("Slider");
            sfxSlider = settingsUiRoot.Q<Slider>("Sfx");
            fullscreenToggle = settingsUiRoot.Q<Toggle>("FullScreenT");

            // Résolutions
            var resolutionButtons = settingsUiRoot.Query<Button>("ButtonRes").ToList();
            for (int i = 0; i < resolutionButtons.Count; i++)
            {
                int index = i;
                resolutionButtons[i].clicked += () =>
                    settings.SetResolution(settings.resolutions[index].width, settings.resolutions[index].height);
            }

            // Difficultés
            var difficultyButtons = settingsUiRoot.Query<Button>("ButtonDiff").ToList();
            for (int i = 0; i < difficultyButtons.Count; i++)
            {
                int index = i;
                difficultyButtons[i].clicked += () => settings.SetDifficulty(settings.difficulties[index]);
            }

            // Sliders et toggle
            musicSlider.RegisterValueChangedCallback(evt => settings.SetVolume(evt.newValue));
            sfxSlider.RegisterValueChangedCallback(evt => settings.SetSfxVolume(evt.newValue));
            fullscreenToggle.RegisterValueChangedCallback(evt => settings.SetFullScreen(evt.newValue));

            fullscreenToggle.value = Screen.fullScreen;
        }

        public void SetupCreateAccountUI()
        {
            username = createAccountUiRoot.Q<TextField>("UserText");
            emailAdress = createAccountUiRoot.Q<TextField>("MailText");
            password = createAccountUiRoot.Q<TextField>("PassText");
            createAccountButton = createAccountUiRoot.Q<Button>("CreateAccount");

            createAccountButton.clicked += OnCreateAccountClicked;
        }

        public void SetupSignUpUI()
        {
            username = signUpUiRoot.Q<TextField>("UserText");
            password = signUpUiRoot.Q<TextField>("PassText");
            signUpButton = signUpUiRoot.Q<Button>("SignUp");

            signUpButton.clicked += OnSignUpClicked;
        }

        private void OnCreateAccountClicked()
        {
            AudioManager.Instance.PlayGameClickSound();
            PlayfabManager.Instance.CreateAccount(
                username.text,
                emailAdress.text,
                password.text
            );
        }

        private void OnSignUpClicked()
        {
            AudioManager.Instance.PlayGameClickSound();
            PlayfabManager.Instance.SignIn(username.text, password.text);
        }

        // --- Fonctions utiles pour mettre à jour les champs ---
        public void UpdateCreateAccountFields(string _username, string _email, string _password)
        {
            username.value = _username;
            emailAdress.value = _email;
            password.value = _password;
        }

        public void UpdateSignUpFields(string _username, string _password)
        {
            username.value = _username;
            password.value = _password;
        }
    }
}