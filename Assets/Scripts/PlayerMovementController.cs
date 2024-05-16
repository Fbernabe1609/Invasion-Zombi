using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

public class PlayerMovementController : MonoBehaviour
{
    public GameObject[] attackTriggers = new GameObject[2];

    [SerializeField] private float charSpeed = 5.0f;
    [SerializeField] private Animator animator;
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioClip hitSound;

    internal bool isAttacking = false;

    private CharacterController charController;
    private Vector3 moveDirection;
    private Vector2 movementInput;
    private float verticalVelocity;
    private float gravity = 20f;
    private PlayerControls playerControls;
    private CharacterController controller;
    private AudioSource audioSource;
    private static readonly int Attack1 = Animator.StringToHash("Attack");
    private static readonly int Speed = Animator.StringToHash("Speed");

    private void Awake()
    {
        charController = GetComponent<CharacterController>();
        controller = GetComponent<CharacterController>();
        audioSource = GetComponent<AudioSource>();
        playerControls = new PlayerControls();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        for (int i = 0; i < attackTriggers.Length; i++)
        {
            attackTriggers[i].SetActive(false);
        }
    }

    private void Start()
    {
        animator.Play("Idle");
    }

    private void OnEnable()
    {
        playerControls.Enable();

        playerControls.Gameplay.Movement.performed += OnMovement;
        playerControls.Gameplay.Movement.canceled += OnMovement;
        playerControls.Gameplay.Attack.performed += Attack;
    }

    private void OnDisable()
    {
        playerControls.Gameplay.Movement.performed -= OnMovement;
        playerControls.Gameplay.Movement.canceled -= OnMovement;
        playerControls.Gameplay.Attack.performed -= Attack;

        playerControls.Disable();
    }

    private void Update()
    {
        MovementCalculation();
        SendSpeedToAnimator();
    }

    private void ApplyGravity()
    {
        if (controller.isGrounded)
        {
            verticalVelocity = -5f;
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }

        moveDirection.y = verticalVelocity * Time.deltaTime;
    }

    private void SendSpeedToAnimator()
    {
        float speed = charController.velocity.magnitude;
        animator.SetFloat(Speed, speed);
    }

    private void MovementCalculation()
    {
        moveDirection = new Vector3(movementInput.x, 0.0f, movementInput.y);

        moveDirection = transform.TransformDirection(moveDirection);
        moveDirection *= charSpeed * Time.deltaTime;

        ApplyGravity();

        charController.Move(moveDirection);
    }

    private void OnMovement(InputAction.CallbackContext context)
    {
        movementInput = context.ReadValue<Vector2>();
    }

    private void Attack(InputAction.CallbackContext context)
    {
        if (isAttacking) return;

        isAttacking = true;

        for (int i = 0; i < attackTriggers.Length; i++)
        {
            attackTriggers[i].SetActive(true);
        }

        animator.SetTrigger(Attack1);
    }

    internal void PlayHitSound()
    {
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.PlayOneShot(hitSound);
    }

    internal void PlayAttackSound()
    {
        audioSource.pitch = Random.Range(0.8f, 1.2f);
        audioSource.PlayOneShot(attackSound);
    }
}
