using UnityEngine;
using UnityEngine.Tilemaps;

public class ExitCellObject : MonoBehaviour
{
    public Tile EndTile;

    public override void Init(Vector2Int coord)
    {
       base.Init(coord);
       GameManager.Instance.BoardManager.SetCellTile(coord, EndTile);
    }

    public override void PlayerEntered()
    {
       Debug.Log("Reached the exit cell");
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
