using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class MainMenuEvents : MonoBehaviour
{
    private UIDocument _document;
    private Button _button;
    private List<Button> _menuButtons = new List<Button>();
    private AudioSource _audioSource;

    void Awake()
    {
        _audioSource = GetComponent<AudioSource>();
        _document = GetComponent<UIDocument>();

        _button = _document.rootVisualElement.Q("StartButton") as Button;
        _button.RegisterCallback<ClickEvent>(OnStartClick);

        _menuButtons = _document.rootVisualElement.Query<Button>().ToList();

        for (int i = 0; i < _menuButtons.Count; i++)
        {
            _menuButtons[i].RegisterCallback<ClickEvent>(OnAllButtonClick);
        }
    }

    private void OnDisable()
    {
        _button.UnregisterCallback<ClickEvent>(OnStartClick);

        for (int i = 0; i < _menuButtons.Count; i++)
        {
            _menuButtons[i].UnregisterCallback<ClickEvent>(OnStartClick);  
        }
    }


    private void OnStartClick(ClickEvent evt)
    {
        Debug.Log("You Pressed the Start Button");

    }

    private void OnAllButtonClick(ClickEvent evt)
    {
        _audioSource.Play();   
    }
}
