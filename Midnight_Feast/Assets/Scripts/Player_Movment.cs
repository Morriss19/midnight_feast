using UnityEngine;

public class Player_Movment : MonoBehaviour
{

    private float xMovement;
    private float yMovement;
    private float speed; 
    bool isFacingLeft;
    bool isFacingRight;
    bool isFacingUp;
    bool isfacingDown;

    [SerializedField] private RigidBody2D rb ;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        xMove = Input.getAxisRaw("xMovement"); 

    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(xMovement * speed, rb.velocity.y); 
    }
}
