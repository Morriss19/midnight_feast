using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Camera mainCamera;
    
    private BoardManager m_Board;
    private Vector2Int m_CellPosition;
    private Vector3 m_TargetPosition;
    private bool m_IsMoving = false;
    
    private Vector2 minBounds;
    private Vector2 maxBounds;
    
    public void Spawn(BoardManager boardManager, Vector2Int cell)
    {
        m_Board = boardManager;
        m_CellPosition = cell;
        m_TargetPosition = m_Board.CellToWorld(cell);
        transform.position = m_TargetPosition;
        
        // Get camera if not assigned
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        
        // Calculate camera bounds
        CalculateCameraBounds();
    }
    
    private void CalculateCameraBounds()
    {
        if (mainCamera == null) return;
        
        float cameraHeight = mainCamera.orthographicSize * 2f;
        float cameraWidth = cameraHeight * mainCamera.aspect;
        
        Vector3 cameraPos = mainCamera.transform.position;
        
        minBounds = new Vector2(cameraPos.x - cameraWidth / 2f, cameraPos.y - cameraHeight / 2f);
        maxBounds = new Vector2(cameraPos.x + cameraWidth / 2f, cameraPos.y + cameraHeight / 2f);
    }

    void Update()
    {
        // Don't do anything if not spawned yet
        if (m_Board == null) return;

        // Update camera bounds every frame (in case camera moves)
        CalculateCameraBounds();

        if (m_IsMoving)
        {   
            GameManager.Instance.turnManager.Tick();
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

        // Check if the target cell is passable
        if (m_Board.IsCellPassable(targetCell))
        {
            m_CellPosition = targetCell;
            m_TargetPosition = m_Board.CellToWorld(targetCell);
            m_IsMoving = true;
        }
        else
        {
            Debug.Log("Can't move through walls!");
        }
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