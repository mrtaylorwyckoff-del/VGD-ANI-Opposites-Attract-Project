using UnityEngine;
using UnityEngine.SceneManagement;

public class LV2Button : MonoBehaviour
{
    public void OnLV2ButtonClick()
    {
        SceneManager.LoadScene("PLLoading");
    }
}