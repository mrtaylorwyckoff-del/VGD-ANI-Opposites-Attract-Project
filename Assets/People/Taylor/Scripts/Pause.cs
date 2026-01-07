using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject pausePanel; // Drag your UI Panel here in the Inspector
    private bool isPaused = false;

    void Update()
    {
        // Toggle pause when 'Escape' or 'P' is pressed
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused) Resume();
            else Pause();
        }
    }

    public void Pause()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f; // Freezes time
        isPaused = true;
    }

    public void Resume()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f; // Resumes time
        isPaused = false;
    }
}
