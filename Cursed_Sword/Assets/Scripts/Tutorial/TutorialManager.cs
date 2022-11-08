using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{
    [Header("Tombs")]
    [SerializeField] private GameObject lockedTomb;
    [SerializeField] private GameObject breakedTomb;

    [Header("Mage")]
    [SerializeField] private Animator mageAnim;
    [SerializeField] private ParticleSystem magicParticle;

    [Header("Sword")]
    [SerializeField] private GameObject swordObj;

    [Header("White Fade")]
    [SerializeField] private GameObject whiteObj;
    [SerializeField] private Animator whiteAnim;

    [Header("Sword Params")]
    [SerializeField] private CharacterController cc;
    [SerializeField] private CharacterMovement cm;
    [SerializeField] private Skill sk;
    [SerializeField] private CharacterDamage cd;

    [Header("Training Objects")]
    [SerializeField] private GameObject trainingDummy;
    [SerializeField] private GameObject trainingBarricade;

    [Header("Tutorial Texts")]
    [SerializeField] private GameObject pressAttack;
    [SerializeField] private GameObject pressJump;
    [SerializeField] private GameObject pressSkill1;
    [SerializeField] private GameObject pressSkill2;

    [Header("Dummy Health")]
    [SerializeField] private Health dummyHealth;

    [Header("Skill Call Collider")]
    [SerializeField] private GameObject skillCallCollider;

    [Header("Black Fade")]
    [SerializeField] private GameObject blackFade;
    [SerializeField] private Animator blackAnim;

    [Header("Audio")]
    [SerializeField] private AudioMixer am;
    [SerializeField] private AudioMixerSnapshot fadeSnapshot;
    [SerializeField] private AudioMixerSnapshot mainSnapshot;

    private bool makeMagicAnim = false;
    private bool waitParticle = false;
    private bool whiteFade = true;
    private bool waitWhiteFade = false;
    private bool canKillDummy = false;
    private bool dummyKilled = false;
    private bool jumping = false;
    private bool waitingSkill1 = false;
    private bool waitingSkill1End = false;
    private bool waitingSkill2 = false;
    private bool waitingSkill2End = false;
    private bool waitEarthquake = false;
    private bool endStarted = false;
    private bool goNextScene = false;
    private bool whiteBegun = false;
    private bool whiteFadingIn = false;

    private float timer;

    private void Update()
    {
        if (!PauseController.gamePaused)
        {
            if (makeMagicAnim) // mage made the magic anim
            {
                
                if (timer <= 0)
                {
                    makeMagicAnim = false;
                    whiteFadingIn = false;

                    FindObjectOfType<AudioManager>().PlaySound("Magic");
                    magicParticle.Play();
                    timer = 1;
                    whiteBegun = false;
                    waitParticle = true;
                    whiteFade = true;
                }

                else
                    timer -= Time.deltaTime;
            }

            if (waitParticle) // waiting for magic particle
            {

                if (timer <= 0)
                {
                    if (whiteFade) // white screen right appear
                    {
                        if (!whiteBegun)
                        {
                            if (!whiteFadingIn)
                            {
                                whiteObj.SetActive(true);
                                StartCoroutine("WhiteFadeBegin");
                                whiteFadingIn = true;
                            }
                        }

                        else if (!dummyKilled)
                        {
                            waitParticle = false;
                            whiteFade = false;
                            
                            timer = 3.45f;
                            mageAnim.SetTrigger("EndMagic");

                            lockedTomb.SetActive(false);
                            breakedTomb.SetActive(true);
                            trainingDummy.SetActive(true);

                            swordObj.SetActive(true);

                            cc.canAttack = false;
                            cd.cannotAttack = true;
                            cm.canWalk = false;
                            cc.canJump = false;
                            sk.canUseSkill = false;

                            waitWhiteFade = true;
                        }

                        else
                        {
                            waitParticle = false;
                            whiteFade = false;

                            pressAttack.SetActive(false);
                            timer = 3.45f;
                            mageAnim.SetTrigger("EndMagic");

                            swordObj.transform.position = new Vector2(-7.69f, swordObj.transform.position.y);

                            cc.canAttack = false;
                            cd.cannotAttack = true;
                            cm.canWalk = false;
                            cc.canJump = false;
                            sk.canUseSkill = false;

                            dummyHealth.currentHealth = 50;
                            trainingDummy.SetActive(false);
                            trainingBarricade.SetActive(true);
                            skillCallCollider.SetActive(true);

                            waitWhiteFade = true;
                        }
                    }
                }

                else
                    timer -= Time.deltaTime;
            }

            if (waitWhiteFade) // waiting for white fade to finish
            {
                if (timer <= 0)
                {
                    waitWhiteFade = false;
                    whiteObj.SetActive(false);

                    if (!dummyKilled) // dummy appear
                    {
                        pressAttack.SetActive(true);
                        canKillDummy = true;

                        cc.canAttack = true;
                        cd.cannotAttack = false;
                        cm.canWalk = true;
                    }

                    else if (!jumping) // barricades appear
                    {
                        jumping = true;
                        pressJump.SetActive(true);

                        cm.canWalk = true;
                        cc.canJump = true;
                    }
                }

                else
                    timer -= Time.deltaTime;
            }

            if (canKillDummy) // kill the training dummy
            {
                if (dummyHealth.currentHealth <= 0)
                {
                    canKillDummy = false;
                    dummyKilled = true;

                    MagicAnim();
                }
            }

            if (waitingSkill1)
            {
                if (sk.lowflightUsed) // skill start
                {
                    pressSkill1.SetActive(false);
                    Time.timeScale = 1;
                    waitingSkill1 = false;
                    waitingSkill1End = true;
                }
            }

            if (waitingSkill1End)
            {
                if (!sk.lowflightUsed) // skill end
                {
                    waitingSkill1End = false;
                    sk.canSkill1 = false;
                    sk.canSkill2 = true;

                    pressSkill2.SetActive(true);

                    waitingSkill2 = true;

                    Time.timeScale = 0;
                }
            }

            if (waitingSkill2)
            {
                if (sk.earthquakeUsed) // skill start
                {
                    pressSkill2.SetActive(false);
                    Time.timeScale = 1;
                    waitingSkill2 = false;
                    waitingSkill2End = true;
                }
            }

            if (waitingSkill2End)
            {
                if (!sk.earthquakeUsed) // skill end
                {
                    waitingSkill2End = false;
                    sk.canSkill1 = true;
                    sk.canUseSkill = false;

                    StartCoroutine("WaitEarthquake");
                }
            }

            if (endStarted)
            {
                if (timer <= 0)
                {
                    endStarted = false;

                    FindObjectOfType<AudioManager>().StopAll();
                    mainSnapshot.TransitionTo(0.01f);
                    am.SetFloat("masterVol", PlayerPrefs.GetFloat("masterVol"));
                    am.SetFloat("musicVol", PlayerPrefs.GetFloat("musicVol"));
                    am.SetFloat("soundVol", PlayerPrefs.GetFloat("soundVol"));

                    timer = 2;

                    goNextScene = true;
                }

                else
                    timer -= Time.deltaTime;
            }

            if (goNextScene) // go to cutscene
            {
                if (timer <= 0)
                {
                    PauseController.canPause = true;
                    SceneManager.LoadScene("Cutscene");
                }

                else
                    timer -= Time.deltaTime;
            }
        }
    }

    public void StartTutorial()
    {
        MagicAnim();
    }

    private void MagicAnim()
    {
        //cc.canAttack = false;
        //cd.cannotAttack = true;
        //cm.canWalk = false;
        //cc.canJump = false;
        //sk.canUseSkill = false;

        mageAnim.SetTrigger("Magic");
        timer = 0.5f;
        makeMagicAnim = true;
    }

    public void PressSkill()
    {
        cm.canWalk = false;
        cc.canJump = false;
        sk.canSkill2 = false;
        sk.canUseSkill = true;

        pressJump.SetActive(false);
        pressSkill1.SetActive(true);

        waitingSkill1 = true;

        PauseController.canPause = false;
        Time.timeScale = 0;
    }

    private IEnumerator WaitEarthquake()
    {
        cm.canWalk = false;
        cc.canJump = false;

        yield return new WaitForSeconds(1);

        TutorialEnd();
    }

    private IEnumerator WhiteFadeBegin()
    {
        yield return new WaitForSeconds(0.5f);

        whiteBegun = true;
    }

    private void TutorialEnd()
    {
        am.ClearFloat("soundVol");
        am.ClearFloat("masterVol");
        am.ClearFloat("musicVol");
        fadeSnapshot.TransitionTo(2f);

        timer = 2f;

        blackAnim.SetTrigger("BlackIn");

        endStarted = true;
    }
}
