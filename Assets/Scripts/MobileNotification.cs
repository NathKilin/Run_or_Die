using UnityEngine;
using Unity.Notifications.Android;

public class MobileNotificationInit : MonoBehaviour
{
    private void Awake()
    {
        // Cria um canal de notificação (obrigatório no Android moderno)
        var channel = new AndroidNotificationChannel()
        {
            Id = "daily_bonus_channel",              // id interno
            Name = "Daily Bonus",                    // nome mostrado ao usuário
            Importance = Importance.Default,
            Description = "Reminders to claim your daily coins."
        };

        AndroidNotificationCenter.RegisterNotificationChannel(channel);
    }
}
