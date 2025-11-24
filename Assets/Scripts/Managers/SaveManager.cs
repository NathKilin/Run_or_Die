using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

#region === DATA CLASSES ===

[Serializable]
public class ObstacleData
{
    public string prefabId;      
    public Vector3 position;     
    public Quaternion rotation;  
}

[Serializable]
public class GameSaveData
{
    public int score;
    public float scoreStartY;
    public float bestDistance;
    public float scoreBonus;

    public string lastPlayedDate;

    public Vector3 playerPosition;
    public Vector3 playerVelocity;

    public List<ObstacleData> activeObstacles = new();
}

[Serializable]
public class ObstaclePrefabEntry
{
    public string id;          
    public GameObject prefab;  
}

#endregion

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    [Header("Save Slots")]
    public int maxSlots = 4;
    public static int slotToLoad = 0;

    [Header("Obstacle Prefabs Registry")]
    [SerializeField] private List<ObstaclePrefabEntry> obstaclePrefabs = new();

    //  SINGLETON
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    //  LOAD AFTER SCENE START

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (slotToLoad > 0 && slotToLoad <= maxSlots)
        {
            StartCoroutine(DelayedLoad(slotToLoad));
        }

        slotToLoad = 0;
    }

    private IEnumerator DelayedLoad(int slot)
    {
        yield return null; 
        LoadGame(slot);
    }

    public void SaveGame(int slot = 1)
    {
        GameSaveData data = GetDataToSave();

        if (slot > maxSlots || slot <= 0)
        {
            Debug.LogError($"Save Slot [{slot}] Out Of Bounds [1 - {maxSlots}]");
            return;
        }

        string folderPath = Path.Combine(Application.persistentDataPath, "Saves");
        if (!Directory.Exists(folderPath))
            Directory.CreateDirectory(folderPath);

        string filePath = Path.Combine(folderPath, $"Save_{slot}.json");

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(filePath, json);

        Debug.Log($"[SaveManager] Game saved at slot {slot}");
    }
    private GameSaveData GetDataToSave()
    {
        GameSaveData data = new GameSaveData();

        // PLAYER
        Rigidbody playerRb = GameObject.FindFirstObjectByType<PlayerMovement>().GetComponent<Rigidbody>();
        data.playerPosition = playerRb.position;
        data.playerVelocity = playerRb.linearVelocity;

        data.lastPlayedDate = DateTime.Now.ToString("dd-MM-yy");

        // SCORE
        var sm = ScoreManager.Instance;
        data.score        = (int)sm.currentScore;
        data.scoreStartY  = sm.startY;
        data.bestDistance = sm.bestDistance;
        data.scoreBonus   = sm.scoreBonus;

        List<ObstacleData> obstacles = new();

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Obstacle"))
        {
            if (obj == null) continue;

            string rawName = obj.name;
            int cloneIndex = rawName.IndexOf("(Clone)", StringComparison.Ordinal);
            string prefabId = (cloneIndex >= 0) ? rawName[..cloneIndex] : rawName;

            ObstacleData od = new ObstacleData();
            od.prefabId = prefabId;
            od.position = obj.transform.position;
            od.rotation = obj.transform.rotation;

            obstacles.Add(od);
        }

        data.activeObstacles = obstacles;

        return data;
    }

    public GameSaveData GetSavedGameData(int slot)
    {
        string folderPath = Path.Combine(Application.persistentDataPath, "Saves");
        string filePath = Path.Combine(folderPath, $"Save_{slot}.json");

        if (!File.Exists(filePath))
            return null;

        string json = File.ReadAllText(filePath);
        return JsonUtility.FromJson<GameSaveData>(json);
    }

    public void LoadGame(int slot)
    {
        GameSaveData data = GetSavedGameData(slot);
        if (data == null)
        {
            Debug.LogWarning("[SaveManager] No save data found.");
            return;
        }

        Debug.Log("[SaveManager] Loading Save Data...");

        // PLAYER

PlayerMovement playerMovement = GameObject.FindFirstObjectByType<PlayerMovement>();
if (playerMovement == null)
{
    Debug.LogWarning("[SaveManager] No PlayerMovement found in scene.");
    return;
}

Rigidbody rb = playerMovement.GetComponent<Rigidbody>();


        playerMovement.transform.position = data.playerPosition;
        rb.linearVelocity = data.playerVelocity;

        
        var sm = ScoreManager.Instance;
        sm.currentScore = data.score;
        sm.startY       = data.scoreStartY;
        sm.bestDistance = data.bestDistance;
        sm.scoreBonus   = data.scoreBonus;

        foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Obstacle"))
            Destroy(obj);

        foreach (ObstacleData o in data.activeObstacles)
        {
            GameObject prefab = GetObstaclePrefabById(o.prefabId);
            if (prefab == null)
            {
                Debug.LogWarning($"[SaveManager] Missing prefab for id: {o.prefabId}");
                continue;
            }

            GameObject instance = Instantiate(prefab, o.position, o.rotation);
            instance.tag = "Obstacle"; 
        }

        Debug.Log("[SaveManager] Load Complete.");
    }

    private GameObject GetObstaclePrefabById(string id)
    {
        foreach (var entry in obstaclePrefabs)
        {
            if (entry.id == id)
                return entry.prefab;
        }

        return null;
    }
}
