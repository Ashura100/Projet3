using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json.Linq;
using System.Linq;

namespace Hangman
{
    public class Category : MonoBehaviour
    {
        public const string CATEGORIE = "https://trouve-mot.fr/api/categorie/";

        public void SelectCategory(string categoryId)
        {
            AudioManager.Instance.PlayClickSound();
            GameManager.Instance.currentCategory = categoryId;
            StartCoroutine(GetCategoryData(categoryId));
        }

        IEnumerator GetCategoryData(string category)
        {
            string url = CATEGORIE + category;
            using (UnityWebRequest webRequest = UnityWebRequest.Get(url))
            {
                yield return webRequest.SendWebRequest();

                if (webRequest.result == UnityWebRequest.Result.ConnectionError ||
                    webRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError(webRequest.error);
                }
                else
                {
                    ProcessCategoryData(webRequest.downloadHandler.text);
                }
            }
        }

        void ProcessCategoryData(string jsonData)
        {
            JArray jArray = JArray.Parse(jsonData);
            JObject jo = jArray.Children<JObject>().FirstOrDefault();

            Debug.Log(jo.GetValue("categorie"));
        }
    }
}
