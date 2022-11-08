using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]

public class Sound
{
    public string name; // name of the sound

    public AudioClip clip; // reference to the audio clip

    public AudioMixerGroup audioMixerGroup; // reference to the audio mixer group

    [Range(0f, 1f)]
    public float volume;

    [Range(0.1f, 3f)]
    public float pitch;

    public bool loop;

    public bool hasIntro; // if a music has an intro before starts

    public AudioClip introClip;

    public bool IgonreListenerPause;

    [HideInInspector]
    public AudioSource source; // to store the AudioSource in an instance
}
