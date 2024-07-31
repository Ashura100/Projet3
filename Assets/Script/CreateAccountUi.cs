using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Hangman
{
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

        //cr�e un compte une fois les chanmps texts rempli et le bouton cliqu� (joue le son click)
        private void Clickable_clicked()
        {
            Debug.Log("click");
            CreateAccount();
            AudioManager.Instance.PlayGameClickSound();
        }

        //bouton retour
        void ReturnToAccountMenu()
        {
            UiManager.Instance.ReturnToAccountMenu();
            AudioManager.Instance.PlayClickSound();
        }

        //met � jour le champ text du pseudo
        public void UpdateUserName(string userName)
        {
            string _username = username.text;
            _username = userName;
        }

        //met � jour le champ text du mot de passe
        public void UpdatePassword(string passWord)
        {
            string _password = password.text;
            password.value = passWord;
        }

        //met � jour le champ text de l'adresse mail
        public void UpdateEmailAdress(string adressMail)
        {
            string _emailAdress = emailAdress.text;
            _emailAdress = adressMail;
        }

        //cr�e le compte sur playfab
        public void CreateAccount() => PlayfabManager.Instance.CreateAccount(username.text, emailAdress.text, password.text);
    }
}
