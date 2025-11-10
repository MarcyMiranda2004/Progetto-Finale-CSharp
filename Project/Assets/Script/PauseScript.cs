using UnityEngine;
using UnityEngine.UI;

public class PauseScript : MonoBehaviour
{
    public GameObject pauseMenuUI;

    [SerializeField] private Button _resumeButton;
    bool isPaused = false;

    void Start()
    {
        _resumeButton.onClick.AddListener(ResumeGame);
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            print("sto premendo esc correttamente");
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        Time.timeScale = 0f; // Ferma il tempo di gioco
        isPaused = true;
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(true);
            print("sto mettendo in pausa correttamente"); // Mostra il menu pausa
        }
        AudioListener.pause = false;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f; // Riattiva il tempo
        isPaused = false;
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
            print("ho premuto resume correttamente"); // Nasconde il menu pausa
        }
    }

    public void QuitGame()
    {
        Debug.Log("Uscita dal gioco...");
        Application.Quit();
    }
}
