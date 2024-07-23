using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource musicPlayer;
    public AudioSource sfxPlayer;

    [SerializeField]
    private AudioClip clickSound, gameClickSound, themeSound, gameSound, winSound, gameOverSound;

    private void Awake()
    {

        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void PlayTheme()
    {
        musicPlayer.clip = themeSound;
        musicPlayer.Play();
    }

    public void PlayGameTheme()
    {
        musicPlayer.clip = gameSound;
        musicPlayer.Play();
    }

    public void PlayGameClickSound()
    {
        sfxPlayer.clip = gameClickSound;
        sfxPlayer.Play();
    }

    public void PlayClickSound()
    {
        sfxPlayer.clip = clickSound;
        sfxPlayer.Play();
    }

    public void PlayWinSound()
    {
        sfxPlayer.clip = winSound;
        sfxPlayer.Play();
    }

    public void PlayGameOverSound()
    {
        sfxPlayer.clip = gameOverSound;
        sfxPlayer.Play();
    }

    public void StopCurrentSound()
    {
        musicPlayer.Stop();
    }
}