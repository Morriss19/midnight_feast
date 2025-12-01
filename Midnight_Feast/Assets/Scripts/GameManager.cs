using UnityEngine;
using UnityEngine.UIElements;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}
    [SerializeField] public BoardManager boardManager;
    [SerializeField] public PlayerController playerController;

    [SerializeField] private UIDocument uiDocument;
    [SerializeField] public TurnManager turnManager {get; private set;}
    [SerializeField] private Vector2Int playerStartCell = new Vector2Int(1, 1);

    private VisualElement m_GameOverPanel;
    private Label m_GameOverMessage;
    [SerializeField] private int m_CurrentLevel;
    [SerializeField] public int m_FoodAmount;
    private Label m_FoodLabel;
    
    private void Awake()
   {
       if (Instance != null)
       {
           Destroy(gameObject);
           return;
       }
      
       Instance = this;

       turnManager = new TurnManager();
   }
    void Start()
    {
        Debug.Log("GameManager Start() called");

        var root = uiDocument.rootVisualElement;

        m_GameOverPanel = root.Q<VisualElement>("GameOverPanel");
        m_GameOverMessage = root.Q<Label>("GameOverMessage");
        m_FoodLabel = root.Q<Label>("FoodLabel");

        StartNewGame();
    }

    public void StartNewGame()
    {
        m_GameOverPanel.style.visibility = Visibility.Hidden;
        
        turnManager.m_TurnCount = 1;
        m_CurrentLevel = 1;
        m_FoodAmount = 10;
        m_FoodLabel.text = "Food : " + m_FoodAmount;

        if (playerController != null && boardManager != null)
        {   
            //boardManager.Clean();
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
        m_GameOverMessage.text = "Game Over!\n\nYou traveled through " + m_CurrentLevel + " levels\n\nPress Enter to play again!";
    }
    }
}