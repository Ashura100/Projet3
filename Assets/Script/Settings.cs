using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Device;
using UnityEngine.UIElements;

public class Settings : MonoBehaviour
{
    public enum Difficulty
    {
        Easy,
        Medium,
        Hard
    }

    [SerializeField]
    GameManager gameManager;
    [SerializeField]
    UIDocument uIDocument;
    VisualElement root;

    Slider slider;
    Toggle toggle;

    private List<Button> resolutionButtons;
    private List<(int width, int height)> resolutions = new List<(int, int)>
    {
        (1280, 720),
        (1920, 1080),
        (2048, 1556)
    };

    private List<Button> difficultyButtons;
    private List<Difficulty> difficulties = new List<Difficulty>
    {
        Difficulty.Easy,
        Difficulty.Medium,
        Difficulty.Hard
    };

    public AudioMixer audioMixer;

    public static Difficulty currentDifficulty = Difficulty.Medium;

    void Awake()
    {
        root = GetComponent<UIDocument>().rootVisualElement;
        slider = root.Q<Slider>("Slider");
        toggle = root.Q<Toggle>("FullScreenT");

        resolutionButtons = root.Query<Button>("ButtonRes").ToList();
        for (int i = 0; i < resolutionButtons.Count; i++)
        {
            int index = i;
            resolutionButtons[i].clicked += () => SetQuality(resolutions[index].width, resolutions[index].height);
        }

        difficultyButtons = root.Query<Button>("ButtonDiff").ToList();
        for (int i = 0; i < difficultyButtons.Count; i++)
        {
            int index = i;
            difficultyButtons[i].clicked += () => SetDifficulty(difficulties[index]);
        }

        slider.RegisterValueChangedCallback(evt => SetVolume(evt.newValue));

        toggle.RegisterValueChangedCallback(evt => SetFullScreen(evt.newValue));

        toggle.value = UnityEngine.Screen.fullScreen;
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnChangeDifficulty(EventBase eventBase)
    {
        Button button = (Button)eventBase.target;
        SetDifficulty(0);
    }

    public void SetVolume(float volume)
    {
        float dB = Mathf.Log10(volume) * 20;
        audioMixer.SetFloat("Volume", dB);
    }

    public void SetQuality(int width, int height)
    {
        UnityEngine.Screen.SetResolution(width, height, FullScreenMode.FullScreenWindow);
        Debug.Log($"Resolution changed to: {width}x{height}");
    }

    public void SetFullScreen(bool isFullScreen)
    {
        UnityEngine.Screen.fullScreen = isFullScreen;
        Debug.Log($"Fullscreen mode set to {isFullScreen}");
    }

    public void SetDifficulty(Difficulty newDifficulty)
    {
        currentDifficulty = newDifficulty;
        Debug.Log($"Difficulty changed to: {newDifficulty}");
    }
    void OnReturnTouch()
    {
        gameManager.Ui.isSettings = false;
        gameManager.Ui.ChangeScreen();
    }
}
