using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterDamage : MonoBehaviour
{
    [SerializeField] private float knockbackVelocity = 1;
    [SerializeField] private Animator anim;

    [HideInInspector] public bool cannotAttack = false;

    private Health he;
    private CharacterController cc;
    private CharacterMovement cm;
    private Skill sk;
    private Rigidbody2D rb;

    private bool takedDamage = false;
    private bool knockback = false;
    private float fixedGravity;
    private float hitAnimTime = 0.33f * 4;
    private float fixedHitAnimTime = 0.33f * 4;

    private void Awake()
    {
        he = GetComponent<Health>();
        cc = GetComponent<CharacterController>();
        cm = GetComponent<CharacterMovement>();
        sk = GetComponent<Skill>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        fixedGravity = rb.gravityScale;
    }

    private void Update()
    {
        if (!PauseController.gamePaused)
        {
            if (takedDamage)
            {
                if (hitAnimTime <= 0)
                {
                    takedDamage = false;
                    he.damageable = true;
                    cc.animator.SetTrigger("DamageNull");
                    hitAnimTime = fixedHitAnimTime;
                }

                hitAnimTime -= Time.deltaTime;
            }
        }
    }

    private void FixedUpdate()
    {
        if (knockback)
        {
            if (cc.facingRight)
                rb.velocity = new Vector2(-knockbackVelocity, 0);

            else
                rb.velocity = new Vector2(knockbackVelocity, 0);
        }
    }

    public void DamagePlayer(float dmg)
    {
        if (he.damageable && !sk.lowflightInvulnerable && !sk.earthquakeInvulnerable)
        {
            he.damageable = false;
            cm.canWalk = false;
            cc.canAttack = false;
            cannotAttack = true;
            cc.attackDelay = false;
            cc.canJump = false;
            sk.canUseSkill = false;
            rb.gravityScale = 0;

            he.HealthLossVariation(dmg, false);

            FindObjectOfType<AudioManager>().PlaySound("DamageTaken");
            anim.SetBool("Hurt", true);
            cc.animator.SetTrigger("Damage");
            knockback = true;

            if (he.currentHealth > 0)
                StartCoroutine("DamageReceived");

            else
            {
                knockback = false;
                rb.velocity = Vector2.zero;
            }
        }
    }

    private IEnumerator DamageReceived()
    {
        yield return new WaitForSeconds(0.25f);

        anim.SetBool("Hurt", false);
        if (he.currentHealth > 0)
        {
            cc.animator.SetTrigger("DamageBlink");
            knockback = false;
            cm.canWalk = true;
            takedDamage = true;
            cc.canJump = true;
            cc.canAttack = true;
            cannotAttack = false;
            sk.canUseSkill = true;
            rb.gravityScale = fixedGravity;
        }
    }
}
