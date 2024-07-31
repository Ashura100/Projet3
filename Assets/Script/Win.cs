using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Hangman
{
    public class Win : MonoBehaviour
    {
        [SerializeField]
        GameManager gameManager;
        [SerializeField]
        UIDocument uIDocument;
        VisualElement root;

        Button continueButton;

        void Awake()
        {
            root = uIDocument.rootVisualElement;

            continueButton = root.Q<Button>("Continue");

            continueButton.clickable.clicked += ContinueToPlay;
        }

        //joue le son de victoire
        private void Start()
        {
            AudioManager.Instance.PlayWinSound();
        }

        //continuer de jouer
        private void ContinueToPlay()
        {
            AudioManager.Instance.PlayClickSound();
            gameManager.Restart();
        }
    }

}
