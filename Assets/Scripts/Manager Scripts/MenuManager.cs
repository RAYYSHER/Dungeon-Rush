using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
    [Header("Menu Objects")]
    [SerializeField] private GameObject _mainMenuCanvasGO;
    [SerializeField] private GameObject _settingMenuCanvasGO;
    [SerializeField] private GameObject _AudioMenuCanvasGO;
    [SerializeField] private GameObject _GuidePanelCanvasGO;
    


    [Header("First Selected Options")]
    [SerializeField] private GameObject _mainMenuFirst;
    [SerializeField] private GameObject _settingsMenuFirst;
    [SerializeField] private GameObject _audioMenuFirst;
    [SerializeField] private GameObject _GuidePanelFirst;
    

    private bool isPaused;
    private UISelectionGuard _selectionGuard;
    private PlayerController _playerController;

    void Awake()
    {
        _selectionGuard = FindFirstObjectByType<UISelectionGuard>();
        _playerController = FindFirstObjectByType<PlayerController>();
    }
    void Start()
    {
        _mainMenuCanvasGO.SetActive(false);
        _settingMenuCanvasGO.SetActive(false); 
        _AudioMenuCanvasGO.SetActive(false);
        _GuidePanelCanvasGO.SetActive(false);

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
        if (_playerController != null) _playerController.enabled = false;

        OpenMainMenu();
    }

    public void UnPause()
    {
        isPaused = false;
        Time.timeScale = 1f;
        if (_playerController != null) _playerController.enabled = true;

        CloseAllMenus();
    }

    private void OpenMainMenu()
    {
        _mainMenuCanvasGO.SetActive(true);
        _settingMenuCanvasGO.SetActive(false);
        _AudioMenuCanvasGO.SetActive(false); 
        _GuidePanelCanvasGO.SetActive(false);

        // EventSystem.current.SetSelectedGameObject(_mainMenuFirst);
        StartCoroutine(SelectAfterFrame(_mainMenuFirst));
    }

    private void OpenSettingsMenuHandle()
    {
        EventSystem.current.SetSelectedGameObject(null);

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

        StartCoroutine(SelectAfterFrame(_audioMenuFirst)); 
    }

    private void OpenGuidePanel()
    {
        EventSystem.current.SetSelectedGameObject(null);

        _mainMenuCanvasGO.SetActive(false);
        _GuidePanelCanvasGO.SetActive(true);
        _settingMenuCanvasGO.SetActive(false);

        StartCoroutine(SelectAfterFrame(_GuidePanelFirst)); 
    }

    private void CloseAllMenus()
    {
        _mainMenuCanvasGO.SetActive(false);
        _settingMenuCanvasGO.SetActive(false);
        _AudioMenuCanvasGO.SetActive(false);   

        _selectionGuard?.ClearLastSelected();  
        EventSystem.current.SetSelectedGameObject(null);
    }

    private IEnumerator SelectAfterFrame(GameObject target)
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return null;
        EventSystem.current.SetSelectedGameObject(target);
    }

    

    public void OnResumePress()
    {
        UnPause();   
    }
    
    public void OnSettingPress()
    {
        OpenSettingsMenuHandle();
    }

    public void OnSettingsBackPress()
    {
        OpenMainMenu();
    }

    public void OnAudioPress()
    {
        OpenAudioMenus();
    }

    public void OnAudioBackPress()
    {
        OpenSettingsMenuHandle();
    }

    public void OnGuideMenuPress()
    {
        OpenGuidePanel();
    }

    public void OnGuideMenuBackPress()
    {
        OpenMainMenu();
    }

    public void OnSurrenderPress()
    {
        UnPause(); // ปิด menu ก่อน
        FindFirstObjectByType<Player>()?.Die();
    }
}
