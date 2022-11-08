using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private GameObject fuinhaLoka;
    [SerializeField] private GameObject mainObj;
    [SerializeField] private GameObject blackFade;
    [SerializeField] private AudioMixer am;

    [Header("Snapshot")]
    [SerializeField] private AudioMixerSnapshot fadeSnapshot;
    [SerializeField] private AudioMixerSnapshot mainSnapshot;

    [Header("Initial Button")]
    public GameObject initialSelectedButton;

    [Header("Button")]
    [SerializeField] private GameObject buttons;
    [SerializeField] private GameObject startButton;
    [SerializeField] private GameObject optionsButton;
    public GameObject creditsButton;
    [SerializeField] private GameObject quitButton;

    [Header("Text Colors")]
    [SerializeField] private SpriteRenderer white;
    [SerializeField] private SpriteRenderer colorful;

    [Header("Menu Texts")]
    [SerializeField] private Text startText;
    [SerializeField] private Text optionsText;
    [SerializeField] private Text creditsText;
    [SerializeField] private Text quitText;

    [Header("Height Width Modifier")]
    [SerializeField] private float heightMod = 1.2f;
    [SerializeField] private float widthtMod = 1.2f;

    private GameObject currentSelectedButton;

    private Color selectedColor;
    private Color unselectColor;

    private bool beginFuinha = true;
    [HideInInspector] public bool playUpdate = false;
    private bool waitMainAnim = false;
    private bool waitBlackAnim = false;
    private bool waitStart = false;
    public bool[] alreadySelected;

    private float timer = 6f;

    private void Awake()
    {
        alreadySelected = new bool[4];

        for (int i = 0; i < alreadySelected.Length; i++)
            alreadySelected[i] = false;

        currentSelectedButton = initialSelectedButton;

        selectedColor = colorful.GetComponent<SpriteRenderer>().color;
        unselectColor = white.GetComponent<SpriteRenderer>().color;
    }

    private void Start()
    {
        FindObjectOfType<AudioManager>().PlaySound("MainMenu");
        am.SetFloat("masterVol", PlayerPrefs.GetFloat("masterVol"));
        am.SetFloat("musicVol", PlayerPrefs.GetFloat("musicVol"));
        am.SetFloat("soundVol", PlayerPrefs.GetFloat("soundVol"));
    }

    private void Update()
    {
        if (beginFuinha)
        {
            if (timer <= 0)
            {
                Destroy(fuinhaLoka);
                mainObj.SetActive(true);
                beginFuinha = false;
                timer = 1f;
                waitMainAnim = true;
            }

            else
                timer -= Time.deltaTime;
        }

        if (waitMainAnim)
        {
            if (timer <= 0)
            {
                waitMainAnim = false;
                buttons.SetActive(true);
                playUpdate = true;
                MM_StartSelected();
            }

            else
                timer -= Time.deltaTime;
        }

        if (waitBlackAnim)
        {
            if (timer <= 0)
            {
                waitBlackAnim = false;
                FindObjectOfType<AudioManager>().StopAll();
                mainSnapshot.TransitionTo(0.01f);
                timer = 2f;

                am.SetFloat("masterVol", PlayerPrefs.GetFloat("masterVol"));
                am.SetFloat("musicVol", PlayerPrefs.GetFloat("musicVol"));
                am.SetFloat("soundVol", PlayerPrefs.GetFloat("soundVol"));

                waitStart = true;
            }

            else
                timer -= Time.deltaTime;
        }

        if (waitStart)
        {
            if (timer <= 0)
            {
                waitStart = false;
                SceneManager.LoadScene("Gaia_Room_Tutorial");
            }

            else
                timer -= Time.deltaTime;
        }

        if (playUpdate)
        {
            EventSystem.current.SetSelectedGameObject(currentSelectedButton);
        }
    }

    #region Selection

    public void MM_StartSelected()
    {
        if (!alreadySelected[0])
        {
            FindObjectOfType<AudioManager>().PlaySound("ChoiceHover");
            for (int i = 0; i < alreadySelected.Length; i++)
            {
                if (i == 0)
                    alreadySelected[i] = true;
                else
                    alreadySelected[i] = false;
            }
        }

        currentSelectedButton = startButton;

        startText.color = selectedColor;
        optionsText.color = unselectColor;
        creditsText.color = unselectColor;
        quitText.color = unselectColor;

        startButton.transform.localScale = new Vector3(widthtMod, heightMod, 0);
        optionsButton.transform.localScale = new Vector3(1, 1, 0);
        creditsButton.transform.localScale = new Vector3(1, 1, 0);
        quitButton.transform.localScale = new Vector3(1, 1, 0);
    }

    public void MM_OptionsSelected()
    {
        if (!alreadySelected[1])
        {
            FindObjectOfType<AudioManager>().PlaySound("ChoiceHover");
            for (int i = 0; i < alreadySelected.Length; i++)
            {
                if (i == 1)
                    alreadySelected[i] = true;
                else
                    alreadySelected[i] = false;
            }
        }

        currentSelectedButton = optionsButton;

        startText.color = unselectColor;
        optionsText.color = selectedColor;
        creditsText.color = unselectColor;
        quitText.color = unselectColor;

        startButton.transform.localScale = new Vector3(1, 1, 0);
        optionsButton.transform.localScale = new Vector3(widthtMod, heightMod, 0);
        creditsButton.transform.localScale = new Vector3(1, 1, 0);
        quitButton.transform.localScale = new Vector3(1, 1, 0);
    }

    public void MM_CreditsSelected()
    {
        if (!alreadySelected[2])
        {
            FindObjectOfType<AudioManager>().PlaySound("ChoiceHover");
            for (int i = 0; i < alreadySelected.Length; i++)
            {
                if (i == 2)
                    alreadySelected[i] = true;
                else
                    alreadySelected[i] = false;
            }
        }

        currentSelectedButton = creditsButton;

        startText.color = unselectColor;
        optionsText.color = unselectColor;
        creditsText.color = selectedColor;
        quitText.color = unselectColor;

        startButton.transform.localScale = new Vector3(1, 1, 0);
        optionsButton.transform.localScale = new Vector3(1, 1, 0);
        creditsButton.transform.localScale = new Vector3(widthtMod, heightMod, 0);
        quitButton.transform.localScale = new Vector3(1, 1, 0);
    }

    public void MM_QuitSelected()
    {
        if (!alreadySelected[3])
        {
            FindObjectOfType<AudioManager>().PlaySound("ChoiceHover");
            for (int i = 0; i < alreadySelected.Length; i++)
            {
                if (i == 3)
                    alreadySelected[i] = true;
                else
                    alreadySelected[i] = false;
            }
        }

        currentSelectedButton = quitButton;

        startText.color = unselectColor;
        optionsText.color = unselectColor;
        creditsText.color = unselectColor;
        quitText.color = selectedColor;

        startButton.transform.localScale = new Vector3(1, 1, 0);
        optionsButton.transform.localScale = new Vector3(1, 1, 0);
        creditsButton.transform.localScale = new Vector3(1, 1, 0);
        quitButton.transform.localScale = new Vector3(widthtMod, heightMod, 0);
    }

    #endregion

    #region Submit

    public void MM_StartSubmit()
    {
        FindObjectOfType<AudioManager>().PlaySound("Started");

        am.ClearFloat("soundVol");
        am.ClearFloat("masterVol");
        am.ClearFloat("musicVol");

        fadeSnapshot.TransitionTo(4f);
        timer = 4;
        blackFade.SetActive(true);
        playUpdate = false;
        EventSystem.current.SetSelectedGameObject(null);
        waitBlackAnim = true;
    }

    public void MM_QuitSubmit()
    {
        Application.Quit();
    }

    #endregion
}
