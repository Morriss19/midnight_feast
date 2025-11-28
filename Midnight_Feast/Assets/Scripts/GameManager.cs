using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private BoardManager boardManager;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Vector2Int playerStartCell = new Vector2Int(0, 0);

    private void Start()
    {
        Debug.Log("GameManager Start() called");
        
        if (playerController != null && boardManager != null)
        {
            Debug.Log("Spawning player at " + playerStartCell);
            playerController.Spawn(boardManager, playerStartCell);
        }
        else
        {
            Debug.LogError("GameManager: Missing references! BoardManager: " + (boardManager != null) + ", PlayerController: " + (playerController != null));
        }
    }
}