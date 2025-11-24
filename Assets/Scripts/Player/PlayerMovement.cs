using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Values")]
    [Header("Basic Variables")]
    public float horizontalSpeed = 5.0f;
    public float jumpForce = 7.0f;

    private Vector3 currentDirection = Vector3.right;

    public float boostAmount = 1f;
    public float boostFadeRate = .2f;

    [Header("Dash Variables")]
    [SerializeField] private float dashForce = 4.5f;
    private float currentDashForce = 1f;
    [SerializeField] private float dashForceFadeRate = 1.5f;
    [SerializeField] private bool isResetVerticalOnJump = false;

    private InputHandler inputHandler;
    private Rigidbody rigidBody;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();

        inputHandler = FindFirstObjectByType<InputHandler>();
        inputHandler.OnScreenTapped += Jump;
        inputHandler.OnScreenSwiped += Dash;
    }

    void HandleHorizontalMovement()
    {
        if (currentDashForce > 1f)
        {
            currentDashForce = Mathf.Lerp(currentDashForce, 1f, dashForceFadeRate);
        }

        Vector3 currentVelocity = rigidBody.linearVelocity;

        currentVelocity.x = currentDirection.x * horizontalSpeed * currentDashForce * boostAmount;
        currentVelocity.z = 0f;

        rigidBody.linearVelocity = currentVelocity;
    }

    void Update()
    {
        HandleHorizontalMovement();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        if (boostAmount != 1f)
        {
            boostAmount = Mathf.Lerp(boostAmount, 1f, boostFadeRate * Time.deltaTime);
            if (Mathf.Abs(boostAmount - 1f) < .1f)
            {
                boostAmount = 1f;
            }
        }
    }

    void Jump()
    {
        rigidBody.linearVelocity = new Vector3(
            rigidBody.linearVelocity.x,
            jumpForce * boostAmount,
            rigidBody.linearVelocity.z);

        GameManager.Instance.timesJumped++;
    }

    void Dash(Directions direction)
    {
        if (direction == Directions.Up || direction == Directions.Down)
        {
            return;
        }

        Vector3 translatedDirection = direction == Directions.Left ? Vector3.left : Vector3.right;

        if (translatedDirection != currentDirection)
        {
            FlipPlayer();
        }

        currentDashForce = dashForce;

        if (isResetVerticalOnJump)
        {
            rigidBody.linearVelocity = new Vector3(
                rigidBody.linearVelocity.x,
                Mathf.Min(rigidBody.linearVelocity.y, 0),
                0);
        }

        GameManager.Instance.timesDashed++;
    }

    void FlipPlayer()
    {
        transform.Rotate(Vector3.up, 180);
        currentDirection = currentDirection == Vector3.right ? Vector3.left : Vector3.right;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Level"))
        {
            FlipPlayer();
        }
        else if (other.gameObject.CompareTag("Collectible"))
        {
            CollectiblesManager.Instance.ConsumeCollectible();
        }
    }

    public void PressedDashButtonLeft()
    {
        Dash(Directions.Left);
    }

    public void PressedDashButtonRight()
    {
        Dash(Directions.Right);
    }
}
