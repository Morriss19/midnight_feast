using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttack : MonoBehaviour
{
    private void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null) return;

        // Detect attack button press
        if (keyboard.spaceKey.wasPressedThisFrame)
        {
            Debug.Log("Attack button pressed!");
            Attack();
        }
    }

    private void Attack()
    {
        Debug.Log("Player attempted to attack!");
        
        // TODO: Add attack logic later
        // - Detect enemies
        // - Deal damage
    }
}