using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetSliderValue : MonoBehaviour
{
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider soundSlider;

    private void Start()
    {
        masterSlider.value = VolumeSliderController.masterVolValue;
        musicSlider.value = VolumeSliderController.musicVolValue;
        soundSlider.value = VolumeSliderController.soundVolValue;
    }
}
