using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioIntroManager : MonoBehaviour
{
    private AudioManager am;
    private AudioClip introClip;

    private void Awake()
    {
        am = GetComponent<AudioManager>();
    }

    public void IntroPlay(string name)
    {
        Sound s = Array.Find(am.intro, sound => sound.name == name);

        introClip = s.introClip;

        s.source.PlayOneShot(introClip);
        s.source.PlayScheduled(AudioSettings.dspTime + introClip.length);
    }
}
