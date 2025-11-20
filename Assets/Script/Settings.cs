using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Hangman
{
    public class Settings : MonoBehaviour
    {
        [SerializeField] UiManager uiManager;
        public enum Difficulty { Easy, Medium, Hard }
        public static Difficulty currentDifficulty = Difficulty.Medium;

        [Header("Audio Mixers")]
        [SerializeField] private AudioMixer musicMixer;
        [SerializeField] private AudioMixer sfxMixer;

        [Header("Resolutions & Difficulties")]
        public List<(int width, int height)> resolutions = new List<(int, int)>
        {
            (1280, 720),
            (1920, 1080),
            (2048, 1556)
        };

        public List<Difficulty> difficulties = new List<Difficulty>
        {
            Difficulty.Easy,
            Difficulty.Medium,
            Difficulty.Hard
        };

        private void Awake()
        {
            // Appelle UiManager pour configurer l'UI des settings
            uiManager.SetupSettingsUI(this);
        }

        public void SetVolume(float volume)
        {
            float dB = Mathf.Log10(volume) * 20;
            musicMixer.SetFloat("Volume", dB);
        }

        public void SetSfxVolume(float volume)
        {
            float dB = Mathf.Log10(volume) * 20;
            sfxMixer.SetFloat("Volume", dB);
        }

        public void SetResolution(int width, int height)
        {
            Screen.SetResolution(width, height, FullScreenMode.FullScreenWindow);
            Debug.Log($"Resolution changed to: {width}x{height}");
        }

        public void SetFullScreen(bool isFullScreen)
        {
            Screen.fullScreen = isFullScreen;
            Debug.Log($"Fullscreen mode set to {isFullScreen}");
        }

        public void SetDifficulty(Difficulty difficulty)
        {
            currentDifficulty = difficulty;
            Debug.Log($"Difficulty changed to: {difficulty}");
        }
    }
}
