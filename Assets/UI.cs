using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [HideInInspector] public bool isPaused = false; 
    private PlayerMovement playerMovement;
    
    [SerializeField] private Button pauseButton;
    [SerializeField] private Image settingsMenu; // OpaqueBG
    
    [SerializeField] private VerticalLayoutGroup menuButtons;
    [SerializeField] private VerticalLayoutGroup settingsButtons;
    [SerializeField] private TextMeshProUGUI titleText;
    
    [SerializeField] private Button dashButtonLeft;
    [SerializeField] private Button dashButtonRight;
    private bool isDashModeSwipe = true;
    
    
    void Start()
    {
        playerMovement = FindFirstObjectByType<PlayerMovement>();   
        SetUIVisiblity(false);
    }


    void SetUIVisiblity(bool isVisible)
    {
        settingsMenu.gameObject.SetActive(isVisible);
        pauseButton.gameObject.SetActive(!isVisible);
        dashButtonLeft.gameObject.SetActive(!isVisible && !isDashModeSwipe);
        dashButtonRight.gameObject.SetActive(!isVisible && !isDashModeSwipe);
        SetUIMode(true);
    }


    void SetUIMode(bool isMainMenu)
    {
        // Debug.Log($"Setting UI Mode|\t\t|Main Menu : {isMainMenu}");
        titleText.text = isMainMenu ? "Main Menu" : "Settings";
        settingsButtons.gameObject.SetActive(!isMainMenu);
        menuButtons.gameObject.SetActive(isMainMenu);
    }


    public void PressedChangeDashMode()
    {
        isDashModeSwipe = !isDashModeSwipe;
        // TODO :
        // Create a button that does dash instead of swipe
    }
    

    public void PressedPause()
    {
        isPaused = true;
        Time.timeScale = 0;
        SetUIVisiblity(isPaused);
    }


    public void PressedPlay()
    {
        isPaused = false;
        Time.timeScale = 1;
        SetUIVisiblity(isPaused);
    }


    public void PressedSettings() { SetUIMode(false); }
    
    public void PressedQuit() { Application.Quit(); }
    
    public void PressedGoBack() { SetUIMode(true); }
    
    
    
    
}
