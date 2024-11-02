using UnityEngine;

public class PlaySoundAndDestroy : MonoBehaviour
{

    [SerializeField] private AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            UpdateVolume();
            audioSource.Play();
            Destroy(gameObject, audioSource.clip.length);
        }
    }

    private void UpdateVolume()
    {
        if (audioSource != null && SoundController.Instance != null)
        {
            audioSource.volume = SoundController.Instance.musicVolume;
        }
    }
}
