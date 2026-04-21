using TMPro;
using UnityEngine;

public class VolumeDisplay : MonoBehaviour
{
    public enum VolumeType { Master, SFX, Music }
    
    [SerializeField] private VolumeType volumeType;
    [SerializeField] private TMP_Text text;
    private SoundMixerManager _mixer;

    void Start()
    {
        _mixer = FindFirstObjectByType<SoundMixerManager>();
    }

    void Update()
    {
        if (_mixer == null || text == null) return;

        float val = volumeType switch
        {
            VolumeType.Master => _mixer.GetMasterVolume(),
            VolumeType.SFX    => _mixer.GetSFXVolume(),
            VolumeType.Music  => _mixer.GetMusicVolume(),
            _                 => 0f
        };

        text.text = Mathf.RoundToInt(val * 100f) + "%";
    }
}