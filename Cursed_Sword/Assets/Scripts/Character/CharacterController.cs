using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Audio;

public class CharacterController : MonoBehaviour
{
    [SerializeField] private CharacterMovement cm;
    private Skill skill;

    #region Physics Variables

    [Header("Physics")]
    public PolygonCollider2D gripCol;
    public CircleCollider2D faceCol;
    public PolygonCollider2D bladeCol;
    public CapsuleCollider2D shadowCol;

    [Space(7)]

    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask whatIsGround;

    private Rigidbody2D rb;
    //private Vector3 velocity = Vector3.zero;

    #endregion

    #region Audio Variables

    [Header("Global Audio")]
    [SerializeField] private AudioMixerSnapshot mainSnapshot;

    #endregion

    #region Jump Variables

    [Header("Jump")]
    [SerializeField] private float jumpForce = 50;
    [SerializeField] private float doubleJumpVariation = 10; // to make the second jump to be lower than the first jump
    [SerializeField] private float releaseJump = 0.5f; // the value to decrease the jump height after releasing the button
    [SerializeField] private ParticleSystem jumpParticle;

    [HideInInspector] public bool canJump = true;

    private bool doubleJump = true;

    #endregion

    #region Attack Variables

    [Header("Attack")]
    public float attackDamageValue = 10;
    [SerializeField] private AudioMixerSnapshot attackSnapshot;
    [SerializeField] private TrailRenderer attackTrail;

    [HideInInspector] public bool canAttack = true; // changed mid animation, to check if player can perform the attack again
    [HideInInspector] public bool attackDamage = false; // changed mid animation, to check if player is attacking to cause damage
    [HideInInspector] public bool ignoreBladeColl = false; // changed mid animation, to check if player can receive damage by the blade collider
    [HideInInspector] public bool causeDamage; // to cause damage only one time after performing the attack
    [HideInInspector] public bool attackDelay = false; // to attack when pressing the attack button before its permited
    [HideInInspector] public bool spikeAttackDmg = true;
    [HideInInspector] public bool emitTrail = false;

    

    #endregion

    #region Animation Variables

    [Header("Animation")]

    public Animator animator;

    #endregion

    #region Transform Variables

    private Vector3 playerScale;
    private Vector3 playerPosition;

    #endregion

    #region Objects Variables

    [Header("GameObjects")]
    [SerializeField] private GameObject rightBlade;
    [SerializeField] private GameObject leftBlade;

    #endregion

    #region Others Variables

    [Header("Others")]
    [SerializeField] private float xPosChange = 1;

    UnityEvent onLandEvent;
    const float groundedRadius = .2f;
    private CharacterDamage cd;

    [HideInInspector] public bool grounded;
    [HideInInspector] public bool facingRight = true;

    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        skill = GetComponent<Skill>();
        cd = GetComponent<CharacterDamage>();

        if (onLandEvent == null)
            onLandEvent = new UnityEvent();
    }

    private void Start()
    {
        Physics2D.IgnoreLayerCollision(12, 10, true); // ignore collisions between shadow collider (layer 12) and enemies colliders (layer 10)
    }

    private void Update() // animations params
    {
        if (!PauseController.gamePaused)
        {
            // JUMP ANIMATIONS ///////////////////////////////

            animator.SetFloat("Jump", rb.velocity.y);

            if (!grounded)
                animator.SetBool("Ground", false);

            else
                animator.SetBool("Ground", true);

            // ATTACK ANIMATIONS //////////////////////////////

            if (attackDelay) // to perform the attack after some attack animation when pressing the button before the animation completes
                Attack();

            if (emitTrail)
                attackTrail.emitting = true;

            else
                attackTrail.emitting = false;
        }
    }

    public void FixedUpdate() // to check grounded
    {
        bool wasGrounded = grounded;
        grounded = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheck.position, groundedRadius, whatIsGround);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                grounded = true;

                if (!doubleJump)
                    doubleJump = true;

                if (!wasGrounded)
                    onLandEvent.Invoke();
            }
        }

    } // close fixed update

    public void Jump(InputAction.CallbackContext context)
    {
        if (canJump)
        {
            if (grounded && context.performed) // normal jump
            {
                FindObjectOfType<AudioManager>().PlaySound("Jump");
                jumpParticle.Play();
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            }

            if (!grounded && doubleJump && context.performed) // double jump (one jump in the air)
            {
                FindObjectOfType<AudioManager>().PlaySound("Jump2");
                jumpParticle.Play();
                rb.velocity = new Vector2(rb.velocity.x, jumpForce - doubleJumpVariation);
                doubleJump = false;
            }

            if (context.canceled && rb.velocity.y > 0) // hold the button to reach the apex of the jump
                rb.velocity = new Vector2(rb.velocity.x, jumpForce * releaseJump);
        }

    } // close jump method

    public void Flip()
    {
        facingRight = !facingRight;

        playerScale = transform.localScale;
        playerPosition = transform.localPosition;

        playerScale.x *= -1;

        if (!facingRight) // facing left
        {
            playerPosition.x += xPosChange;
            leftBlade.SetActive(true);
            rightBlade.SetActive(false);
        }

        else // facing right
        {
            playerPosition.x -= xPosChange;
            rightBlade.SetActive(true);
            leftBlade.SetActive(false);
        }

        transform.localScale = playerScale;
        transform.localPosition = playerPosition;

    } // close flip method

    public void Attack()
    {
        if (canAttack && !skill.usingSkill && !cd.cannotAttack)
        {

            #region Animation

            causeDamage = true;
            attackDelay = false;
            spikeAttackDmg = true;

            if (animator.GetCurrentAnimatorStateInfo(0).IsName("Sword_Attack1"))
            {
                attackSnapshot.TransitionTo(0.01f);
                FindObjectOfType<AudioManager>().PlaySound("AttackSwing");
                animator.SetTrigger("Attack2");
            }

            else if (animator.GetCurrentAnimatorStateInfo(0).IsName("Sword_Attack2"))
            {
                mainSnapshot.TransitionTo(0.01f);
                FindObjectOfType<AudioManager>().PlaySound("AttackSwing");
                animator.SetTrigger("Attack");
            }

            else
            {
                mainSnapshot.TransitionTo(0.01f);
                FindObjectOfType<AudioManager>().PlaySound("AttackSwing");
                animator.SetTrigger("Attack");
            }

            if (animator.IsInTransition(0))
            {
                //animator.ResetTrigger("Attack");
                animator.ResetTrigger("Attack2");
            }

            #endregion

        }

        else if(!canAttack && !skill.usingSkill && !cd.cannotAttack)
            attackDelay = true; // call to perform the attack right after the attack animation, if the player pressed the button befor its completion

    } // close attack method

}
