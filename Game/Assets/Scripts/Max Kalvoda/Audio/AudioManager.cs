using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioMixerGroup sfxGroup;
    public AudioMixerGroup soundtrackGroup;

    public AudioClip soundtrack;
    public MyAudio[] soundEffects;

    private static AudioManager _instance;
    [HideInInspector]
    public static AudioManager instance {
        get { return _instance; }
        set { Debug.LogWarning("AudioManager instance can not be set!"); }
    }

    // Start is called before the first frame update
    void Awake()
    {
        if(_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            _instance = this;
        }

        AudioSource soundtrackSource = gameObject.AddComponent<AudioSource>();
        soundtrackSource.loop = true;
        soundtrackSource.clip = soundtrack;
        soundtrackSource.outputAudioMixerGroup = soundtrackGroup;
        soundtrackSource.Play();

        for (int i = 0; i < soundEffects.Length; i++)
        {
            AudioSource sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.outputAudioMixerGroup = sfxGroup;

            MyAudio audio = soundEffects[i];
            sfxSource.loop = audio.loop;
            sfxSource.pitch = audio.pitch;
            sfxSource.volume = audio.volume;
            sfxSource.clip = audio.clip;

            audio.audioSrc = sfxSource;

            for (int k = 0; k < soundEffects.Length; k++)/**/
            {
                if (k == i)
                    continue;

                if (audio.name == soundEffects[k].name)
                    Debug.LogWarning("2 soundEffects have the same name!");
            }
        }
    }

    //play an audio clip
    public void Play(string clipName)
    {
        for (int i = 0; i < soundEffects.Length; i++)
        {
            MyAudio sfx = soundEffects[i];
            if (sfx.name == clipName)
            {
                sfx.Play();
                break;
            }
            if(i == soundEffects.Length - 1)
            {
                Debug.LogError("Sound " + clipName + " not found!");
            }
        }
    }

    //mutes an audio clip
    public void Mute(string clipName)
    {
        for (int i = 0; i < soundEffects.Length; i++)
        {
            MyAudio sfx = soundEffects[i];
            if (sfx.name == clipName)
            {
                sfx.Mute();
            }
        }
    }

    //unmutes an audio clip
    public void Unmute(string clipName)
    {
        for (int i = 0; i < soundEffects.Length; i++)
        {
            MyAudio sfx = soundEffects[i];
            if (sfx.name == clipName)
            {
                sfx.Unmute();
            }
        }
    }
}
