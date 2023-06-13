using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaiaBattleManager : MonoBehaviour
{
    #region Inspector
    [Header("Instantiations")]
    public GameObject[] foldedVines; 
    public GameObject[] risedVines; 
    public Animator[] risedVinesAnims;
    public Animator[] foldedVinesAnims;
    public GameObject allBelowVines;
    public GameObject[] evenBelowVines;
    public GameObject[] oddBelowVines;
    public GameObject[] oneByOneBelowVines;

    [Header("Plant Parts")]
    public GameObject tongue;
    public Animator tongueAnim;

    [Header("Time to Begin")]
    [SerializeField] private float timeToBegin = 1; // time to really begin the battle after BattleBeginManager

    [Header("Fold Vines")]
    [SerializeField] private float firstFoldedVel = 1;
    private float fixedFirstFoldedVel;

    #endregion

    #region Instantiations

    private BattleBeginManager bbm;
    private SpikeBattle sb;

    #endregion

    #region First Stage

    private bool chooseFirstStage = true;
    private bool wasFirstStage = false;
    private bool firstVineFold = false;
    private bool rndmFoldVine = false;
    private bool returnFirstStage = false;
    private int foldVineIndex;

    #endregion

    #region Second Stage

    [HideInInspector] public bool isSecondStage = false; // to trigger on tongue battle managaer

    #endregion

    #region Spike

    [HideInInspector] public int spikeCounter = 0;
    private float spikeChance;
    private bool startSpike = false;

    #endregion

    #region Others

    private bool battleBegin = false;
    private int[] rndmNumbers;
    private int numbersQuantity;
    [HideInInspector] public bool playUpdate = true;
    private float foldAnimTime = 2.083f;
    private float fixedFoldAnimTime = 2.083f;
    private bool startedFoldAnim = false;
    private bool tongueDrop = true;
    private bool tongueStay = false;

    #endregion

    private void Awake()
    {
        bbm = GetComponent<BattleBeginManager>();
        sb = GetComponent<SpikeBattle>();

        rndmNumbers = new int[6];
        fixedFirstFoldedVel = firstFoldedVel;
}

    private void Update()
    {
        if (!PauseController.gamePaused)
        {
            #region Begin Battle

            if (bbm.battleBegin)
            {
                if (!battleBegin)
                {
                    if (timeToBegin <= 0)
                        battleBegin = true;

                    timeToBegin -= Time.deltaTime;
                }
            }

            #endregion

            //////////////////////////////////////////////////////////////

            if (battleBegin)
            {
                if (playUpdate)
                {
                    if (!isSecondStage)
                    {
                        if (startedFoldAnim)
                        {
                            foldAnimTime -= Time.deltaTime;
                        }

                        #region First Stage

                        if (chooseFirstStage) // first stage ///////////////////////////////////////////////////////////
                        {
                            numbersQuantity = RandomNumbersFirst(1);

                            for (int i = 0; i < numbersQuantity; i++)
                            {
                                risedVinesAnims[rndmNumbers[i]].SetTrigger("Dig");
                                StartCoroutine("WaitFoldedVine", 2);
                                wasFirstStage = true;
                                chooseFirstStage = false;
                            }

                        } // close choose first stage

                        if (firstVineFold)
                        {
                            if (rndmNumbers[0] % 2 == 0)
                            {
                                if (rndmFoldVine)
                                {
                                    foldVineIndex = Random.Range(0, 3);
                                    foldedVines[foldVineIndex].SetActive(true);
                                    foldedVinesAnims[foldVineIndex].SetBool("Go", true);
                                    rndmFoldVine = false; // to stop entering this if
                                    startedFoldAnim = true;
                                }

                                if (foldAnimTime <= 0)
                                {
                                    foldedVinesAnims[foldVineIndex].SetBool("Go", false);
                                    foldedVines[foldVineIndex].SetActive(false);
                                    firstVineFold = false;
                                    startedFoldAnim = false;
                                    foldAnimTime = fixedFoldAnimTime;
                                    returnFirstStage = true;
                                    risedVinesAnims[rndmNumbers[0]].SetTrigger("Wait");
                                    spikeCounter++;
                                    StartCoroutine(WaitRiseVine(2, rndmNumbers[0]));
                                }
                            }

                            else
                            {
                                if (rndmFoldVine)
                                {
                                    foldVineIndex = Random.Range(3, 6);
                                    foldedVines[foldVineIndex].SetActive(true);
                                    foldedVinesAnims[foldVineIndex].SetBool("Go", true);
                                    rndmFoldVine = false; // to stop entering this if
                                    startedFoldAnim = true;
                                }

                                if (foldAnimTime <= 0)
                                {
                                    foldedVinesAnims[foldVineIndex].SetBool("Go", false);
                                    foldedVines[foldVineIndex].SetActive(false);
                                    firstVineFold = false;
                                    startedFoldAnim = false;
                                    foldAnimTime = fixedFoldAnimTime;
                                    returnFirstStage = true;
                                    risedVinesAnims[rndmNumbers[0]].SetTrigger("Wait");
                                    spikeCounter++;
                                    StartCoroutine(WaitRiseVine(2, rndmNumbers[0]));
                                }
                            }

                        } // close vine attack

                        #endregion

                    } // close isSecondStage

                } // close playupdate

            } // close battlebegin

        } // close game paused

    } // close update

    private int RandomNumbersFirst(int numbers)
    {
        for (int i = 0; i < numbers; i++)
        {
            if (i == 0)
            {
                rndmNumbers[i] = Random.Range(0, 2);
            }

            else
            {
                rndmNumbers[i] = Random.Range(0, 2);

                while (rndmNumbers[i] == rndmNumbers[i - 1])
                    rndmNumbers[i] = Random.Range(0, 2);
            }
        }

        return numbers;
    }

    IEnumerator WaitFoldedVine(float wait)
    {
        yield return new WaitForSeconds(wait);

        if (wasFirstStage)
        {
            wasFirstStage = false;
            rndmFoldVine = true;
            firstVineFold = true;
        }
    }

    IEnumerator WaitRiseVine(float wait, int index)
    {
        yield return new WaitForSeconds(wait);

        risedVinesAnims[index].SetTrigger("Rise");
        FindObjectOfType<AudioManager>().PlaySound("VineRise");
        StartCoroutine("WaitTime");
    }

    IEnumerator WaitTime()
    {
        yield return new WaitForSeconds(2);

        if (returnFirstStage)
        {
            if (spikeCounter >= 1)
                SpikeCall();

            wasFirstStage = true;
            returnFirstStage = false;
            chooseFirstStage = true;
        }
    }

    private void SpikeCall()
    {
        if (spikeCounter == 2)
            spikeChance = Random.Range(0, 100);

        else if (spikeCounter == 3)
            spikeChance = Random.Range(25, 100);

        if (spikeCounter == 2 || spikeCounter == 3)
        {
            if (spikeChance >= 50)
            {
                startSpike = true;

                chooseFirstStage = false;
                firstVineFold = false;
                spikeCounter = 0;
            }
        }

        if (spikeCounter >= 4)
        {
            startSpike = true;

            chooseFirstStage = false;
            firstVineFold = false;
            spikeCounter = 0;
        }

        if (startSpike)
        {
            playUpdate = false;
            sb.BeginSpike(1);
        }
    }

}
