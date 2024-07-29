using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    GameManager gameManager;
    [SerializeField]
    UIDocument uIDocument;
    VisualElement root;

    Button continueButton;
    Button settingsButton;
    Button goBackMenu;

    void OnEnable()
    {
        root = uIDocument.rootVisualElement;

        continueButton = root.Q<Button>("Continue");
        settingsButton = root.Q<Button>("Settings");
        goBackMenu = root.Q<Button>("ReturnMenu");

        continueButton.clicked += () => ContinueToPlay();
        goBackMenu.clicked += () => GoBack();

        List<Button> buttons = new List<Button> { settingsButton };
        foreach (var button in buttons)
        {
            button.clickable.clicked += () => OnButtonTouch(button);
        }
    }

    void ContinueToPlay()
    {
        UiManager.Instance.ChangeScreen(UiManager.Instance.currentScreen, UiManager.Instance.gameUi);
    }

    private void OnButtonTouch(Button button)
    {
        AudioManager.Instance.PlayGameClickSound();
        UiManager.Instance.OnButtonTouch(button);
    }

    void GoBack()
    {
        UiManager.Instance.GoBackToMenu();
    }
}

