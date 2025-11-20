using System;
using System.Collections.Generic;
using System.IO;
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
    public int score;
    public DateTime lastPlayedDate;
    public PlayerData playerData;
    public List<ObstacleData> activeObstacles;
}


public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance; // The instance of which you can access the functions
    
    public int maxSlots = 4; // How many slots are there for saving

    public static int slotToLoad = 0;
    
    /// <summary>
    /// Awake function to set the singleton.
    /// singleton must be a game object attached with a SaveManager script
    /// </summary>
    private void Awake()
    {
        if (Instance != null && Instance != this) {
            Destroy(gameObject); 
        } else {
            Instance = this;
            DontDestroyOnLoad(gameObject); 
        }

        if (slotToLoad != 0) {
            LoadGame(slotToLoad);
        }
    }


    /// <summary>
    /// The function to save the game data given to disk, on a certain slot
    /// If the slot is invalid the game will crash with an error
    /// </summary>
    public void SaveGame(GameSaveData data, int slot = 1)
    {
        if (slot > maxSlots || slot <= 0) {
            throw new System.Exception($"Save Slot [{slot}] Out Of Bounds [1 - {maxSlots}]");
        }
        string json = JsonUtility.ToJson(data, true);
        string filePath = Path.Combine(Application.persistentDataPath, $"Saves/Save_{slot}");
        File.WriteAllText(filePath, json);
        Debug.Log("Game saved to: " + filePath);
    }


    /// <summary>
    /// Returns a GameSaveData type object in a slot
    /// Only returns whether the save exists in the slot
    /// </summary>
    public GameSaveData GetSavedGameData(int slot)
    {
        string filePath = Path.Combine(Application.persistentDataPath, $"Saves/Save_{slot}");
        if (File.Exists(filePath)) {
            string json = File.ReadAllText(filePath);
            GameSaveData loadedData = JsonUtility.FromJson<GameSaveData>(json);
            Debug.Log("Game loaded from: " + filePath);
            return loadedData;
        }
        
        Debug.LogWarning("Save file not found: " + filePath);
        return null;
    }


    /// <summary>
    /// A function that should be called from within the main game to load the game 
    /// </summary>
    public void LoadGame(int slot)
    {
        PlayerMovement playerMovement = Transform.FindFirstObjectByType<PlayerMovement>();
        if (playerMovement == null) {
            Debug.Log("Scene does not contain player, aborting load.");
            return;
        }
        
        // Get save data
        GameSaveData data = GetSavedGameData(slot);
        
        // Set player
        playerMovement.gameObject.transform.position = data.playerData.position;
        playerMovement.gameObject.GetComponent<Rigidbody>().linearVelocity = data.playerData.velocity;
        
        // Set obstacles
        // TODO
        
        // Set scene others ( score , etc.. )
        // TODO
    }
    
    
    
    
    
    
    
    
    
    
    
    
}













