using UnityEngine;

public class DollarIncreaser : MonoBehaviour
{
    [Header("Cheat / Debug (Inspector)")]
    [Tooltip("Key used to increase currency")]
    [SerializeField] private KeyCode increaseKey = KeyCode.R;
    [Tooltip("Amount to add when key is pressed")]
    [SerializeField] private int amount = 1000;
    [Tooltip("Require the application window to have focus before accepting input")]
    [SerializeField] private bool requireFocus = true;
    [Tooltip("Enable/disable this behaviour without removing the component")]
    [SerializeField] private bool enabledInInspector = true;

    // Update is called once per frame
    private void Update()
    {
        if (!enabledInInspector) return;
        if (requireFocus && !Application.isFocused) return;

        if (Input.GetKeyDown(increaseKey))
        {
            if (LevelManager.main != null)
            {
                LevelManager.main.AddCurrency(amount);
            }
            else
            {
                Debug.LogWarning("DollarIncreaser: LevelManager.main is null. Ensure LevelManager exists in the scene.");
            }
        }
    }
}
