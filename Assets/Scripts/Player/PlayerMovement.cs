using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] private Animator _animator;

    Rigidbody2D rb;
    InputSystem_Actions controls;
    Vector2 moveInput;
    string lastDirection = "down";

    void Awake()
    {
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
                _animator.Play("Idle_left");
            else if (lastDirection == "right")
                _animator.Play("Idle_right");
            else if (lastDirection == "up")
                _animator.Play("Idle_up");
            else
                _animator.Play("Idle");

            return;
        }

        if (Mathf.Abs(moveInput.x) > Mathf.Abs(moveInput.y))
        {
            if (moveInput.x < 0)
            {
                lastDirection = "left";
                _animator.Play("Run_left");
            }
            else if (moveInput.x > 0)
            {
                lastDirection = "right";
                _animator.Play("Run_Right");
            }
        }
        else
        {
            if (moveInput.y > 0)
            {
                lastDirection = "up";
                _animator.Play("Run_up");
            }
            else if (moveInput.y < 0)
            {
                lastDirection = "down";
                _animator.Play("Run_down");
            }
        }
    }
}