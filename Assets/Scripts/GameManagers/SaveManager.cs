using System.Collections.Generic;
using UnityEngine;


public class PlayerData
{
    public Vector3 position;
    public Vector3 velocity;
}


public class ObstacleData
{
    public GameObject obstaclePrefab;
    public Vector3 position;
}


public class GameSaveData
{
    public PlayerData playerData;
    public List<ObstacleData> activeObstacles;
}


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
