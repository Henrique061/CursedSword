using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class GameOverController : MonoBehaviour
{
    [SerializeField] private AudioMixer am;

    [Header("Snapshot")]
    [SerializeField] private AudioMixerSnapshot fadeSnapshot;
    [SerializeField] private AudioMixerSnapshot mainSnapshot;

    [Header("GameOver Object && Animator")]
    [SerializeField] private GameObject gameOverObj;
    [SerializeField] private GameObject gameOverAnim;

    [Header("Initial Button")]
    [SerializeField] private GameObject initialSelectedButton;

    [Header("Button")]
    [SerializeField] private GameObject retryButton;
    [SerializeField] private GameObject rechooseButton;
    [SerializeField] private GameObject menuButton;

    [Header("Text Colors")]
    [SerializeField] private SpriteRenderer white;
    [SerializeField] private SpriteRenderer colorful;

    [Header("Menu Texts")]
    [SerializeField] private Text retryText;
    [SerializeField] private Text rechooseText;
    [SerializeField] private Text menuText;

    [Header("Height Width Modifier")]
    [SerializeField] private float heightMod = 1.2f;
    [SerializeField] private float widthtMod = 1.2f;

    private Animation deadAnim;

    private CharacterController cc;
    private CharacterMovement cm;
    private CharacterDamage cd;
    private Health he;
    private Skill sk;
    private Animator anim;
    private Rigidbody2D rb;

    private GameObject currentSelectedButton;

    private Color selectedColor;
    private Color unselectColor;

    [HideInInspector] public bool playUpdate = false;
    private bool checkLife = true;
    private bool countDieTime = false;
    private bool checkGOAnim = false;
    private bool[] alreadySelected;

    private float timer = 2.5f;

    private void Awake()
    {
        cc = GetComponent<CharacterController>();
        cm = GetComponent<CharacterMovement>();
        he = GetComponent<Health>();
        anim = GetComponent<Animator>();
        sk = GetComponent<Skill>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CharacterDamage>();

        alreadySelected = new bool[3];

        for (int i = 0; i < alreadySelected.Length; i++)
            alreadySelected[i] = false;

        selectedColor = colorful.GetComponent<SpriteRenderer>().color;
        unselectColor = white.GetComponent<SpriteRenderer>().color;
    }

    private void Update()
    {
        if (checkLife)
        {
            if (he.currentHealth <= 0)
            {
                checkLife = false;
                PlayerDead();
            }
        }

        if (countDieTime)
        {
            if (timer <= 0)
            {
                am.ClearFloat("soundVol");
                am.ClearFloat("masterVol");
                am.ClearFloat("musicVol");
                fadeSnapshot.TransitionTo(1f);
                countDieTime = false;

                gameOverObj.SetActive(true);
                timer = 4f;
                checkGOAnim = true;
                //Time.timeScale = 0;
            }

            else
                timer -= Time.unscaledDeltaTime;
        }

        if (checkGOAnim)
        {
            if (timer <= 0)
            {
                FindObjectOfType<AudioManager>().StopAll();
                mainSnapshot.TransitionTo(0.01f);
                am.SetFloat("masterVol", PlayerPrefs.GetFloat("masterVol"));
                am.SetFloat("musicVol", PlayerPrefs.GetFloat("musicVol"));
                am.SetFloat("soundVol", PlayerPrefs.GetFloat("soundVol"));
                Time.timeScale = 0;
                checkGOAnim = false;
                currentSelectedButton = initialSelectedButton;
                playUpdate = true;
                GO_RetrySelected();
            }

            else
                timer -= Time.unscaledDeltaTime;
        }

        if (playUpdate)
        {
            EventSystem.current.SetSelectedGameObject(currentSelectedButton);
        }
    }

    private void PlayerDead()
    {
        rb.velocity = Vector2.zero;
        PauseController.canPause = false;
        PauseController.gamePaused = false;
        cc.canAttack = false;
        cc.canJump = false;
        cm.canWalk = false;
        sk.canUseSkill = false;
        he.damageable = false;
        cc.attackDelay = false;
        cd.cannotAttack = true;
        rb.gravityScale = 1;

        anim.SetTrigger("Dead");

        countDieTime = true;
    }

    public void GO_RetrySelected()
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

        currentSelectedButton = retryButton;

        rechooseText.color = unselectColor;
        menuText.color = unselectColor;
        retryText.color = selectedColor;

        rechooseButton.transform.localScale = new Vector3(1, 1, 0);
        menuButton.transform.localScale = new Vector3(1, 1, 0);
        retryButton.transform.localScale = new Vector3(widthtMod, heightMod, 0);
    }

    public void GO_RechooseSelected()
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

        currentSelectedButton = rechooseButton;

        rechooseText.color = selectedColor;
        menuText.color = unselectColor;
        retryText.color = unselectColor;

        rechooseButton.transform.localScale = new Vector3(widthtMod, heightMod, 0);
        menuButton.transform.localScale = new Vector3(1, 1, 0);
        retryButton.transform.localScale = new Vector3(1, 1, 0);
    }

    public void GO_MenuSelected()
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

        currentSelectedButton = menuButton;

        rechooseText.color = unselectColor;
        menuText.color = selectedColor;
        retryText.color = unselectColor;

        rechooseButton.transform.localScale = new Vector3(1, 1, 0);
        menuButton.transform.localScale = new Vector3(widthtMod, heightMod, 0);
        retryButton.transform.localScale = new Vector3(1, 1, 0);
    }
}
