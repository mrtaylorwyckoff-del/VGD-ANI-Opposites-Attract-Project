using UnityEngine;
using UnityEngine.SceneManagement;

public class Quit : MonoBehaviour
{
    public void OnStartButtonClick()
    {
        SceneManager.LoadScene("Quit");
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