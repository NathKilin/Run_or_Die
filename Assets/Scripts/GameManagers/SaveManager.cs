using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;
    
    
    private void Awake()
    {
        if (Instance != null && Instance != this) {
            Destroy(gameObject); 
        } else {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }
    }


    public void SaveGame()
    {
        Json = Scene.Player.position;
    }


    public void LoadGame()
    {
        save = GetJson();
        if (PlayerMovement in Scene)
        {
            PlayerMovement.gameObject.position = save.playerPosition;
        }
    }
}
