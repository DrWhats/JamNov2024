using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    [SerializeField] private AudioSource musicSource;

    void Start()
    {
        if (musicSource == null)
        {
            musicSource = GetComponent<AudioSource>();
        }

        if (musicSource != null && SoundController.Instance != null)
        {
            musicSource.volume = SoundController.Instance.musicVolume;
            SoundController.Instance.soundChanged += OnSoundChanged;
        }
        else
        {
            Debug.LogWarning("AudioSource not found on " + gameObject.name);
        }
    }

    void OnSoundChanged()
    {
        if (musicSource != null && SoundController.Instance != null) 
        {
            musicSource.volume = SoundController.Instance.musicVolume;
        }
    }

    void OnDestroy()
    {
        if (SoundController.Instance != null)
        {
            SoundController.Instance.soundChanged -= OnSoundChanged;
        }
    }
}