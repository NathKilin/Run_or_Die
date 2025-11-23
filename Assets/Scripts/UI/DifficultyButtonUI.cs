using TMPro;
using UnityEngine;

public class DifficultyButtonUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI label; 

    private void Start()
    {
        UpdateLabel();
    }

    public void OnButtonClicked()
    {
        if (DifficultyManager.Instance == null) return;

        DifficultyManager.Instance.ToggleDifficulty();
        UpdateLabel();
    }

    private void UpdateLabel()
    {
        if (DifficultyManager.Instance == null || label == null) return;

        var profile = DifficultyManager.Instance.currentProfile;
        if (profile == null) return;

        label.text = "Difficulty: " + profile.difficultyName;
    }
}
