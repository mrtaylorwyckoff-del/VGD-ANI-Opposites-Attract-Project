using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSMScene : MonoBehaviour
{
    public float delayBeforeLoad = 10f;

    void Start()
    {
        Invoke(nameof(LoadNextScene), delayBeforeLoad);
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene("FINAL Level 1");
    }
}
