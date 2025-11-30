using UnityEngine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    [SerializeField] private float attackRange = 0.5f;
    [SerializeField] private LayerMask EnemyLayer; 
    
    private PlayerController playerController;
    private Vector2Int lastDirection = Vector2Int.down;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        
        if (playerController == null)
        {
            Debug.LogError("PlayerAttack requires a PlayerController component!");
        }
    }

    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null) return;

        if (keyboard.spaceKey.wasPressedThisFrame)
        {
            Attack();
        }

        // Track direction for attacks (same as movement direction)
        UpdateFacingDirection();
    }

    private void UpdateFacingDirection()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null) return;

        if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed)
        {
            lastDirection = Vector2Int.up;
        }
        else if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed)
        {
            lastDirection = Vector2Int.down;
        }
        else if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed)
        {
            lastDirection = Vector2Int.left;
        }
        else if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed)
        {
            lastDirection = Vector2Int.right;
        }
    }

    private void Attack()
    {
        Vector3 attackPosition = transform.position + new Vector3(lastDirection.x, lastDirection.y, 0) * attackRange;

        Debug.Log("Player attacks in direction: " + lastDirection);

        // Find all colliders in attack range
        Collider2D[] hits = Physics2D.OverlapCircleAll(attackPosition, attackRange, enemyLayer);

        foreach (Collider2D hit in hits)
        {
            Debug.Log("Hit: " + hit.gameObject.name);

            // Try to damage enemy
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
        }
    }

    // Visualize attack range in Scene view
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 attackPos = transform.position + new Vector3(lastDirection.x, lastDirection.y, 0) * attackRange;
        Gizmos.DrawWireSphere(attackPos, attackRange);
    }
}