using UnityEngine;

public class PauseButton : MonoBehaviour
{
    public static bool gameIsPaused = false;

    [SerializeField] private GameObject pausePanel = null;
    [SerializeField] private GameObject confirmPanel = null;

    private Player player;
    private PlayerInput playerInput;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        playerInput = player.GetPlayerInput();

        playerInput.UI.Pause.performed += _ => SetPauseLogic();
    }

    private void PauseGame()
    {
        Time.timeScale = 0f;
        pausePanel.SetActive(true);
        playerInput.PlayerMain.Disable();
        gameIsPaused = !gameIsPaused;
    }

    private void ResumeGame()
    {
        Time.timeScale = 1f;
        pausePanel.SetActive(false);
        playerInput.PlayerMain.Enable();
        gameIsPaused = !gameIsPaused;
        confirmPanel.SetActive(false);
    }

    private void SetPauseLogic()
    {
        if (gameIsPaused)
        {
            ResumeGame();
        } else
        {
            PauseGame();
        }
    }

    public void ShowConfirmMenu()
    {
        confirmPanel.SetActive(true);
    }

    public void HideConfirmMenu()
    {
        confirmPanel.SetActive(false);
    }

    public void LoadMainMenu()
    {
        LevelChanger levelChanger = GameObject.FindGameObjectWithTag("LevelChanger").GetComponent<LevelChanger>();
        levelChanger.BackToMainMenu();
        Time.timeScale = 1f;
    }
}
