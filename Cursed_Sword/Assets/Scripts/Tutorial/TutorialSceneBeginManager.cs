using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSceneBeginManager : MonoBehaviour
{
    [SerializeField] private MageMovement mm;
    [SerializeField] private GameObject blackFade;
    [SerializeField] private Animator blackAnim;
    [SerializeField] private GameObject pressToWalk;
    [SerializeField] private Transform mmTranform;
    [SerializeField] private GameObject gaiaRoom;
    [SerializeField] private GameObject beforeSwordRoom;
    [SerializeField] private GameObject swordRoom;
    [SerializeField] private GameObject gaiaObj;
    [SerializeField] private Animator mageAnim;
    [SerializeField] private Animator pressWalkAnim;
    [SerializeField] private GameObject pressWalkObj;
    [SerializeField] private MageCollisionDetect mcd;
    [SerializeField] private Collider2D mageCollider;
    [SerializeField] private Transform leftLimit;
    [SerializeField] private ConfirmController confirmController;
    [SerializeField] private GameObject fadesObj;
    [SerializeField] private GameObject blackObj;

    private int alreadyPlayed;

    private TutorialManager tm;

    private bool startScene = false;
    private bool startPress = false;
    private bool startedTransition1 = false;
    private bool waitTransition1 = false;
    private bool beginScene2 = false;
    private bool startedTransition2 = false;
    private bool waitTransition2 = false;
    private bool waitMagicCall = false;

    [HideInInspector] public bool transition1 = false;
    [HideInInspector] public bool transition2 = false;

    private float timer = 1;

    private void Awake()
    {
        tm = GetComponent<TutorialManager>();
        SkillChooseController.skill1 = "lowflight";
        SkillChooseController.skill2 = "earthquake";

        alreadyPlayed = PlayerPrefs.GetInt("AlreadyPlayed");
    }

    private void Start()
    {
        if (alreadyPlayed == 0)
        {
            startScene = true;
            mm.canMove = false;
            PauseController.canPause = false;
            fadesObj.SetActive(true);
            FindObjectOfType<AudioManager>().PlaySound("BGMusic");
        }

        else
        {
            PauseController.canPause = false;
            mm.canMove = false;
            blackObj.SetActive(true);
            confirmController.SkipIntroCalled();
        }
    }

    private void Update()
    {
        if (!PauseController.gamePaused)
        {
            if (startScene) // start tutorial scene
            {
                if (timer <= 0)
                {
                    startScene = false;
                    mm.canMove = true;
                    PauseController.canPause = true;
                    timer = 0.5f;
                    startPress = true;
                }

                else
                    timer -= Time.deltaTime;
            }

            if (startPress) // start press to move anim
            {
                if (timer <= 0)
                {
                    startPress = false;
                    pressToWalk.SetActive(true);
                    timer = 2;
                }

                else
                    timer -= Time.deltaTime;
            }

            if (transition1) // transitioned first time
            {
                if (!startedTransition1)
                {
                    PauseController.canPause = false;
                    mm.canMove = false;
                    mm.rb.velocity = Vector2.zero;

                    pressWalkAnim.SetTrigger("Fade");
                    mageAnim.SetFloat("Walk", 0);

                    blackAnim.SetTrigger("BlackIn");
                    startedTransition1 = true;
                }

                if (timer <= 0)
                {
                    transition1 = false;
                    timer = 1;
                    waitTransition1 = true;
                }

                else
                    timer -= Time.deltaTime;
            }

            if (waitTransition1) // wait between black and scene appear 1
            {
                if (timer <= 0)
                {
                    mm.transform.position = new Vector2(7.66f, mm.transform.position.y);

                    if (mm.facingRight)
                        mm.facingRight = false;

                    gaiaRoom.SetActive(false);
                    beforeSwordRoom.SetActive(true);
                    gaiaObj.SetActive(false);
                    pressWalkObj.SetActive(false);

                    blackAnim.ResetTrigger("BlackIn");
                    waitTransition1 = false;
                    timer = 2;
                    blackAnim.SetTrigger("BlackOut");
                    beginScene2 = true;
                }

                else
                    timer -= Time.deltaTime;
            }

            if (beginScene2) // wait animation to appear scene 1
            {
                if (timer <= 0)
                {
                    mcd.canTransition2 = true;
                    beginScene2 = false;
                    PauseController.canPause = true;
                    mm.canMove = true;
                    timer = 2;
                }

                else
                    timer -= Time.deltaTime;
            }

            if (transition2) // transitioned second time
            {
                if (!startedTransition2)
                {
                    PauseController.canPause = false;
                    mm.canMove = false;
                    mm.rb.velocity = Vector2.zero;

                    mageAnim.SetFloat("Walk", 0);

                    blackAnim.SetTrigger("BlackIn");
                    startedTransition2 = true;
                }

                if (timer <= 0)
                {
                    transition2 = false;
                    timer = 1;
                    waitTransition2 = true;
                }

                else
                    timer -= Time.deltaTime;
            }

            if (waitTransition2) // wait between black and scene appear 2
            {
                if (timer <= 0)
                {
                    mm.transform.position = new Vector2(5.68f, mm.transform.position.y);
                    mm.rb.gravityScale = 0;
                    mageCollider.isTrigger = true;

                    if (mm.facingRight)
                        mm.facingRight = false;

                    leftLimit.position = new Vector2(4.37f, leftLimit.position.y);

                    beforeSwordRoom.SetActive(false);
                    swordRoom.SetActive(true);

                    blackAnim.ResetTrigger("BlackIn");
                    waitTransition2 = false;
                    blackAnim.SetTrigger("BlackOut");
                    PauseController.canPause = true;

                    timer = 2f;
                    waitMagicCall = true;

                }

                else
                    timer -= Time.deltaTime;
            }

            if (waitMagicCall) // wait between black and scene appear 2
            {
                if (timer <= 0)
                {
                    waitMagicCall = false;
                    tm.StartTutorial();
                }

                else
                    timer -= Time.deltaTime;
            }
        }
    }

    public void PlayIntro()
    {
        fadesObj.SetActive(true);
        startScene = true;
        mm.canMove = false;
        blackObj.SetActive(false);
        PauseController.canPause = false;
        FindObjectOfType<AudioManager>().PlaySound("BGMusic");
    }
}
