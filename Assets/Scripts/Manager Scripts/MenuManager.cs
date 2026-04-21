using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
    [Header("Menu Objects")]
    [SerializeField] private GameObject _mainMenuCanvasGO;
    [SerializeField] private GameObject _settingMenuCanvasGO;
    [SerializeField] private GameObject _AudioMenuCanvasGO;


    [Header("First Selected Options")]
    [SerializeField] private GameObject _mainMenuFirst;
    [SerializeField] private GameObject _settingsMenuFirst;

    private bool isPaused;

    void Start()
    {
        _mainMenuCanvasGO.SetActive(false);
        _settingMenuCanvasGO.SetActive(false); 
        _AudioMenuCanvasGO.SetActive(false);

    }

    void Update()
    {
        if (InputManager.instance.MenuOpenCloseInput)
        {
            if (!isPaused)
            {
                Pause();
            }
            else
            {
                UnPause();
            }
        }
    }

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;

        OpenMainMenu();
    }

    public void UnPause()
    {
        isPaused = false;
        Time.timeScale = 1f;

        CloseAllMenus();
    }

    private void OpenMainMenu()
    {
        _mainMenuCanvasGO.SetActive(true);
        _settingMenuCanvasGO.SetActive(false);
        _AudioMenuCanvasGO.SetActive(false); 

        // EventSystem.current.SetSelectedGameObject(_mainMenuFirst);
        StartCoroutine(SelectAfterFrame(_mainMenuFirst));
    }

    private void OpenSettingsMenuHandle()
    {
        _mainMenuCanvasGO.SetActive(false);
        _settingMenuCanvasGO.SetActive(true);
        _AudioMenuCanvasGO.SetActive(false); 

        // EventSystem.current.SetSelectedGameObject(_settingsMenuFirst);
        StartCoroutine(SelectAfterFrame(_settingsMenuFirst));
    }

    private void OpenAudioMenus()
    {
        _mainMenuCanvasGO.SetActive(false);
        _settingMenuCanvasGO.SetActive(false);
        _AudioMenuCanvasGO.SetActive(true);  
    }

    private void CloseAllMenus()
    {
        _mainMenuCanvasGO.SetActive(false);
        _settingMenuCanvasGO.SetActive(false);
        _AudioMenuCanvasGO.SetActive(false);   

        EventSystem.current.SetSelectedGameObject(null);
    }

    private IEnumerator SelectAfterFrame(GameObject target)
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return null;
        EventSystem.current.SetSelectedGameObject(target);
    }

    public void OnSettingPress()
    {
        OpenSettingsMenuHandle();
    }

    public void OnResumePress()
    {
        UnPause();   
    }

    public void OnSettingsBackPress()
    {
        OpenMainMenu();
    }

    public void OnAudioPress()
    {
        OpenAudioMenus();
    }
}
