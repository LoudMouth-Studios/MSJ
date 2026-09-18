using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _renderer;
    [SerializeField] private int sortbehind;
    [SerializeField] private int sortdefault;
    Rigidbody2D rb;
    InputSystem_Actions controls;
    Vector2 moveInput;
    string lastDirection = "down";
    string currentAnimation;
    
    

    void Awake()
    {
        sortdefault = _renderer.sortingOrder;
        rb = GetComponent<Rigidbody2D>();
        controls = new InputSystem_Actions();

        if (_animator == null)
            _animator = GetComponentInChildren<Animator>();
    }

    void OnEnable()
    {
        controls.Player.Enable();
        controls.Player.Move.performed += OnMove;
        controls.Player.Move.canceled += OnMove;
    }

    void OnDisable()
    {
        controls.Player.Move.performed -= OnMove;
        controls.Player.Move.canceled -= OnMove;
        controls.Player.Disable();
    }

    void OnMove(InputAction.CallbackContext ctx)
    {
        moveInput = ctx.ReadValue<Vector2>();
        UpdateAnimation();
    }

    void FixedUpdate()
    {
        Vector2 delta = moveInput.normalized * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(rb.position + delta);

        if (moveInput == Vector2.zero)
        {
            UpdateAnimation();
            return;
        }

        UpdateAnimation();
    }

    void UpdateAnimation()
    {
        if (_animator == null)
            return;

        if (moveInput == Vector2.zero)
        {
            if (lastDirection == "left")
                PlayAnimation("Idle_left");
            else if (lastDirection == "right")
                PlayAnimation("Idle_right");
            else if (lastDirection == "up")
                PlayAnimation("Idle_up");
            else
                PlayAnimation("Idle");

            return;
        }

        if (Mathf.Abs(moveInput.x) > Mathf.Abs(moveInput.y))
        {
            if (moveInput.x < 0)
            {
                lastDirection = "left";
                PlayAnimation("Run_left");
            }
            else if (moveInput.x > 0)
            {
                lastDirection = "right";
                PlayAnimation("Run_Right");
            }
        }
        else
        {
            if (moveInput.y > 0)
            {
                lastDirection = "up";
                PlayAnimation("Run_up");
            }
            else if (moveInput.y < 0)
            {
                lastDirection = "down";
                PlayAnimation("Run_down");
            }
        }
    }

    public void BottomTriggerEnter(Collider2D other)
    {
        if (other.CompareTag("sortcol"))
        {
            _renderer.sortingOrder = sortbehind;
        }
    }

    public void BottomTriggerExit(Collider2D other)
    {
        if (other.CompareTag("sortcol"))
        {
            _renderer.sortingOrder = sortdefault;
        }
    }

    void PlayAnimation(string stateName)
    {
        if (currentAnimation == stateName)
            return;

        _animator.Play(stateName);
        currentAnimation = stateName;
    }
}