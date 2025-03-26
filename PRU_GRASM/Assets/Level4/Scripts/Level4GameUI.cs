using UnityEngine;
using UnityEngine.SceneManagement;

public class Level4GameUI : MonoBehaviour
{
    [SerializeField] private Level4GameManager gameManager;

    public void StartGame()
    {
        gameManager.StartGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void ContinueGame()
    {
        gameManager.ResumeGame();
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
