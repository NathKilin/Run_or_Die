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
    public void SaveGame(int slot = 1)
    {
        Debug.Log("[SaveManager] Saving Game On Slot : " + slot);
        GameSaveData data = GetDataToSave(); 
        
        if (slot > maxSlots || slot <= 0) {
            Debug.Log("[SaveManager] Error, Invalid Slot. Out of Bounds");
            throw new System.Exception($"Save Slot [{slot}] Out Of Bounds [1 - {maxSlots}]");
        }
        Debug.Log("[SaveManager] No invalid save slot error, proceeding");
        
        string json = JsonUtility.ToJson(data, true);
        Debug.Log($"[SaveManager] Save Data : [{json}]");
        string folderPath = Path.Combine(Application.persistentDataPath, "Saves");
        if (!Directory.Exists(folderPath)) {
            Directory.CreateDirectory(folderPath);
        }
        string filePath = Path.Combine(folderPath, $"Save_{slot}.json");
        Debug.Log($"[SaveManager] File Path : [{filePath}]");
        // Doesnt go further than here vvvvvvv
        File.WriteAllText(filePath, json);
        Debug.Log($"[SaveManager] Written Json Data to file path");
        Debug.Log("Game saved to: " + filePath);
    }


    private GameSaveData GetDataToSave()
    {
        GameSaveData data = new GameSaveData();
        
        data.playerData = new PlayerData();
        Rigidbody player =  GameObject.FindFirstObjectByType<PlayerMovement>().GetComponent<Rigidbody>();
        data.playerData.position = player.position;
        data.playerData.velocity = player.linearVelocity;
        
        data.lastPlayedDate = DateTime.UtcNow;
        
        // TODO : 
        
        //data.score = FindAnyObjectByType<>()

        //List<ObstacleData> activeObstacles = new();
        //foreach (GameObject obstacle in GameObject.FindGameObjectsWithTag("Obstacle")) {
            
        //}
        //data.activeObstacles = activeObstacles;
        
        // TODO :
        
        return data;
    }


    /// <summary>
    /// Returns a GameSaveData type object in a slot
    /// Only returns whether the save exists in the slot
    /// </summary>
    public GameSaveData GetSavedGameData(int slot)
    {
        string folderPath = Path.Combine(Application.persistentDataPath, "Saves");
        if (!Directory.Exists(folderPath)) {
            Directory.CreateDirectory(folderPath);
        }
        string filePath = Path.Combine(folderPath, $"Save_{slot}.json");
        
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













