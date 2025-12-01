using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private int attackDamage = 1;
    [SerializeField] private PlayerController playerController;
    
    private BoardManager m_Board;
    private bool m_IsGameOver = false;

    private void Start()
    {
        // Get reference to PlayerController if not assigned
        if (playerController == null)
        {
            playerController = GetComponent<PlayerController>();
        }
        
        // Get reference to the board
        m_Board = GameManager.Instance.boardManager;
    }

    private void Update()
    {
        // Don't process input if game is over or player is currently moving
        if (m_IsGameOver || playerController.IsMoving() || m_Board == null) 
            return;
        
        HandleInput();
    }

    private void HandleInput()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null) return;

        // Press Space to attack adjacent enemies
        if (keyboard.spaceKey.wasPressedThisFrame)
        {
            Debug.Log("Attack button pressed!");
            TryAttackAdjacent();
        }
    }

    private void TryAttackAdjacent()
    {
        Vector2Int playerCell = playerController.GetCellPosition();
        
        // Check all four adjacent directions for enemies
        Vector2Int[] directions = { 
            Vector2Int.up, 
            Vector2Int.down, 
            Vector2Int.left, 
            Vector2Int.right 
        };
        
        bool attackedEnemy = false;
        
        foreach (Vector2Int direction in directions)
        {
            Vector2Int targetCell = playerCell + direction;
            
            if (TryAttackCell(targetCell))
            {
                attackedEnemy = true;
                break; // Only attack one enemy per turn
            }
        }
        
        if (attackedEnemy)
        {
            // Consume a turn after successful attack
            GameManager.Instance.turnManager.Tick();
        }
        else
        {
            Debug.Log("No adjacent enemies to attack!");
        }
    }

    private bool TryAttackCell(Vector2Int targetCell)
    {
        // Get the cell data at the target position
        BoardManager.CellData cellData = m_Board.GetCellData(targetCell);
        
        // Check if cell exists and contains an object
        if (cellData != null && cellData.ContainedObject != null)
        {
            // Try to get Enemy component
            Enemy enemy = cellData.ContainedObject as Enemy;
            
            if (enemy != null)
            {
                Debug.Log($"Player attacked enemy at {targetCell}!");
                enemy.TakeDamage(attackDamage);
                return true;
            }
        }
        
        return false;
    }

    public void GameOver()
    {
        m_IsGameOver = true;
    }
}