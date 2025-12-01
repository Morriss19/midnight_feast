using UnityEngine;

public class FoodObject : CellObject
{
    [SerializeField] private int foodValue = 10;
    public override void PlayerEntered()
    {
        Debug.Log("Food Collected");

        if (GameManager.Instance != null)
        {
            GameManager.Instance.ChangeFood(foodValue);
        }
        Destroy(/*Let me get the food object*/); 
    }
    
}
