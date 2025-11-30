using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}
    [SerializeField] public BoardManager boardManager;
    [SerializeField] public PlayerController playerController;

    [SerializeField] public TurnManager turnManager {get; private set;}
    [SerializeField] private Vector2Int playerStartCell = new Vector2Int(1, 1);

    [SerializeField] private int m_Health = 200;
    private void Awake()
   {
       if (Instance != null)
       {
           Destroy(gameObject);
           return;
       }
      
       Instance = this;
   }
    void Start()
    {
        Debug.Log("GameManager Start() called");
        
        if (playerController != null && boardManager != null)
        {   
            turnManager = new TurnManager();

            boardManager.Init();

            Debug.Log("Spawning player at " + playerStartCell);
            playerController.Spawn(boardManager, playerStartCell);
        }
        else
        {
            Debug.LogError("GameManager: Missing references! BoardManager: " + (boardManager != null) + ", PlayerController: " + (playerController != null));
        }
    }
}