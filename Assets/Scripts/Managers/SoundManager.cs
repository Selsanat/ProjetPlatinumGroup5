using UnityEngine.Audio;
using System;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Script managing every audio in the game
/// </summary>
public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [Header("Main Mixer")]
    [SerializeField] private AudioMixerGroup audioMixerGroup;
    [SerializeField] private AudioMixer audioMixer;

    [Header("All the clips")]
    [SerializeField] private Sounds[] sounds;

    //Singleton initialization
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        InitializeAllClips();
    }

    /// <summary>
    /// Create an audio source for each clip and set it with the right parameter
    /// </summary>
    void InitializeAllClips()
    {
        if(sounds.Length == 0)
        {
            Debug.LogWarning("No sounds in the sound manager !");
            return;
        }
        else if(sounds.Length == 1)
        {
            foreach (Sounds s in sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clips[0];
                s.source.pitch = s.pitch;
                s.source.volume = s.volume;
                s.source.outputAudioMixerGroup = audioMixerGroup;
                s.source.loop = s.loop;
                s.source.playOnAwake = s.playeOnAwake;
                print("Sound " + s.name + " initialized");
            }
        }
        else
        {
            foreach (Sounds s in sounds)
            {
                s.source = gameObject.AddComponent<AudioSource>();
                s.source.clip = s.clips[UnityEngine.Random.Range(0, s.clips.Length - 1)];
                s.source.pitch = s.pitch;
                s.source.volume = s.volume;
                s.source.outputAudioMixerGroup = audioMixerGroup;
                s.source.loop = s.loop;
                s.source.playOnAwake = s.playeOnAwake;
                print("Sound " + s.name + " initialized");

            }


        }
        
    }

    /// <summary>
    /// Play a clip
    /// </summary>
    /// 
    /// <param name="name">The name of the clip</param>
    public void PlayClip(string name)
    {
        
        if (name == "")
        {
            return;
        }
        Sounds s = Array.Find(sounds, sound => sound.name == name);
        if (s == null || s.clip == null)
        {
            Debug.LogWarning("The clip " + name + " doesn't exist !");
            return;
        }
        if (s.Oneshot) s.source.PlayOneShot(s.clip);
        else s.source.Play();
    }


    public void Pauseclip(string name)
    {
        if (name == "")
        {
            return;
        }
        Sounds s = Array.Find(sounds, sound => sound.name == name);
        if (s == null || s.clip == null)
        {
            Debug.LogWarning("The clip " + name + " doesn't exist !");
            return;
        }
        s.source.Stop();
    }
}

[System.Serializable]
public class Sounds
{
    public string name;
    public AudioClip[] clips;
    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;
    [Range(.1f, 3f)]
    public float pitch;
    public bool loop;
    public bool Oneshot;
    public bool playeOnAwake;

    [HideInInspector]
    public AudioSource source;
}
