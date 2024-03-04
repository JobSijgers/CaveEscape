using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    [SerializeField] private AudioMixer mixer;
    public Sound[] sounds;


    private void Awake()
    {
        Instance = this;
        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.outputAudioMixerGroup = sound.output;

            sound.source.volume = sound.volume;
            sound.source.loop = sound.loop;
            sound.source.mute = sound.mute;

            if (sound.playOnAwake)
            {
                Play(sound.name);
            }
        }
    }

    public void Play(string name)
    {
        Sound sound = Array.Find(sounds, sound => sound.name == name);
        if (sound == null)
        {
            Debug.LogError("Sound: " + name + " not found");
            return;
        }
        sound.source.Play();
    }

    public void Stop(string name)
    {
        Sound sound = Array.Find(sounds, sound => sound.name == name);

        if (sound == null)
        {
            Debug.LogError("Sound: " + name + " not found");
        }
        sound.source.Stop();
    }
}