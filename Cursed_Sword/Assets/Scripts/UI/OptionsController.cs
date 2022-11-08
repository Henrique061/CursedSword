using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OptionsController : MonoBehaviour
{
    [Header("Options")]
    [SerializeField] private GameObject options;
    [SerializeField] private GameObject general;

    [Header("Button")]
    [SerializeField] private GameObject initialSelectedButton;

    [Header("Sliders & Button")]
    [SerializeField] private GameObject masterSlid;
    [SerializeField] private GameObject musicSlid;
    [SerializeField] private GameObject soundSlid;
    [SerializeField] private GameObject backButton;

    [Header("Text Colors")]
    [SerializeField] private SpriteRenderer white;
    [SerializeField] private SpriteRenderer colorful;

    [Header("Height Width Modifier")]
    [SerializeField] private float heightMod = 1.2f;
    [SerializeField] private float widthtMod = 1.2f;

    [Header("PauseController && MainMenuController")]
    [SerializeField] private PauseController pc;
    [SerializeField] private MainMenuController mmc;

    private GameObject currentSelectedButton;

    private Text backText;

    private Color selectedColor;
    private Color unselectColor;

    [HideInInspector] public bool playUpdate = false;
    private bool[] alreadySelected;

    private void Awake()
    {
        currentSelectedButton = initialSelectedButton;
        alreadySelected = new bool[4];

        backText = backButton.GetComponentInChildren<Text>();
        selectedColor = colorful.GetComponent<SpriteRenderer>().color;
        unselectColor = white.GetComponent<SpriteRenderer>().color;

        for (int i = 0; i < alreadySelected.Length; i++)
            alreadySelected[i] = false;
    }

    private void Update()
    {
        if (playUpdate)
        {
            EventSystem.current.SetSelectedGameObject(currentSelectedButton);
        }
    }

    public void OptionsCalled()
    {
        FindObjectOfType<AudioManager>().PlaySound("ChoiceSelect");

        general.SetActive(false);
        options.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(initialSelectedButton);

        playUpdate = true;

        if (pc != null)
            pc.alreadySelected[1] = false;

        else if (mmc != null)
            mmc.alreadySelected[1] = false;

        MasterSliderSelected();
    }

    public void OptionsQuit()
    {
        FindObjectOfType<AudioManager>().PlaySound("ChoiceSelect");

        general.SetActive(true);
        options.SetActive(false);

        if (pc != null)
            pc.currentSelectedButton = pc.resumeButton;

        EventSystem.current.SetSelectedGameObject(null);

        if (pc != null)
            EventSystem.current.SetSelectedGameObject(pc.initialSelectedButton);

        else if (mmc != null)
            EventSystem.current.SetSelectedGameObject(mmc.initialSelectedButton);

        playUpdate = false;

        if (pc != null)
        {
            pc.alreadySelected[0] = true;
            pc.ResumeSelected();
        }

        else if (mmc != null)
        {
            mmc.alreadySelected[0] = true;
            mmc.MM_OptionsSelected();
        }
    }

    // SELECTION ///////////////////////////////////////////////////////////////////////////////////////////

    public void MasterSliderSelected()
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

        currentSelectedButton = masterSlid;

        backText.color = unselectColor;
        backButton.transform.localScale = new Vector3(1, 1, 0);
    }

    public void MusicSliderSelected()
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

        currentSelectedButton = musicSlid;

        backText.color = unselectColor;
        backButton.transform.localScale = new Vector3(1, 1, 0);
    }

    public void SoundSliderSelected()
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

        currentSelectedButton = soundSlid;

        backText.color = unselectColor;
        backButton.transform.localScale = new Vector3(1, 1, 0);
    }

    public void BackButtonSelected()
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

        currentSelectedButton = backButton;

        backText.color = selectedColor;
        backButton.transform.localScale = new Vector3(widthtMod, heightMod, 0);
    }
}
