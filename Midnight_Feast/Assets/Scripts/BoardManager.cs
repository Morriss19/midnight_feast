using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class BoardManager : MonoBehaviour
{
    public class CellData
    {
        public bool Passable; // Decides if tiles can be passed through or not
        public CellObject ContainedObject;
    }

    private CellData[,] m_BoardData;
    private List<Vector2Int> m_EmptyCellsList;
    private Tilemap m_Tilemap;
    private Grid m_Grid;

    public int Width;
    public int Height;
    public Tile[] GroundTiles;
    public Tile[] WallTiles;
    public PlayerController Player;
    public FoodObject FoodPrefab;
    public int foodCountMin;
    public int foodCountMax;
    
    public Vector3 CellToWorld(Vector2Int cellIndex)
    {
        return m_Grid.GetCellCenterWorld((Vector3Int)cellIndex);
    }
    public bool IsCellPassable(Vector2Int cellIndex)
    {
    // Check if cell is within bounds
    if (cellIndex.x < 0 || cellIndex.x >= Width || cellIndex.y < 0 || cellIndex.y >= Height)
    {
        return false;
    }    
    // Check if cell is passable
    return m_BoardData[cellIndex.x, cellIndex.y].Passable;
    }
    public CellData GetCellData(Vector2Int cellIndex)
    {
        
        if (cellIndex.x < 0 || cellIndex.x >= Width || cellIndex.y < 0 || cellIndex.y >= Height)
        {
            return null;
        }
        return m_BoardData[cellIndex.x, cellIndex.y];
    }
    void GenerateFood()
    {
        int foodCount = Random.Range(foodCountMin, foodCountMax);
        for (int i = 0; i < foodCount; ++i)
        {
            int randomIndex = Random.Range(0, m_EmptyCellsList.Count);
            Vector2Int coord = m_EmptyCellsList[randomIndex];
            
            m_EmptyCellsList.RemoveAt(randomIndex);
            CellData data = m_BoardData[coord.x, coord.y];
            FoodObject newFood = Instantiate(FoodPrefab);
            newFood.transform.position = CellToWorld(coord);
            data.ContainedObject = newFood;
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Init()
    {
        m_Tilemap = GetComponentInChildren<Tilemap>();
        m_Grid = GetComponentInChildren<Grid>();

        // initializes the list
        m_EmptyCellsList = new List<Vector2Int>();

        m_BoardData = new CellData[Width, Height];

        for (int y = 0; y < Height; y++)
        {
            for(int x = 0; x < Width; x++)
            {
                Tile tile;
                m_BoardData[x,y] = new CellData();

                if(x == 0 || y == 0 || x == Width-1 || y == Height - 1)
                {
                    tile = WallTiles[Random.Range(0, WallTiles.Length)];
                    m_BoardData[x,y].Passable = false;
                }
                else
                {
                    tile = GroundTiles[Random.Range(0, GroundTiles.Length)];
                    m_BoardData[x,y].Passable = true;

                    //passable empty cell
                    m_EmptyCellsList.Add(new Vector2Int(x, y));
                }

                m_Tilemap.SetTile(new Vector3Int(x, y, 0), tile);
            }
        }

        // remove the starting point of player
        m_EmptyCellsList.Remove(new Vector2Int(1, 1));
        GenerateFood();
    }
    public CellObject GetCellObject(Vector2Int cellIndex)
    {
    // Check if cell is within bounds
        if (cellIndex.x < 0 || cellIndex.x >= Width || cellIndex.y < 0 || cellIndex.y >= Height)
        {
            return null;
        }
    
        return m_BoardData[cellIndex.x, cellIndex.y].ContainedObject;
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
