using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayfabManager : MonoBehaviour
{
    public static PlayfabManager Instance;

    private void Awake()
    {
        Instance = this;
    }
    public void CreateAccount (string userName, string adressName, string passWord)
    {
        PlayFabClientAPI.RegisterPlayFabUser(
            new RegisterPlayFabUserRequest()
            {
                Email = adressName,
                Password = passWord,
                Username = userName,
                RequireBothUsernameAndEmail = true
            },
            response =>
            {
                Debug.Log($"Successful Account Creation: {userName}, {adressName}");
                SignIn(userName, passWord);
                UiManager.Instance.ChangeScreen(UiManager.Instance.currentScreen, UiManager.Instance.signUpUi);
            },
            error =>
            {
                Debug.Log($"Unsuccessful Account Creation: {userName}, {adressName} \n { error.ErrorMessage}");
            }
        );
    }

    public void SignIn(string userName, string passWord)
    {
        PlayFabClientAPI.LoginWithPlayFab(new LoginWithPlayFabRequest()
        {
            Username = userName,
            Password = passWord
        },
        response =>
        {
            Debug.Log($"Successful Account Login: {userName}");
            UiManager.Instance.ChangeScreen(UiManager.Instance.currentScreen, UiManager.Instance.startUi);
        },
        error =>
        {
            Debug.Log($"Unsuccessful Account Login: {userName}\n {error.ErrorMessage}");
        }
        );
    }
}
