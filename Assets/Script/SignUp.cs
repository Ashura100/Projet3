using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using UnityEngine;
using UnityEngine.UIElements;

namespace Hangman
{
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

        //connecte au moment du click
        private void Clickable_clicked()
        {
            Debug.Log("click");
            SignIn();
            AudioManager.Instance.PlayGameClickSound();
        }

        //retour menu compte
        void ReturnToAccountMenu()
        {
            UiManager.Instance.ReturnToAccountMenu();
            AudioManager.Instance.PlayClickSound();
        }

        //met à jour le champ text pseudonyme
        public void UpdateUserName(string userName)
        {
            string _username = username.text;
            _username = userName;
        }

        //met à jour le champ text mot de passe
        public void UpdatePassword(string passWord)
        {
            string _password = password.text;
            password.value = passWord;
        }

        //connecte le joueur
        public void SignIn() => PlayfabManager.Instance.SignIn(username.text, password.text);
    }
}

