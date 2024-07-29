using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.UIElements;

public class StartMenu : MonoBehaviour
{
    [SerializeField]
    GameManager gameManager;
    [SerializeField]
    UIDocument uIDocument;

    VisualElement root;
    VisualElement startMenu;
    Button screen;
    Button settings;
    Button category;
    Button classement;

    void OnEnable()
    {
        root = uIDocument.rootVisualElement;

        startMenu = root.Q<VisualElement>("StartMenu");
        screen = root.Q<Button>("TouchToPlay");
        settings = root.Q<Button>("Settings");
        category = root.Q<Button>("Category");
        classement = root.Q<Button>("Classement");

        List<Button> buttons = new List<Button> { screen, settings, category, classement };
        foreach (var button in buttons)
        {
            button.clickable.clicked += () => OnButtonTouch(button);
        }
    }

    private void OnButtonTouch(Button button)
    {
        AudioManager.Instance.PlayGameClickSound();
        UiManager.Instance.OnButtonTouch(button);
    }
}
