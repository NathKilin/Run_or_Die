using UnityEngine;

public class UI : MonoBehaviour
{
    [HideInInspector] public bool isPaused = false; 
    private PlayerMovement playerMovement;


    void Start()
    {
        playerMovement = FindFirstObjectByType<PlayerMovement>();
    }
    

    public void PressedPause()
    {
        Debug.Log("Pressed pause");
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
    }
}
