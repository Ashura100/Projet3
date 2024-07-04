using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class UiManager : MonoBehaviour
{
    [SerializeField]
    UIDocument uIDocument;

    VisualElement root;
    Button button;
    // Start is called before the first frame update
    void Awake()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        button = root.Q<Button>("AlphaButton");
        button.clicked += OnLetterTouch;
    }

    private void OnButtonTouch()
    {
        
    }

    void OnLetterTouch()
    {
        Debug.Log("click" + (button.text));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ChangeSprite()
    {

    }

    void ChangeScreen()
    {

    }

    void OnScreenTouch()
    {

    }

    void OnWin()
    {

    }

    void OnLose()
    {

    }

}
