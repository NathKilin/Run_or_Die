using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header("Movement Values")]
    public float horizontalSpeed = 5.0f;
    public float jumpForce = 7.0f;
    public Vector3 currentDirection = Vector3.right;
    
    private InputHandler inputHandler;
    private Rigidbody rigidBody;
    
    
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        inputHandler = FindObjectOfType<InputHandler>();
        inputHandler.OnScreenTapped += Jump;
    }

    
    void HandleHorizontalMovement()
    {
        rigidBody.linearVelocity = new Vector3(
            currentDirection.x * horizontalSpeed,
            rigidBody.linearVelocity.y,
            currentDirection.z * horizontalSpeed);
    }


    void Update()
    {
        HandleHorizontalMovement();
        
        if (Input.GetKeyDown(KeyCode.Space)) {
            Jump();    
        }
    }
    

    void Jump(){
        // Creates a constant jump force independent on speed
        rigidBody.linearVelocity = new Vector3(
            rigidBody.linearVelocity.x,
            jumpForce,
            rigidBody.linearVelocity.z);
    }
    

    void FlipPlayer()
    {
        transform.Rotate(Vector3.up, 180);
        currentDirection = currentDirection ==  Vector3.right ? Vector3.left : Vector3.right;
    }


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Level")) {
            FlipPlayer();
            Debug.Log("Collision Enter");
        }
    }
}