using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Audio;
public class SoundMixerManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;
    private float masterVolume = 1f;
    private float sfxVolume = 1f;
    private float musicVolume = 1f;

    public float volumeStep = 0.01f;

    #region Master Volume -------------------------------------------------------------------------------------------------------------------------

    public void IncreaseMasterVolume()
    {
        masterVolume = Mathf.Clamp(masterVolume + volumeStep, 0.0001f, 1f);
        SetMasterVolume(masterVolume);
    }
    public void DecreaseMasterVolume()
    {
        masterVolume = Mathf.Clamp(masterVolume - volumeStep, 0.0001f, 1f);
        SetMasterVolume(masterVolume);
    }
    public void SetMasterVolume(float level)
    {
        masterVolume = Mathf.Clamp(level, 0.0001f, 1f);
        audioMixer.SetFloat("masterVolume", Mathf.Log10(masterVolume) * 20f);
    }

    public float GetMasterVolume()
    {
        return masterVolume;
    }
    #endregion

    #region Sound FX -------------------------------------------------------------------------------------------------------------------------
    public void IncreaseSFXVolume()
    {
        sfxVolume = Mathf.Clamp(sfxVolume + volumeStep, 0.0001f, 1f);
        SetSoundFXVolume(sfxVolume);
    }
    public void DecreaseSFXVolume()
    {
        sfxVolume = Mathf.Clamp(sfxVolume - volumeStep, 0.0001f, 1f);
        SetSoundFXVolume(sfxVolume);
    }
    public void SetSoundFXVolume(float level)
    {
        sfxVolume = Mathf.Clamp(level, 0.0001f, 1f);
        audioMixer.SetFloat("soundFXVolume", Mathf.Log10(sfxVolume) * 20f);
    }

    public float GetSFXVolume()
    {
        return sfxVolume;
    }
    #endregion

    #region Music -------------------------------------------------------------------------------------------------------------------------
    
    public void IncreaseMusicVolume()
    {
        musicVolume = Mathf.Clamp(musicVolume + volumeStep, 0.0001f, 1f);
        SetMusicVolume(musicVolume);
    }
    
    public void DecreaseMusicVolume()
    {
        musicVolume = Mathf.Clamp(musicVolume - volumeStep, 0.0001f, 1f);
        SetMusicVolume(musicVolume);
    }

    public void SetMusicVolume(float level)
    {
        musicVolume = Mathf.Clamp(level, 0.0001f, 1f);
        audioMixer.SetFloat("musicVolume", Mathf.Log10(musicVolume) * 20f);
    }

    public float GetMusicVolume()
    {
        return musicVolume;
    }
        
    #endregion

}
