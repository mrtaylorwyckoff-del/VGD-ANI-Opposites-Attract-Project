using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    public void OnStartButtonClick()
    {
        SceneManager.LoadScene("Credits");
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