using System;
using UnityEngine;
using Unity.Notifications.Android;  
#if UNITY_ANDROID
using Unity.Notifications.Android;   // para notificações locais no Android
#endif

public class DailyBonusManager : MonoBehaviour
{
    [Header("Daily Bonus Settings")]
    [SerializeField] private int coinsReward = 100; 

    // Em produção use 24f. Para testar, você já está usando 0.01f ≈ 36 segundos
    [SerializeField] private float hoursBetweenBonuses = 0.01f;

    [SerializeField] private bool giveTestBonusOnStart = true;

    private const string LastPlayTimeKey = "LAST_PLAY_TIME";

    // IDs de canal de notificação (apenas nomes de string)
    private const string NotificationChannelId = "daily_bonus_channel";
    private const string NotificationChannelName = "Daily Bonus";
    private const string NotificationChannelDescription = "Reminders to collect daily bonus.";

private void Start()
{
    SetupNotificationChannel();
    
    Debug.Log($"DailyBonusManager: Start. giveTestBonusOnStart={giveTestBonusOnStart}");

    if (giveTestBonusOnStart)
    {
        Debug.Log("DailyBonusManager: TEST MODE → GrantDailyBonus()");
        GrantDailyBonus();

        // Agenda próxima notificação de teste
        ScheduleNextBonusNotification();
        return;
    }
        
    if (IsBonusAvailable())
    {
        Debug.Log("DailyBonusManager: Bonus is available → GrantDailyBonus()");
        GrantDailyBonus();
    }
    else
    {
        Debug.Log("DailyBonusManager: Bonus NOT available.");
    }

    SaveLastPlayTime(DateTime.Now);

    // Toda vez que o jogador entra, agendamos o próximo lembrete
    ScheduleNextBonusNotification();
}



    private void GrantDailyBonus()
    {
        // Mostra o popup de bonus
        if (DailyBonusPopup.Instance != null)
        {
            Debug.Log($"DailyBonusManager: Showing daily bonus popup for {coinsReward} coins.");
            DailyBonusPopup.Instance.Show(coinsReward);
        }
        else
        {
            Debug.LogWarning("DailyBonusManager: DailyBonusPopup.Instance is null. Popup not found.");
        }

        // Depois que concedemos o bônus de hoje,
        // já agendamos a próxima notificação para daqui a X horas
        ScheduleNextBonusNotification();
    }

    private bool IsBonusAvailable()
    {
        if (!PlayerPrefs.HasKey(LastPlayTimeKey))
        {
            // primeira vez jogando → você decidiu NÃO dar bonus automático
            return false;
        }

        string storedTime = PlayerPrefs.GetString(LastPlayTimeKey, string.Empty);
        if (string.IsNullOrEmpty(storedTime))
            return false;

        DateTime lastPlayTime;
        if (!DateTime.TryParse(storedTime, out lastPlayTime))
            return false;

        TimeSpan timeSinceLastPlay = DateTime.Now - lastPlayTime;

        return timeSinceLastPlay.TotalHours >= hoursBetweenBonuses;
    }

    private void SaveLastPlayTime(DateTime time)
    {
        PlayerPrefs.SetString(LastPlayTimeKey, time.ToString());
        PlayerPrefs.Save();
    }

    // -----------------------------
    // NOTIFICAÇÕES ANDROID
    // -----------------------------

    // Cria/Registra o canal onde as notificações vão aparecer (apenas uma vez)
    private void SetupNotificationChannel()
    {
    #if UNITY_ANDROID
        var channel = new AndroidNotificationChannel
        {
            Id = NotificationChannelId,          // id interno
            Name = NotificationChannelName,      // nome mostrado pro usuário
            Importance = Importance.Default,
            Description = NotificationChannelDescription
        };

        AndroidNotificationCenter.RegisterNotificationChannel(channel);
    #endif
    }

    // Agenda uma notificação local para "daqui a X horas"
private void ScheduleNextBonusNotification()
{
    // Quando a próxima notificação deve acontecer:
    var fireTime = DateTime.Now.AddHours(hoursBetweenBonuses);

    var notification = new AndroidNotification
    {
        Title   = "Daily bonus ready!",
        Text    = $"Come back and claim +{coinsReward} coins.",
        FireTime = fireTime
    };

    // Envia para o canal que criamos antes
    AndroidNotificationCenter.SendNotification(
        notification,
        "daily_bonus_channel"
    );
}
}
