using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CloseScript : MonoBehaviour
{
    public string sceneToQuitOn = "Quit";
    public float quitDelay = 0.5f;

    public Vector3 startSize;
    public Vector3 endSize;

    void Start()
    {
        if (SceneManager.GetActiveScene().name == sceneToQuitOn)
        {
            StartCoroutine(QuitGameAfterDelay(quitDelay));
        }
    }

    IEnumerator QuitGameAfterDelay(float delay)
    {
        for (float t = 0; t < 1; t += Time.deltaTime)
        {
            transform.localScale = Vector3.Lerp(startSize, endSize, t);
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(delay);

        QuitGame();
    }

    void QuitGame()
    {
        Debug.Log("Quitting application...");

        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}

