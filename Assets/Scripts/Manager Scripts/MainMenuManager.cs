using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Canvas / Panels")]
    [SerializeField] private GameObject _mainMenuPanel;
    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private GameObject _creditsPanel;
    [SerializeField] private GameObject _audioSettingsPanel;

    [Header("First Selected (Controller Support)")]
    [SerializeField] private GameObject _mainMenuFirst;
    [SerializeField] private GameObject _settingsFirst;
    [SerializeField] private GameObject _creditsFirst;

    void Start()
    {
        _mainMenuPanel.SetActive(true);
        _settingsPanel.SetActive(false);
    }

    // ปุ่ม PLAY
    public void OnStartPress()
    {
        SceneManager.LoadScene("GameScene"); // ใส่ชื่อ Scene ให้ตรง
    }

    // ปุ่ม SETTINGS
    public void OnSettingsPress()
    {
        // _mainMenuPanel.SetActive(false);
        _settingsPanel.SetActive(true);
        _creditsPanel.SetActive(false);

        EventSystem.current.SetSelectedGameObject(_settingsFirst);
    }

    // ปุ่ม BACK ใน Settings
    public void OnSettingsBackPress()
    {
        _settingsPanel.SetActive(false);
        // _mainMenuPanel.SetActive(true);
        _creditsPanel.SetActive(false);

        EventSystem.current.SetSelectedGameObject(_mainMenuFirst);
    }

    public void OnAudioSettingsPress()
    {
        _settingsPanel.SetActive(false);
        _audioSettingsPanel.SetActive(true);
    }

    public void OnAudioSettingsBackPress()
    {
        _audioSettingsPanel.SetActive(false);
        _settingsPanel.SetActive(true);
    }

    // ปุ่ม CREDITS
    public void OnCreditsPress()
    {
        _settingsPanel.SetActive(false);
        _mainMenuPanel.SetActive(false);
        _creditsPanel.SetActive(true);

        Debug.Log("You are pressed the Credits Button");

        EventSystem.current.SetSelectedGameObject(_creditsFirst);
    }

    public void OnCreditsBackPress()
    {
        _creditsPanel.SetActive(false);
        _mainMenuPanel.SetActive(true);
        _creditsPanel.SetActive(false);

        EventSystem.current.SetSelectedGameObject(_mainMenuFirst);
    }

    // ปุ่ม QUIT
    public void OnQuitPress()
    {
        Application.Quit();
    }
}