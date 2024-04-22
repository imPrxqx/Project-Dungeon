using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

//helper class for working with audio
[System.Serializable]
public class MyAudio
{
    public string name;
    public float volume;
    public float pitch;
    public bool loop;

    public AudioClip clip;

    private AudioSource _audioSrc;
    [HideInInspector]
    public AudioSource audioSrc {
        set { _audioSrc = value; }
    }

    //play the audio clip
    public void Play()
    {
        _audioSrc.Play();
    }

    //mute the audio clip
    public void Mute (){
        _audioSrc.mute = true;
    }

    //unmute the audio clip
    public void Unmute()
    {
        _audioSrc.mute = false;
    }
}
