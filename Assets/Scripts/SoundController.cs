using System;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public static SoundController Instance { get; private set; }

    [Range(0f, 1f)]
    public float musicVolume = 1f;

    [SerializeField] float lastVolume = 1f;

    public bool soundEnabled = true;

    public event Action soundChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        soundChanged?.Invoke();

    }

    public void EnableMusicVolume()
    {

        if (soundEnabled)
        {
            lastVolume = musicVolume;
            musicVolume = 0;
        }
        else
        {
            musicVolume = lastVolume;
        }
        soundEnabled = !soundEnabled;
        soundChanged?.Invoke();
    }
}