using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void OnStartButtonClick()
    {
        SceneManager.LoadScene("MainMenu");
    }

public void OnSettingsButtonClick()
    {
      Debug.Log("*flips* hooray!");
    }

public void OnQuitButtonClick()
    {
        Application.Quit();
        Debug.Log("*flips* Hooray!");
    }
}