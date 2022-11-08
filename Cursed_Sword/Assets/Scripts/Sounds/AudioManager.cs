using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    // FindObjectOfType<AudioManager>().FUNCTION_NAME("ARRAY_NAME");

    #region Variables

    public Sound[] sounds;

    [HideInInspector] public List<Sound> introList = new List<Sound>();
    [HideInInspector] public Sound[] intro;

    public static AudioManager am;
    private AudioIntroManager aim;

    #endregion

    // AWAKE ////////////////////////////////////////////////////////////////////////////
    void Awake()
    {
        if (am == null)
            am = this;

        else
        {
            Destroy(gameObject);
            return;
        }
        // this if and else statements is to not duplicate the AudioManager gameObject when transitioning through scenes (if the scenes have audio manager)

        aim = GetComponent<AudioIntroManager>();

        //DontDestroyOnLoad(gameObject); // to not destroy the game Object when transitioning through scenes (in case of using the same song in different scenes)

        foreach (Sound s in sounds) // to loop through the list of sounds, an add an AudioSource for each (when open the game)
        {
            s.source = gameObject.AddComponent<AudioSource>(); // storing the AudioSource in the method "source"

            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.outputAudioMixerGroup = s.audioMixerGroup;

            if(s.hasIntro)
                introList.Add(s);

            if (s.IgonreListenerPause)
                s.source.ignoreListenerPause = true;

        } // end of foreach

        intro = new Sound[am.introList.Count];

        for (int i = 0; i < intro.Length; i++)
            intro[i] = am.introList[i];
    }

    #region Sound Manipulation Functions

    // PLAY SOUND ///////////////////////////////////////////////////////////////////////
    public void PlaySound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name); // find the sound of the correct name to play
        // the parameters: (the array, who to find -> in this case, its the sound where the sound name it's equals to the name)

        if (s == null) // just to prevent to not let test the game in case of something writed wrong
        {
            Debug.LogWarning("SOUND NOT FOUND! PROBABLY WRONG WRITED IN INSPECTOR OF AUDIOMANAGER OR THE FILE ITSELF!");
            return;
        }

        if (s.hasIntro)
            aim.IntroPlay(name); // to play an intro befor the sound itself

        else
            s.source.Play();
    }

    // PLAY ONLY ////////////////////////////////////////////////////////////////////////
    public void PlayOnly(string _name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (sounds[i].name != _name)
            {
                Sound s = sounds[i];
                s.source.Pause();
            }

            else
            {
                Sound s = sounds[i];
                s.source.Play();
            }
        }
    }

    // PAUSE ALL ////////////////////////////////////////////////////////////////////////
    public void PauseAll()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            Sound s = sounds[i];
            s.source.Pause();
        }
    }

    // UNPAUSE ALL //////////////////////////////////////////////////////////////////////
    public void UnPauseAll()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            Sound s = sounds[i];
            s.source.UnPause();
        }
    }

    // PAUSE SOUND //////////////////////////////////////////////////////////////////////
    public void PauseSound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name); // find the sound of the correct name to play
        // the parameters: (the array, who to find -> in this case, its the sound where the sound name it's equals to the name)

        if (s == null) // just to prevent to not let test the game in case of something writed wrong
        {
            Debug.LogWarning("SOUND NOT FOUND! PROBABLY WRONG WRITED IN INSPECTOR OF AUDIOMANAGER OR THE FILE ITSELF!");
            return;
        }

        s.source.Pause();
    }

    // UNPAUSE SOUND ////////////////////////////////////////////////////////////////////
    public void UnPauseSound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name); // find the sound of the correct name to play
        // the parameters: (the array, who to find -> in this case, its the sound where the sound name it's equals to the name)

        if (s == null) // just to prevent to not let test the game in case of something writed wrong
        {
            Debug.LogWarning("SOUND NOT FOUND! PROBABLY WRONG WRITED IN INSPECTOR OF AUDIOMANAGER OR THE FILE ITSELF!");
            return;
        }

        s.source.UnPause();
    }

    // STOP SOUND ///////////////////////////////////////////////////////////////////////
    public void StopSound(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name); // find the sound of the correct name to play
        // the parameters: (the array, who to find -> in this case, its the sound where the sound name it's equals to the name)

        if (s == null) // just to prevent to not let test the game in case of something writed wrong
        {
            Debug.LogWarning("SOUND NOT FOUND! PROBABLY WRONG WRITED IN INSPECTOR OF AUDIOMANAGER OR THE FILE ITSELF!");
            return;
        }

        s.source.Stop();
    }

    // STOP ALL /////////////////////////////////////////////////////////////////////////
    public void StopAll()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            Sound s = sounds[i];
            s.source.Stop();
        }
    }

    #endregion
}
