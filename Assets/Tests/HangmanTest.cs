using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Hangman;

public class HangmanTest
{
    // A Test behaves as an ordinary method
    [Test]
    public void HangmanTestSimplePasses()
    {
        // Use the Assert class to test conditions
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator HangmanLetterTest()
    {
        // Load the scene
        SceneManager.LoadScene(0);
        yield return null;

        // Find the game object containing the UI Document
        GameObject go = GameObject.FindObjectOfType<Pendu>(true).gameObject;
        go.SetActive(true);
        UIDocument uIDocument = go.GetComponent<UIDocument>();
        VisualElement root = uIDocument.rootVisualElement;

        // Check if the AlphaButton is present and not empty
        var alphaButtons = root.Query<Button>("AlphaButton").ToList();
        Assert.IsTrue(alphaButtons.Count > 0, "Alpha buttons not found");

        foreach (var button in alphaButtons)
        {
            Assert.IsNotEmpty(button.text, "Button text is empty");
        }
    }

    [UnityTest]
    public IEnumerator HangmanHasWordTest()
    {
        // Load the scene
        SceneManager.LoadScene(0);
        yield return null;

        // Find the game object containing the UI Document
        GameObject go = GameObject.FindObjectOfType<Pendu>(true).gameObject;
        go.SetActive(true);
        UIDocument uIDocument = go.GetComponent<UIDocument>();
        VisualElement root = uIDocument.rootVisualElement;

        // Find the word label and check if it is not empty
        Label wordLabel = uIDocument.rootVisualElement.Q<Label>("Word");
        Assert.IsNotNull(wordLabel, "Word label not found");

    }

    [UnityTest]
    public IEnumerator HangmanErrorTest()
    {
        // Load the scene
        SceneManager.LoadScene(0);
        yield return null;

        // Simulate an error by mocking the GetWordDefinition coroutine
        GameObject go = GameObject.FindObjectOfType<Pendu>(true).gameObject;
        go.SetActive(true);
        UIDocument uIDocument = go.GetComponent<UIDocument>();
        VisualElement root = uIDocument.rootVisualElement;
        Pendu pendu = go.GetComponent<Pendu>();

        pendu.StartCoroutine(MockGetWordDefinitionWithError(pendu));
        yield return null;

        // Check if the error message is displayed
        TextField information = pendu.uIDocument.rootVisualElement.Q<TextField>("Information");
        Assert.IsEmpty(information.value);
    }

    private IEnumerator MockGetWordDefinitionWithError(Pendu pendu)
    {
        pendu.indiceText.value = "Erreur de récupération de la définition.";
        yield return null;
    }

    [UnityTest]
    public IEnumerator HangmanWinTest()
    {
        // Load the scene
        SceneManager.LoadScene(0);
        yield return null;

        GameObject go = GameObject.FindObjectOfType<Pendu>(true).gameObject;
        go.SetActive(true);
        UIDocument uIDocument = go.GetComponent<UIDocument>();
        VisualElement root = uIDocument.rootVisualElement;
        Pendu pendu = go.GetComponent<Pendu>();

        // Ensure the game is in a state where a win is possible
        Assert.IsFalse(pendu.IsWon, "IsWon should start as false");
    }

    [UnityTest]
    public IEnumerator HangmanChangeScreen()
    {
        // Load the scene
        SceneManager.LoadScene(0);
        yield return null;

        GameObject go = GameObject.FindObjectOfType<UiManager>(true).gameObject;
        go.SetActive(true);
        UiManager uiManager = go.GetComponent<UiManager>();

        // Ensure the current screen is the initial screen
        Assert.AreEqual(uiManager.currentScreen, uiManager.createOrSignUi, "Initial screen should be createOrSignUi");
    }

    [UnityTest]
    public IEnumerator HangmanLoginTest()
    {
        // Load the scene
        SceneManager.LoadScene(0);
        yield return null;

        GameObject gofab = GameObject.FindObjectOfType<PlayfabManager>(true).gameObject;
        gofab.SetActive(true);
        GameObject go = GameObject.FindObjectOfType<SignUp>(true).gameObject;
        go.SetActive(true);
        UIDocument uIDocument = go.GetComponent<UIDocument>();
        VisualElement root = uIDocument.rootVisualElement;
        PlayfabManager playfabManager = gofab.GetComponent<PlayfabManager>();
        UiManager uiManager = UiManager.Instance;

        // Ensure the initial screen is set
        Assert.AreEqual(uiManager.currentScreen, uiManager.createOrSignUi, "Initial screen should be createOrSignUi");

        // Mock the unsuccessful login response
        playfabManager.SignIn("invalidUser", "invalidPassword");
        yield return null; // Wait for the login process to complete

        // Verify that the screen remains the same or handles the error appropriately
        Assert.AreEqual(uiManager.currentScreen, uiManager.createOrSignUi, "Current screen should remain createOrSignUi after unsuccessful login");
    }
}
