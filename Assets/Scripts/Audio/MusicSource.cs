using UnityEngine;

// Attach to any AudioSource that plays background music (e.g. a level's looping track) to keep
// its volume in sync with the music slider, without needing a dedicated script like MenuMusic.
[RequireComponent(typeof(AudioSource))]
public class MusicSource : MonoBehaviour
{
    private AudioSource audioSource;
    private float baseVolume;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        baseVolume = audioSource.volume;
    }

    private void OnEnable()
    {
        ApplyVolume(VolumeManager.GetInstance().MusicVolume);
        VolumeManager.OnMusicVolumeChanged += ApplyVolume;
    }

    private void OnDisable()
    {
        VolumeManager.OnMusicVolumeChanged -= ApplyVolume;
    }

    private void ApplyVolume(float musicVolume)
    {
        audioSource.volume = baseVolume * musicVolume;
    }
}
