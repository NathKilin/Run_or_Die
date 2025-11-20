using UnityEngine;
using UnityEngine.UI;

public class MenuUI : MonoBehaviour
{
    [SerializeField] private VerticalLayoutGroup mainButtons;
    [SerializeField] private VerticalLayoutGroup playButtons;
    [SerializeField] private VerticalLayoutGroup settingsButtons;

    private void Start()
    {
        SetMenuMode(MenuMode.Main);
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
