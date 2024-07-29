using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using UnityEngine;
using UnityEngine.UIElements;

public class SignUp : MonoBehaviour
{
    [SerializeField]
    UIDocument uIDocument;

    VisualElement root;
    TextField username;
    TextField password;
    Button signUp;
    Button goBack;

    void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        username = root.Q<TextField>("UserText");
        password = root.Q<TextField>("PassText");
        signUp = root.Q<Button>("SignUp");
        goBack = root.Q<Button>("Return");

        signUp.clicked += Clickable_clicked;
        goBack.clicked += ReturnToAccountMenu;
    }

    private void Clickable_clicked()
    {
        Debug.Log("click");
        SignIn();
        AudioManager.Instance.PlayGameClickSound();
    }

    void ReturnToAccountMenu()
    {
        UiManager.Instance.ReturnToAccountMenu();
        AudioManager.Instance.PlayClickSound();
    }

    public void UpdateUserName(string userName)
    {
        string _username = username.text;
        _username = userName;
    }

    public void UpdatePassword(string passWord)
    {
        string _password = password.text;
        password.value = passWord;
    }

    public void SignIn() => PlayfabManager.Instance.SignIn(username.text, password.text);
}

