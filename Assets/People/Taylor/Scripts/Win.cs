using UnityEngine;
using UnityEngine.SceneManagement;

public class Win : MonoBehaviour
{
    public void OnWinButtonClick()
    {
        SceneManager.LoadScene("Win");
    }

}