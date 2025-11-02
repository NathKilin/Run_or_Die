using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [Header("Movement Values")]
    [Header("Basic Variables")]
    public float horizontalSpeed = 5.0f;
    public float jumpForce = 7.0f;
    
    private Vector3 currentDirection = Vector3.right;
    
    [Header("Dash Variables")]
    // How much force is added to the current dash force when dashing
    [SerializeField] private float dashForce = 4.5f;
    // How much to multiply the horizontal movement by per frame
    private float currentDashForce = 1f;
    // How fast the dash effect dissipates
    [SerializeField] private float dashForceFadeRate = 1.5f;
    // Whether to reset the vertical movement when dashing
    [SerializeField] private bool isResetVerticalOnJump = false;
    
    
    private InputHandler inputHandler;
    private Rigidbody rigidBody;
    
    
    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
        inputHandler = FindObjectOfType<InputHandler>();
        inputHandler.OnScreenTapped += Jump;
        inputHandler.OnScreenSwiped += Dash;
    }

    
    void HandleHorizontalMovement()
    {
        if (currentDashForce > 1f) {
            currentDashForce = Mathf.Lerp(currentDashForce, 1f, dashForceFadeRate);    
        }
        
        rigidBody.linearVelocity = new Vector3(
            currentDirection.x * horizontalSpeed * currentDashForce,
            rigidBody.linearVelocity.y,
            0f);
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


    void Dash(Directions direction)
    {
        Debug.Log($"Swiped {direction.ToString()}");
        
        // Don't dash vertically 
        if (direction == Directions.Up || direction == Directions.Down) {
            return;
        }

        Vector3 translatedDirection = direction == Directions.Left ? Vector3.left : Vector3.right;
        if (translatedDirection != currentDirection) {
            FlipPlayer();
        }
        
        currentDashForce = dashForce;
        // TODO
        // Figure out if resetting the horizontal movement is good 
        
        if (isResetVerticalOnJump) {
            rigidBody.linearVelocity = new Vector3(
                rigidBody.linearVelocity.x,
                Mathf.Min(rigidBody.linearVelocity.y,0),
                0);
        }
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
            // Debug.Log("Collision Enter");
        }
    }


    public void PressedDashButtonLeft() //Directions direction
    {
        Dash(Directions.Left);
    }
    
    public void PressedDashButtonRight()
    {
        Dash(Directions.Right);
    }
}