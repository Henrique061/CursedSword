using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CreditsController : MonoBehaviour
{
    [SerializeField] private GameObject creditsObj;
    [SerializeField] private Animator creditsAnim;
    [SerializeField] private GameObject mmObj;
    [SerializeField] private GameObject button;

    private MainMenuController mmc;

    private float timer;
    
    private bool mmOut = false;
    private bool creditsIn = false;
    private bool creditsOut = false;
    private bool mmIn = false;
    private bool playUpdate = false;

    private void Awake()
    {
        mmc = GetComponent<MainMenuController>();
    }

    private void Update()
    {
        if (mmOut)
        {
            if (timer <= 0)
            {
                mmOut = false;
                mmObj.SetActive(false);
                timer = 0.5f;
                creditsIn = true;
            }

            else
                timer -= Time.deltaTime;
        }

        if (creditsIn)
        {
            if (timer <= 0)
            {
                creditsIn = false;
                playUpdate = true;
            }

            else
                timer -= Time.deltaTime;
        }

        if (playUpdate)
        {
            EventSystem.current.SetSelectedGameObject(button);
        }

        if (creditsOut)
        {
            if (timer <= 0)
            {
                creditsOut = false;
                timer = 0.5f;
                mmObj.SetActive(true);
                mmIn = true;
            }

            else
                timer -= Time.deltaTime;
        }

        if (mmIn)
        {
            if (timer <= 0)
            {
                mmIn = false;
                creditsObj.SetActive(false);
                mmc.playUpdate = true;
                EventSystem.current.SetSelectedGameObject(mmc.creditsButton);
                mmc.MM_CreditsSelected();
            }

            else
                timer -= Time.deltaTime;
        }

    }

    public void CreditsSubmited()
    {
        EventSystem.current.SetSelectedGameObject(null);
        mmc.playUpdate = false;
        FindObjectOfType<AudioManager>().PlaySound("ChoiceSelect");
        timer = 0.5f;
        creditsObj.SetActive(true);
        mmOut = true;
    }

    public void CreditsExit()
    {
        FindObjectOfType<AudioManager>().PlaySound("ChoiceSelect");
        timer = 0.5f;
        creditsAnim.SetTrigger("CreditsExit");
        playUpdate = false;
        EventSystem.current.SetSelectedGameObject(null);
        creditsOut = true;
    }
}
