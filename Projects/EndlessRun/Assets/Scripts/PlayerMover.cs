using UnityEngine;

/// Simple vertical controller that works with or without a Rigidbody.
/// If Rigidbody exists and isKinematic == false, we drive its velocity.
/// If kinematic (or no RB), we move transform directly.
/// Exposes VerticalSpeed + IsResting for WorldConductor.
[DisallowMultipleComponent]
public class PlayerMover : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float gravity = -20f;
    [SerializeField] float flapImpulse = 8f;

    public float VerticalSpeed { get; private set; }
    public bool IsResting { get; private set; }

    Rigidbody _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Basic input (keep even if you also use the new Input System)
        if (Input.GetKeyDown(KeyCode.Space))
            Flap();

        // Integrate gravity
        VerticalSpeed += gravity * Time.deltaTime;

        // Apply motion
        if (_rb != null && !_rb.isKinematic)
        {
            var v = _rb.linearVelocity;
            v.y = VerticalSpeed;
            _rb.linearVelocity = v;
        }
        else
        {
            transform.position += Vector3.up * VerticalSpeed * Time.deltaTime;
        }

        // Dead-zone flag
        IsResting = Mathf.Abs(VerticalSpeed) < 0.01f;
    }

    public void Flap()
    {
        VerticalSpeed = flapImpulse;
        IsResting = false;
    }

    public void Stop()
    {
        VerticalSpeed = 0f;
        if (_rb != null && !_rb.isKinematic)
        {
            var v = _rb.linearVelocity; v.y = 0f; _rb.linearVelocity = v;
        }
    }
}
