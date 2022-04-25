using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager
{
    private AudioSource audio;
    private bool isPlaying;

    public AudioManager(AudioSource audio)
    {
        this.audio = audio;
        isPlaying = false;
    }

    public void PlayAudioWithLoop()
    {
        if (!isPlaying)
        {
            audio.Play();
            isPlaying = true;
        }
    }

    public void PlayAudioWithoutLoop()
    {
        audio.Play();
    }
    public void StopAudio()
    {
        if (isPlaying)
        {
            audio.Stop();
            isPlaying = false;
        }
    }
}
