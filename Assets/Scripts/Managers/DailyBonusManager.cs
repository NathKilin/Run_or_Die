using System;
using UnityEngine;
using Unity.Notifications.Android;  
#if UNITY_ANDROID
using Unity.Notifications.Android;   
#endif


public class DailyBonusManager : MonoBehaviour
{
    [Header("Daily Bonus Settings")]
    [SerializeField] private int coinsReward = 100;


    [SerializeField] private float hoursBetweenBonuses = 0.01f;


    [SerializeField] private bool giveTestBonusOnStart = true;


    private const string LastPlayTimeKey = "LAST_PLAY_TIME";


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


    ScheduleNextBonusNotification();
}


    private void GrantDailyBonus()
    {
        if (DailyBonusPopup.Instance != null)
        {
            Debug.Log($"DailyBonusManager: Showing daily bonus popup for {coinsReward} coins.");
            DailyBonusPopup.Instance.Show(coinsReward);
        }
        else
        {
            Debug.LogWarning("DailyBonusManager: DailyBonusPopup.Instance is null. Popup not found.");
        }


        ScheduleNextBonusNotification();
    }


    private bool IsBonusAvailable()
    {
        if (!PlayerPrefs.HasKey(LastPlayTimeKey))
        {
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


    private void SetupNotificationChannel()
    {
    #if UNITY_ANDROID
        var channel = new AndroidNotificationChannel
        {
            Id = NotificationChannelId,          
            Name = NotificationChannelName,     
            Importance = Importance.Default,
            Description = NotificationChannelDescription
        };


        AndroidNotificationCenter.RegisterNotificationChannel(channel);
    #endif
    }


private void ScheduleNextBonusNotification()
{
    var fireTime = DateTime.Now.AddHours(hoursBetweenBonuses);


    var notification = new AndroidNotification
    {
        Title   = "Daily bonus ready!",
        Text    = $"Come back and claim +{coinsReward} coins.",
        FireTime = fireTime
    };


    AndroidNotificationCenter.SendNotification(
        notification,
        "daily_bonus_channel"
    );
}
}



