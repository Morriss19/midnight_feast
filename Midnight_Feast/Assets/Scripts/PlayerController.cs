using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Camera mainCamera;
    
    private BoardManager m_Board;
    public Vector2Int m_CellPosition;
    private Vector3 m_TargetPosition;
    private bool m_IsMoving = false;

    private bool m_IsGameOver;
    private Animator m_Animator;
    
    private Vector2 minBounds;
    private Vector2 maxBounds;
    private float animationCooldown = 0f;
    
    private void Awake()
    {
        m_Animator = GetComponent<Animator>();
    }
    
    public void Init()
    {
        m_IsGameOver = false;
    }
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

        if (m_IsGameOver) // Checks for game over and allows restart with pressing enter
        {
            if (Keyboard.current.enterKey.wasPressedThisFrame)
            {
                GameManager.Instance.StartNewGame();
            }

            return;
        }

        if (!m_IsMoving && animationCooldown > 0)
        {
            animationCooldown -= Time.deltaTime;

            if (animationCooldown <= 0 && m_Animator != null)
            {
                m_Animator.SetBool("Walk", false);
            }
        }

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
            RotatePlayer(direction);
            TryMove(direction);
        }
    }

    private void TryMove(Vector2Int direction)
    {
        Vector2Int targetCell = m_CellPosition + direction;

        BoardManager.CellData cellData = m_Board.GetCellData(targetCell);

        if (cellData != null && cellData.Passable)
        {
            m_CellPosition = targetCell;
            m_TargetPosition = m_Board.CellToWorld(targetCell); 
            m_IsMoving = true;

            GameManager.Instance.turnManager.Tick();

            if (cellData.ContainedObject != null)
            {
                cellData.ContainedObject.PlayerEntered();
            } 
        }
        else
        {
            Debug.Log("Can't Move through Walls") ;
        }

        if (m_Animator != null) 
            m_Animator.SetBool("Walk", m_IsMoving);
    }

    private void RotatePlayer(Vector2Int direction)
    {
        if (m_Animator == null) return;

        if (direction == Vector2Int.up)
            m_Animator.SetInteger("Rotate", 3);
        else if (direction == Vector2Int.down)
            m_Animator.SetInteger("Rotate", 1);
        else if (direction == Vector2Int.left)
            m_Animator.SetInteger("Rotate", 2);
        else if (direction == Vector2Int.right)
            m_Animator.SetInteger("Rotate", 4);

        Debug.Log("Rotate set to: " + direction);
    }
    
    private void MoveTowardsTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, m_TargetPosition, moveSpeed * Time.deltaTime);

        // Check if we've reached the target
        if (Vector3.Distance(transform.position, m_TargetPosition) < 0.001f)
        {
            transform.position = m_TargetPosition;
            m_IsMoving = false;
            animationCooldown = 0.4f;
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

    public void GameOver()
    {
        m_IsGameOver = true;
    }
}