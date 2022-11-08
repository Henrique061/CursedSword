using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private CharacterController cc;

    #region Physics Variables

    Rigidbody2D rb;
    private Vector3 velocity = Vector3.zero;

    #endregion

    #region Walk Variables

    [Header("Walk")]

    [SerializeField] private float walkForce = 5;

    [HideInInspector] public float attackingWalk = 1; // to slow down the character walk while attacking
    [HideInInspector] public bool canWalk = true;

    #endregion

    #region Input Variables

    public InputMaster input;

    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        input = new InputMaster();

        // Jump
        input.PlayerControl.Jump.performed += ctx => cc.Jump(ctx); // access the InputMaster/ActionMap/Action.performed  += context => method(context*); *if neccessary
        input.PlayerControl.Jump.canceled  += ctx => cc.Jump(ctx); // to make the player drop after release the button

        // Attack
        input.PlayerControl.Attack.performed += ctx => cc.Attack(); // perform attack when pressing square

    } // close Awake()

    private void Update()
    {
        if (PauseController.gamePaused)
            input.PlayerControl.Disable();

        else
            input.PlayerControl.Enable();
    }
    private void FixedUpdate()
    {
        // Move
        if (canWalk)
            Move();

    } // close FixedUpdate()

    public void Move()
    {
        Vector2 inputVector = input.PlayerControl.Movement.ReadValue<Vector2>(); // reads the value of vector2 of movement input

        if (inputVector.x > 0 && !cc.facingRight) // facing left to right
            cc.Flip();

        else if (inputVector.x < 0 && cc.facingRight) // facing right to left
            cc.Flip();

        rb.velocity = new Vector2((inputVector.x * walkForce) * attackingWalk, rb.velocity.y);

        cc.animator.SetFloat("Walk", Mathf.Abs(inputVector.x));

    } // close method Move()

    

    private void OnEnable()
    {
        input.Enable();
    }

    private void OnDisable()
    {
        input.Disable();
    }

}
