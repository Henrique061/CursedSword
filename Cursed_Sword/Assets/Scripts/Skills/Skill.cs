using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    #region Instantiations
    [Header("EnemyDamage Script")]
    [SerializeField] private EnemyDamage ed;
    [SerializeField] private EnemyDamage[] spikeED;

    private SkillController sc;
    private CharacterMovement cm;
    private Rigidbody2D rb;
    private CharacterController cc;
    private Animator anim;
    private Health he;

    #endregion

    #region Skills Variables

    [HideInInspector] public string skill1 = "."; // the name of the skill to be allocated on position 1
    [HideInInspector] public string skill2 = "."; // the name of the skill to be allocated on position 1
    [HideInInspector] public bool usingSkill = false; // to block attack action
    [HideInInspector] public bool canSkill1 = true; // to check if skill 1 can be used, for cooldown
    [HideInInspector] public bool canSkill2 = true; // to check if skill 2 can be used, for cooldown
    [HideInInspector] public bool canUseSkill = true;

    #endregion

    #region Cooldown Variables

    private float cooldown1; // cooldown to be passed for skill 1
    private float cooldown2; // cooldown to be passed for skill 1

    #endregion

    #region Physics Variables

    private float playerGravity;

    #endregion

    #region Skill Calls

    private int skillNum; // the skill number to call on update
    private float skillTime; // the duration of the skill
    private bool skill1Activate = false; // to enter to the skill 1 action after activating it
    private bool skill2Activate = false; // to enter to the skill 2 action after activating it

    // variables to call the skill action on update
    [HideInInspector] public bool lowflightUsed = false;
    [HideInInspector] public bool earthquakeUsed = false;

    #endregion

    #region LowFlight Variables
    
    [Header("Low Flight")]
    [SerializeField] private float flyVelocity = 30;
    [SerializeField] private float flyDuration = 0.5f;
    [SerializeField] private TrailRenderer lowflightTrail;
    [SerializeField] private float trailEndTime = 0.1f;
    [SerializeField] private float invencibleDelay = 0.5f;
    public float flyCooldown = 7;
    public float lowflightDamage = 35;

    [HideInInspector] public bool usingLowflight = false;
    [HideInInspector] public bool lowflightCauseDmg = true;
    [HideInInspector] public bool[] spikeLowDmg;
    [HideInInspector] public bool lowflightInvulnerable = false;

    private float fixedFlyDuration;

    private bool trailEmitted = false;

    #endregion

    #region Earthquake Variables

    [Header("Earthquake")]
    [SerializeField] private float earthLiftVelocity = 50;
    [SerializeField] private float earthPoundVelocity = 50;
    [SerializeField] private float earthDuration = 1;
    [SerializeField] private GameObject earthquakeCollider;
    public float earthCooldown = 12;
    public float earthquakeDamage = 50; // the amount of health that will be caused
    [SerializeField] private ParticleSystem earthParticle;
    [SerializeField] private Transform swordTransform;

    [HideInInspector] public bool earthCauseDmg = true; // to validate only one hit on collision detect
    [HideInInspector] public bool[] spikeEarthDmg;
    [HideInInspector] public bool earthquakeInvulnerable = false;

    private float fixedEarthDuration;
    private bool wasGrounded = false; // to check if activated earthquake while grounded
    private bool pound = false; // to activate the power when reaching the ground
    private bool dontCountdown = false; // to make update dont countdown the earthDuration
    private bool fromAir = true; // to check if player performed earthquake mid-air
    private bool fixParticlePos;

    private float particleTime = 2;
    private float fixedParticleTime = 2;

    private Vector2 particlePos;

    #endregion

    #region FireUp Variables

    [Header("Fire Up")]
    [SerializeField] private float fireupTime = 10;
    public float fireupCooldown = 30;
    [SerializeField] private Material fireupMaterial;
    [SerializeField] private SpriteRenderer leftBlade;
    [SerializeField] private SpriteRenderer hurtMouth;
    [SerializeField] private float fireupDamageIncrese = 1.75f;

    private Material defaultMaterial;
    private SpriteRenderer[] srChildren;

    #endregion

    #region LaserBeam Variables

    [Header("Laser Beam")]
    //[SerializeField] private GameObject laserObject;
    public float laserDamage = 25;
    public float laserCooldown = 10f;

    [HideInInspector] public bool laserCauseDmg = true; // to validate only one hit on collision detect
    [HideInInspector] public bool[] spikeLaserDmg;

    private bool laserUsed = false;
    private float laserDuration = 0.5f;
    private float fixedLaserDuration = 0.5f;

    #endregion

    private void Awake()
    {
        sc = GetComponent<SkillController>();
        cm = GetComponent<CharacterMovement>();
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
        he = GetComponent<Health>();
        srChildren = GetComponentsInChildren<SpriteRenderer>();

        spikeLaserDmg = new bool[5];
        spikeEarthDmg = new bool[5];
        spikeLowDmg = new bool[5];

        for (int i = 0; i < spikeLaserDmg.Length; i++)
            spikeLaserDmg[i] = true;

        for (int i = 0; i < spikeEarthDmg.Length; i++)
            spikeEarthDmg[i] = true;

        for (int i = 0; i < spikeLowDmg.Length; i++)
            spikeLowDmg[i] = true;

        defaultMaterial = srChildren[0].material;

        fixedFlyDuration = flyDuration;
        fixedEarthDuration = earthDuration;

        playerGravity = rb.gravityScale;
    }

    private void Update()
    {
        if (!PauseController.gamePaused)
        {
            if (fixParticlePos)
            {
                if (particleTime > 0)
                {
                    earthParticle.gameObject.transform.position = particlePos;
                    particleTime -= Time.deltaTime;
                }

                else
                {
                    particleTime = fixedParticleTime;
                    fixParticlePos = false;
                }
            }

            else
            {
                earthParticle.gameObject.transform.position = swordTransform.position;
            }
        }
    }

    private void FixedUpdate() // for calling skills
    {
        if (lowflightUsed)
        {
            flyDuration -= Time.fixedDeltaTime;
            LowFlight(skillNum);
        }

        if (earthquakeUsed)
        {
            if(!dontCountdown)
                earthDuration -= Time.fixedDeltaTime;

            Earthquake(skillNum);
        }

        if (laserUsed)
        {
            laserDuration -= Time.deltaTime;
            LaserBeam(skillNum);
        }
    }

    #region Skill Call

    public void SkillUse(int skillNumber)
    {
        if (skillNumber == 1)
        {
            switch (skill1) // call skill number 1
            {
                case "lowflight":
                    LowFlight(skillNumber);
                    break;

                case "earthquake":
                    Earthquake(skillNumber);
                    break;

                case "fireup":
                    FireUp(skillNumber);
                    break;

                case "laser":
                    LaserBeam(skillNumber);
                    break;
            }
        }

        else
        {
            switch (skill2) // call skill number 2
            {
                case "lowflight":
                    LowFlight(skillNumber);
                    break;

                case "earthquake":
                    Earthquake(skillNumber);
                    break;

                case "fireup":
                    FireUp(skillNumber);
                    break;

                case "laser":
                    LaserBeam(skillNumber);
                    break;
            }
        }
    }

    #endregion

    #region Skill Actions

    // LOW FLIGHT /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private void LowFlight(int skillNumber)
    {
        if (((skillNumber == 1 && canSkill1) || (skillNumber == 2 && canSkill2)) && canUseSkill) // attend these conditions to perform the skill action
        {
            skillNum = skillNumber;
            canUseSkill = false;
            lowflightUsed = true;

            if (skillNumber == 1)
            {
                skill1Activate = true;
                sc.skill1Activated.SetActive(true);
            }

            else
            {
                skill2Activate = true;
                sc.skill2Activated.SetActive(true);
            }
        }
        
        if ((skillNumber == 1 && skill1Activate) || (skillNumber == 2 && skill2Activate))
        {
            if (!usingSkill) // start action //////////////////////////////
            {
                cm.canWalk = false;
                cc.canJump = false;
                usingSkill = true;
                usingLowflight = true;
                rb.gravityScale = 0;

                FindObjectOfType<AudioManager>().PlaySound("Lowflight");
                anim.SetBool("Lowflight", true);
                

                lowflightInvulnerable = true;
            }

            else if (usingSkill && flyDuration > 0) // call action //////////////////////////
            {
                if (!trailEmitted)
                {
                    lowflightTrail.emitting = true;
                    trailEmitted = true;
                }

                if (cc.facingRight) // fly to the right
                    rb.velocity = new Vector2(flyVelocity, 0);

                else // fly to the left
                    rb.velocity = new Vector2(-flyVelocity, 0);

                if (flyDuration <= trailEndTime)
                    lowflightTrail.emitting = false;
            }

            else // finish action ////////////////////////////////////////////////////////////
            {
                rb.velocity = Vector2.zero;
                anim.SetBool("Lowflight", false);

                if (skillNumber == 1)
                {
                    sc.SkillCooldown(flyCooldown, 0);
                    sc.skill1Activated.SetActive(false);
                    sc.skill1Used = true;
                }

                else
                {
                    sc.SkillCooldown(0, flyCooldown);
                    sc.skill2Activated.SetActive(false);
                    sc.skill2Used = true;
                }

                canUseSkill = true;
                cm.canWalk = true;
                cc.canJump = true;
                usingSkill = false;
                usingLowflight = false;

                lowflightCauseDmg = true;
                for (int i = 0; i < spikeLowDmg.Length; i++)
                    spikeLowDmg[i] = true;

                rb.gravityScale = playerGravity;
                lowflightUsed = false;
                trailEmitted = false;

                if (skillNumber == 1)
                    skill1Activate = false;
                else
                    skill2Activate = false;

                flyDuration = fixedFlyDuration;

                StartCoroutine("InvencibleDelay");
            }

        } // finish low flight activation
    }

    private IEnumerator InvencibleDelay()
    {
        yield return new WaitForSeconds(invencibleDelay);

        lowflightInvulnerable = false;
    }

    // EARTHQUAKE /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private void Earthquake(int skillNumber)
    {
        if (((skillNumber == 1 && canSkill1) || (skillNumber == 2 && canSkill2)) && canUseSkill) // attend these conditions to perform the skill action
        {
            skillNum = skillNumber;
            canUseSkill = false;
            earthquakeUsed = true;

            if (skillNumber == 1)
            {
                skill1Activate = true;
                sc.skill1Activated.SetActive(true);
            }

            else
            {
                skill2Activate = true;
                sc.skill2Activated.SetActive(true);
            }
        }

        if ((skillNumber == 1 && skill1Activate) || (skillNumber == 2 && skill2Activate))
        {
            if (!usingSkill) // start action //////////////////////////////
            {
                cm.canWalk = false;
                cc.canJump = false;
                usingSkill = true;
                rb.gravityScale = 0;
                anim.SetBool("Earthquake", true);
                earthquakeInvulnerable = true;
            }

            else if (usingSkill && (cc.grounded || wasGrounded) && !pound) // lifting, will only enter here if used the skill while grounded
            {
                if (!wasGrounded)
                {
                    FindObjectOfType<AudioManager>().PlaySound("Jump");
                    anim.SetBool("Earthquake_Performed", true); // to not make the transition activate after performing skill
                    earthDuration = 0.35f; // duration for the lifting action
                    fromAir = false;
                    wasGrounded = true;
                }

                if (earthDuration > 0.3) // will move up a bit
                    rb.velocity = new Vector2(0, earthLiftVelocity);
                else // stop mid-air for cursed sword "prepare" it self
                    rb.velocity = Vector2.zero;

                if (earthDuration < 0)
                {
                    wasGrounded = false;
                    pound = true;
                    anim.SetTrigger("Earthquake_Pounding");
                }
            }

            else if (usingSkill && !cc.grounded && !wasGrounded) // falling, can enter here from grounded or not
            {
                if (!dontCountdown) // to make the update dont countdown the earthDuration
                {
                    dontCountdown = true;
                    earthDuration = fixedEarthDuration;
                }

                if (fromAir) // to activate the necessary bool's if player used the skill already in mid-air
                {
                    pound = true;
                    FindObjectOfType<AudioManager>().PlaySound("Fall");
                    anim.SetBool("Earthquake_Performed", true);
                    fromAir = false;
                }

                rb.velocity = new Vector2(0, -earthPoundVelocity); // move the player down, 'til the ground
            }

            else if (cc.grounded && pound && earthDuration > 0) // pound, when performing the falling action and reaching the ground
            {
                if (dontCountdown) // make the update countdown the earthDuration again
                {
                    rb.velocity = Vector2.zero; // stop de falling movement
                    FindObjectOfType<AudioManager>().PlaySound("Earthquake");
                    anim.SetTrigger("Earthquake_Land");

                    earthParticle.Play();
                    particlePos = earthParticle.gameObject.transform.position;
                    fixParticlePos = true;
                    dontCountdown = false;
                }

                earthquakeCollider.SetActive(true); // activate the collider of damage area
            }

            else // finish action ////////////////////////////////////////////////////////////
            {
                anim.SetBool("Earthquake", false);
                anim.SetBool("Earthquake_Performed", false);
                earthquakeCollider.SetActive(false);

                if (skillNumber == 1)
                {
                    sc.SkillCooldown(earthCooldown, 0);
                    sc.skill1Activated.SetActive(false);
                    sc.skill1Used = true;
                }

                else
                {
                    sc.SkillCooldown(0, earthCooldown);
                    sc.skill2Activated.SetActive(false);
                    sc.skill2Used = true;
                }

                canUseSkill = true;
                cm.canWalk = true;
                cc.canJump = true;
                usingSkill = false;
                rb.gravityScale = playerGravity;

                earthCauseDmg = true;
                for (int i = 0; i < spikeEarthDmg.Length; i++)
                    spikeEarthDmg[i] = true;

                earthquakeInvulnerable = false;
                pound = false;
                fromAir = true;
                earthquakeUsed = false;

                if (skillNumber == 1)
                    skill1Activate = false;
                else
                    skill2Activate = false;

                earthDuration = fixedEarthDuration;
            }

        } // finish earthquake activation

    }

    // FIRE UP ///////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private void FireUp(int skillNumber)
    {
        if (((skillNumber == 1 && canSkill1) || (skillNumber == 2 && canSkill2)) && canUseSkill) // attend these conditions to perform the skill action
        {
            skillNum = skillNumber;
            canUseSkill = false;
            usingSkill = true;

            if (skillNumber == 1)
            {
                skill1Activate = true;
                sc.skill1Activated.SetActive(true);
                canSkill1 = false;
            }

            else
            {
                skill2Activate = true;
                sc.skill2Activated.SetActive(true);
                canSkill2 = false;
            }
        }

        if ((skillNumber == 1 && skill1Activate) || (skillNumber == 2 && skill2Activate))
        {
            if (skillNumber == 1)
                skill1Activate = false;
            else
                skill2Activate = false;

            StartCoroutine("FireUpActivation", skillNumber);
        }
    }

    private IEnumerator FireUpActivation(int skillNumber)
    {
        FindObjectOfType<AudioManager>().PlaySound("FireupActive");

        foreach (SpriteRenderer material in srChildren)
            material.material = fireupMaterial;

        leftBlade.material = fireupMaterial;
        hurtMouth.material = fireupMaterial;

        ed.fireupVariation = fireupDamageIncrese;

        canUseSkill = true;
        usingSkill = false;

        yield return new WaitForSeconds(fireupTime);

        FindObjectOfType<AudioManager>().PlaySound("FireupDeactive");

        foreach (SpriteRenderer material in srChildren)
            material.material = defaultMaterial;

        leftBlade.material = defaultMaterial;
        hurtMouth.material = defaultMaterial;

        if (skillNumber == 1)
        {
            sc.SkillCooldown(fireupCooldown, 0);
            sc.skill1Activated.SetActive(false);
            sc.skill1Used = true;
        }

        else
        {
            sc.SkillCooldown(0, fireupCooldown);
            sc.skill2Activated.SetActive(false);
            sc.skill2Used = true;
        }

        ed.fireupVariation = 1.0f;

        if (skillNumber == 1)
            skill1Activate = false;
        else
            skill2Activate = false;


    }

    // LASER BEAM ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

    private void LaserBeam(int skillNumber)
    {
        if (((skillNumber == 1 && canSkill1) || (skillNumber == 2 && canSkill2)) && canUseSkill) // attend these conditions to perform the skill action
        {
            skillNum = skillNumber;
            canUseSkill = false;
            laserUsed = true;

            if (skillNumber == 1)
            {
                skill1Activate = true;
                sc.skill1Activated.SetActive(true);
            }

            else
            {
                skill2Activate = true;
                sc.skill2Activated.SetActive(true);
            }
        }

        if ((skillNumber == 1 && skill1Activate) || (skillNumber == 2 && skill2Activate))
        {
            if (!usingSkill) // start action
            {
                cm.canWalk = false;
                cc.canJump = false;
                usingSkill = true;
                rb.gravityScale = 0;
                rb.velocity = Vector2.zero;
                FindObjectOfType<AudioManager>().PlaySound("Laser");
                //laserObject.SetActive(true);
                anim.SetBool("Laser", true);
            }

            else if (usingSkill && laserDuration <= 0) // after animation
            {
                anim.SetBool("Laser", false);
                //laserObject.SetActive(false);

                if (skillNumber == 1)
                {
                    sc.SkillCooldown(laserCooldown, 0);
                    sc.skill1Activated.SetActive(false);
                    sc.skill1Used = true;
                }

                else
                {
                    sc.SkillCooldown(0, laserCooldown);
                    sc.skill2Activated.SetActive(false);
                    sc.skill2Used = true;
                }

                canUseSkill = true;
                cm.canWalk = true;
                cc.canJump = true;
                usingSkill = false;
                rb.gravityScale = playerGravity;
                laserUsed = false;

                if (skillNumber == 1)
                    skill1Activate = false;
                else
                    skill2Activate = false;

                laserDuration = fixedLaserDuration;
            }
        }
    }

    #endregion
}
