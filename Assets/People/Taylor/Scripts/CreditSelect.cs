using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditSelect : MonoBehaviour
{
    public void OnCreditsButtonClick()
    {
        SceneManager.LoadScene("CreditSelect");
    }

}