using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaiaSecondStage : MonoBehaviour
{
    [Header("Folded Vines")]
    public Animator[] secondFoldedVinesAnims;
    public GameObject[] secondFoldedVines;

    private GaiaBattleManager gbm;
    private SpikeBattle sb;

    #region General and Choose Attack

    [HideInInspector] public bool isThirdStage = false; // to call thirdstage

    private bool start = true; // to make the inital vines appear (start stage 2)
    private bool playUpdate = false; // to play the update method;
    private bool dontActivate = false; // to not activate variables when entereting the coroutine "WaitController"

    private int whatAttack; // to draw what attack will be performed

    #endregion

    #region FoldVineAttack

    private bool activateFolded = false; // to activate the foldd actions on update;
    private bool activateFoldedMet = false; // to activate the folded attack method;
    private bool startFoldAttack = true; // to not call the "VineFoldAttack" twice while in coroutine
    private bool pauseFoldAttack = false; // to continue the fold attack in sequential
    private bool continueFoldAttack = false; // to continue the fold attack in sequential
    private bool waitFinishFoldAttack = false; // to wait the finish action of fold attack
    private bool finishFoldedAction = false; // to finish the fold attack action

    private int[] riseIndexes; // to choose which rised vines will interact
    private int[] foldedIndexes; // to select which foldd vine will attack
    private int firstFoldVine; // to store which folded vine has already choosed

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

    private int evenOrOdd; // to choose what vines will attack from below

    private float fixedBelowVineVel; // to fix the below vine vel preparation

    #endregion

    #region Spike

    private int spikeCounter = 0; // to count when to call the SpikeCall
    private int spikeChance; // to see what the chances to call the spike action (if > 50, will be called)

    #endregion


    private void Awake()
    {
        gbm = GetComponent<GaiaBattleManager>();
        sb = GetComponent<SpikeBattle>();

        riseIndexes = new int[2];
        foldedIndexes = new int[2];

        fixedBelowVineVel = belowVineVel;
    }

    private void Update()
    {
        if (!PauseController.gamePaused)
        {
            if (gbm.isSecondStage)
            {
                if (start) // activated by default, deactivated on "VineAppear"
                {
                    VineAppear();
                    StartCoroutine("WaitController", 2);
                }

                if (playUpdate) // activated in BelowVinePrepare and WaitController (BelowVineAttack)
                {
                    if (evenVinePrepare || oddVinePrepare) // activated in BelowVinePrepare
                        BelowVinePrepare();

                    if (belowAttack) // activated in WaitController (BelowVineAttack)
                        BelowVineAttack();

                    if (belowDescend) // activated in WaitController (BelowVineDescend)
                        BelowVineDescend();

                } // close playUpdate

            } // close second stage
        }
    }

    private void VineAppear() // start of stage 2
    {
        start = false;

        gbm.risedVines[2].SetActive(true);
        gbm.risedVines[3].SetActive(true);

        FindObjectOfType<AudioManager>().PlaySound("VineRise");
        gbm.risedVinesAnims[2].SetTrigger("Rise");
        gbm.risedVinesAnims[3].SetTrigger("Rise");
    }

    public void DrawAttack()
    {
        whatAttack = Random.Range(0, 2);

        if (whatAttack == 0)
            VineFoldBegin();

        else
            BelowVinePrepare();
    }

    #region Folded Vine Methods

    private void VineFoldBegin() // side attack
    {
        for (int i = 0; i < riseIndexes.Length; i++) // choosing what vines will attack
        {
            if (i == 0)
            {
                riseIndexes[i] = Random.Range(0, 2); // to choose between the 1 right or 1 left only
                firstFoldVine = riseIndexes[i];
            }

            else
            {
                riseIndexes[i] = Random.Range(0, 4); // to choose whoever

                while (riseIndexes[i] == firstFoldVine)
                    riseIndexes[i] = Random.Range(0, 4);
            }

        }

        // digin animation by the selected vines indexes
        gbm.risedVinesAnims[riseIndexes[0]].SetTrigger("Dig");
        gbm.risedVinesAnims[riseIndexes[1]].SetTrigger("Dig");

        activateFolded = true;
        dontActivate = true;
        StartCoroutine("WaitController", 2);

    }

    private void VineFoldAttack() // rised indexes: evens = right, odds = left (Rise, Dig, Wait) |||| folded obj and anims indexes: 0, 1, 2 = right / 3, 4, 5 = left (bool Go)
    {
        if (startFoldAttack) // activated by default, deactivated inside
        {
            for (int i = 0; i < riseIndexes.Length; i++) // attaching the fold indexes by the rised indexes
            {
                if (riseIndexes[i] % 2 == 0) // if is even, is a vine on the right
                    foldedIndexes[i] = Random.Range(0, 3);

                else // if is odd, is a vine on the left
                    foldedIndexes[i] = Random.Range(3, 6);
            }

            gbm.foldedVines[foldedIndexes[0]].SetActive(true);
            secondFoldedVines[foldedIndexes[1]].SetActive(true);

            gbm.foldedVinesAnims[foldedIndexes[0]].SetBool("Go", true); // first folded go
            startFoldAttack = false;
            pauseFoldAttack = true;
            StartCoroutine("WaitController", 1.2f);
        }

        else if (continueFoldAttack)
        {
            secondFoldedVinesAnims[foldedIndexes[1]].SetBool("Go", true); // second folded go
            waitFinishFoldAttack = true;
            StartCoroutine("WaitController", 3.283f);
        }
    }

    private void VineFoldFinish()
    {
        gbm.foldedVinesAnims[foldedIndexes[0]].SetBool("Go", false);
        secondFoldedVinesAnims[foldedIndexes[1]].SetBool("Go", false);

        gbm.foldedVines[foldedIndexes[0]].SetActive(false);
        secondFoldedVines[foldedIndexes[1]].SetActive(false);

        ReturnRisedVines();
    }

    private void ReturnRisedVines()
    {
        gbm.risedVines[riseIndexes[0]].SetActive(true);
        gbm.risedVines[riseIndexes[1]].SetActive(true);

        FindObjectOfType<AudioManager>().PlaySound("VineRise");
        gbm.risedVinesAnims[riseIndexes[0]].SetTrigger("Rise");
        gbm.risedVinesAnims[riseIndexes[1]].SetTrigger("Rise");

        finishFoldedAction = true;
        StartCoroutine("WaitController", 2);
    }

    #endregion

    /////////////////////////////////////////////////////////////

    #region Below Vine Methods

    private void BelowVinePrepare() // below attack
    {
        if (!belowVinesAlreadyCalled) // deactivated by default and in final WaitController, activated inside
        {
            playUpdate = true;
            belowVinesAlreadyCalled = true;
            gbm.allBelowVines.SetActive(true);
            evenOrOdd = Random.Range(0, 2);
        }

        if (evenOrOdd == 0) // if even
        {
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
                StartCoroutine("WaitController", 1.5f);
            }
        }

        else // if odd
        {
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
                StartCoroutine("WaitController", 1.5f);
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
                StartCoroutine("WaitController", 2);
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
                StartCoroutine("WaitController", 2);
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
                StartCoroutine("WaitController", 2);
            }
        }
    }

    #endregion

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
                spikeCounter = 0;
                sb.BeginSpike(2);
            }

            else
            {
                DrawAttack();
            }
        }

        else if (spikeCounter == 4)
        {
            spikeCounter = 0;
            sb.BeginSpike(2);
        }
    }

    private IEnumerator WaitController(float time)
    {
        yield return new WaitForSeconds(time);

        if (!dontActivate) // deactivated in VineFoldBegin
        {
            //Debug.Log("1");
            //playUpdate = true;
            DrawAttack();
        }

        else if (activateFolded) // when the rised vine digs (activated in VineFoldBegin)
        {
            //Debug.Log("2");
            gbm.risedVinesAnims[riseIndexes[0]].SetTrigger("Wait");
            gbm.risedVinesAnims[riseIndexes[1]].SetTrigger("Wait");
            gbm.risedVines[riseIndexes[0]].SetActive(false);
            gbm.risedVines[riseIndexes[1]].SetActive(false);

            //drawAttack = false;
            activateFolded = false;
            VineFoldAttack();
            //activateFoldedMet = true;
        }

        else if (pauseFoldAttack) // activated in VineFoldAttack
        {
            //Debug.Log("3");
            pauseFoldAttack = false;
            continueFoldAttack = true;
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

            spikeCounter++;
            if (spikeCounter >= 4)
                SpikeCall();
            else
                DrawAttack();
        }

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

            spikeCounter++;
            if (spikeCounter >= 4)
                SpikeCall();
            else
                DrawAttack();
        }

    }
}
