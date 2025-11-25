using UnityEngine;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    public void OnSettingsButtonClick()
    {
        SceneManager.LoadScene("Settings");
    }

 
}