using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; set; }

    //Sound FX
    public AudioSource dropItemSound;
    public AudioSource craftingSound;
    public AudioSource toolSwingSound;
    public AudioSource chopSound;
    public AudioSource pickupItemSound;
    public AudioSource grassWalkSound;

    public AudioSource drawMetalicToolSound;
    public AudioSource addToQuickSlots;

    public AudioSource treeFall;

    //Music
    public AudioSource startingZoneBGMusic;
    public AudioSource wateringChannel;
    public AudioClip wateringCan;
    public AudioSource rainChannel;
    public AudioClip rainSound;

    //voice
    public AudioSource voiceovers;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;

        }
    }

    public void PlaySound(AudioSource soundToPlay)
    {
        if(!soundToPlay.isPlaying)
        {
            soundToPlay.Play();
        }
    }
    
    public void PlayVoiceOvers(AudioClip clip)
    {
        voiceovers.clip = clip;
        if (!voiceovers.isPlaying)
        {
            voiceovers.Play();
        }
        else
        {
            voiceovers.Stop();
            voiceovers.Play();
        }
    }

}
