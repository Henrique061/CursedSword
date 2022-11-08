using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TongueBattleManager : MonoBehaviour
{
    [SerializeField] private Animator anim;
    [SerializeField] private float tongueStayTimer = 10;
    [SerializeField] private Health he;
    [SerializeField] private GameObject tongueObj;

    [Header("Spikes")]
    [SerializeField] private SpriteRenderer[] srSpikes;

    private GaiaBattleManager gbm;
    private GaiaSecondStage gss;
    private GaiaThirdStage gts;
    private SpikeBattle sb;

    [HideInInspector] public bool spikeReturn = false;
    [HideInInspector] public bool isSecondStage = false; // to activate second stage on spikeBattle
    [HideInInspector] public bool isThirdStage = false; // to activate third stage on spikeBattle
    private bool tongueDrop = true;
    private bool tongueStay = false;
    private bool tongueUp = false;
    private bool tongueNull = false;
    private bool killedTongue = false;

    private Color[] spikeColors;

    [HideInInspector] public float stageHealth = 125;
    private float fixedTongueStayTimer;

    private int globalStage;

    private void Awake()
    {
        gbm = GetComponent<GaiaBattleManager>();
        gss = GetComponent<GaiaSecondStage>();
        gts = GetComponent<GaiaThirdStage>();
        sb = GetComponent<SpikeBattle>();

        spikeColors = new Color[5];

        for (int i = 0; i < srSpikes.Length; i++)
            spikeColors[i] = srSpikes[i].GetComponent<SpriteRenderer>().color;
    }

    private void Start()
    {
        fixedTongueStayTimer = tongueStayTimer;

        for (int i = 0; i < spikeColors.Length; i++)
        {
            spikeColors[i].b = 1;
            spikeColors[i].g = 1;
        }
    }

    private void Update()
    {
        if (!PauseController.gamePaused)
        {
            if (tongueStay)
            {
                if (globalStage == 1)
                {
                    if (stageHealth <= 0 || tongueStayTimer <= 0 || he.currentHealth <= 750)
                    {
                        tongueStay = false;
                        tongueUp = true;
                        TongueRecover(globalStage);
                    }
                }

                if (globalStage == 2)
                {
                    if (stageHealth <= 0 || tongueStayTimer <= 0 || he.currentHealth <= 400)
                    {
                        tongueStay = false;
                        tongueUp = true;
                        TongueRecover(globalStage);
                    }
                }

                if (globalStage == 3)
                {
                    if (stageHealth <= 0 || tongueStayTimer <= 0 || he.currentHealth <= 0)
                    {
                        tongueStay = false;
                        tongueUp = true;
                        TongueRecover(globalStage);
                    }
                }

                tongueStayTimer -= Time.deltaTime;
            }
        }
    }

    public void TongueAppear(int stage) // triggers: Drop, Stay, Up, Null
    {
        foreach (Health health in sb.spikeHe)
            health.currentHealth = 120;

        for (int i = 0; i < srSpikes.Length; i++)
            srSpikes[i].color = spikeColors[i];

        FindObjectOfType<AudioManager>().PlaySound("DropTongue");
        globalStage = stage;
        tongueObj.SetActive(true);
        anim.SetTrigger("Drop");
        StartCoroutine("WaitTongueAnim", 0.74f);
    }

    IEnumerator WaitTongueAnim(float time)
    {
        yield return new WaitForSeconds(time);

        if (tongueDrop)
        {
            tongueDrop = false;
            anim.SetTrigger("Stay");
            tongueStay = true;
        }

        else if (tongueUp)
        {
            if (he.currentHealth > 750)
            {
                stageHealth = 128;
            }

            else if (he.currentHealth > 400 && he.currentHealth <= 750)
            {
                isSecondStage = true;
                stageHealth = 178;
            }

            else if (he.currentHealth <= 400)
            {
                isThirdStage = true;
                isSecondStage = false;
                stageHealth = 210;
            }

            
            tongueStayTimer = fixedTongueStayTimer;
            tongueObj.SetActive(false);
            anim.SetTrigger("Null");
            tongueUp = false;
            tongueNull = true;
            StartCoroutine("WaitTongueAnim", 2f);
        }


        else if (tongueNull)
        {
            tongueNull = false;
            tongueDrop = true;
            spikeReturn = true;

            if (globalStage == 2)
                StartCoroutine(sb.Wait(2));

            else if (globalStage == 3)
                StartCoroutine(sb.Wait(3));

            else
                StartCoroutine(sb.Wait(1));
        }
    }

    private void TongueRecover(int stage)
    {
        anim.SetTrigger("Up");

        for (int i = 0; i < sb.spikeBreaked.Length; i++)
            sb.spikeBreaked[i] = false;

        StartCoroutine("WaitTongueAnim", 0.74f);
    }
}
