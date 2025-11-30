using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}
    [SerializeField] public BoardManager boardManager;
    [SerializeField] public PlayerController playerController;
    [Header("References")]
    [SerializeField] private BoardManager boardManager;
    [SerializeField] private PlayerController playerController;
    
    [Header("Spawn Settings")]
    [SerializeField] private Vector2Int playerStartCell = new Vector2Int(1, 1);
    
    [Header("Game State")]
    [SerializeField] private int score = 0;
    [SerializeField] private int enemiesDefeated = 0;
    
    // Singleton pattern for easy access from other scripts
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        // Singleton setup
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [SerializeField] public TurnManager turnManager {get; private set;}
    [SerializeField] private Vector2Int playerStartCell = new Vector2Int(1, 1);

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

        {
            // Spawn the player
            Debug.Log("Spawning player at " + playerStartCell);
            playerController.Spawn(boardManager, playerStartCell);
            
            // Spawn enemies, items, etc.
            SpawnEnemies();
            SpawnObjectives();
        }
        else
        {
            Debug.LogError("GameManager: Missing references! BoardManager: " + (boardManager != null) + ", PlayerController: " + (playerController != null));
        }
    }

    private void SpawnEnemies()
    {
        // TODO: Add enemy spawning logic here
        Debug.Log("Spawning enemies...");
        
        // Example: Spawn 5 random enemies
        // for (int i = 0; i < 5; i++)
        // {
        //     Vector2Int randomCell = GetRandomPassableCell();
        //     Instantiate(enemyPrefab, boardManager.CellToWorld(randomCell), Quaternion.identity);
        // }
    }

    private void SpawnObjectives()
    {
        // TODO: Add objective/collectible spawning logic here
        Debug.Log("Spawning objectives...");
        
        // Example: Spawn collectibles, keys, goals, etc.
    }

    private Vector2Int GetRandomPassableCell()
    {
        Vector2Int randomCell;
        int attempts = 0;
        
        do
        {
            randomCell = new Vector2Int(
                Random.Range(1, boardManager.Width - 1),
                Random.Range(1, boardManager.Height - 1)
            );
            attempts++;
        } 
        while (!boardManager.IsCellPassable(randomCell) && attempts < 100);
        
        return randomCell;
    }

    // Called by enemies or other scripts when defeated
    public void OnEnemyDefeated(int points)
    {
        enemiesDefeated++;
        AddScore(points);
        Debug.Log("Enemy defeated! Total: " + enemiesDefeated);
    }

    public void AddScore(int points)
    {
        score += points;
        Debug.Log("Score: " + score);
        // TODO: Update UI
    }

    public int GetScore()
    {
        return score;
    }

    public int GetEnemiesDefeated()
    {
        return enemiesDefeated;
    }
}