using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSliderController : MonoBehaviour
{
    [SerializeField] private AudioMixer am;

    private float masterValue = 0;

    public static float masterVolValue;
    public static float musicVolValue;
    public static float soundVolValue;

    private void Awake()
    {
        am.SetFloat("masterVol", PlayerPrefs.GetFloat("masterVol"));
        am.SetFloat("musicVol",  PlayerPrefs.GetFloat("musicVol"));
        am.SetFloat("soundVol",  PlayerPrefs.GetFloat("soundVol"));

        masterVolValue = PlayerPrefs.GetFloat("masterVol");
        musicVolValue = PlayerPrefs.GetFloat("musicVol");
        soundVolValue = PlayerPrefs.GetFloat("soundVol");
    }

    private void Start()
    {
        masterValue = GetMasterLevel();
    }

    #region Set Levels

    public void SetMasterLvl(float masterLvl)
    {
        am.SetFloat("masterVol", masterLvl);

        PlayerPrefs.SetFloat("masterVol", masterLvl);
        PlayerPrefs.Save();

    }

    public void SetBgmLvl(float bgmLvl)
    {
        am.SetFloat("musicVol", bgmLvl);

        PlayerPrefs.SetFloat("musicVol", bgmLvl);
        PlayerPrefs.Save();
    }

    public void SetSfxLvl(float sfxLvl)
    {
        am.SetFloat("soundVol", sfxLvl);

        PlayerPrefs.SetFloat("soundVol", sfxLvl);
        PlayerPrefs.Save();
    }

    #endregion

    public void ClearVolume()
    {
        am.ClearFloat("soundVol");
        am.ClearFloat("masterVol");
        am.ClearFloat("musicVol");
    }

    public float GetMasterLevel()
    {
        float value;
        bool result = am.GetFloat("masterVol", out value);

        if (result)
        {
            return value;
        }

        else
        {
            return -80f;
        }
    }
}
