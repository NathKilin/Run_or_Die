using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class MainMenuBird : MonoBehaviour
{
    [SerializeField] private GameObject targetsParent;
    [SerializeField] private float speed = 12f;
    [SerializeField] private float rotationSpeed = 5f;
    private Vector3 currentTarget;


    void Start()
    {
        ChooseRandomPosition();
    }


    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, currentTarget, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, currentTarget) < 0.1f) {
            ChooseRandomPosition();
        }
        
        RotateTowardsTarget();
    }


    void ChooseRandomPosition()
    {
        currentTarget = targetsParent.transform.GetChild(Random.Range(0,targetsParent.transform.childCount)).transform.position;
    }
    
    
    void RotateTowardsTarget()
    {
        // Calculate the direction to the target (ignoring the Y axis to avoid vertical rotation)
        Vector3 targetDirection = new Vector3(currentTarget.x, transform.position.y, currentTarget.z) - transform.position;

        // Calculate the rotation step
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);

        if (Quaternion.Angle(transform.rotation, targetRotation) < 0.1f) {
            return;
        }
        
        // Rotate smoothly towards the target on the Y-axis
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}
