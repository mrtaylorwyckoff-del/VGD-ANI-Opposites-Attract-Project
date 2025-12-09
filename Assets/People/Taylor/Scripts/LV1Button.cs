using UnityEngine;
using UnityEngine.SceneManagement;

public class LV1Button : MonoBehaviour
{
    public void OnLV1ButtonClick()
    {
        SceneManager.LoadScene("SMLoading");
    }
}