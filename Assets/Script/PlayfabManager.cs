using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

namespace Hangman
{
    public class PlayfabManager : MonoBehaviour
    {
        public static PlayfabManager Instance;

        private void Awake()
        {
            Instance = this;
        }

        public void CreateAccount(string username, string email, string password)
        {
            PlayFabClientAPI.RegisterPlayFabUser(new RegisterPlayFabUserRequest()
            {
                Username = username,
                Email = email,
                Password = password,
                RequireBothUsernameAndEmail = true
            },
            response =>
            {
                Debug.Log($"Compte créé : {username}, {email}");
                SignIn(username, password);
                //GameManager.Instance.SwitchScreen(ScreenType.SignUpUI);
            },
            error =>
            {
                Debug.LogError($"Échec création compte : {username}, {email}\n{error.ErrorMessage}");
            });
        }

        public void SignIn(string username, string password)
        {
            PlayFabClientAPI.LoginWithPlayFab(new LoginWithPlayFabRequest()
            {
                Username = username,
                Password = password
            },
            response =>
            {
                Debug.Log($"Connexion réussie : {username}");
                GameManager.Instance.SwitchScreen(ScreenType.StartUI);
            },
            error =>
            {
                Debug.LogError($"Échec connexion : {username}\n{error.ErrorMessage}");
            });
        }
    }
}
