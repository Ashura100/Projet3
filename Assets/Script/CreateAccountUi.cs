using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CreateAccountUi : MonoBehaviour
{
    [SerializeField]
    UIDocument uIDocument;

    VisualElement root;
    TextField username;
    TextField emailAdress;
    TextField password;
    Button createAccount;
    Button goBack;

    void OnEnable()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        username = root.Q<TextField>("UserText");
        emailAdress = root.Q<TextField>("MailText");
        password = root.Q<TextField>("PassText");
        createAccount = root.Q<Button>("CreateAccount");
        goBack = root.Q<Button>("Return");

        createAccount.clicked += Clickable_clicked;
        goBack.clicked += ReturnToAccountMenu;
    }

    private void Clickable_clicked()
    {
        Debug.Log("click");
        CreateAccount();
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

    public void UpdateEmailAdress(string adressMail)
    {
        string _emailAdress = emailAdress.text;
        _emailAdress = adressMail;
    }

    public void CreateAccount() =>PlayfabManager.Instance.CreateAccount(username.text, emailAdress.text, password.text);
}
