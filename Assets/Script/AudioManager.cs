using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    private AudioSource audioPlayer;

    [SerializeField]
    private AudioClip clickSound, winSound, gameOverSound;

    private void Awake()
    {
        audioPlayer = GetComponent<AudioSource>();

        if (Instance != null)
            Destroy(gameObject);
        else
            Instance = this;

        DontDestroyOnLoad(gameObject);
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