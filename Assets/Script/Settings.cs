using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Device;
using UnityEngine.UIElements;

public class Settings : MonoBehaviour
{
    [SerializeField]
    GameManager gameManager;
    [SerializeField]
    UIDocument uIDocument;
    VisualElement root;

    void Awake()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        List<Button> resButton = root.Query<Button>("ResButton").ToList();
        foreach (Button button in resButton)
        {
            button.clickable.clickedWithEventInfo += OnChangeResolution;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnChangeResolution(EventBase eventBase)
    {
        Button button = (Button)eventBase.target;
    }

    void OnReturnTouch()
    {
        gameManager.Ui.isSettings = false;
        gameManager.Ui.ChangeScreen();
    }
}
