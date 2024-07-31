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

        //joue le thème de l'ui de création de compte
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

        //joue le thème du jeu
        public void PlayGameTheme()
        {
            musicPlayer.clip = gameSound;
            musicPlayer.Play();
        }

        //joue le son de click pour avancée dans les Ui
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

        //joue le son de défaite
        public void PlayGameOverSound()
        {
            sfxPlayer.clip = gameOverSound;
            sfxPlayer.Play();
        }

        //arrête les sons
        public void StopCurrentSound()
        {
            musicPlayer.Stop();
        }
    }
}