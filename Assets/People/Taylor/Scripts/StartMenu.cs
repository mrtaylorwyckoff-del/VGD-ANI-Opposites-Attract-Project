using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    public void OnStartButtonClick()
    {
        SceneManager.LoadScene("TaylorScene");
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