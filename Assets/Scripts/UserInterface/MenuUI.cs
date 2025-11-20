using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    
    [SerializeField] private VerticalLayoutGroup mainButtons;
    [SerializeField] private VerticalLayoutGroup playButtons;
    [SerializeField] private VerticalLayoutGroup settingsButtons;
    [SerializeField] private GridLayoutGroup savedGamesButtons;
    
    
    private void Start()
    {
        SetMenuMode(MenuMode.Main);
        SetSavedGamesButtons();
    }


    public void PressedNewGame()
    {
        SwitchToGame();
    }
    

    private void SetSavedGamesButtons()
    {
        for (int i = 0; i < savedGamesButtons.transform.childCount; i++)
        {
            Button child = savedGamesButtons.transform.GetChild(i).GetComponent<Button>();
            TextMeshProUGUI text = child.GetComponentInChildren<TextMeshProUGUI>();
            int saveSlot = i + 1;
            GameSaveData savedGame = SaveManager.Instance.GetSavedGameData(saveSlot);
            if (savedGame != null) {
                child.onClick.AddListener(() => SwitchToGame(saveSlot));
                text.text = $"Last Played : \n[{savedGame.lastPlayedDate}]";
            } else {
              child.enabled = false;
              text.text = "[EMPTY]";
            }
        }
    }


    private void SwitchToGame(int saveSlot = 0)
    {
        SaveManager.slotToLoad = saveSlot;
        // TODO : 
        // SceneManager.LoadScene();
        SceneManager.LoadScene("SampleScene");
    }
    

    private void SetMenuMode(MenuMode newMode)
    {
        switch (newMode)
        {
            case MenuMode.Main:
                mainButtons.gameObject.SetActive(true);
                playButtons.gameObject.SetActive(false);
                settingsButtons.gameObject.SetActive(false);
                break;
            case MenuMode.Play:
                mainButtons.gameObject.SetActive(false);
                playButtons.gameObject.SetActive(true);
                settingsButtons.gameObject.SetActive(false);
                break;
            case MenuMode.Settings:
                mainButtons.gameObject.SetActive(false);
                playButtons.gameObject.SetActive(false);
                settingsButtons.gameObject.SetActive(true);
                break;
        }
    }


    public void PressedBackToMenu()
    {
        SetMenuMode(MenuMode.Main);
    }
    
    
    public void PressedPlay()
    {
        SetMenuMode(MenuMode.Play);
    }


    public void PressedSettings()
    {
        SetMenuMode(MenuMode.Settings);
    }
    

    public void PressedQuit()
    {
        Application.Quit();
    }


    private enum MenuMode
    {
        Main,
        Play,
        Settings
    }
}
