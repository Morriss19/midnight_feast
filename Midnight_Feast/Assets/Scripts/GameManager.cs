using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {get; private set;}
    [SerializeField] public BoardManager boardManager;
    [SerializeField] public PlayerController playerController;

    [SerializeField] public TurnManager turnManager {get; private set;}
    [SerializeField] private Vector2Int playerStartCell = new Vector2Int(1, 1);

    [SerializeField] private int m_FoodAmount = 200;
    [SerializeField] public UIDocument UIDoc;
    [SerializeField] private Label m_FoodLabel;

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
        
        m_FoodLabel = UIDoc.rootVisualElement.Q<Label>("FoodLabel");
        m_FoodLabel.text = "Food : " + m_FoodAmount;

        if (playerController != null && boardManager != null)
        {   
            turnManager = new TurnManager();
            turnManager.OnTick += OnTurnHappen;

            boardManager.Init();

            Debug.Log("Spawning player at " + playerStartCell);
            playerController.Spawn(boardManager, playerStartCell);
        }
        else
        {
            Debug.LogError("GameManager: Missing references! BoardManager: " + (boardManager != null) + ", PlayerController: " + (playerController != null));
        }
    }

    void OnTurnHappen()
    {
       ChangeFood(-1);
    }

    public void ChangeFood(int amount)
    {
        m_FoodAmount += amount;
        m_FoodLabel = "Food : " + m_FoodAmount;
    }
}