using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    public AudioSource musicAudioSource;
    public AudioSource effectsAudioSource;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        DontDestroyOnLoad(gameObject);
    }

    // Cập nhật âm lượng cho các audio sources
    public void SetMasterVolume(float volume)
    {
        AudioListener.volume = volume;
    }

    public void SetMusicVolume(float volume)
    {
        if (musicAudioSource != null)
            musicAudioSource.volume = volume;
    }

    public void SetEffectsVolume(float volume)
    {
        if (effectsAudioSource != null)
            effectsAudioSource.volume = volume;
    }
}
