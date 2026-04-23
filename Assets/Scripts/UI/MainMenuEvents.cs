using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuEvents : MonoBehaviour
{
    private UIDocument _document;

    [Header ("Button")]
    private Button _startButton;
    private Button _settingsButton;
    private Button _creditsButton;
    private Button _exitButton;


    [Header ("ButtonSFX")]
    private List<Button> _menuButtons = new List<Button>();
    private AudioSource _audioSource;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _document = GetComponent<UIDocument>();

        _startButton = _document.rootVisualElement.Q("StartButton") as Button;
        _startButton.RegisterCallback<ClickEvent>(OnStartClick);

        _settingsButton = _document.rootVisualElement.Q("SettingsButton") as Button;
        _settingsButton.RegisterCallback<ClickEvent>(OnSettingsClick);

        _creditsButton = _document.rootVisualElement.Q("CreditsButton") as Button;
        _creditsButton.RegisterCallback<ClickEvent>(OnCreditsClick);

        _exitButton = _document.rootVisualElement.Q("ExitButton") as Button;
        _exitButton.RegisterCallback<ClickEvent>(OnExitClick);

        _menuButtons = _document.rootVisualElement.Query<Button>().ToList();

        for (int i = 0; i < _menuButtons.Count; i++)
        {
            _menuButtons[i].RegisterCallback<ClickEvent>(OnAllButtonClick);
        }
    }

    private void OnDisable()
    {
        _startButton.UnregisterCallback<ClickEvent>(OnStartClick);

        _startButton.UnregisterCallback<ClickEvent>(OnCreditsClick);

        for (int i = 0; i < _menuButtons.Count; i++)
        {
            _menuButtons[i].UnregisterCallback<ClickEvent>(OnStartClick);  
        }
    }


    private void OnStartClick(ClickEvent evt)
    {
        Debug.Log("You Pressed the Start Button");
        Time.timeScale = 1f;

        _document.rootVisualElement.focusable = false;
        _document.rootVisualElement.Blur();

        SceneManager.LoadScene("GameScene");

    }

    private void OnSettingsClick(ClickEvent evt)
    {
        Debug.Log("You Pressed the Settings Button");
    }

    private void OnCreditsClick(ClickEvent evt)
    {
        Debug.Log("You Pressed the Credits Button");
    }

    private void OnExitClick(ClickEvent evt)
    {
        Debug.Log("You Pressed the Exit Button");
        Application.Quit();
    }

    private void OnAllButtonClick(ClickEvent evt)
    {
        _audioSource.Play();   
    }
}
