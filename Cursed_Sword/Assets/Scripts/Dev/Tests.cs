using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tests : MonoBehaviour
{
    public InputMaster input;

    private void Awake()
    {
        input = new InputMaster();

        input.PlayerControl.Test.performed += ctx => SoundTest();
    }

    private void SoundTest()
    {
        FindObjectOfType<AudioManager>().PlaySound("GaiaBattle");
    }
    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }
}
