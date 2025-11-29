using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] public BoardManager boardManager;
    [SerializeField] public PlayerController playerController;

    [SerializeField] private TurnManager m_TurnManager;
    [SerializeField] private Vector2Int playerStartCell = new Vector2Int(0, 0);

    void Start()
    {
        Debug.Log("GameManager Start() called");
        
        if (playerController != null && boardManager != null)
        {   
            m_TurnManager = new TurnManager();

            Debug.Log("Spawning player at " + playerStartCell);
            playerController.Spawn(boardManager, playerStartCell);
        }
        else
        {
            Debug.LogError("GameManager: Missing references! BoardManager: " + (boardManager != null) + ", PlayerController: " + (playerController != null));
        }
    }
}