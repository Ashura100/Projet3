using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Hangman
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance;

        public AudioSource musicPlayer;
        public AudioSource sfxPlayer;

        [SerializeField]
        private AudioClip clickSound, gameClickSound, createAccountSound, themeSound, gameSound, winSound, gameOverSound;

        private void Awake()
        {

            if (Instance != null)
                Destroy(gameObject);
            else
                Instance = this;

            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// 
        /// </summary>

        //joue le th�me de l'ui de cr�ation de compte
        public void CreateAccountTheme()
        {
            musicPlayer.clip = createAccountSound;
            musicPlayer.Play();
        }

        //joue le theme de l'ui du menu start
        public void PlayTheme()
        {
            musicPlayer.clip = themeSound;
            musicPlayer.Play();
        }

        //joue le th�me du jeu
        public void PlayGameTheme()
        {
            musicPlayer.clip = gameSound;
            musicPlayer.Play();
        }

        //joue le son de click pour avanc�e dans les Ui
        public void PlayGameClickSound()
        {
            sfxPlayer.clip = gameClickSound;
            sfxPlayer.Play();
        }

        //joue les son click mineur
        public void PlayClickSound()
        {
            sfxPlayer.clip = clickSound;
            sfxPlayer.Play();
        }

        //joue le son de victoire
        public void PlayWinSound()
        {
            sfxPlayer.clip = winSound;
            sfxPlayer.Play();
        }

        //joue le son de d�faite
        public void PlayGameOverSound()
        {
            sfxPlayer.clip = gameOverSound;
            sfxPlayer.Play();
        }

        //arr�te les sons
        public void StopCurrentSound()
        {
            musicPlayer.Stop();
        }
    }
}