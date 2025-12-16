using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoLoadSM : MonoBehaviour
{
    public float delayBeforeLoad = 1f;

    void Start()
    {
        Invoke(nameof(LoadNextScene), delayBeforeLoad);
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene("FINAL Level 1");
    }
}
