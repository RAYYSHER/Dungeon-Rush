using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenuManager : MonoBehaviour
{
    [Header("Canvas / Panels")]
    [SerializeField] private GameObject _mainMenuPanel;
    [SerializeField] private GameObject _settingsPanel;
    [SerializeField] private GameObject _creditsPanel;
    [SerializeField] private GameObject _audioSettingsPanel;
    [SerializeField] private GameObject _GuideMenPanel;

    [Header("First Selected (Controller Support)")]
    [SerializeField] private GameObject _mainMenuFirst;
    [SerializeField] private GameObject _settingsFirst;
    [SerializeField] private GameObject _creditsFirst;
    [SerializeField] private GameObject _audioSettingsFirst;
    [SerializeField] private GameObject _GuideMenuFirst;
    [SerializeField] private ButtonHighlight _creditsButtonHighlight;

    void Start()
    {
        _mainMenuPanel.SetActive(true);
        _settingsPanel.SetActive(false);
        StartCoroutine(SelectAfterFrame(_mainMenuFirst));
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

       StartCoroutine(SelectAfterFrame(_settingsFirst));
    }

    // ปุ่ม BACK ใน Settings
    public void OnSettingsBackPress()
    {
        EventSystem.current.SetSelectedGameObject(null);

        _settingsPanel.SetActive(false);
        _creditsPanel.SetActive(false);
        StartCoroutine(SelectAfterFrame(_mainMenuFirst));
    }

    // ปุ่ม Back
    public void OnAudioSettingsPress()
    {
        _settingsPanel.SetActive(false);
        _audioSettingsPanel.SetActive(true);
        StartCoroutine(SelectAfterFrame(_audioSettingsFirst));
    }

    public void OnAudioSettingsBackPress()
    {
        _audioSettingsPanel.SetActive(false);
        _settingsPanel.SetActive(true);
        StartCoroutine(SelectAfterFrame(_settingsFirst));
    }

    // ปุ่ม CREDITS
    public void OnCreditsPress()
    {
        EventSystem.current.SetSelectedGameObject(null);

        _settingsPanel.SetActive(false);
        _mainMenuPanel.SetActive(false);
        _creditsPanel.SetActive(true);

        StartCoroutine(SelectAfterFrame(_creditsFirst));
    }

    public void OnCreditsBackPress()
    {
        EventSystem.current.SetSelectedGameObject(null);

        _creditsPanel.SetActive(false);
        _mainMenuPanel.SetActive(true);
        
        StartCoroutine(SelectAfterFrame(_mainMenuFirst));
    }

    public void OnGuidesPress()
    {
        EventSystem.current.SetSelectedGameObject(null);

        _GuideMenPanel.SetActive(true);

        StartCoroutine(SelectAfterFrame(_GuideMenuFirst));
    }

    public void OnGuidesBackPress()
    {
        EventSystem.current.SetSelectedGameObject(null);

        _GuideMenPanel.SetActive(false);
        _mainMenuPanel.SetActive(true);
        
        StartCoroutine(SelectAfterFrame(_mainMenuFirst));
    }



    // ปุ่ม QUIT
    public void OnQuitPress()
    {
        Application.Quit();
    }


    // Controller Support (make button pre highlighted for controller)
    private IEnumerator SelectAfterFrame(GameObject target)
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return null;
        EventSystem.current.SetSelectedGameObject(target);
    }
}