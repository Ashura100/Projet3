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
    private UiManager uiManager;
    private Pendu pendu;

    [UnitySetUp]
    public IEnumerator SetUp()
    {
        // Charger la scène principale
        SceneManager.LoadScene(0);
        yield return null;

        uiManager = GameObject.FindObjectOfType<UiManager>();
        pendu = GameObject.FindObjectOfType<Pendu>();

        Assert.IsNotNull(uiManager, "UiManager introuvable");
        Assert.IsNotNull(pendu, "Pendu introuvable");
    }

    [UnityTest]
    public IEnumerator AlphaButtonsExist()
    {
        yield return null;
        var alphaButtons = uiManager.gameUiRoot.Query<Button>("AlphaButton").ToList();
        Assert.IsTrue(alphaButtons.Count > 0, "Pas de boutons de lettres trouvés");
        foreach (var b in alphaButtons)
            Assert.IsFalse(string.IsNullOrEmpty(b.text), "Bouton de lettre vide");
    }

    [UnityTest]
    public IEnumerator WordIsLoaded()
    {
        yield return new WaitForSeconds(0.5f); // attendre la récupération du mot
        string word = pendu.targetWord;
        Assert.IsFalse(string.IsNullOrEmpty(word), "Le mot cible est vide");
        Assert.AreEqual(word.Length, pendu.GetDisplayWord().Replace(" ", "").Length, "Le mot affiché ne correspond pas");
    }

    [UnityTest]
    public IEnumerator WrongLetterIncreasesError()
    {
        string wrongLetter = "Z";
        int initialErrors = pendu.GetErrorCount();

        pendu.OnLetterTouch(wrongLetter);
        yield return null;

        Assert.AreEqual(initialErrors + 1, pendu.GetErrorCount(), "Le nombre d'erreurs n'a pas augmenté après une lettre incorrecte");
    }

    [UnityTest]
    public IEnumerator RevealAllLettersWinsGame()
    {
        yield return new WaitForSeconds(0.5f); // attendre que le mot soit chargé
        foreach (char c in pendu.targetWord)
        {
            pendu.OnLetterTouch(c.ToString());
        }

        yield return null;

        Assert.IsTrue(pendu.IsWon, "Le jeu n'est pas gagné après avoir révélé toutes les lettres");
    }

    [UnityTest]
    public IEnumerator SwitchScreenWorks()
    {
        uiManager.SetupButtons();

        uiManager.gameUiRoot.style.display = DisplayStyle.None;
        uiManager.startUiRoot.style.display = DisplayStyle.Flex;

        Assert.AreEqual(DisplayStyle.Flex, uiManager.startUiRoot.style.display);
        Assert.AreEqual(DisplayStyle.None, uiManager.gameUiRoot.style.display);
        yield return null;
    }
}
