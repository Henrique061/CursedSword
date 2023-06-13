using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBattle : MonoBehaviour
{
    // anims trigger: Up, Stay, Die

    [SerializeField] private Animator[] spikeAnims;
    [SerializeField] private PolygonCollider2D[] spikesColliders;
    [SerializeField] private Transform[] spikesTransform;
    [SerializeField] private float spikeVel = 0.5f;
    [SerializeField] private float spikeDieVel = 1f;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Health gaiaHealth;
    public Health[] spikeHe;

    private GaiaBattleManager gbm;
    private TongueBattleManager tbm;
    private GaiaSecondStage gss;
    private GaiaThirdStage gts;

    [SerializeField] private float spike4XOff = 10;
    [SerializeField] private float spike4YOff = 10;
    [SerializeField] private float spike5XOff = 10;
    [SerializeField] private float spike5YOff = 10;
    private float downTimer = 3;
    private float fixedDownTimer = 3;
    private float stuckTimer = 5f;
    private float fixedStuckTimer = 5f;
    private float upAnimTimer = 1.17f;

    private int battleStage;
    private int spikeStuckCounter = 0;
    private int spikeBreakCounter = 0; // to count which spike is broked on the stage

    [HideInInspector] public bool[] spikeHighDmg;
    [HideInInspector] public bool[] spikeBreaked; // to know wich spike was breaked
    private bool spikeIsBroked;
    private bool lockOnPlayer = false;
    private bool dontEnableCol = false;
    private bool soundReproduced = false;
    private bool soundReproduced2 = false;
    private bool dontMakeDieSound = false; // to not reproduce the sound of spike die on update when died by timer
    private bool beginBreakTimer = false; // to begin the stuck timer
    private bool[] spikeGoesDown; // to maker spike goes down in update, when dead
    private bool[] breakSound; // to reproduce the sound for each spike
    private bool[] breakInUpdate; // to break the spike while going down

    private void Awake()
    {
        gbm = GetComponent<GaiaBattleManager>();
        tbm = GetComponent<TongueBattleManager>();
        gss = GetComponent<GaiaSecondStage>();
        gts = GetComponent<GaiaThirdStage>();

        spikeHighDmg = new bool[5];
        spikeBreaked = new bool[5];
        spikeGoesDown = new bool[5];
        breakSound = new bool[5];
        breakInUpdate = new bool[5];

        for (int i = 0; i < spikeBreaked.Length; i++)
        {
            spikeBreaked[i] = false;
            spikeGoesDown[i] = false;
            breakSound[i] = true;
            breakInUpdate[i] = true;
        }
    }

    private void Update()
    {
        if (!PauseController.gamePaused)
        {
            if (lockOnPlayer)
            {
                SpikeLock(battleStage);
            }

            #region breakInUpdate

            if (spikeHe[0].spikeBreak && breakInUpdate[0])
            {
                spikeGoesDown[0] = true;
                breakInUpdate[0] = false;
            }

            if (spikeHe[1].spikeBreak && breakInUpdate[1])
            {
                spikeGoesDown[1] = true;
                breakInUpdate[1] = false;
            }

            if (spikeHe[2].spikeBreak && breakInUpdate[2])
            {
                spikeGoesDown[2] = true;
                breakInUpdate[2] = false;
            }

            if (spikeHe[3].spikeBreak && breakInUpdate[3])
            {
                spikeGoesDown[3] = true;
                breakInUpdate[3] = false;
            }

            if (spikeHe[4].spikeBreak && breakInUpdate[4])
            {
                spikeGoesDown[4] = true;
                breakInUpdate[4] = false;
            }

            #endregion

            if (spikeGoesDown[0] == true)
            {
                if (breakSound[0] == true && dontMakeDieSound == false)
                {
                    FindObjectOfType<AudioManager>().PlaySound("SpikeBreak");
                    breakSound[0] = false;
                }

                if (spikesTransform[0].position.y > -6.38 - 8.65f) // until goes down
                {
                    spikesTransform[0].position = new Vector2(spikesTransform[0].position.x, spikesTransform[0].position.y - spikeDieVel);
                }

                else
                    spikeGoesDown[0] = false;

            }

            if (spikeGoesDown[1] == true)
            {
                if (breakSound[1] == true && dontMakeDieSound == false)
                {
                    FindObjectOfType<AudioManager>().PlaySound("SpikeBreak");
                    breakSound[1] = false;
                }

                if (spikesTransform[1].position.y > -6.38 - 9.5f) // until goes down
                {
                    spikesTransform[1].position = new Vector2(spikesTransform[1].position.x, spikesTransform[1].position.y - spikeDieVel);
                }

                else
                    spikeGoesDown[1] = false;

            }

            if (spikeGoesDown[2] == true)
            {
                if (breakSound[2] == true && dontMakeDieSound == false)
                {
                    FindObjectOfType<AudioManager>().PlaySound("SpikeBreak");
                    breakSound[2] = false;
                }

                if (spikesTransform[2].position.y > -6.38 - 9.5f) // until goes down
                {
                    spikesTransform[2].position = new Vector2(spikesTransform[2].position.x, spikesTransform[2].position.y - spikeDieVel);
                }

                else
                    spikeGoesDown[2] = false;

            }

            if (spikeGoesDown[3] == true)
            {
                if (breakSound[3] == true && dontMakeDieSound == false)
                {
                    FindObjectOfType<AudioManager>().PlaySound("SpikeBreak");
                    breakSound[3] = false;
                }

                if (spikesTransform[3].position.y > -6.38 + spike4YOff) // until goes down
                {
                    spikesTransform[3].position = new Vector2(spikesTransform[3].position.x, spikesTransform[3].position.y - spikeDieVel);
                }

                else
                    spikeGoesDown[3] = false;

            }

            if (spikeGoesDown[4] == true)
            {
                if (breakSound[4] == true && dontMakeDieSound == false)
                {
                    FindObjectOfType<AudioManager>().PlaySound("SpikeBreak");
                    breakSound[4] = false;
                }

                if (spikesTransform[4].position.y > -6.38 + spike5YOff) // until goes down
                {
                    spikesTransform[4].position = new Vector2(spikesTransform[4].position.x, spikesTransform[4].position.y - spikeDieVel);
                }

                else
                    spikeGoesDown[4] = false;

            }
        }
    }

    public void BeginSpike(int stage)
    {
        if (stage == 1)
        {
            battleStage = 1;
            spikeAnims[0].SetTrigger("Up");
        }

        else if (stage == 2)
        {
            battleStage = 2;

            if(!spikeBreaked[0])
                spikeAnims[0].SetTrigger("Up");

            if (!spikeBreaked[1])
                spikeAnims[1].SetTrigger("Up");

            if (!spikeBreaked[2])
                spikeAnims[2].SetTrigger("Up");
        }

        else if (stage == 3)
        {
            battleStage = 3;

            if (!spikeBreaked[0])
                spikeAnims[0].SetTrigger("Up");

            if (!spikeBreaked[1])
                spikeAnims[1].SetTrigger("Up");

            if (!spikeBreaked[2])
                spikeAnims[2].SetTrigger("Up");

            if (!spikeBreaked[3])
                spikeAnims[3].SetTrigger("Up");

            if (!spikeBreaked[4])
                spikeAnims[4].SetTrigger("Up");
        }

        StartCoroutine("WaitAnimation");
    }

    IEnumerator WaitAnimation()
    {
        yield return new WaitForSeconds(upAnimTimer);

        if (battleStage == 1)
        {
            spikesTransform[0].position = new Vector2(playerTransform.position.x + 0.2f, 7.21f - 8.65f);
            spikeAnims[0].SetTrigger("Stay");
        }

        else if (battleStage == 2)
        {
            if (!spikeBreaked[0])
            {
                spikesTransform[0].position = new Vector2(playerTransform.position.x + 0.2f, 7.21f - 8.65f);
                spikeAnims[0].SetTrigger("Stay");
            }

            if (!spikeBreaked[1])
            {
                spikesTransform[1].position = new Vector2(playerTransform.position.x + 1.9f, 7.21f - 9);
                spikeAnims[1].SetTrigger("Lock");
            }

            if (!spikeBreaked[2])
            {
                spikesTransform[2].position = new Vector2(playerTransform.position.x - 1.5f, 7.21f - 9);
                spikeAnims[2].SetTrigger("Lock");
            }
            
        }

        else if (battleStage == 3)
        {
            if (!spikeBreaked[0])
            {
                spikesTransform[0].position = new Vector2(playerTransform.position.x + 0.2f, 7.21f - 8.65f);
                spikeAnims[0].SetTrigger("Stay");
            }

            if (!spikeBreaked[1])
            {
                spikesTransform[1].position = new Vector2(playerTransform.position.x + 1.9f, 7.21f - 9);
                spikeAnims[1].SetTrigger("Lock");
            }

            if (!spikeBreaked[2])
            {
                spikesTransform[2].position = new Vector2(playerTransform.position.x - 1.5f, 7.21f - 9);
                spikeAnims[2].SetTrigger("Lock");
            }

            if (!spikeBreaked[3])
            {
                spikesTransform[3].position = new Vector2(playerTransform.position.x + 1.9f, 7.21f - 9);
                spikeAnims[3].SetTrigger("Lock");
            }

            if (!spikeBreaked[4])
            {
                spikesTransform[4].position = new Vector2(playerTransform.position.x - 1.5f, 7.21f - 9);
                spikeAnims[4].SetTrigger("Lock");
            }

        }

        lockOnPlayer = true; // begin update
    }

    private void SpikeLock(int stage)
    {
        if (downTimer > 0) // spike is locking into player
        {
            if (stage == 1)
            {
                if (!dontEnableCol)
                {
                    spikesColliders[0].enabled = true;
                    dontEnableCol = true;
                }

                spikeHighDmg[0] = true;
                spikesTransform[0].position = new Vector2(playerTransform.position.x + 0.2f, 7.21f - 8.65f);
            }

            else if (stage == 2)
            {
                if (!dontEnableCol)
                {
                    if (!spikeBreaked[0])
                        spikesColliders[0].enabled = true;

                    if (!spikeBreaked[1])
                        spikesColliders[1].enabled = true;

                    if (!spikeBreaked[2])
                        spikesColliders[2].enabled = true;
                    dontEnableCol = true;
                }

                if (spikeStuckCounter == 0)
                {
                    spikeHighDmg[0] = true;

                    if (!spikeBreaked[0])
                        spikesTransform[0].position = new Vector2(playerTransform.position.x + 0.2f, 7.21f - 8.65f);

                    if (!spikeBreaked[1])
                        spikesTransform[1].position = new Vector2(playerTransform.position.x + 1.9f, 7.21f - 9.5f);

                    if (!spikeBreaked[2])
                        spikesTransform[2].position = new Vector2(playerTransform.position.x - 1.5f, 7.21f - 9.5f);
                }

                else if (spikeStuckCounter == 1)
                {
                    spikeHighDmg[1] = true;

                    if (!spikeBreaked[1])
                        spikesTransform[1].position = new Vector2(playerTransform.position.x + 1.9f, 7.21f - 9.5f);

                    if (!spikeBreaked[2])
                        spikesTransform[2].position = new Vector2(playerTransform.position.x - 1.5f, 7.21f - 9.5f);
                }

                else
                {
                    spikeHighDmg[2] = true;

                    if (!spikeBreaked[2])
                        spikesTransform[2].position = new Vector2(playerTransform.position.x - 1.5f, 7.21f - 9.5f);
                }
            }

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            else if (stage == 3)
            {
                if (!dontEnableCol)
                {
                    if (!spikeBreaked[0])
                        spikesColliders[0].enabled = true;

                    if (!spikeBreaked[1])
                        spikesColliders[1].enabled = true;

                    if (!spikeBreaked[2])
                        spikesColliders[2].enabled = true;

                    if (!spikeBreaked[3])
                        spikesColliders[3].enabled = true;

                    if (!spikeBreaked[4])
                        spikesColliders[4].enabled = true;

                    dontEnableCol = true;
                }

                if (spikeStuckCounter == 0)
                {
                    spikeHighDmg[0] = true;

                    if (!spikeBreaked[0])
                        spikesTransform[0].position = new Vector2(playerTransform.position.x + 0.2f, 7.21f - 8.65f);

                    if (!spikeBreaked[1])
                        spikesTransform[1].position = new Vector2(playerTransform.position.x + 1.9f, 7.21f - 9.5f);

                    if (!spikeBreaked[2])
                        spikesTransform[2].position = new Vector2(playerTransform.position.x - 1.5f, 7.21f - 9.5f);

                    if (!spikeBreaked[3])
                        spikesTransform[3].position = new Vector2(playerTransform.position.x + spike4XOff, 7.21f + spike4YOff);

                    if (!spikeBreaked[4])
                        spikesTransform[4].position = new Vector2(playerTransform.position.x + spike5XOff, 7.21f + spike5YOff);
                }

                else if (spikeStuckCounter == 1)
                {
                    spikeHighDmg[1] = true;

                    if (!spikeBreaked[1])
                        spikesTransform[1].position = new Vector2(playerTransform.position.x + 1.9f, 7.21f - 9.5f);

                    if (!spikeBreaked[2])
                        spikesTransform[2].position = new Vector2(playerTransform.position.x - 1.5f, 7.21f - 9.5f);

                    if (!spikeBreaked[3])
                        spikesTransform[3].position = new Vector2(playerTransform.position.x + spike4XOff, 7.21f + spike4YOff);

                    if (!spikeBreaked[4])
                        spikesTransform[4].position = new Vector2(playerTransform.position.x + spike5XOff, 7.21f + spike5YOff);
                }

                else if (spikeStuckCounter == 2)
                {
                    spikeHighDmg[2] = true;

                    if (!spikeBreaked[2])
                        spikesTransform[2].position = new Vector2(playerTransform.position.x - 1.5f, 7.21f - 9.5f);

                    if (!spikeBreaked[3])
                        spikesTransform[3].position = new Vector2(playerTransform.position.x + spike4XOff, 7.21f + spike4YOff);

                    if (!spikeBreaked[4])
                        spikesTransform[4].position = new Vector2(playerTransform.position.x + spike5XOff, 7.21f + spike5YOff);
                }

                else if (spikeStuckCounter == 3)
                {
                    spikeHighDmg[3] = true;

                    if (!spikeBreaked[3])
                        spikesTransform[3].position = new Vector2(playerTransform.position.x + spike4XOff, 7.21f + spike4YOff);

                    if (!spikeBreaked[4])
                        spikesTransform[4].position = new Vector2(playerTransform.position.x + spike5XOff, 7.21f + spike5YOff);
                }

                else // spikeStuckCounter == 4
                {
                    spikeHighDmg[4] = true;

                    if (!spikeBreaked[4])
                        spikesTransform[4].position = new Vector2(playerTransform.position.x + spike5XOff, 7.21f + spike5YOff);
                }
            }

            downTimer -= Time.deltaTime;
        }

        else // spike will attack
        {
            #region Stage 1

            if (stage == 1)
            {
                if (spikesTransform[0].position.y > -2.58 - 8.65f) // until stuck
                {
                    spikesTransform[0].position = new Vector2(spikesTransform[0].position.x, spikesTransform[0].position.y - spikeVel * Time.deltaTime);
                }

                else
                {
                    if (!soundReproduced)
                    {
                        FindObjectOfType<AudioManager>().PlaySound("SpikeStuck");
                        spikeHighDmg[0] = false;
                        soundReproduced = true;
                    }

                    if (stuckTimer <= 0 || spikeHe[0].spikeBreak == true)
                    {
                        if (spikeHe[0].spikeBreak == true)
                            spikeIsBroked = true;

                        SpikeDie(spikeIsBroked, stage);
                    }

                    stuckTimer -= Time.deltaTime;
                }
            }

            #endregion

            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            #region Stage 2

            else if (stage == 2)
            {
                if (spikeStuckCounter == 0) // spike 1 stucking
                {
                    if (spikeBreaked[0] == false)
                    {
                        if (!spikeBreaked[1]) // positioning other spikes
                            spikesTransform[1].position = new Vector2(playerTransform.position.x + 1.9f, 7.21f - 9.5f);

                        if (!spikeBreaked[2])
                            spikesTransform[2].position = new Vector2(playerTransform.position.x - 1.5f, 7.21f - 9.5f);

                        ////////////////////////////////////////////////////////////////////////// finish positioning other spikes

                        if (spikesTransform[0].position.y > -2.58 - 8.65f) // until stuck
                        {
                            spikesTransform[0].position = new Vector2(spikesTransform[0].position.x, spikesTransform[0].position.y - spikeVel * Time.deltaTime);
                        }

                        else
                        {
                            if (!soundReproduced)
                            {
                                FindObjectOfType<AudioManager>().PlaySound("SpikeStuck");
                                spikeHighDmg[0] = false;
                                //downTimer = 0.5f;

                                if (!spikeBreaked[1])
                                    spikeStuckCounter = 1;

                                else if (!spikeBreaked[2])
                                    spikeStuckCounter = 2;

                                else if (spikeBreaked[1] && spikeBreaked[2])
                                {
                                    spikeStuckCounter = -1;
                                    beginBreakTimer = true;
                                }
                            }
                        }
                    }

                    else
                    {
                        spikeStuckCounter = 1;
                    }

                } // spike 1 stucked

                if (spikeStuckCounter == 1) // spike 2 stucking
                {
                    if (!spikeBreaked[1])
                    {
                        if (!spikeBreaked[2]) // positioning spike 3
                        {
                            spikesTransform[2].position = new Vector2(playerTransform.position.x - 1.5f, 7.21f - 9.5f);
                        }

                        if (spikesTransform[1].position.y > -2.58 - 9.5f) // until stuck
                        {
                            spikesTransform[1].position = new Vector2(spikesTransform[1].position.x, spikesTransform[1].position.y - spikeVel * Time.deltaTime);
                        }

                        else
                        {
                            if (!soundReproduced)
                            {
                                FindObjectOfType<AudioManager>().PlaySound("SpikeStuck");
                                spikeHighDmg[1] = false;
                                //downTimer = 0.5f;

                                if (!spikeBreaked[2])
                                    spikeStuckCounter = 2;

                                else
                                {
                                    spikeStuckCounter = -1;
                                    beginBreakTimer = true;
                                }
                            }

                        }
                    }

                    else
                    {
                        spikeStuckCounter = 2;
                    }

                } // spike 2 stucked

                if (spikeStuckCounter == 2) // spike 3 stucking
                {
                    if (!spikeBreaked[2])
                    {
                        if (spikesTransform[2].position.y > -2.58 - 9.5f) // until stuck
                        {
                            spikesTransform[2].position = new Vector2(spikesTransform[2].position.x, spikesTransform[2].position.y - spikeVel * Time.deltaTime);
                        }

                        else
                        {
                            if (!soundReproduced)
                            {
                                FindObjectOfType<AudioManager>().PlaySound("SpikeStuck");
                                spikeHighDmg[2] = false;
                                soundReproduced = true;

                                spikeStuckCounter = -1;
                                beginBreakTimer = true;
                            }
                        }
                    }

                } // spike 3 stucked

                if (beginBreakTimer)
                {
                    if (spikeHe[0].spikeBreak == true)
                    {
                        spikeBreakCounter++;
                        spikeBreaked[0] = true;
                        spikeGoesDown[0] = true;
                    }

                    if (spikeHe[1].spikeBreak == true)
                    {
                        spikeBreakCounter++;
                        spikeBreaked[1] = true;
                        spikeGoesDown[1] = true;
                    }

                    if (spikeHe[2].spikeBreak == true)
                    {
                        spikeBreakCounter++;
                        spikeBreaked[2] = true;
                        spikeGoesDown[2] = true;
                    }

                    if (stuckTimer <= 0 || (spikeHe[0].spikeBreak && spikeHe[1].spikeBreak && spikeHe[2].spikeBreak))
                    {
                        if (spikeHe[0].spikeBreak && spikeHe[1].spikeBreak && spikeHe[2].spikeBreak)
                            spikeIsBroked = true;

                        SpikeDie(spikeIsBroked, stage);
                    }

                    stuckTimer -= Time.deltaTime;
                }

            } // close stage 2

            #endregion

            //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            #region Stage 3

            else if (stage == 3)
            {
                if (spikeStuckCounter == 0) // spike 1 stucking
                {
                    if (spikeBreaked[0] == false)
                    {
                        if (!spikeBreaked[1]) // positioning other spikes
                            spikesTransform[1].position = new Vector2(playerTransform.position.x + 1.9f, 7.21f - 9.5f);

                        if (!spikeBreaked[2])
                            spikesTransform[2].position = new Vector2(playerTransform.position.x - 1.5f, 7.21f - 9.5f);

                        if (!spikeBreaked[3])
                            spikesTransform[3].position = new Vector2(playerTransform.position.x + spike4XOff, 7.21f + spike4YOff);

                        if (!spikeBreaked[4])
                            spikesTransform[4].position = new Vector2(playerTransform.position.x + spike5XOff, 7.21f + spike5YOff);

                        ////////////////////////////////////////////////////////////////////////// finish positioning other spikes

                        if (spikesTransform[0].position.y > -2.58 - 8.65f) // until stuck
                            spikesTransform[0].position = new Vector2(spikesTransform[0].position.x, spikesTransform[0].position.y - spikeVel * Time.deltaTime);

                        else
                        {
                            if (!soundReproduced)
                            {
                                FindObjectOfType<AudioManager>().PlaySound("SpikeStuck");
                                spikeHighDmg[0] = false;
                                //downTimer = 0.5f;

                                if (!spikeBreaked[1])
                                    spikeStuckCounter = 1;

                                else if (!spikeBreaked[2])
                                    spikeStuckCounter = 2;

                                else if (!spikeBreaked[3])
                                    spikeStuckCounter = 3;

                                else if (!spikeBreaked[4])
                                    spikeStuckCounter = 4;

                                else if (spikeBreaked[1] && spikeBreaked[2] && spikeBreaked[3] && spikeBreaked[4])
                                {
                                    spikeStuckCounter = -1;
                                    beginBreakTimer = true;
                                }
                            }
                        }
                    }

                    else
                    {
                        spikeStuckCounter = 1;
                    }

                } // spike 1 stucked

                if (spikeStuckCounter == 1) // spike 2 stucking
                {
                    if (!spikeBreaked[1])
                    {
                        if (!spikeBreaked[2])
                            spikesTransform[2].position = new Vector2(playerTransform.position.x - 1.5f, 7.21f - 9.5f);

                        if (!spikeBreaked[3])
                            spikesTransform[3].position = new Vector2(playerTransform.position.x + spike4XOff, 7.21f + spike4YOff);

                        if (!spikeBreaked[4])
                            spikesTransform[4].position = new Vector2(playerTransform.position.x + spike5XOff, 7.21f + spike5YOff);

                        ////////////////////////////////////////////////////////////////////////// finish positioning other spikes

                        if (spikesTransform[1].position.y > -2.58 - 9.5f) // until stuck
                            spikesTransform[1].position = new Vector2(spikesTransform[1].position.x, spikesTransform[1].position.y - spikeVel * Time.deltaTime);

                        else
                        {
                            if (!soundReproduced)
                            {
                                FindObjectOfType<AudioManager>().PlaySound("SpikeStuck");
                                spikeHighDmg[1] = false;
                                //downTimer = 0.5f;

                                if (!spikeBreaked[2])
                                    spikeStuckCounter = 2;

                                else if (!spikeBreaked[3])
                                    spikeStuckCounter = 3;

                                else if (!spikeBreaked[4])
                                    spikeStuckCounter = 4;

                                else if (spikeBreaked[2] && spikeBreaked[3] && spikeBreaked[4])
                                {
                                    spikeStuckCounter = -1;
                                    beginBreakTimer = true;
                                }
                            }
                        }
                    }

                    else
                    {
                        spikeStuckCounter = 2;
                    }

                } // spike 2 stucked

                if (spikeStuckCounter == 2) // spike 3 stucking
                {
                    if (!spikeBreaked[2])
                    {
                        if (!spikeBreaked[3])
                            spikesTransform[3].position = new Vector2(playerTransform.position.x + spike4XOff, 7.21f + spike4YOff);

                        if (!spikeBreaked[4])
                            spikesTransform[4].position = new Vector2(playerTransform.position.x + spike5XOff, 7.21f + spike5YOff);

                        ////////////////////////////////////////////////////////////////////////// finish positioning other spikes

                        if (spikesTransform[2].position.y > -2.58 - 9.5f) // until stuck
                            spikesTransform[2].position = new Vector2(spikesTransform[2].position.x, spikesTransform[2].position.y - spikeVel * Time.deltaTime);

                        else
                        {
                            if (!soundReproduced)
                            {
                                FindObjectOfType<AudioManager>().PlaySound("SpikeStuck");
                                spikeHighDmg[2] = false;
                                //downTimer = 0.5f;

                                if (!spikeBreaked[3])
                                    spikeStuckCounter = 3;

                                else if (!spikeBreaked[4])
                                    spikeStuckCounter = 4;

                                else if (spikeBreaked[3] && spikeBreaked[4])
                                {
                                    spikeStuckCounter = -1;
                                    beginBreakTimer = true;
                                }
                            }
                        }
                    }

                    else
                    {
                        spikeStuckCounter = 3;
                    }

                } // spike 3 stucked

                if (spikeStuckCounter == 3) // spike 4 stucking
                {
                    if (!spikeBreaked[3])
                    {
                        if (!spikeBreaked[4])
                            spikesTransform[4].position = new Vector2(playerTransform.position.x + spike5XOff, 7.21f + spike5YOff);

                        ////////////////////////////////////////////////////////////////////////// finish positioning other spikes

                        if (spikesTransform[3].position.y > -2.58 + spike4YOff) // until stuck
                            spikesTransform[3].position = new Vector2(spikesTransform[3].position.x, spikesTransform[3].position.y - spikeVel * Time.deltaTime);

                        else
                        {
                            if (!soundReproduced)
                            {
                                FindObjectOfType<AudioManager>().PlaySound("SpikeStuck");
                                spikeHighDmg[3] = false;
                                //downTimer = 0.5f;

                                if (!spikeBreaked[4])
                                    spikeStuckCounter = 4;

                                else
                                {
                                    spikeStuckCounter = -1;
                                    beginBreakTimer = true;
                                }
                            }
                        }
                    }

                    else
                    {
                        spikeStuckCounter = 4;
                    }

                } // spike 4 stucked

                if (spikeStuckCounter == 4) // spike 5 stucking
                {
                    if (!spikeBreaked[4])
                    {
                        if (spikesTransform[4].position.y > -2.58 + spike5YOff) // until stuck
                        {
                            spikesTransform[4].position = new Vector2(spikesTransform[4].position.x, spikesTransform[4].position.y - spikeVel * Time.deltaTime);
                        }

                        else
                        {
                            if (!soundReproduced)
                            {
                                FindObjectOfType<AudioManager>().PlaySound("SpikeStuck");
                                spikeHighDmg[4] = false;
                                soundReproduced = true;

                                spikeStuckCounter = -1;
                                beginBreakTimer = true;
                            }
                        }
                    }

                } // spike 5 stucked

                /////////////////////////////////////////////////////////////////////////////////////////////

                if (beginBreakTimer)
                {
                    if (spikeHe[0].spikeBreak == true)
                    {
                        spikeBreakCounter++;
                        spikeBreaked[0] = true;
                        spikeGoesDown[0] = true;
                    }

                    if (spikeHe[1].spikeBreak == true)
                    {
                        spikeBreakCounter++;
                        spikeBreaked[1] = true;
                        spikeGoesDown[1] = true;
                    }

                    if (spikeHe[2].spikeBreak == true)
                    {
                        spikeBreakCounter++;
                        spikeBreaked[2] = true;
                        spikeGoesDown[2] = true;
                    }

                    if (spikeHe[3].spikeBreak == true)
                    {
                        spikeBreakCounter++;
                        spikeBreaked[3] = true;
                        spikeGoesDown[3] = true;
                    }

                    if (spikeHe[4].spikeBreak == true)
                    {
                        spikeBreakCounter++;
                        spikeBreaked[4] = true;
                        spikeGoesDown[4] = true;
                    }

                    if (stuckTimer <= 0 || (spikeHe[0].spikeBreak && spikeHe[1].spikeBreak && spikeHe[2].spikeBreak && spikeHe[3].spikeBreak && spikeHe[4].spikeBreak))
                    {
                        if (spikeHe[0].spikeBreak && spikeHe[1].spikeBreak && spikeHe[2].spikeBreak && spikeHe[3].spikeBreak && spikeHe[4].spikeBreak)
                            spikeIsBroked = true;

                        SpikeDie(spikeIsBroked, stage);
                    }

                    stuckTimer -= Time.deltaTime;
                }

            } // close stage 3

            #endregion
        }
    }

    private void SpikeDie(bool broked, int stage)
    {
        if (stage == 1)
        {
            if (broked == true)
            {
                if (!soundReproduced2)
                {
                    FindObjectOfType<AudioManager>().PlaySound("SpikeBreak");
                    soundReproduced2 = true;
                }

                if (spikesTransform[0].position.y > -6.38 - 8.65f) // until goes down
                {
                    spikesTransform[0].position = new Vector2(spikesTransform[0].position.x, spikesTransform[0].position.y - spikeDieVel);
                }

                else
                {
                    lockOnPlayer = false;
                    tbm.TongueAppear(stage);
                }

            } // if broked end

            else // if not broked
            {
                if (spikesTransform[0].position.y > -6.38 - 8.65f)
                    spikesTransform[0].position = new Vector2(spikesTransform[0].position.x, spikesTransform[0].position.y - spikeDieVel);

                else
                {
                    StartCoroutine("Wait", stage);
                    lockOnPlayer = false;
                }
            }
        }

        ////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        else if (stage == 2)
        {
            if (!broked)
                dontMakeDieSound = true;

            spikeGoesDown[0] = true;
            spikeGoesDown[1] = true;
            spikeGoesDown[2] = true;

            if (broked == true)
            {
                lockOnPlayer = false;
                tbm.TongueAppear(stage);
            }

            else
            {
                StartCoroutine("Wait", stage);
                lockOnPlayer = false;
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        else if (stage == 3)
        {
            if (!broked)
                dontMakeDieSound = true;

            spikeGoesDown[0] = true;
            spikeGoesDown[1] = true;
            spikeGoesDown[2] = true;
            spikeGoesDown[3] = true;
            spikeGoesDown[4] = true;

            if (broked == true)
            {
                lockOnPlayer = false;
                tbm.TongueAppear(stage);
            }

            else
            {
                StartCoroutine("Wait", stage);
                lockOnPlayer = false;
            }
        }
    }

    public IEnumerator Wait(int stage)
    {
        spikeStuckCounter = 0;
        yield return new WaitForSeconds(1f);
        StartCoroutine("FinishReturn", stage);
    }
    IEnumerator FinishReturn(int stage)
    {
        if (stage == 1)
        {
            spikesTransform[0].position = new Vector2(0 + 0.27f, 0 - 8.65f);
            spikesColliders[0].enabled = false;
            spikeAnims[0].SetTrigger("Die");
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        
        else if (stage == 2)
        {

            spikesColliders[0].enabled = false;
            spikesColliders[1].enabled = false;
            spikesColliders[2].enabled = false;

            if (tbm.spikeReturn)
            {
                spikesTransform[0].position = new Vector2(0 + 0.27f, 0 - 8.65f);
                spikesTransform[1].position = new Vector2(0 + 0.27f, 0 - 8.65f);
                spikesTransform[2].position = new Vector2(0 + 0.27f, 0 - 8.65f);

                spikeAnims[0].SetTrigger("Die");
                spikeAnims[1].SetTrigger("Die");
                spikeAnims[2].SetTrigger("Die");
            }

            else
            {
                if (!spikeBreaked[0])
                {
                    //Debug.Log("volta 1");
                    spikesTransform[0].position = new Vector2(0 + 0.27f, 0 - 8.65f);
                    spikeAnims[0].SetTrigger("Die");
                }

                if (!spikeBreaked[1])
                {
                    //Debug.Log("volta 2");
                    spikesTransform[1].position = new Vector2(0 + 0.27f, 0 - 8.65f);
                    spikeAnims[1].SetTrigger("Die");
                }

                if (!spikeBreaked[2])
                {
                    //Debug.Log("volta 3");
                    spikesTransform[2].position = new Vector2(0 + 0.27f, 0 - 8.65f);
                    spikeAnims[2].SetTrigger("Die");
                }
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        else if (stage == 3)
        {

            spikesColliders[0].enabled = false;
            spikesColliders[1].enabled = false;
            spikesColliders[2].enabled = false;
            spikesColliders[3].enabled = false;
            spikesColliders[4].enabled = false;

            if (tbm.spikeReturn)
            {
                spikesTransform[0].position = new Vector2(0 + 0.27f, 0 - 8.65f);
                spikesTransform[1].position = new Vector2(0 + 0.27f, 0 - 8.65f);
                spikesTransform[2].position = new Vector2(0 + 0.27f, 0 - 8.65f);
                spikesTransform[3].position = new Vector2(0 + 0.27f, 0 - 8.65f);
                spikesTransform[4].position = new Vector2(0 + 0.27f, 0 - 8.65f);

                spikeAnims[0].SetTrigger("Die");
                spikeAnims[1].SetTrigger("Die");
                spikeAnims[2].SetTrigger("Die");
                spikeAnims[3].SetTrigger("Die");
                spikeAnims[4].SetTrigger("Die");
            }

            else
            {
                if (!spikeBreaked[0])
                {
                    //Debug.Log("volta 1");
                    spikesTransform[0].position = new Vector2(0 + 0.27f, 0 - 8.65f);
                    spikeAnims[0].SetTrigger("Die");
                }

                if (!spikeBreaked[1])
                {
                    //Debug.Log("volta 2");
                    spikesTransform[1].position = new Vector2(0 + 0.27f, 0 - 8.65f);
                    spikeAnims[1].SetTrigger("Die");
                }

                if (!spikeBreaked[2])
                {
                    //Debug.Log("volta 3");
                    spikesTransform[2].position = new Vector2(0 + 0.27f, 0 - 8.65f);
                    spikeAnims[2].SetTrigger("Die");
                }

                if (!spikeBreaked[3])
                {
                    //Debug.Log("volta 4");
                    spikesTransform[3].position = new Vector2(0 + 0.27f, 0 - 8.65f);
                    spikeAnims[3].SetTrigger("Die");
                }

                if (!spikeBreaked[4])
                {
                    //Debug.Log("volta 5");
                    spikesTransform[4].position = new Vector2(0 + 0.27f, 0 - 8.65f);
                    spikeAnims[4].SetTrigger("Die");
                }
            }
        }

        yield return new WaitForSeconds(1.5f);

        if (stage == 1)
        {
            foreach (Health sBreak in spikeHe)
                sBreak.spikeBreak = false;

            spikeAnims[0].SetTrigger("Stay");

            if (gaiaHealth.currentHealth > 400 && gaiaHealth.currentHealth <= 750)
            {
                gbm.isSecondStage = true;
            }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        if (stage == 2)
        {
            if (tbm.spikeReturn)
            {
                foreach (Health sBreak in spikeHe)
                    sBreak.spikeBreak = false;

                spikeAnims[0].SetTrigger("Stay");
                spikeAnims[1].SetTrigger("Stay");
                spikeAnims[2].SetTrigger("Stay");
            }

            else
            {
                if (!spikeBreaked[0])
                    spikeAnims[0].SetTrigger("Stay");

                if (!spikeBreaked[1])
                    spikeAnims[1].SetTrigger("Stay");

                if (!spikeBreaked[2])
                    spikeAnims[2].SetTrigger("Stay");
            }

            if (gaiaHealth.currentHealth <= 400)
            {
                gss.isThirdStage = true;
            }
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        if (stage == 3)
        {
            if (tbm.spikeReturn)
            {
                foreach (Health sBreak in spikeHe)
                    sBreak.spikeBreak = false;

                spikeAnims[0].SetTrigger("Stay");
                spikeAnims[1].SetTrigger("Stay");
                spikeAnims[2].SetTrigger("Stay");
                spikeAnims[3].SetTrigger("Stay");
                spikeAnims[4].SetTrigger("Stay");
            }

            else
            {
                if (!spikeBreaked[0])
                    spikeAnims[0].SetTrigger("Stay");

                if (!spikeBreaked[1])
                    spikeAnims[1].SetTrigger("Stay");

                if (!spikeBreaked[2])
                    spikeAnims[2].SetTrigger("Stay");

                if (!spikeBreaked[3])
                    spikeAnims[3].SetTrigger("Stay");

                if (!spikeBreaked[4])
                    spikeAnims[4].SetTrigger("Stay");
            }

            if (gaiaHealth.currentHealth <= 0)
            {
                // DIE MOTHA FOCKA
            }
        }

        dontEnableCol = false;
        soundReproduced = false;
        soundReproduced2 = false;
        spikeIsBroked = false;
        tbm.spikeReturn = false;
        downTimer = fixedDownTimer;
        stuckTimer = fixedStuckTimer;
        beginBreakTimer = false;
        gbm.spikeCounter = 0;
        gbm.playUpdate = true;
        dontMakeDieSound = false;

        for (int i = 0; i < breakSound.Length; i++)
        {
            breakSound[i] = true;
            breakInUpdate[i] = true;
        }

        if (stage == 2 && !gss.isThirdStage)
            gss.DrawAttack();

        else if (stage == 2 && gss.isThirdStage)
            gts.VineAppear();

        else if (stage == 3)
            gts.DrawAttack();
    }
}
