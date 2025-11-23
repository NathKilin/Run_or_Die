using UnityEngine;
using Unity.Notifications.Android;


public class MobileNotificationInit : MonoBehaviour
{
    private void Awake()
    {
        var channel = new AndroidNotificationChannel()
        {
            Id = "daily_bonus_channel",            
            Name = "Daily Bonus",                    
            Importance = Importance.Default,
            Description = "Reminders to claim your daily coins."
        };


        AndroidNotificationCenter.RegisterNotificationChannel(channel);
    }
}



