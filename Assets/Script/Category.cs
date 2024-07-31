using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UIElements;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace Hangman
{
    public class Category : MonoBehaviour
    {
        [SerializeField]
        GameManager gameManager;
        [SerializeField]
        UIDocument uIDocument;
        VisualElement root;
        Button goBack;
        Button humanCorpseButton;
        Button artButton;
        Button animalButton;
        Button armyButton;
        public const string CATEGORIE = "https://trouve-mot.fr/api/categorie/";
        void OnEnable()
        {
            root = uIDocument.rootVisualElement;

            goBack = root.Q<Button>("Return");
            humanCorpseButton = root.Q<Button>("CorpsButton");
            artButton = root.Q<Button>("ArtButton");
            animalButton = root.Q<Button>("AnimalButton");
            armyButton = root.Q<Button>("ArmyButton");

            goBack.clicked += GoBack;

            humanCorpseButton.clicked += () => OnCategoryButtonClicked("6");
            artButton.clicked += () => OnCategoryButtonClicked("10");
            animalButton.clicked += () => OnCategoryButtonClicked("19");
            armyButton.clicked += () => OnCategoryButtonClicked("26");
        }

        //permet le choix des categories, joue le son de click et sauvegarde la categorie choisi
        void OnCategoryButtonClicked(string category)
        {
            AudioManager.Instance.PlayClickSound();
            GameManager.Instance.currentCategory = category;
            StartCoroutine(GetCategoryData(category));
        }

        //appel les categorie de l'api
        IEnumerator GetCategoryData(string category)
        {
            string url = CATEGORIE + category;
            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                // Request and wait for the desired page.
                yield return webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError(webRequest.error);
                }
                else
                {
                    // Show results as text
                    Debug.Log(webRequest.downloadHandler.text);

                    // Process the data as needed
                    ProcessCategoryData(webRequest.downloadHandler.text);
                }
            }
        }

        //parse la categorie
        void ProcessCategoryData(string jsonData)
        {
            JArray jArray = JArray.Parse(jsonData);
            JObject jo = jArray.Children<JObject>().FirstOrDefault();

            Debug.Log(jo.GetValue("categorie"));
        }

        //retour menu
        void GoBack()
        {
            AudioManager.Instance.PlayClickSound();
            UiManager.Instance.GoBackToMenu();
        }
    }

}