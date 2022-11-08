using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Audio;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseController : MonoBehaviour
{
    public static bool gamePaused = false; // condition that pauses the entire game
    public static bool canPause = true; // condition that make he player pause the game

    [Header("Pause Objects")]
    [SerializeField] private GameObject pauseUI; // the gameobject that will ne activated when pausing the game
    public GameObject initialSelectedButton; // the button that will be selected by default when pausing the game

    [Header("Buttons")]
    public GameObject resumeButton;
    [SerializeField] private GameObject retryButton;
    [SerializeField] private GameObject optionsButton;
    [SerializeField] private GameObject rechooseButton;
    [SerializeField] private GameObject menuButton;

    [Header("Text Colors")]
    [SerializeField] private SpriteRenderer white;
    [SerializeField] private SpriteRenderer colorful;

    [Header("Height Width Modifier")]
    [SerializeField] private float heightMod = 1.1f;
    [SerializeField] private float widthtMod = 1.1f;

    [Header("General and Options Objects")]
    [SerializeField] private GameObject general;
    [SerializeField] private GameObject options;

    [Header("OptionsController")]
    [SerializeField] private OptionsController oc;

    [Header("Time to activate pause")]
    [SerializeField] private float pauseDelay = 0.1f;

    private float fixedPauseDelay;

    private Text resumeText;
    private Text optionsText;
    private Text rechooseText;
    private Text menuText;
    private Text retryText;

    private Color selectedColor;
    private Color unselectColor;

    [HideInInspector] public GameObject currentSelectedButton;

    [HideInInspector] public bool playUpdate;
    [HideInInspector] public bool[] alreadySelected;
    private bool countDelay = false;
    private bool canUnpause = false;

    public InputMaster input;

    private void Awake()
    {
        input = new InputMaster();

        resumeText = resumeButton.GetComponentInChildren<Text>();
        optionsText = optionsButton.GetComponentInChildren<Text>();

        if (rechooseButton != null)
            rechooseText = rechooseButton.GetComponentInChildren<Text>();

        menuText = menuButton.GetComponentInChildren<Text>();

        if (retryButton != null)
            retryText = retryButton.GetComponentInChildren<Text>();

        selectedColor = colorful.GetComponent<SpriteRenderer>().color;
        unselectColor = white.GetComponent<SpriteRenderer>().color;

        alreadySelected = new bool[5];
        fixedPauseDelay = pauseDelay;

        for (int i = 0; i < alreadySelected.Length; i++)
            alreadySelected[i] = false;

        // PRESSES PAUSE
        input.PlayerControl.Pause.performed += ctx => PauseAction();
    }

    private void Update()
    {
        if (playUpdate)
        {
            EventSystem.current.SetSelectedGameObject(currentSelectedButton);
        }

        if (countDelay)
        {
            if (pauseDelay <= 0)
            {
                canUnpause = true;
            }

            else
                pauseDelay -= Time.unscaledDeltaTime;
        }
    }

    private void PauseAction()
    {
        if (canPause)
        {
            if (!gamePaused) // will pause the game
            {
                PauseGame();
            }

            else // will unpause the game
            {
                UnpauseGame();
            }
        }
    }

    private void PauseGame() // pause game action
    {
        FindObjectOfType<AudioManager>().PlayOnly("PauseOn");

        gamePaused = !gamePaused;
        AudioListener.pause = true;
        canPause = true;
        pauseUI.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(initialSelectedButton);

        Time.timeScale = 0;

        playUpdate = true;
        countDelay = true;
    }

    public void UnpauseGame() // unpause game action
    {
        if (canUnpause)
        {
            oc.playUpdate = false;
            playUpdate = false;

            Time.timeScale = 1;

            //input.PlayerControl.Enable();
            gamePaused = !gamePaused;
            AudioListener.pause = false;
            pauseUI.SetActive(false);
            options.SetActive(false);
            general.SetActive(true);

            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(initialSelectedButton);

            pauseDelay = fixedPauseDelay;
            canUnpause = false;
            countDelay = false;

            FindObjectOfType<AudioManager>().PlaySound("PauseOff");
            FindObjectOfType<AudioManager>().UnPauseAll();
        }
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

    // BUTTONS FUNCTIONS /////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region Selection

    public void ResumeSelected()
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

        currentSelectedButton = resumeButton;

        resumeText.color   = selectedColor;
        optionsText.color  = unselectColor;

        if (rechooseButton != null)
            rechooseText.color = unselectColor;

        menuText.color     = unselectColor;

        if (retryButton != null)
            retryText.color = unselectColor;

        resumeButton.transform.localScale   = new Vector3(widthtMod, heightMod, 0);
        optionsButton.transform.localScale  = new Vector3(1, 1, 0);

        if (rechooseButton != null)
            rechooseButton.transform.localScale = new Vector3(1, 1, 0);

        menuButton.transform.localScale     = new Vector3(1, 1, 0);

        if (retryButton != null)
            retryButton.transform.localScale    = new Vector3(1, 1, 0);
    }

    public void RetrySelected()
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

        currentSelectedButton = retryButton;

        resumeText.color = unselectColor;
        optionsText.color = unselectColor;

        if (rechooseButton != null)
            rechooseText.color = unselectColor;

        menuText.color = unselectColor;

        if (retryButton != null)
            retryText.color = selectedColor;

        resumeButton.transform.localScale = new Vector3(1, 1, 0);
        optionsButton.transform.localScale = new Vector3(1, 1, 0);

        if (rechooseButton != null)
            rechooseButton.transform.localScale = new Vector3(1, 1, 0);

        menuButton.transform.localScale = new Vector3(1, 1, 0);

        if (retryButton != null)
            retryButton.transform.localScale = new Vector3(widthtMod, heightMod, 0);
    }

    public void OptionsSelected()
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

        currentSelectedButton = optionsButton;

        resumeText.color   = unselectColor;
        optionsText.color  = selectedColor;

        if (rechooseButton != null)
            rechooseText.color = unselectColor;

        menuText.color     = unselectColor;

        if (retryButton != null)
            retryText.color = unselectColor;

        resumeButton.transform.localScale   = new Vector3(1, 1, 0);
        optionsButton.transform.localScale  = new Vector3(widthtMod, heightMod, 0);

        if (rechooseButton != null)
            rechooseButton.transform.localScale = new Vector3(1, 1, 0);

        menuButton.transform.localScale     = new Vector3(1, 1, 0);

        if (retryButton != null)
            retryButton.transform.localScale    = new Vector3(1, 1, 0);
    }

    public void RechooseSelected()
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

        currentSelectedButton = rechooseButton;

        resumeText.color   = unselectColor;
        optionsText.color  = unselectColor;

        if (rechooseButton != null)
            rechooseText.color = selectedColor;

        menuText.color     = unselectColor;

        if (retryButton != null)
            retryText.color = unselectColor;

        resumeButton.transform.localScale   = new Vector3(1, 1, 0);
        optionsButton.transform.localScale  = new Vector3(1, 1, 0);

        if (rechooseButton != null)
            rechooseButton.transform.localScale = new Vector3(widthtMod, heightMod, 0);

        menuButton.transform.localScale     = new Vector3(1, 1, 0);

        if (retryButton != null)
            retryButton.transform.localScale    = new Vector3(1, 1, 0);
    }

    public void MenuSelected()
    {
        if (!alreadySelected[4])
        {
            FindObjectOfType<AudioManager>().PlaySound("ChoiceHover");
            for (int i = 0; i < alreadySelected.Length; i++)
            {
                if (i == 4)
                    alreadySelected[i] = true;
                else
                    alreadySelected[i] = false;
            }
        }

        currentSelectedButton = menuButton;

        resumeText.color   = unselectColor;
        optionsText.color  = unselectColor;

        if (rechooseButton != null)
            rechooseText.color = unselectColor;

        menuText.color     = selectedColor;

        if (retryButton != null)
            retryText.color = unselectColor;

        resumeButton.transform.localScale   = new Vector3(1, 1, 0);
        optionsButton.transform.localScale  = new Vector3(1, 1, 0);

        if (rechooseButton != null)
            rechooseButton.transform.localScale = new Vector3(1, 1, 0);

        menuButton.transform.localScale     = new Vector3(widthtMod, heightMod, 0);

        if (retryButton != null)
            retryButton.transform.localScale    = new Vector3(1, 1, 0);
    }

    #endregion
}
