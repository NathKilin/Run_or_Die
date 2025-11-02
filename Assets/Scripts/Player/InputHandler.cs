using System;
using UnityEngine;
    

public enum Directions
{
    Left,
    Right,
    Up,
    Down
}


public class InputHandler : MonoBehaviour
{

    
    
    public delegate void ScreenTapHandler();
    public event ScreenTapHandler OnScreenTapped;
    public event ScreenTapHandler OnTouchStarted;
    public event ScreenTapHandler OnTouchEnded;

    public delegate void ScreenSwipeHandler(Directions direction);
    public event ScreenSwipeHandler OnScreenSwiped;
    

    private int previousTouchCount = 0;
    private Touch[] currentTouches;
    private Touch[] previousTouches;

    private Vector2 touchStartPosition;
    private Vector2 touchEndPosition;
    public float swipeThreshold = 30f;
    private bool isTouching = false;
    private float timeTouching = .0f;
    public float swipeTimeThresholdSeconds = .5f;
    

    private void Awake()
    {
        // TODO : 
        // Set the swipe threshold automatically to be worth a fraction of the screen ( for example : 1/5 of the screen's width ) instead of a set amount of pixels ?
        
        // swipeThreshold = 30f;
        
        
        OnTouchStarted += TouchStarted;
        OnTouchEnded += TouchEnded;
    }


    void Update()
    {
        currentTouches = Input.touches;
        if (Input.touchCount > 0 && previousTouchCount == 0) {
            OnScreenTapped?.Invoke();
            OnTouchStarted?.Invoke();
            isTouching = true;
            timeTouching = .0f;
        }

        if (isTouching) timeTouching += Time.deltaTime;
        
        if (Input.touchCount == 0 && previousTouchCount > 0) {
            OnTouchEnded?.Invoke();
            isTouching = false;
        }
        
        previousTouchCount = Input.touchCount;
        previousTouches = currentTouches;
    }


    void TouchStarted()
    {
        touchStartPosition = currentTouches[0].position;
    }


    void TouchEnded()
    {
        touchEndPosition = previousTouches[0].position;
        DetectSwipe();
    }


    private void DetectSwipe()
    {
        // Longer swipes aren't considered
        if (timeTouching > swipeTimeThresholdSeconds) {
            return;
        }
        
        // If the length of the swipe is short it is not considered a swipe
        Vector2 swipeVector = touchEndPosition - touchStartPosition;
        if (swipeVector.magnitude < swipeThreshold) {
            return;
        }
        
        // Figure out which direction is swiped
        if (Mathf.Abs(swipeVector.x) > Mathf.Abs(swipeVector.y))
        {
            if (swipeVector.x > 0) {
                OnScreenSwiped?.Invoke(Directions.Right);
            } else {
                OnScreenSwiped?.Invoke(Directions.Left);
            }
        } 
        else {
            if (swipeVector.y > 0) {
                OnScreenSwiped?.Invoke(Directions.Up);
            } else {
                OnScreenSwiped?.Invoke(Directions.Down);
            }
        }
    }

}