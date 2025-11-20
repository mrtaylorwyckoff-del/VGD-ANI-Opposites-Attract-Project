using UnityEngine;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    public void OnStartButtonClick()
    {
        SceneManager.LoadScene("Settings");
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