using UnityEngine;
using TMPro;

public class AudioSettingController : MonoBehaviour
{
    [SerializeField] private SoundMixerManager soundMixerManager;

    [Header("UI Text")]
    [SerializeField] private TMP_Text masterText;
    [SerializeField] private TMP_Text sfxText;
    [SerializeField] private TMP_Text musicText;

    void Start()
    {
        UpdateUI();
    }

    #region Master Volume -------------------------------------------------------------------------------------------------------------------------
    public void IncreaseMaster() 
    {
        soundMixerManager.IncreaseMasterVolume(); UpdateUI(); 
    }
    public void DecreaseMaster() 
    {
        soundMixerManager.DecreaseMasterVolume(); UpdateUI();
    }
    
    #endregion

    #region Sound FX      -------------------------------------------------------------------------------------------------------------------------
    public void IncreaseSFX() 
    {
        soundMixerManager.IncreaseSFXVolume(); UpdateUI();
    }
    public void DecreaseSFX() 
    {
        soundMixerManager.DecreaseSFXVolume(); UpdateUI();
    }
    #endregion

    #region Music         -------------------------------------------------------------------------------------------------------------------------
    public void IncreaseMusic()
    {
        soundMixerManager.IncreaseMusicVolume(); UpdateUI();
    }
    public void DecreaseMusic()
    { 
        soundMixerManager.DecreaseMusicVolume(); UpdateUI();
    }
    #endregion            -------------------------------------------------------------------------------------------------------------------------

    private void UpdateUI()
    {
        if (masterText) masterText.text = $"{Mathf.RoundToInt(soundMixerManager.GetMasterVolume() * 100)}%";
        if (sfxText)    sfxText.text    = $"{Mathf.RoundToInt(soundMixerManager.GetSFXVolume()    * 100)}%";
        if (musicText)  musicText.text  = $"{Mathf.RoundToInt(soundMixerManager.GetMusicVolume()  * 100)}%";
    }
}