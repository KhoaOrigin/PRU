using UnityEngine;
using UnityEngine.SceneManagement;

public class Level2GameUI : MonoBehaviour
{
    [SerializeField] private Level2GameManager gameManager;
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
