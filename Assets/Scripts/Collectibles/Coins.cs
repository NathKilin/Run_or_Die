using System;
using UnityEngine;

public class Coins : MonoBehaviour
{
    public static Coins Instance { get; private set; }

    [SerializeField] private int startingCoins = 0;

    private int coins;
    public event Action<int> OnCoinsChanged;

    private const string CoinsKey = "COINS";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadCoins();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void LoadCoins()
    {
        coins = PlayerPrefs.GetInt(CoinsKey, startingCoins);
        OnCoinsChanged?.Invoke(coins);
    }

    private void SaveCoins()
    {
        PlayerPrefs.SetInt(CoinsKey, coins);
        PlayerPrefs.Save();
    }

public void AddCoins(int amount)
{
    if (amount <= 0) return;
    int oldCoins = coins;          // guarda valor anterior
    coins += amount;
    SaveCoins();
    OnCoinsChanged?.Invoke(coins);

    // Analytics: reporta mudança de coins
    if (AnalyticsManager.Instance != null)
    {
        int delta = coins - oldCoins; // deve ser igual a amount
        AnalyticsManager.Instance.TrackCoinsChanged(coins, delta);
    }
}

    public bool SpendCoins(int amount)
    {
        if (amount <= 0) return false;
        if (coins < amount) return false;
        coins -= amount;
        SaveCoins();
        OnCoinsChanged?.Invoke(coins);
        return true;
    }

    public int GetCoins() => coins;
}