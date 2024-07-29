using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.UIElements;

public class CreateOrSign : MonoBehaviour
{
    [SerializeField]
    UIDocument uIDocument;
    VisualElement root;
    Button create;
    Button sign;

    private void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        create = root.Q<Button>("Create");
        sign = root.Q<Button>("Sign");

        List<Button> buttons = new List<Button> { create, sign };
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
