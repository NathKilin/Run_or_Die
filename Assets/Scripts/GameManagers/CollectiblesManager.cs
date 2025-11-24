using System.Collections.Generic;
using TMPro;
using UnityEngine;


// No, the comments aren't AI I just think they're cool and organized

public class CollectiblesManager : MonoBehaviour
{
    #region Counters Variables
    [Header("Labels")]
    [SerializeField] private TextMeshProUGUI healthCollectiblesLabel;
    [SerializeField] private TextMeshProUGUI boostCollectibleLabel;
    [SerializeField] private TextMeshProUGUI scoreCollectibleLabel;

    public Dictionary<CollectibleType, CollectibleCounter> counters;
    #endregion
    
    #region Objects
    private GameObject playerObject;
    private PlayerMovement playerMovement;
    private PlayerHealth playerHealth;
    
    [SerializeField] private GameObject collectiblePrefab;
    public GameObject collectibleInstance;
    private Light collectibleLight;
    #endregion

    public CollectibleType currentCollectibleType;
    public static CollectiblesManager Instance;
    
    #region Adjustment Parameters
    [Header("Spawn Parameters")]
    [SerializeField] private float spawnObstacleDetectionRadius = 0.5f;
    
    [SerializeField] private float minSpawnableXDistance = -2.5f;
    [SerializeField] private float maxSpawnableXDistance = 5.5f;
    
    [SerializeField] private float minHeightFromPlayer = 24f;
    [SerializeField] private float maxHeightFromPlayer = 36f;
    #endregion
    
    private void Awake()
    {
        // -------- Set Singleton Instance --------
        if (Instance != null && Instance != this) {
            Destroy(gameObject); 
        }
        Instance = this;
        
        // -------- Initialize the counters and labels --------
        InitializeCounters();
        UpdateLabels();
        
        // -------- Set player object and components --------
        playerObject = GameObject.FindGameObjectWithTag("Player");
        playerMovement = playerObject.GetComponent<PlayerMovement>();
        playerHealth = playerObject.GetComponent<PlayerHealth>();
        
        // -------- Set collectible values for scene --------
        collectibleInstance = Instantiate(collectiblePrefab);
        collectibleLight = collectibleInstance.GetComponentInChildren<Light>();
        SpawnNewCollectible();

        
    }
    
    
    public void ConsumeCollectible()
    {
        // -------- Increase Current Collectible Amount --------
        SetCurrentCollectibles(currentCollectibleType, counters[currentCollectibleType].currentAmount+1);
        
        // -------- Have the effect of the collectible --------
        switch (currentCollectibleType)
        {
            case CollectibleType.Health:
                // TODO 
                // Add +1 Heart to health
                playerHealth.Heal(1);
                break;
            case CollectibleType.Boost:
                // TODO 
                // Add Velocity Boost to PlayerMovement
                playerMovement.boostAmount = 1.5f;
                break;
            case CollectibleType.Score:
                // TODO
                // Add Score to ScoreManager
                ScoreManager.Instance.scoreBonus += 500;
                break;
        }
        
        
        // TODO 
        // Add Sound ?
        
        
        // -------- After the player has consumed the collectible spawn a new one --------
        SpawnNewCollectible();
    }
    
    
    #region Spawning Methods
    // -------------------
    //   Spawning Methods
    // -------------------
    public void SpawnNewCollectible()
    {
        // -------- Choose a random available type --------
        List<CollectibleType> types = new List<CollectibleType>();
        foreach (KeyValuePair<CollectibleType, CollectibleCounter> pair in counters) {
            if (pair.Value.maxAmount != pair.Value.currentAmount) {
                types.Add(pair.Key);
            }
        }
        //-------- Set the current collectible type randomly --------
        if (types.Count <= 0) // Don't spawn if the limit is reached on all
            return;
        
        CollectibleType type = types[Random.Range(0, types.Count)];
        currentCollectibleType = type;
        
        //-------- Position the collectible in a viable location --------
        Vector3 position = GetPositionForCollectible();
        collectibleInstance.transform.position = position;
        
        //-------- Have the visual clarity of the current collectible --------
        Color lightColor = counters[type].color;
        collectibleLight.color = lightColor;
    }
    
    
    private Vector3 GetPositionForCollectible()
    {
        float horizontalPosition = Random.Range((int)minSpawnableXDistance * 10, (int)(maxSpawnableXDistance * 10) + 1) / 10f;
        float verticalDistance = playerObject.transform.position.y + Random.Range((int)minHeightFromPlayer * 10, (int)maxHeightFromPlayer * 10) / 10f;
        Vector3 position = new Vector3(horizontalPosition, verticalDistance, playerObject.transform.position.z);
        
        LayerMask mask = LayerMask.GetMask("Platform", "Obstacle"); 
        bool isAnotherObjectInPosition = Physics.CheckSphere(position, spawnObstacleDetectionRadius, mask);
        
        if (isAnotherObjectInPosition) {
            return GetPositionForCollectible();
        }
        
        return position;
    }
    #endregion
    
    
    #region Setter Methods
    // -------------------
    //   Setter Methods
    // -------------------
    public void SetMaxCollectibles(CollectibleType type, int newMax)
    {
        counters[type].maxAmount = newMax;
        UpdateLabels();
    }

    
    public void SetCurrentCollectibles(CollectibleType type, int newCurrent)
    {
        counters[type].currentAmount = newCurrent;
        UpdateLabels();
    }
    #endregion


    #region Private Methods
    // -------------------
    //   Private Methods
    // -------------------
    private void UpdateLabels()
    {
        foreach (KeyValuePair<CollectibleType,CollectibleCounter> pair in counters){
            CollectibleCounter collectibleCounter = counters[pair.Key];
            collectibleCounter.label.text = $"{pair.Key.ToString()} Collectibles : {collectibleCounter.currentAmount}/{collectibleCounter.maxAmount}";
        }
    }


    private void InitializeCounters()
    {
        counters = new Dictionary<CollectibleType, CollectibleCounter>()
        {
            { CollectibleType.Health, new CollectibleCounter(healthCollectiblesLabel,5,new Color(200,50,85)) },
            { CollectibleType.Boost,  new CollectibleCounter(boostCollectibleLabel,5,new Color(50,85,200)) },
            { CollectibleType.Score,  new CollectibleCounter(scoreCollectibleLabel,5,new Color(85,200,50)) }
        };
    }
    #endregion
    
    
    #region Enums
    public enum CollectibleType
    {
        Health,
        Boost,
        Score
    }
    #endregion
}


#region Collectible Class
public class CollectibleCounter
{
    public TextMeshProUGUI label;
    public int currentAmount;
    public int maxAmount;
    public Color color;

    public CollectibleCounter(TextMeshProUGUI label, int maxAmount, Color color)
    {
        this.label = label;
        this.currentAmount = 0;
        this.maxAmount = maxAmount;
        this.color = color;
    }
}
#endregion