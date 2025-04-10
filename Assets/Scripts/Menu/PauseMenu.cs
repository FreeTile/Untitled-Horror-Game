using UnityEngine;
using UnityEngine.SceneManagement;
using FMODUnity;

public class PauseMenu : MonoBehaviour
{
    public EventReference resumeSound;
    public EventReference clickSound;

    // Resume the game
    public void ResumeGame()
    {
        if (!resumeSound.IsNull)
            RuntimeManager.PlayOneShot(resumeSound);

        if (GameManager.Instance != null)
            GameManager.Instance.ProceedEsc();
    }

    // Return to Main Menu
    public void ReturnToMainMenu()
    {
        if (!clickSound.IsNull)
            RuntimeManager.PlayOneShot(clickSound);

        Time.timeScale = 1f; // Unpause game
        SceneManager.LoadScene("Menu");
    }

    // Quit the game completely
    public void QuitGame()
    {
        if (!clickSound.IsNull)
            RuntimeManager.PlayOneShot(clickSound);

        Application.Quit();
    }

}
