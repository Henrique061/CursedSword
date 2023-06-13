using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaiaThirdStage : MonoBehaviour
{
    public Animator[] thirdFoldedVinesAnims;
    public GameObject[] thirdFoldedVines;

    private GaiaSecondStage gss;
    private GaiaBattleManager gbm;
    private SpikeBattle sb;

    #region General and Choose Attack

    [HideInInspector] public bool isThirdStage = false; // to call thirdstage

    private bool playUpdate = false; // to play the update method;
    private bool dontActivate = false; // to not activate variables when entereting the coroutine "WaitController"
    private bool supremeAttack = false; // to know when is a supreme attack

    private int whatAttack; // to draw what attack will be performed

    #endregion

    #region FoldVineAttack

    private bool activateFolded = false; // to activate the foldd actions on update;
    private bool activateFoldedMet = false; // to activate the folded attack method;
    private bool startFoldAttack = true; // to not call the "VineFoldAttack" twice while in coroutine
    private bool pauseFoldAttack = false; // to continue the fold attack in sequential
    private bool waitFinishFoldAttack = false; // to wait the finish action of fold attack
    private bool finishFoldedAction = false; // to finish the fold attack action
    private bool rightFoldAttack = true; // to declare which side will come the attack on supreme
    private bool[] continueFoldAttack; // to continue the fold attack in sequential

    private int[] riseIndexes; // to choose which rised vines will interact
    private int[] foldedIndexes; // to select which foldd vine will attack
    private int continueFoldIndex = 0; // to count how many repeats will have on fold attack
    private int firstFoldVine; // to store which folded vine has already choosed
    private int foldedSupremeIndex = -1; // the index of vine folds to attack on supreme

    #endregion

    #region BelowVineAttack

    [Header("Below Vines")]
    [SerializeField] private float belowVineVel = 0.3f;

    private bool belowVinesAlreadyCalled = false; // to call the playUpdate only one time
    private bool evenVinePrepare = false; // to call the even below vines in update
    private bool oddVinePrepare = false; // to call the even below vines in update
    private bool belowWillAttack = false; // to call in WaitController to wait the preper to attack 
    private bool belowAttack = false; // to cal the BelowVineAttack in update
    private bool belowSound = true; // to reproduce the below vine attack sound only one time
    private bool belowWillDescend = false; // to call the finish below action on waitcontroller
    private bool belowDescend = false; // to call the finish below vine acton in update
    private bool belowFinish = false; // finish the below vine action
    private bool repeatBelowAttack = false; // to repeat the below attack
    private bool endBelowAttack = false; // to finish the below atack

    private int evenOrOdd; // to choose what vines will attack from below
    private int belowAttackCounter = 0; // to count how many attack habe been activated
    private int totalBelowRepeat; // to know how many repeats the below attack will do
    private int otherEvenOdd; // to store the other choice not choosed in evenOrOdd

    private float fixedBelowVineVel; // to fix the below vine vel preparation

    #endregion

    #region Spike

    private int spikeCounter = 0; // to count when to call the SpikeCall
    private int spikeChance; // to see what the chances to call the spike action (if > 50, will be called)

    #endregion

    private void Awake()
    {
        gss = GetComponent<GaiaSecondStage>();
        gbm = GetComponent<GaiaBattleManager>();
        sb = GetComponent<SpikeBattle>();

        riseIndexes = new int[3];
        foldedIndexes = new int[3];
        continueFoldAttack = new bool[5];

        for (int i = 0; i < continueFoldAttack.Length; i++)
            continueFoldAttack[i] = false;

        fixedBelowVineVel = belowVineVel;
    }

    private void Update()
    {
        if (!PauseController.gamePaused)
        {
            if (playUpdate) // activated in BelowVinePrepare and WaitController (BelowVineAttack)
            {
                if (evenVinePrepare || oddVinePrepare) // activated in BelowVinePrepare
                    BelowVinePrepare();

                if (belowAttack) // activated in WaitController (BelowVineAttack)
                    BelowVineAttack();

                if (belowDescend) // activated in WaitController (BelowVineDescend)
                    BelowVineDescend();

            } // close playUpdate
        }
    }

    public void VineAppear() // start of stage 2
    {
        gbm.risedVines[4].SetActive(true);
        gbm.risedVines[5].SetActive(true);

        FindObjectOfType<AudioManager>().PlaySound("VineRise");
        gbm.risedVinesAnims[4].SetTrigger("Rise");
        gbm.risedVinesAnims[5].SetTrigger("Rise");

        StartCoroutine("WaitController", 2);
    }

    public void DrawAttack()
    {
        whatAttack = Random.Range(0, 2);

        if (whatAttack == 0)
            VineFoldBegin();

        else
            BelowVinePrepare();
    }

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region Folded Vine Methods

    private void VineFoldBegin() // side attack
    {
        if (!supremeAttack)
        {
            for (int i = 0; i < riseIndexes.Length; i++) // choosing what vines will attack
            {
                if (i == 0)
                {
                    riseIndexes[i] = Random.Range(0, 6);
                }

                else if (i == 1)
                {
                    riseIndexes[i] = Random.Range(0, 6); // to choose whoever

                    while (riseIndexes[i] == riseIndexes[i - 1])
                        riseIndexes[i] = Random.Range(0, 4);
                }

                else
                {
                    riseIndexes[i] = Random.Range(0, 6); // to choose whoever

                    while (riseIndexes[i] == riseIndexes[i - 1] || riseIndexes[i] == riseIndexes[i - 2])
                        riseIndexes[i] = Random.Range(0, 4);
                }

            }
        }

        // digin animation by the selected vines indexes
        if (!supremeAttack)
        {
            gbm.risedVinesAnims[riseIndexes[0]].SetTrigger("Dig");
            gbm.risedVinesAnims[riseIndexes[1]].SetTrigger("Dig");
            gbm.risedVinesAnims[riseIndexes[2]].SetTrigger("Dig");
        }

        else
        {
            foreach (Animator anim in gbm.risedVinesAnims)
                anim.SetTrigger("Dig");
        }

        activateFolded = true;
        dontActivate = true;
        StartCoroutine("WaitController", 2);

    }

    private void VineFoldAttack() // rised indexes: evens = right, odds = left (Rise, Dig, Wait) |||| folded obj and anims indexes: 0, 1, 2 = right / 3, 4, 5 = left (bool Go)
    {
        if (startFoldAttack) // activated by default, deactivated inside
        {
            if (!supremeAttack)
            {
                for (int i = 0; i < riseIndexes.Length; i++) // attaching the fold indexes by the rised indexes
                {
                    if (riseIndexes[i] % 2 == 0) // if is even, is a vine on the right
                        foldedIndexes[i] = Random.Range(0, 3);

                    else // if is odd, is a vine on the left
                        foldedIndexes[i] = Random.Range(3, 6);
                }

                gbm.foldedVines[foldedIndexes[0]].SetActive(true);
                gss.secondFoldedVines[foldedIndexes[1]].SetActive(true);
                thirdFoldedVines[foldedIndexes[2]].SetActive(true);

                gbm.foldedVinesAnims[foldedIndexes[0]].SetBool("Go", true); // first folded go
            }

            else
            {
                foldedSupremeIndex = Random.Range(0, 3);
                gbm.foldedVines[foldedSupremeIndex].SetActive(true);
                gbm.foldedVinesAnims[foldedSupremeIndex].SetBool("Go", true);
            }

            startFoldAttack = false;
            pauseFoldAttack = true;

            if (!supremeAttack)
                StartCoroutine("WaitController", 1.1f);

            else
                StartCoroutine("WaitController", 0.9f);
        }

        else if (continueFoldAttack[0]) // activated on waitcontroller
        {
            if (!supremeAttack)
            {
                gss.secondFoldedVinesAnims[foldedIndexes[1]].SetBool("Go", true); // second folded go
                StartCoroutine("WaitController", 1.1f);
            }

            else
            {
                foldedSupremeIndex = Random.Range(3, 6);
                gbm.foldedVines[foldedSupremeIndex].SetActive(true);
                gbm.foldedVinesAnims[foldedSupremeIndex].SetBool("Go", true);
                StartCoroutine("WaitController", 0.9f);
            }
        }

        else if (continueFoldAttack[1])
        {
            if (!supremeAttack)
            {
                thirdFoldedVinesAnims[foldedIndexes[2]].SetBool("Go", true);
                continueFoldAttack[1] = false;
                pauseFoldAttack = false;
                waitFinishFoldAttack = true;

                StartCoroutine("WaitController", 3.283f);
            }

            else
            {
                foldedSupremeIndex = Random.Range(0, 3);
                gss.secondFoldedVines[foldedSupremeIndex].SetActive(true);
                gss.secondFoldedVinesAnims[foldedSupremeIndex].SetBool("Go", true);
                StartCoroutine("WaitController", 0.9f);
            }
        }

        else if (continueFoldAttack[2]) // only supreme
        {
            foldedSupremeIndex = Random.Range(3, 6);
            gss.secondFoldedVines[foldedSupremeIndex].SetActive(true);
            gss.secondFoldedVinesAnims[foldedSupremeIndex].SetBool("Go", true);
            StartCoroutine("WaitController", 0.9f);
        }

        else if (continueFoldAttack[3]) // only supreme
        {
            foldedSupremeIndex = Random.Range(0, 3);
            thirdFoldedVines[foldedSupremeIndex].SetActive(true);
            thirdFoldedVinesAnims[foldedSupremeIndex].SetBool("Go", true);
            StartCoroutine("WaitController", 0.9f);
        }

        else if (continueFoldAttack[4]) // only supreme
        {
            foldedSupremeIndex = Random.Range(3, 6);
            thirdFoldedVines[foldedSupremeIndex].SetActive(true);
            thirdFoldedVinesAnims[foldedSupremeIndex].SetBool("Go", true);

            continueFoldAttack[1] = false;
            pauseFoldAttack = false;
            waitFinishFoldAttack = true;

            StartCoroutine("WaitController", 3.283f);
        }
    }

    private void VineFoldFinish()
    {
        if (!supremeAttack)
        {
            gbm.foldedVinesAnims[foldedIndexes[0]].SetBool("Go", false);
            gss.secondFoldedVinesAnims[foldedIndexes[1]].SetBool("Go", false);
            thirdFoldedVinesAnims[foldedIndexes[2]].SetBool("Go", false);

            gbm.foldedVines[foldedIndexes[0]].SetActive(false);
            gss.secondFoldedVines[foldedIndexes[1]].SetActive(false);
            thirdFoldedVines[foldedIndexes[2]].SetActive(false);
        }

        else
        {
            for (int i = 0; i < thirdFoldedVinesAnims.Length; i++)
            {
                gbm.foldedVinesAnims[i].SetBool("Go", false);
                gss.secondFoldedVinesAnims[i].SetBool("Go", false);
                thirdFoldedVinesAnims[i].SetBool("Go", false);
            }

            for (int i = 0; i < thirdFoldedVinesAnims.Length; i++)
            {
                gbm.foldedVines[i].SetActive(false);
                gss.secondFoldedVines[i].SetActive(false);
                thirdFoldedVines[i].SetActive(false);
            }
        }

        ReturnRisedVines();
    }

    private void ReturnRisedVines()
    {
        if (!supremeAttack)
        {
            gbm.risedVines[riseIndexes[0]].SetActive(true);
            gbm.risedVines[riseIndexes[1]].SetActive(true);
            gbm.risedVines[riseIndexes[2]].SetActive(true);

            FindObjectOfType<AudioManager>().PlaySound("VineRise");
            gbm.risedVinesAnims[riseIndexes[0]].SetTrigger("Rise");
            gbm.risedVinesAnims[riseIndexes[1]].SetTrigger("Rise");
            gbm.risedVinesAnims[riseIndexes[2]].SetTrigger("Rise");
        }

        else
        {
            foreach (GameObject vine in gbm.risedVines)
                vine.SetActive(true);

            FindObjectOfType<AudioManager>().PlaySound("VineRise");

            foreach (Animator anim in gbm.risedVinesAnims)
                anim.SetTrigger("Rise");
        }

        finishFoldedAction = true;
        StartCoroutine("WaitController", 2);
    }

    #endregion

    ////////////////////////////////////////////////////////////////////////////////////////////////////////////

    #region Below Vine Methods

    private void BelowVinePrepare() // below attack
    {
        repeatBelowAttack = true;

        if (supremeAttack)
            totalBelowRepeat = 5; // will do 6 attacks

        else
            totalBelowRepeat = 1; // will do 2 attacks

        if (!belowVinesAlreadyCalled) // deactivated by default and in final WaitController, activated inside
        {
            playUpdate = true;
            belowVinesAlreadyCalled = true;
            gbm.allBelowVines.SetActive(true);

            if (!supremeAttack)
            {
                if (belowAttackCounter == 0)
                    evenOrOdd = Random.Range(0, 2);

                else
                    evenOrOdd = otherEvenOdd;
            }

            else
                evenOrOdd = Random.Range(0, 2);
        }

        if (evenOrOdd == 0) // if even
        {
            otherEvenOdd = 1;

            if (!evenVinePrepare) // deactivated by default and in BelowVinePrepare, activated inside
                evenVinePrepare = true;

            if (gbm.evenBelowVines[0].transform.position.y < 0.7)
            {
                foreach (GameObject vine in gbm.evenBelowVines)
                {
                    vine.transform.position = new Vector2(vine.transform.position.x, vine.transform.position.y + belowVineVel);
                }
            }

            else
            {
                evenVinePrepare = false;
                oddVinePrepare = false;
                playUpdate = false;
                belowWillAttack = true;
                dontActivate = true;

                if (belowAttackCounter == 0)
                {
                    if (!supremeAttack)
                        StartCoroutine("WaitController", 1f);

                    else
                        StartCoroutine("WaitController", 0.7f);
                }

                else
                {
                    if (!supremeAttack)
                        StartCoroutine("WaitController", 0.6f);

                    else
                        StartCoroutine("WaitController", 0.4f);
                }
            }
        }

        else // if odd
        {
            otherEvenOdd = 0;

            if (!oddVinePrepare) // deactivated by default and in BelowVinePrepare, activated inside
                oddVinePrepare = true;

            if (gbm.oddBelowVines[0].transform.position.y < 0.7)
            {
                foreach (GameObject vine in gbm.oddBelowVines)
                {
                    vine.transform.position = new Vector2(vine.transform.position.x, vine.transform.position.y + belowVineVel);
                }
            }

            else
            {
                evenVinePrepare = false;
                oddVinePrepare = false;
                playUpdate = false;
                belowWillAttack = true;
                dontActivate = true;

                if (belowAttackCounter == 0)
                {
                    if (!supremeAttack)
                        StartCoroutine("WaitController", 1f);

                    else
                        StartCoroutine("WaitController", 0.7f);
                }

                else
                {
                    if (!supremeAttack)
                        StartCoroutine("WaitController", 0.6f);

                    else
                        StartCoroutine("WaitController", 0.4f);
                }
            }
        }
    }

    private void BelowVineAttack()
    {
        if (belowSound) // activated by default and on final WaitController, deactivated inside
        {
            FindObjectOfType<AudioManager>().PlaySound("VineRise");
            belowVineVel = 2f;
            belowSound = false;
        }

        if (evenOrOdd == 0) // if even
        {
            if (gbm.evenBelowVines[0].transform.position.y < 8)
            {
                foreach (GameObject vine in gbm.evenBelowVines)
                {
                    vine.transform.position = new Vector2(vine.transform.position.x, vine.transform.position.y + belowVineVel);
                }
            }

            else
            {
                belowAttack = false;
                playUpdate = false;
                belowWillDescend = true;

                if (belowAttackCounter < totalBelowRepeat)
                {
                    belowAttackCounter++;

                    if (!supremeAttack)
                        StartCoroutine("WaitController", 1);

                    else
                        StartCoroutine("WaitController", 0.6f);
                }

                else
                {
                    repeatBelowAttack = false;
                    StartCoroutine("WaitController", 2);
                }
            }
        }

        else // if odd
        {
            if (gbm.oddBelowVines[0].transform.position.y < 8)
            {
                foreach (GameObject vine in gbm.oddBelowVines)
                {
                    vine.transform.position = new Vector2(vine.transform.position.x, vine.transform.position.y + belowVineVel);
                }
            }

            else
            {
                belowAttack = false;
                playUpdate = false;
                belowWillDescend = true;

                if (belowAttackCounter < totalBelowRepeat)
                {
                    belowAttackCounter++;

                    if (!supremeAttack)
                        StartCoroutine("WaitController", 1);

                    else
                        StartCoroutine("WaitController", 0.6f);
                }

                else
                {
                    repeatBelowAttack = false;
                    StartCoroutine("WaitController", 2);
                }
            }
        }
    }

    private void BelowVineDescend()
    {
        if (evenOrOdd == 0) // if even
        {
            if (gbm.evenBelowVines[0].transform.position.y > 0)
            {
                foreach (GameObject vine in gbm.evenBelowVines)
                {
                    vine.transform.position = new Vector2(vine.transform.position.x, vine.transform.position.y - belowVineVel);
                }
            }

            else
            {
                belowDescend = false;
                playUpdate = false;
                belowFinish = true;

                if (repeatBelowAttack)
                    StartCoroutine("WaitController", 0.001f);

                else
                    StartCoroutine("WaitController", 2);
            }
        }

        else // if odd
        {
            if (gbm.oddBelowVines[0].transform.position.y > 0)
            {
                foreach (GameObject vine in gbm.oddBelowVines)
                {
                    vine.transform.position = new Vector2(vine.transform.position.x, vine.transform.position.y - belowVineVel);
                }
            }

            else
            {
                belowDescend = false;
                playUpdate = false;
                belowFinish = true;

                if (repeatBelowAttack)
                    StartCoroutine("WaitController", 0.001f);

                else
                    StartCoroutine("WaitController", 2);
            }
        }
    }

    #endregion

    private void SpikeCall()
    {
        spikeCounter = 0;
        sb.BeginSpike(3);       
    }

    private IEnumerator WaitController(float time)
    {
        yield return new WaitForSeconds(time);

        if (!dontActivate) // deactivated in VineFoldBegin
        {
            //Debug.Log("1");
            DrawAttack();
        }

        #region Folded

        else if (activateFolded) // when the rised vine digs (activated in VineFoldBegin)
        {
            //Debug.Log("2");
            if (!supremeAttack)
            {
                gbm.risedVinesAnims[riseIndexes[0]].SetTrigger("Wait");
                gbm.risedVinesAnims[riseIndexes[1]].SetTrigger("Wait");
                gbm.risedVinesAnims[riseIndexes[2]].SetTrigger("Wait");

                gbm.risedVines[riseIndexes[0]].SetActive(false);
                gbm.risedVines[riseIndexes[1]].SetActive(false);
                gbm.risedVines[riseIndexes[2]].SetActive(false);
            }

            else
            {
                foreach (Animator anim in gbm.risedVinesAnims)
                    anim.SetTrigger("Wait");

                foreach (GameObject vine in gbm.risedVines)
                    vine.SetActive(false);
            }

            //drawAttack = false;
            activateFolded = false;
            VineFoldAttack();
            //activateFoldedMet = true;
        }

        else if (pauseFoldAttack) // activated in VineFoldAttack
        {
            //Debug.Log("3");

            if (continueFoldIndex == 0)
            {
                continueFoldAttack[continueFoldIndex] = true;
                continueFoldIndex++;
            }

            else if (continueFoldIndex < 6)
            {
                continueFoldAttack[continueFoldIndex - 1] = false;
                continueFoldAttack[continueFoldIndex] = true;
                continueFoldIndex++;
            }

            VineFoldAttack();
        }

        else if (waitFinishFoldAttack) // activated in VineFoldAttack, deactivated inside
        {
            //Debug.Log("4");
            activateFoldedMet = false; // to not call VineFoldBegin again
            waitFinishFoldAttack = false;
            VineFoldFinish();
        }

        else if (finishFoldedAction) // finish action (fold)
        {
            //Debug.Log("5");
            startFoldAttack = true;
            finishFoldedAction = false;
            continueFoldIndex = 0;

            spikeCounter++;
            if (spikeCounter >= 2)
            {
                if (spikeCounter == 2)
                {
                    spikeChance = Random.Range(0, 100);

                    if (spikeChance >= 50 || supremeAttack)
                    {
                        if (supremeAttack == false)
                        {
                            supremeAttack = true;
                            DrawAttack();
                        }

                        else
                        {
                            supremeAttack = false;
                            SpikeCall();
                        }
                    }

                    else
                        DrawAttack();
                }

                else if (spikeCounter >= 3)
                {
                    if (supremeAttack == false)
                    {
                        supremeAttack = true;
                        DrawAttack();
                    }

                    else
                    {
                        supremeAttack = false;
                        SpikeCall();
                    }
                }
                
            }

            else
                DrawAttack();
        }

        #endregion

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        #region Below

        else if (belowWillAttack) // activated in BelowVinePrepare, deactivated inside
        {
            //Debug.Log("6");
            belowWillAttack = false;
            playUpdate = true;
            belowAttack = true;
        }

        else if (belowWillDescend) // activated in BelowVineAttack, deactivated inside
        {
            //Debug.Log("7");
            belowWillDescend = false;
            playUpdate = true;
            belowDescend = true;
        }

        else if (belowFinish) // activated in BelowVineDescend, deactivated inside // finish action (below)
        {
            //Debug.Log("8");
            belowFinish = false;
            belowSound = true;
            belowVinesAlreadyCalled = false;
            belowVineVel = fixedBelowVineVel;
            gbm.allBelowVines.SetActive(false);

            if (repeatBelowAttack)
                BelowVinePrepare();

            else
            {
                spikeCounter++;
                if (spikeCounter >= 2)
                {
                    if (spikeCounter == 2)
                    {
                        spikeChance = Random.Range(0, 100);

                        if (spikeChance >= 50 || supremeAttack)
                        {
                            if (supremeAttack == false)
                            {
                                supremeAttack = true;
                                belowAttackCounter = 0;
                                DrawAttack();
                            }

                            else
                            {
                                supremeAttack = false;
                                belowAttackCounter = 0;
                                SpikeCall();
                            }
                        }

                        else
                        {
                            belowAttackCounter = 0;
                            DrawAttack();
                        }
                    }

                    else if (spikeCounter >= 3)
                    {
                        if (supremeAttack == false)
                        {
                            supremeAttack = true;
                            belowAttackCounter = 0;
                            DrawAttack();
                        }

                        else
                        {
                            supremeAttack = false;
                            belowAttackCounter = 0;
                            SpikeCall();
                        }
                    }
                }

                else
                {
                    belowAttackCounter = 0;
                    DrawAttack();
                }
            }
        }

        #endregion

    }
}
