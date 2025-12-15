using UnityEngine;

public class FlipSides : MonoBehaviour
{
    // Assign these in the Unity Inspector
    public GameObject objectToShow;
    public GameObject objectToHide;

    void Start()
    {
        // Optional: Ensure the initial states are correct when the game starts
        if (objectToShow != null)
        {
            objectToShow.SetActive(false); // Start hidden
        }
        if (objectToHide != null)
        {
            objectToHide.SetActive(true); // Start shown
        }
    }

    // Call this method when you want to perform the swap (e.g., via a UI button click)
    public void SwapObjectsVisibility()
    {
        // Hide the first object
        if (objectToHide != null)
        {
            objectToHide.SetActive(false);
        }

        // Show the second object
        if (objectToShow != null)
        {
            objectToShow.SetActive(true);
        }
    }
}