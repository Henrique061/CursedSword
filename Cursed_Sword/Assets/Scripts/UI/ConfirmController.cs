using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class ConfirmController : MonoBehaviour
{
    [SerializeField] private GameObject mainObject;
    [SerializeField] private GameObject[] confirmTexts;
    [SerializeField] private GameObject initialSelectedButton;
    [SerializeField] private PauseController pc;
    [SerializeField] private SkillChooseController scc;
    [SerializeField] private GameOverController goc;
    [SerializeField] private LoadingController lc;
    [SerializeField] private TutorialSceneBeginManager tbm;

    [Header("Buttons")]
    [SerializeField] private GameObject yesButton;
    [SerializeField] private GameObject noButton;

    [Header("Text Colors")]
    [SerializeField] private SpriteRenderer white;
    [SerializeField] private SpriteRenderer colorful;

    [Header("Height Width Modifier")]
    [SerializeField] private float heightMod = 1.2f;
    [SerializeField] private float widthtMod = 1.2f;

    [Header("Fade-In Obj & Animator")]
    [SerializeField] private GameObject fadeObj;
    [SerializeField] private Animator fadeAnim;

    private GameObject currentSelectedButton;

    private Color selectedColor;
    private Color unselectColor;

    private Text yesText;
    private Text noText;

    private bool playUpdate = false;
    private bool[] alreadySelected;
    private bool byRechoose = false;
    private bool byMenu = false;
    private bool byRetry = false;
    private bool byChooseSkills = false;
    private bool bySkipIntro = false;
    private bool playFadeWait = false;
    private bool waitingFade = false;

    private float fadeWait = 1;

    private void Awake()
    {
        selectedColor = colorful.GetComponent<SpriteRenderer>().color;
        unselectColor = white.GetComponent<SpriteRenderer>().color;

        yesText = yesButton.GetComponentInChildren<Text>();
        noText  = noButton.GetComponentInChildren<Text>();

        alreadySelected = new bool[2];
        currentSelectedButton = initialSelectedButton;

        for (int i = 0; i < alreadySelected.Length; i++)
            alreadySelected[i] = false;
    }

    private void Update()
    {
        if (playUpdate)
        {
            EventSystem.current.SetSelectedGameObject(currentSelectedButton);
        }

        if (playFadeWait && fadeWait > 0)
            fadeWait -= Time.unscaledDeltaTime;

        else if (playFadeWait && fadeWait <= 0)
        {
            YesSubmit();
        }
    }

    public void RechooseCalled()
    {
        FindObjectOfType<AudioManager>().PlaySound("ChoiceSelect");

        byRechoose = true;
        byMenu = false;
        byRetry = false;
        pc.playUpdate = false;
        playUpdate = true;

        mainObject.SetActive(true);
        confirmTexts[1].SetActive(false);
        confirmTexts[0].SetActive(true);
        confirmTexts[2].SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(initialSelectedButton);
    }

    public void MainMenuCalled()
    {
        FindObjectOfType<AudioManager>().PlaySound("ChoiceSelect");

        byMenu = true;
        byRechoose = false;
        byRetry = false;
        pc.playUpdate = false;
        playUpdate = true;

        pc.alreadySelected[2] = false;
        pc.alreadySelected[3] = false;

        mainObject.SetActive(true);
        confirmTexts[1].SetActive(true);
        confirmTexts[0].SetActive(false);
        confirmTexts[2].SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(initialSelectedButton);
    }

    public void RetryMenuCalled()
    {
        FindObjectOfType<AudioManager>().PlaySound("ChoiceSelect");

        byRetry = true;
        byMenu = false;
        byRechoose = false;
        pc.playUpdate = false;
        playUpdate = true;

        pc.alreadySelected[2] = false;
        pc.alreadySelected[3] = false;

        mainObject.SetActive(true);
        confirmTexts[2].SetActive(true);
        confirmTexts[0].SetActive(false);
        confirmTexts[1].SetActive(false);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(initialSelectedButton);
    }

    public void ChooseSkillCalled()
    {

        byChooseSkills = true;
        playUpdate = true;

        mainObject.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(initialSelectedButton);

    }

    public void SkipIntroCalled()
    {
        FindObjectOfType<AudioManager>().PlaySound("ChoiceSelect");

        bySkipIntro = true;
        playUpdate = true;

        mainObject.SetActive(true);
        confirmTexts[1].SetActive(false);
        confirmTexts[0].SetActive(false);
        confirmTexts[2].SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(initialSelectedButton);
    }

    public void ConfirmQuit()
    {
        mainObject.SetActive(false);

        if (!byChooseSkills)
        {
            confirmTexts[0].SetActive(false);
            confirmTexts[1].SetActive(false);
        }

        byRechoose = false;
        byMenu = false;
        byRetry = false;

        if (!byChooseSkills && !bySkipIntro)
        {
            pc.currentSelectedButton = pc.resumeButton;
            pc.alreadySelected[0] = true;

            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(pc.initialSelectedButton);

            playUpdate = false;
            pc.playUpdate = true;

            pc.ResumeSelected();
        }

        else if (byChooseSkills)
        {
            byChooseSkills = false;
            scc.NoConfirm();
        }

        else if (bySkipIntro)
        {
            bySkipIntro = false;
            tbm.PlayIntro();
        }

        
    }

    /////////////////////////////////////////////////////////////////////////////////////
    
    public void YesSelected()
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

        currentSelectedButton = yesButton;

        yesText.color = selectedColor;
        noText.color = unselectColor;

        yesButton.transform.localScale = new Vector3(heightMod, widthtMod, 0);
        noButton.transform.localScale = new Vector3(1, 1, 0);
    }

    public void YesSubmit()
    {
        if (!waitingFade)
        {
            playUpdate = false;
            EventSystem.current.SetSelectedGameObject(null);

            FindObjectOfType<AudioManager>().PlaySound("ChoiceSelect");
            playFadeWait = true;

            fadeObj.SetActive(true);
            fadeAnim.SetTrigger("Fadein");
            waitingFade = true;
        }

        if (goc != null)
        {
            goc.playUpdate = false;
            EventSystem.current.SetSelectedGameObject(null);
        }

        if (byRechoose && fadeWait <= 0)
        {
            AudioListener.pause = false;
            PauseController.canPause = true;
            PauseController.gamePaused = false;
            Time.timeScale = 1;
            SceneManager.LoadScene("Skill_Choose");
        }

        else if (byMenu && fadeWait <= 0)
        {
            AudioListener.pause = false;
            PauseController.canPause = true;
            PauseController.gamePaused = false;
            Time.timeScale = 1;
            SceneManager.LoadScene("Main_Menu");
        }

        else if (byRetry && fadeWait <= 0)
        {
            AudioListener.pause = false;
            PauseController.canPause = true;
            PauseController.gamePaused = false;
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        else if (byChooseSkills && fadeWait <= 0)
        {
            AudioListener.pause = false;
            PauseController.canPause = true;
            PauseController.gamePaused = false;
            Time.timeScale = 1;
            lc.StartLoading();
        }

        else if (bySkipIntro && fadeWait <= 0)
        {
            AudioListener.pause = false;
            PauseController.canPause = true;
            PauseController.gamePaused = false;
            Time.timeScale = 1;
            SceneManager.LoadScene("Skill_Choose");
        }
    }

    public void NoSelected()
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

        currentSelectedButton = noButton;

        noText.color = selectedColor;
        yesText.color = unselectColor;

        noButton.transform.localScale = new Vector3(heightMod, widthtMod, 0);
        yesButton.transform.localScale = new Vector3(1, 1, 0);
    }

    public void NoSubmit()
    {
        FindObjectOfType<AudioManager>().PlaySound("ChoiceSelect");

        ConfirmQuit();
    }
}
