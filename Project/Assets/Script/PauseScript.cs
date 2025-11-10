using UnityEngine;
using UnityEngine.UI;

public class PauseScript : MonoBehaviour
{
    // Riferimento al pannello UI del menu pausa (assegnato nell’Inspector)
    public GameObject pauseMenuUI;

    // Bottone per riprendere il gioco
    [SerializeField] private Button _resumeButton;

    // Stato attuale del gioco (true = in pausa, false = in esecuzione)
    private bool isPaused = false;

    void Start()
    {
        // Collega il pulsante "Resume" al metodo ResumeGame()
        _resumeButton.onClick.AddListener(ResumeGame);
    }

    // Controlla ogni frame se viene premuto il tasto ESC
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            print("sto premendo esc correttamente");

            // Se il gioco è già in pausa → riprendi
            // Altrimenti → metti in pausa
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    // Mette in pausa il gioco
    public void PauseGame()
    {
        // Blocca il tempo di gioco
        Time.timeScale = 0f;

        // Imposta lo stato
        isPaused = true;

        // Mostra il menu pausa, se esiste
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(true);
            print("sto mettendo in pausa correttamente");
        }

        // 🔊 Pausa opzionale dell’audio di gioco
        // (Nota: in questo caso è impostato su false, quindi NON mette in pausa l'audio)
        AudioListener.pause = false;
    }

    // Riprende il gioco dopo la pausa
    public void ResumeGame()
    {
        // Riavvia il tempo di gioco
        Time.timeScale = 1f;

        // Aggiorna lo stato
        isPaused = false;

        // Nasconde il menu pausa, se esiste
        if (pauseMenuUI != null)
        {
            pauseMenuUI.SetActive(false);
            print("ho premuto resume correttamente");
        }

        // Riattiva l’audio se era stato messo in pausa
        AudioListener.pause = false;
    }

    // Esce dal gioco
    public void QuitGame()
    {
        Debug.Log("Uscita dal gioco...");
        Application.Quit();

        // Nota: In Editor di Unity, Application.Quit() non ha effetto.
        // Funziona solo nella build finale del gioco.
    }
}
