using UnityEngine;

public class InputHandler : MonoBehaviour
{
    
    public delegate void ScreenTapHandler();
    public event ScreenTapHandler OnScreenTapped;

    private int previousTouchCount = 0;
    
    
    void Update()
    {
        if (Input.touchCount > 0 && previousTouchCount == 0) {
            OnScreenTapped?.Invoke();
        }    
        
        previousTouchCount = Input.touchCount;
    }
}