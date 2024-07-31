using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Hangman
{
    public class Lose : MonoBehaviour
    {
        [SerializeField]
        GameManager gameManager;
        [SerializeField]
        UIDocument uIDocument;
        VisualElement root;

        Button returnButton;

        void Awake()
        {
            root = uIDocument.rootVisualElement;

            returnButton = root.Q<Button>("Return");

            returnButton.clickable.clicked += ReturnToMenu;
        }

        //joue le son de défaite
        private void Start()
        {
            AudioManager.Instance.PlayGameOverSound();
        }

        //retour menu
        private void ReturnToMenu()
        {
            AudioManager.Instance.PlayClickSound();
            gameManager.Restart();
        }
    }
}
