using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource audioPlayer;

    [SerializeField]
    private AudioClip clickSound,gameClickSound, themeSound, winSound, gameOverSound;

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
        audioPlayer.clip = themeSound;
        audioPlayer.Play();
    }

    public void PlayGameClickSound()
    {
        audioPlayer.clip = gameClickSound;
        audioPlayer.Play();
    }
    public void PlayClickSound()
    {
        audioPlayer.clip = clickSound;
        audioPlayer.Play();
    }

    public void PlayWinSound()
    {
        audioPlayer.clip = winSound;
        audioPlayer.Play();
    }

    public void PlayGameOverSound()
    {
        audioPlayer.clip = gameOverSound;
        audioPlayer.Play();
    }
}