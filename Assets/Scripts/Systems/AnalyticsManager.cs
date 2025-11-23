using System;
using System.Collections.Generic;
using UnityEngine;

// Namespaces do Unity Gaming Services
using Unity.Services.Core;          // para UnityServices.InitializeAsync
using Unity.Services.Analytics;     // para AnalyticsService

public class AnalyticsManager : MonoBehaviour
{
    // Singleton simples para podermos chamar AnalyticsManager.Instance em outros scripts
    public static AnalyticsManager Instance { get; private set; }

    // Flag para sabermos se Analytics foi inicializado
    private bool _isInitialized = false;

    private async void Awake()
    {
        // Garante que só exista um AnalyticsManager na cena
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);  // permanece entre cenas

        try
        {
            // Inicializa Unity Gaming Services (Analytics faz parte disso)
            await UnityServices.InitializeAsync();

            // Começa a coletar dados (respeito a consentimentos vai depender da tua config)
            AnalyticsService.Instance.StartDataCollection();

            _isInitialized = true;
            Debug.Log("AnalyticsManager: Unity Services & Analytics inicializados com sucesso.");
        }
        catch (Exception e)
        {
            Debug.LogWarning($"AnalyticsManager: Falha ao inicializar Analytics. Motivo: {e.Message}");
        }
    }

    // Método auxiliar para evitar tentar enviar eventos antes de inicializar
    private bool CanSendEvent()
    {
        if (!_isInitialized)
        {
            // Se quiser, pode logar aqui também
            return false;
        }

        return true;
    }

    // -----------------------
    //  EVENTOS PRINCIPAIS
    // -----------------------

    // 1) Sessão começou
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

    // 2) Daily bonus coletado
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

    // 3) Mudança de coins (podemos usar para milestones também)
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

        // Exemplo simples de milestone:
        // primeira vez que o jogador passa de 500 coins.
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
