using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Movement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;

    private Vector2 movement;
    private Rigidbody2D _rb;
    private Animator animator;

    private const string _xAxis = "Horizontal";
    private const string _yAxis = "Vertical";
    private const string _LastXAxis = "LastHorizontal";
    private const string _LastYAxis = "LastVertical";

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        movement = InputManager.Movement;

        // Set animator parameters for current movement
        animator.SetFloat(_xAxis, movement.x);
        animator.SetFloat(_yAxis, movement.y);

        // Update last direction when moving
        if (movement != Vector2.zero)
        {
            animator.SetFloat(_LastXAxis, movement.x);
            animator.SetFloat(_LastYAxis, movement.y);
        }
    }

    private void FixedUpdate()
    {
        // Move the rigidbody
        _rb.linearVelocity = movement * moveSpeed;
    }
}