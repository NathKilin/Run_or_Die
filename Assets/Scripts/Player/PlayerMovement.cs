using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header("Movement Values")]
    public float horizontalSpeed = 5.0f;
    public float jumpForce = 7.0f;
    public Vector3 currentDirection = Vector3.right;
    
    

    private Rigidbody rigidBody;
    
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    
    void Update()
    {
        // Later Replace With Input Handler -> Screen Tap
        if (Input.GetButtonDown("Jump")) Jump();
        
        // Dont Replace
        HandleHorizontalMovement();
    }


    void HandleHorizontalMovement()
    {
        rigidBody.linearVelocity = new Vector3(
            currentDirection.x * horizontalSpeed * Time.deltaTime,
            rigidBody.linearVelocity.y,
            currentDirection.z * horizontalSpeed * Time.deltaTime);
    }
    

    void Jump()
    {
        rigidBody.AddForce(Vector3.up * (jumpForce * Time.deltaTime));
    }
    

    void FlipPlayer()
    {
        transform.Rotate(Vector3.up, 180);
        currentDirection = currentDirection ==  Vector3.right ? Vector3.left : Vector3.right;
    }


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Level"))
            FlipPlayer();
    }
}
