using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UI : MonoBehaviour
{
    [HideInInspector] public bool isPaused = false; 
    private PlayerMovement playerMovement;
    
    [SerializeField] private Button pauseButton;
    [SerializeField] private Image settingsMenu; // OpaqueBG
    
    [SerializeField] private VerticalLayoutGroup menuButtons;
    [SerializeField] private VerticalLayoutGroup settingsButtons;
    [SerializeField] private VerticalLayoutGroup saveButtons;
    [SerializeField] private GridLayoutGroup saveSlots;
    [SerializeField] private TextMeshProUGUI titleText;

    [SerializeField] private Button dashModeButton;
    [SerializeField] private Button dashButtonLeft;
    [SerializeField] private Button dashButtonRight;
    private bool isDashModeSwipe = true;
    
    
    void Start()
    {
        playerMovement = FindFirstObjectByType<PlayerMovement>();   
        SetUIVisiblity(false);
        isDashModeSwipe = !true;
        PressedChangeDashMode();
        SetSaveSlots();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            PressedPause();
    }


    void SetSaveSlots()
    {
        for (int i = 0; i < saveSlots.transform.childCount; i++)
        {
            Button child = saveSlots.transform.GetChild(i).GetComponent<Button>();
            TextMeshProUGUI text = child.GetComponentInChildren<TextMeshProUGUI>();
            int saveSlot = i + 1;
            GameSaveData savedGame = SaveManager.Instance.GetSavedGameData(saveSlot);
            child.onClick.RemoveAllListeners();
            child.onClick.AddListener(() => SaveGame(saveSlot));
            text.text = savedGame != null ? $"Last Played : \n[{savedGame.lastPlayedDate}]" : "[EMPTY]";
        }
    }


    private void SaveGame(int saveSlot)
    {
        Debug.Log("Saving Game On Slot : " + saveSlot);
        
        SaveManager.Instance.SaveGame(saveSlot);
        Debug.Log("Saved Game Through SaveManager");
        
        Debug.Log("Setting Text Of Button");
        // Set the save button's text again 
        TextMeshProUGUI text = saveSlots.transform.GetChild(saveSlot - 1).GetComponent<Button>().GetComponentInChildren<TextMeshProUGUI>();
        Debug.Log($"{text}, is null : {text == null}");
        GameSaveData savedGame = SaveManager.Instance.GetSavedGameData(saveSlot);
        text.text = $"Last Played : \n[{savedGame.lastPlayedDate}]";
        Debug.Log($"Text's text : {text.text}");
    }
    
    
    void SetUIVisiblity(bool isVisible)
    {
        settingsMenu.gameObject.SetActive(isVisible);
        pauseButton.gameObject.SetActive(!isVisible);
        dashButtonLeft.gameObject.SetActive(!isVisible && !isDashModeSwipe);
        dashButtonRight.gameObject.SetActive(!isVisible && !isDashModeSwipe);
        SetUIMode(MenuMode.Main);
    }


    void SetUIMode(MenuMode mode)
    {
        // Debug.Log($"Setting UI Mode|\t\t|Main Menu : {isMainMenu}");
        switch (mode)
        {
            case MenuMode.Main:
                titleText.text = "Main Menu";
                settingsButtons.gameObject.SetActive(false);
                menuButtons.gameObject.SetActive(true);
                saveButtons.gameObject.SetActive(false);
                break;
            case MenuMode.Settings:
                titleText.text = "Settings";
                settingsButtons.gameObject.SetActive(true);
                menuButtons.gameObject.SetActive(false);
                saveButtons.gameObject.SetActive(false);
                break;
            case MenuMode.Save:
                titleText.text = "Save";
                settingsButtons.gameObject.SetActive(false);
                menuButtons.gameObject.SetActive(false);
                saveButtons.gameObject.SetActive(true);
                break;
        }
       
    }


    public void PressedSaveButton() { SetUIMode(MenuMode.Save); }
    
    
    public void PressedChangeDashMode()
    {
        isDashModeSwipe = !isDashModeSwipe;
        ColorBlock cb = dashModeButton.colors;
        cb.normalColor = isDashModeSwipe ? new Color(124,50,65) : new Color(65,50,124);
        dashModeButton.colors = cb;
        TextMeshProUGUI text = dashModeButton.GetComponentInChildren<TextMeshProUGUI>();
        text.text = "DASH MODE : \n";
        text.text += !isDashModeSwipe ? "ON" : "OFF";
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


    public void PressedSettings() { SetUIMode(MenuMode.Settings); }

    public void PressedQuit()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }
    
    public void PressedGoBack() { SetUIMode(MenuMode.Main); }


    private enum MenuMode
    {
        Main,
        Settings,
        Save,
    }  
    
}
