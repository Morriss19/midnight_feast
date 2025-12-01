using UnityEngine;

public class FoodObject : CellObject
{
    [SerializedFeield] private int foodValue = 10;
    public override void PlayerEntered()
    {
        Debug.log("Food Collected");

        if (GameManager.Instance != null)
        {
            GameManager.Instance.ChangeFood(foodValue);
        }
        Destroy(/*Let me get the food object*/); 
    }
    
}
