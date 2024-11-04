using UnityEngine;
using UnityEngine.UI;

public class SoundSlider : MonoBehaviour
{
    [SerializeField] Slider volumeSlider;



    private void Start()
    {
        volumeSlider = GetComponent<Slider>();
        volumeSlider.value = SoundController.Instance.musicVolume;
        volumeSlider.onValueChanged.AddListener(SetVolume);
        SoundController.Instance.soundChanged += OnSoundChanged;
    }

    private void SetVolume(float volume)
    {
        SoundController.Instance.SetMusicVolume(volume);
    }

    void OnSoundChanged()
    {
        if (volumeSlider != null)
        {
            volumeSlider.value = SoundController.Instance.musicVolume;
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