using UnityEngine;
using UnityEngine.UI;  
using TMPro;            


public class DailyBonusPopup : MonoBehaviour
{
    // trying to make it like singleton
    public static DailyBonusPopup Instance { get; private set; }


    [Header("Popup UI")]
    [SerializeField] private GameObject popupRoot;  
    [SerializeField] private TMP_Text messageText;  
    [SerializeField] private Button closeButton;    


    private int pendingBonusAmount = 0;          
    private bool isOpen = false;                    


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }


        // pop up should start hidden
        if (popupRoot != null)
            popupRoot.SetActive(false);


        // Connect the close button event
        if (closeButton != null)
            closeButton.onClick.AddListener(OnCloseClicked);
    }


    // Called by DailyBonusManager when the bonus is available
    public void Show(int bonusAmount)
    {
        if (popupRoot == null)
        {
            Debug.LogWarning("DailyBonusPopup: popupRoot is not assigned.");
            return;
        }


        // save the pending bonus amount
        pendingBonusAmount = bonusAmount;
        isOpen = true;


        // Update the message text
        if (messageText != null)
        {
            messageText.text = $"Daily bonus granted: +{bonusAmount} coins";
        }


        // Show the panel
        popupRoot.SetActive(true);


        // Pause the game while the popup is open
        Time.timeScale = 0f;
    }


    // when the player clicks the close button
private void OnCloseClicked()
{
    if (!isOpen)
        return;


    isOpen = false;


    if (pendingBonusAmount > 0 && Coins.Instance != null)
    {
        // Adiciona as moedas
        Coins.Instance.AddCoins(pendingBonusAmount);


        // Analytics: registra que o daily bonus foi coletado
        if (AnalyticsManager.Instance != null)
        {
            AnalyticsManager.Instance.TrackDailyBonusCollected(pendingBonusAmount);
        }
    }


    pendingBonusAmount = 0;


    if (popupRoot != null)
        popupRoot.SetActive(false);


    Time.timeScale = 1f;
}


}



