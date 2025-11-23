using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObstacleData
{
    public GameObject obstaclePrefab;
    public Vector3 position;
}


public class GameSaveData
{
    public int score;
    public String lastPlayedDate;
    public Vector3 playerPosition;
    public Vector3 playerVelocity;
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
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (slotToLoad > 0 && slotToLoad <= maxSlots) {
            LoadGame(slotToLoad);
        } 
        
        slotToLoad = 0;
    }


    /// <summary>
    /// The function to save the game data given to disk, on a certain slot
    /// If the slot is invalid the game will crash with an error
    /// </summary>
    public void SaveGame(int slot = 1)
    {
        //Debug.Log("[SaveManager] Saving Game On Slot : " + slot);
        GameSaveData data = GetDataToSave(); 
        
        if (slot > maxSlots || slot <= 0) {
            Debug.Log("[SaveManager] Error, Invalid Slot. Out of Bounds");
            throw new System.Exception($"Save Slot [{slot}] Out Of Bounds [1 - {maxSlots}]");
        }

        
        string json = JsonUtility.ToJson(data, true);
        string folderPath = Path.Combine(Application.persistentDataPath, "Saves");
        if (!Directory.Exists(folderPath)) {
            Directory.CreateDirectory(folderPath);
        }
        string filePath = Path.Combine(folderPath, $"Save_{slot}.json");
        File.WriteAllText(filePath, json);
    }


    private GameSaveData GetDataToSave()
    {
        GameSaveData data = new GameSaveData();
        
        Rigidbody player =  GameObject.FindFirstObjectByType<PlayerMovement>().GetComponent<Rigidbody>();
        data.playerPosition = player.position;
        data.playerVelocity = player.linearVelocity;
        
        data.lastPlayedDate = DateTime.Now.ToString("dd-MM-yy");
        
        // TODO : 
        
        data.score = (int)ScoreManager.Instance.currentScore;

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
            //Debug.Log("Game loaded from: " + filePath);
            return loadedData;
        }
        
        //Debug.LogWarning("Save file not found: " + filePath);
        return null;
    }


    /// <summary>
    /// A function that should be called from within the main game to load the game 
    /// </summary>
    public void LoadGame(int slot)
    {
        PlayerMovement playerMovement = Transform.FindFirstObjectByType<PlayerMovement>();
        if (playerMovement == null) {
            Debug.Log("[SaveManager] Scene does not contain player, aborting load.");
            return;
        }
        Debug.Log("[SaveManager] Loading Save Data & Applying To Game");
        
        // Get save data
        GameSaveData data = GetSavedGameData(slot);
        
        // Set player
        playerMovement.gameObject.transform.position = data.playerPosition;
        playerMovement.gameObject.GetComponent<Rigidbody>().linearVelocity = data.playerVelocity;
        
        // Set obstacles
        // TODO
        
        // Set scene others ( score , etc.. )
        ScoreManager.Instance.currentScore = data.score;
        ScoreManager.Instance.startY = data.playerPosition.y;
        // TODO
    }
    
    
    private void OnEnable()
    {
        // Prevent double-subscription by removing first
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    
    
    
    
    
    
    
}













