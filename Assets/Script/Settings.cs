using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Device;
using UnityEngine.UIElements;

namespace Hangman   
{
    public class Settings : MonoBehaviour
    {
        public static Settings Instance;
        public enum Difficulty
        {
            Easy,
            Medium,
            Hard
        }

        [SerializeField]
        AudioManager audioManager;
        [SerializeField]
        GameManager gameManager;
        [SerializeField]
        UIDocument uIDocument;
        VisualElement root;

        Button goBack;
        Slider slider;
        Slider sfxSlider;
        Toggle toggle;

        private List<Button> resolutionButtons;
        private List<(int width, int height)> resolutions = new List<(int, int)>
        {
        (1280, 720),
        (1920, 1080),
        (2048, 1556)
        };

        private List<Button> difficultyButtons;
        private List<Difficulty> difficulties = new List<Difficulty>
        {
        Difficulty.Easy,
        Difficulty.Medium,
        Difficulty.Hard
        };

        public AudioMixer audioMixer;
        public AudioMixer sfxAudioMixer;

        public static Difficulty currentDifficulty = Difficulty.Medium;

        void Awake()
        {
            if (Instance != null)
                Destroy(gameObject);
            else
                Instance = this;

            DontDestroyOnLoad(gameObject);
        }

        private void OnEnable()
        {
            root = GetComponent<UIDocument>().rootVisualElement;
            slider = root.Q<Slider>("Slider");
            sfxSlider = root.Q<Slider>("Sfx");
            toggle = root.Q<Toggle>("FullScreenT");
            goBack = root.Q<Button>("Return");

            resolutionButtons = root.Query<Button>("ButtonRes").ToList();
            for (int i = 0; i < resolutionButtons.Count; i++)
            {
                int index = i;
                resolutionButtons[i].clicked += () => SetQuality(resolutions[index].width, resolutions[index].height);
            }

            difficultyButtons = root.Query<Button>("ButtonDiff").ToList();
            for (int i = 0; i < difficultyButtons.Count; i++)
            {
                int index = i;
                difficultyButtons[i].clicked += () => SetDifficulty(difficulties[index]);
            }

            goBack.clickable.clicked += GoBack;

            slider.RegisterValueChangedCallback(evt => SetVolume(evt.newValue));

            sfxSlider.RegisterValueChangedCallback(evt => SetSfxVolume(evt.newValue));

            toggle.RegisterValueChangedCallback(evt => SetFullScreen(evt.newValue));

            toggle.value = UnityEngine.Screen.fullScreen;
        }

        //change la difficulté (n'est pas encore en place)
        void OnChangeDifficulty(EventBase eventBase)
        {
            Button button = (Button)eventBase.target;
            SetDifficulty(0);
        }

        //change le volume des musiques
        public void SetVolume(float volume)
        {
            float dB = Mathf.Log10(volume) * 20;
            audioMixer.SetFloat("Volume", dB);
        }

        //change le volume des sfx
        public void SetSfxVolume(float volume)
        {
            float dB = Mathf.Log10(volume) * 20;
            sfxAudioMixer.SetFloat("Volume", dB);
        }

        //change la qualité de l'iamge
        public void SetQuality(int width, int height)
        {
            UnityEngine.Screen.SetResolution(width, height, FullScreenMode.FullScreenWindow);
            Debug.Log($"Resolution changed to: {width}x{height}");
        }

        //met l'écran sans fenêtre seulement pour la version pc
        public void SetFullScreen(bool isFullScreen)
        {
            UnityEngine.Screen.fullScreen = isFullScreen;
            Debug.Log($"Fullscreen mode set to {isFullScreen}");
        }

        public void SetDifficulty(Difficulty newDifficulty)
        {
            currentDifficulty = newDifficulty;
            Debug.Log($"Difficulty changed to: {newDifficulty}");
        }

        //retour menu
        void GoBack()
        {
            UiManager.Instance.GoBackToMenu();
        }
    }

}