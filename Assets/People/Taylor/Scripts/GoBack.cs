using UnityEngine;
using UnityEngine.SceneManagement;

public class GoBack : MonoBehaviour
{
    public void OnGoBackButtonClick()
    {
        SceneManager.LoadScene("StartMenu");
    }

}