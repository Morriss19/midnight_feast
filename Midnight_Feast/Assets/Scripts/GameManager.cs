using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}
    [SerializeField] public BoardManager boardManager;
    [SerializeField] public PlayerController playerController;

    [SerializeField] public TurnManager turnManager {get; private set;}
    [SerializeField] private Vector2Int playerStartCell = new Vector2Int(1, 1);

    [SerializeField] private int m_Health = 200;

    [SerializeField] private VisualElement m_GameOverPanel;
    [SerializeField] private Label m_GameOverMessage;
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

        turnManager = new TurnManager();

        m_GameOverPanel = UIDoc.rootVisualElement.Q<VisualElement>("GameOverPanel");
        m_GameOverMessage = m_GameOverPanel.Q<Label>("GameOverMessage");

        StartNewGame();
    }

    public void StartNewGame()
    {
        m_GameOverPanel.style.visibility = Visibility.Hidden;
        
        m_CurrentLevel = 1;
        m_FoodAmount = 20;
        m_FoodLabel.text = "Food : " + m_FoodAmount;

        if (playerController != null && boardManager != null)
        {   
            boardManager.Clean();
            boardManager.Init();

            playerController.Init();
            Debug.Log("Spawning player at " + playerStartCell);
            playerController.Spawn(boardManager, playerStartCell);
        }
        else
        {
            Debug.LogError("GameManager: Missing references! BoardManager: " + (boardManager != null) + ", PlayerController: " + (playerController != null));
        }
    }

    public void ChangeFood(int amount)
    {
    m_FoodAmount += amount;
    m_FoodLabel.text = "Food : " + m_FoodAmount;

    if (m_FoodAmount <= 0)
    {   
        playerController.GameOver();
        m_GameOverPanel.style.visibility = Visibility.Visible;
        m_GameOverMessage.text = "Game Over!\n\nYou traveled through " + m_CurrentLevel + " levels";
    }
    }
}