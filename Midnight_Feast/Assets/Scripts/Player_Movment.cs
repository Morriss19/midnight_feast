using UnityEngine;
using UnityEngine.inputSystem;

public class Player_Movment : MonoBehaviour
{
[SerializeField] private float moveSpeed = 5f;

private Vector2 movement ;
private Rigidbody2D _rb;
private Animator animator;

private const string _xAxis = "Horizontal";
private const string _yAxis = "Vertical";
private const string _LastXAxis = "LastHorizontal";
private const string _LastYAxis = "LastVertical";
private void update()
    {
        movement.set(InputManager.movement.x, InputManager.movement.y);

        _rb.velocity = movement + moveSpeed;

        animator.SetFloat()
    }
}
