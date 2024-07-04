using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine;
using System;

public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(TrouverMot("https://trouve-mot.fr/api/daily"));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator TrouverMot(string url)
    {
        using (UnityWebRequest req = UnityWebRequest.Get(url))
        {
            yield return req.SendWebRequest();
            WordCollection word = JsonUtility.FromJson<WordCollection>(req.downloadHandler.text);
            Debug.Log(word.words[0].name);
        }
    }
}
public class Word
{ 
    public string name;
    public string text;
}
[Serializable]
public class WordCollection
{
    public Word[] words;
}
