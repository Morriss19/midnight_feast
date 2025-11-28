 using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    
    private BoardManager m_Board;
    private Vector2Int m_CellPosition;
    private Vector3 m_TargetPosition;
    private bool m_IsMoving = false;
    
    public void Spawn(BoardManager boardManager, Vector2Int cell)
    {
        m_Board = boardManager;
        m_CellPosition = cell;
        m_TargetPosition = m_Board.CellToWorld(cell);
        transform.position = m_TargetPosition;
    }

    void Update()
    {
        // Don't do anything if not spawned yet
        if (m_Board == null) return;

        if (m_IsMoving)
        {
            MoveTowardsTarget();
        }
        else
        {
            HandleInput();
        }
    }

    private void HandleInput()
    {
        Vector2Int direction = Vector2Int.zero;

        Keyboard keyboard = Keyboard.current;
        if (keyboard == null) return;

        if (keyboard.wKey.wasPressedThisFrame || keyboard.upArrowKey.wasPressedThisFrame)
        {
            direction = Vector2Int.up;
        }
        else if (keyboard.sKey.wasPressedThisFrame || keyboard.downArrowKey.wasPressedThisFrame)
        {
            direction = Vector2Int.down;
        }
        else if (keyboard.aKey.wasPressedThisFrame || keyboard.leftArrowKey.wasPressedThisFrame)
        {
            direction = Vector2Int.left;
        }
        else if (keyboard.dKey.wasPressedThisFrame || keyboard.rightArrowKey.wasPressedThisFrame)
        {
            direction = Vector2Int.right;
        }

        if (direction != Vector2Int.zero)
        {
            TryMove(direction);
        }
    }

    private void TryMove(Vector2Int direction)
    {
        Vector2Int targetCell = m_CellPosition + direction;

        // Move to the target cell (add validation later if needed)
        m_CellPosition = targetCell;
        m_TargetPosition = m_Board.CellToWorld(targetCell);
        m_IsMoving = true;
    }

    private void MoveTowardsTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, m_TargetPosition, moveSpeed * Time.deltaTime);

        // Check if we've reached the target
        if (Vector3.Distance(transform.position, m_TargetPosition) < 0.001f)
        {
            transform.position = m_TargetPosition;
            m_IsMoving = false;
        }
    }

    public Vector2Int GetCellPosition()
    {
        return m_CellPosition;
    }

    public bool IsMoving()
    {
        return m_IsMoving;
    }
}