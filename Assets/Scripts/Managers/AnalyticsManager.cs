using System;
using System.Collections.Generic;
using UnityEngine;


using Unity.Services.Core;          
using Unity.Services.Analytics;     


public class AnalyticsManager : MonoBehaviour
{
    public static AnalyticsManager Instance { get; private set; }


    private bool _isInitialized = false;


    private async void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }


        Instance = this;
        DontDestroyOnLoad(gameObject);  


        try
        {
            await UnityServices.InitializeAsync();


            AnalyticsService.Instance.StartDataCollection();


            _isInitialized = true;
            Debug.Log("AnalyticsManager: Unity Services & Analytics inicializados com sucesso.");
        }
        catch (Exception e)
        {
            Debug.LogWarning($"AnalyticsManager: Falha ao inicializar Analytics. Motivo: {e.Message}");
        }
    }


    private bool CanSendEvent()
    {
        if (!_isInitialized)
        {
            return false;
        }


        return true;
    }

    public void TrackSessionStart()
    {
        if (!CanSendEvent()) return;


        var data = new Dictionary<string, object>
        {
            { "session_start_time_utc", DateTime.UtcNow.ToString("o") }
        };


        UnityEngine.Analytics.Analytics.CustomEvent("session_start", data);
        Debug.Log("AnalyticsManager: Enviado evento 'session_start'.");
    }


    public void TrackDailyBonusCollected(int amount)
    {
        if (!CanSendEvent()) return;


        var data = new Dictionary<string, object>
        {
            { "amount", amount },
            { "time_utc", DateTime.UtcNow.ToString("o") }
        };


        UnityEngine.Analytics.Analytics.CustomEvent("daily_bonus_collected", data);
        Debug.Log($"AnalyticsManager: Enviado evento 'daily_bonus_collected' (amount={amount}).");
    }


    public void TrackCoinsChanged(int totalCoins, int delta)
    {
        if (!CanSendEvent()) return;


        var data = new Dictionary<string, object>
        {
            { "total_coins", totalCoins },
            { "delta", delta },
            { "time_utc", DateTime.UtcNow.ToString("o") }
        };


        UnityEngine.Analytics.Analytics.CustomEvent("coins_changed", data);
        Debug.Log($"AnalyticsManager: Enviado evento 'coins_changed' (total={totalCoins}, delta={delta}).");



        const string milestoneKey = "COIN_MILESTONE_500_REPORTED";


        if (totalCoins >= 500 && PlayerPrefs.GetInt(milestoneKey, 0) == 0)
        {
            UnityEngine.Analytics.Analytics.CustomEvent("milestone_500_coins", data);
            PlayerPrefs.SetInt(milestoneKey, 1);
            PlayerPrefs.Save();
            Debug.Log("AnalyticsManager: Enviado evento 'milestone_500_coins'.");
        }
    }
}



