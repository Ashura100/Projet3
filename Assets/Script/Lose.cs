using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

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

    private void ReturnToMenu()
    {
        gameManager.Ui.GoBackToMenu();
    }
}
