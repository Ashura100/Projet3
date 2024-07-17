using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;

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

    private void ContinueToPlay()
    {
        UiManager.Instance.GoBackToMenu();
        gameManager.Restart();
    }
}
