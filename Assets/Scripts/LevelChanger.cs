using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChanger : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private int levelToLoad;

    public void StartGame()
    {
        FadeToLevel(1);
    }

    public void NextLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            FadeToLevel(0);
        }
        else
        {
            FadeToLevel(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    public void BackToMainMenu()
    {
        FadeToLevel(0);
    }

    public void RestartLevel()
    {
        FadeToLevel(SceneManager.GetActiveScene().buildIndex);
    }

    public void FadeToLevel(int levelIndex)
    {
        levelToLoad = levelIndex;
        animator.SetTrigger("FadeOut");
    }

    public void OnFadeComplete()
    {
        SceneManager.LoadScene(levelToLoad);
    }
}
