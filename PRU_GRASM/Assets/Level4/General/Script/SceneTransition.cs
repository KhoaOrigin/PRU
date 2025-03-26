using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            MoveToNextScene();
        }
    }

    public void MoveToNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (currentSceneIndex == 3)
        {
            nextSceneIndex++;
        }

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings + 1)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.Log("No next scene available!");
            // Optional: Loop back to Level1
            // SceneManager.LoadScene(0);
        }
    }
}