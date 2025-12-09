using UnityEngine;
using UnityEngine.SceneManagement;

public class AutoLoadSM : MonoBehaviour
{
    public float delayBeforeLoad = 2f;

    void Start()
    {
        Invoke(nameof(LoadNextScene), delayBeforeLoad);
    }

    private void LoadNextScene()
    {
        SceneManager.LoadScene("Supermarket");
    }
}
