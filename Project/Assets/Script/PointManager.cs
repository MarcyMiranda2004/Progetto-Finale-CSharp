using System;
using System.Collections;
using System.Threading.Tasks;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class PointManager : MonoBehaviour
{
    // Singleton — garantisce che esista una sola istanza di PointManager in tutta la partita
    public static PointManager Instance;

    // Riferimenti UI
    [SerializeField]
    private GameObject WinSprite; // Immagine o pannello mostrato alla vittoria

    [SerializeField]
    private TextMeshProUGUI puntiText; // Testo che mostra il punteggio in gioco

    [SerializeField]
    private TextMeshProUGUI eventiText; // Testo per messaggi o eventi (facoltativo)

    // Punteggio corrente del giocatore
    private float points = 0;

    private void Awake()
    {
        // Implementazione del pattern Singleton
        if (Instance == null)
        {
            Instance = this;
            WinSprite.SetActive(false); // Nasconde l’immagine di vittoria all’avvio
            DontDestroyOnLoad(gameObject); // Mantiene l’oggetto anche cambiando scena
        }
        else
        {
            Destroy(gameObject); // Evita duplicati del manager
        }
    }

    // Aggiunge un punto al punteggio totale
    public void AddPoint()
    {
        points++;
        CheckPoints(); // Controlla se il punteggio raggiunge l’obiettivo
        Debug.Log("Punti totali: " + points);
    }

    // Restituisce il punteggio attuale (usato da altri script)
    public float GetPoints()
    {
        return points;
    }

    // Sottrae un punto (es. quando raccoglie cibo spazzatura)
    public void menusPoints()
    {
        points--;
        CheckPoints();
        Debug.Log("Punti totali: " + points);
    }

    // Aggiunge due punti (es. raccolta di un bonus)
    public void AddPointX2()
    {
        points += 2;
        CheckPoints();
        Debug.Log("Punti totali: " + points);
    }

    // Controlla se è stato raggiunto il punteggio per vincere
    public void CheckPoints()
    {
        if (points >= 5) // Condizione di vittoria (puoi personalizzarla)
        {
            Debug.Log("Hai raggiunto il punteggio necessario!");
            Win(); // Avvia la sequenza di vittoria
        }
    }

    // Metodo chiamato quando il giocatore vince
    private async Task Win()
    {
        // Mostra lo sprite o pannello di vittoria
        WinSprite.SetActive(true);

        // Nasconde i testi del punteggio e degli eventi
        puntiText.gameObject.SetActive(false);
        eventiText.gameObject.SetActive(false);

        // Mette in pausa il gioco
        Time.timeScale = 0f;

        // Attende 5 secondi reali (non influenzati dal Time.timeScale)
        await Task.Delay(5000);

        // Cambia scena alla successiva (ad esempio: aspetti sociali)
        GameManager.Instance.ChangeScene("AspettiSocialiScene");
    }
}
