using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip   musicClip;
    [SerializeField] private AudioMixerGroup mixerGroup;  // ลาก Music group ใส่

    void Start()
    {
        if (audioSource == null || musicClip == null) return;

        audioSource.outputAudioMixerGroup = mixerGroup;
        audioSource.clip   = musicClip;
        audioSource.loop   = true;
        audioSource.Play();
    }
}