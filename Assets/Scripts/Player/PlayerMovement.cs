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
    
    [SerializeField] private PlayerFloor playerFloor;
    
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
            PlayAnimation(IdleAnimationFor(lastDirection));
            return;
        }

        lastDirection = DirectionFromInput(moveInput);
        PlayAnimation(RunAnimationFor(lastDirection));
    }

    // Maps analog movement input to one of 8 compass directions.
    static string DirectionFromInput(Vector2 input)
    {
        float angle = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg;
        if (angle < 0) angle += 360f;

        int index = Mathf.RoundToInt(angle / 45f) % 8;
        switch (index)
        {
            case 0: return "right";
            case 1: return "NE";
            case 2: return "up";
            case 3: return "NW";
            case 4: return "left";
            case 5: return "SW";
            case 6: return "down";
            default: return "SE";
        }
    }

    static string IdleAnimationFor(string direction)
    {
        switch (direction)
        {
            case "left": return "Idle_left";
            case "right": return "Idle_right";
            case "up": return "Idle_up";
            case "down": return "Idle";
            case "NE": return "Idle_NE";
            case "NW": return "Idle_NW";
            case "SE": return "Idle_SE";
            default: return "Idle_SW";
        }
    }

    static string RunAnimationFor(string direction)
    {
        switch (direction)
        {
            case "left": return "Run_left";
            case "right": return "Run_Right";
            case "up": return "Run_up";
            case "down": return "Run_down";
            case "NE": return "Run_NE";
            case "NW": return "Run_NW";
            case "SE": return "Run_SE";
            default: return "Run_SW";
        }
    }

    public void BottomTriggerEnter(Collider2D other)
    {
        if (other.CompareTag("sortcol") && (playerFloor.IsOnTopFloor == false))
        {
            _renderer.sortingOrder = sortbehind;
        }
        else
        {
            _renderer.sortingOrder = 13;
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