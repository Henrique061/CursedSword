using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Audio;

public class MageMovement : MonoBehaviour
{
    [SerializeField] private float walkForce = 5;
    [SerializeField] private float xPosChange = 0.3f;
    [SerializeField] private float footStepSound = 0.5f;
    [SerializeField] private AudioMixer am;
    [SerializeField] private AudioMixerSnapshot mainSnap;
    [SerializeField] private AudioMixerSnapshot footSnap;

    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public bool facingRight = false;
    private Vector3 velocity = Vector3.zero;
    private Vector3 playerScale;
    private Vector3 playerPosition;
    private InputMaster input;
    private bool reproduceMainSnap = true;
    private Animator anim;
    private float fixedFootStepSound;

    [HideInInspector] public bool canMove = true;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        input = new InputMaster();

        fixedFootStepSound = footStepSound;
    }

    private void Update()
    {
        if (!PauseController.gamePaused)
        {
            if (footStepSound > 0)
                footStepSound -= Time.deltaTime;
        }
    }


    void FixedUpdate()
    {
        if (canMove)
            Move();
    }

    private void Move()
    {
        if (canMove)
        {
            Vector2 inputVector = input.PlayerControl.Movement.ReadValue<Vector2>(); // reads the value of vector2 of movement input

            if (inputVector.x > 0 && !facingRight) // facing left to right
                Flip();

            else if (inputVector.x < 0 && facingRight) // facing right to left
                Flip();

            rb.velocity = new Vector2((inputVector.x * walkForce), rb.velocity.y);

            if (footStepSound <= 0 && (inputVector.x > 0 || inputVector.x < 0))
            {
                if (reproduceMainSnap)
                {
                    mainSnap.TransitionTo(0.01f);
                    reproduceMainSnap = !reproduceMainSnap;
                }

                else
                {
                    footSnap.TransitionTo(0.01f);
                    reproduceMainSnap = !reproduceMainSnap;
                }

                FindObjectOfType<AudioManager>().PlaySound("MageFootstep");
                footStepSound = fixedFootStepSound;

            }

            anim.SetFloat("Walk", Mathf.Abs(inputVector.x));
        }
    }

    private void Flip()
    {
        facingRight = !facingRight;

        playerScale = transform.localScale;
        playerPosition = transform.localPosition;

        playerScale.x *= -1;

        if (!facingRight) // facing left
            playerPosition.x += xPosChange;

        else // facing right
            playerPosition.x -= xPosChange;

        transform.localScale = playerScale;
        transform.localPosition = playerPosition;
    }

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }
}
