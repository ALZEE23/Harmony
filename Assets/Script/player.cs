using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class Player : MonoBehaviour
{
    public int health = 20;
    public float walkingSpeed = 7.5f;
    public float runningSpeed = 11.5f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;

    public float lookSpeed = 2.0f;
    public float lookXLimit = 45.0f;    

    public Animator lookAnimator;
    public SpriteRenderer spriteRenderer;

    private CharacterController characterController;
    private Vector3 moveDirection = Vector3.zero;
    public static PlayerInputActions playerInputActions; // Make this static

    [HideInInspector]
    public bool canMove = true;

    void Awake()
    {
        characterController = GetComponent<CharacterController>();
        lookAnimator = GetComponent<Animator>();

        playerInputActions = new PlayerInputActions(); // Initialize in Awake
    }

    void OnEnable()
    {
        playerInputActions.Player.Enable();
    }

    void OnDisable()
    {
        playerInputActions.Player.Disable();
    }

    void Update()
    {
        Movement();
    }

    public void Movement()
    {
        Vector2 input = playerInputActions.Player.Move.ReadValue<Vector2>();
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        bool isRunning = playerInputActions.Player.Run.ReadValue<float>() > 0;
        float curSpeedX = canMove ? (isRunning ? runningSpeed : walkingSpeed) * input.y : 0;
        float curSpeedY = canMove ? (isRunning ? runningSpeed : walkingSpeed) * input.x : 0;
        float movementDirectionY = moveDirection.y;
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        if (playerInputActions.Player.Jump.triggered && canMove && characterController.isGrounded)
        {
            moveDirection.y = jumpSpeed;
        }
        else
        {
            moveDirection.y = movementDirectionY;
        }

        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        characterController.Move(moveDirection * Time.deltaTime);

        if (moveDirection.x < 0)
        {
            spriteRenderer.flipX = true;
        }
        else
        {
            spriteRenderer.flipX = false;
        }

        lookAnimator.SetBool("run", moveDirection.x != 0);
    }
}
