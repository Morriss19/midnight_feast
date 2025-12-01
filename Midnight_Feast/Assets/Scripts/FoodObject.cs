using UnityEngine;

public class FoodObject : CellObject
{
    public int AmountGranted = 1; 
    public override void PlayerEntered()
    {
        Destroy(gameObject);

        Debug.Log("Food increased");

        GameManager.Instance.ChangeFood(AmountGranted);
    }
}
