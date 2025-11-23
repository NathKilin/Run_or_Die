using UnityEngine;
using TMPro;


public class CoinsUI : MonoBehaviour
{
    [SerializeField] private TMP_Text coinsText;


    private void Start()
    {
        UpdateText(Coins.Instance.GetCoins());


        Coins.Instance.OnCoinsChanged += UpdateText;
    }


    private void UpdateText(int value)
    {
        coinsText.text = value.ToString();
    }


    private void OnDestroy()
    {
        if (Coins.Instance != null)
            Coins.Instance.OnCoinsChanged -= UpdateText;
    }
}



