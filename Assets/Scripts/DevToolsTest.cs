using UnityEngine;

public class DevTools : MonoBehaviour
{
    [ContextMenu("DEV: Reset All PlayerPrefs")]
    private void ResetAll()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("DEV: PlayerPrefs resetados.");
    }
}
