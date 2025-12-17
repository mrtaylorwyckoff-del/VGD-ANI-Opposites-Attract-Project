using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsGoBack : MonoBehaviour
{
    public void OnGoBackButtonClick()
    {
        SceneManager.LoadScene("CreditSelect");
    }

}