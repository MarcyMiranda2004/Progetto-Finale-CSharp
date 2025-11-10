using System;
using TMPro;
using UnityEngine;

public class Raccolta : MonoBehaviour
{
    // Istanza singleton (accesso globale a Raccolta)
    public static Raccolta Instance;

    // Durata degli effetti temporanei (es. comandi invertiti)
    public float effectDuration = 5f;

    // Evento statico che notifica quando un messaggio deve essere mostrato a schermo
    //    (altri script come "Avvenimenti" possono iscriversi per aggiornare la UI)
    public static event Action<string> OnEventiChanged;

    private void Awake()
    {
        // Imposta l’istanza Singleton di questa classe
        Instance = this;
    }

    // Viene chiamato automaticamente da Unity quando il collider del giocatore
    //    entra in contatto con un altro collider 2D con "IsTrigger" attivo.
    private void OnTriggerEnter2D(Collider2D other)
    {
        //  FRUTTA → Punti positivi
        if (other.CompareTag("Fruit"))
        {
            Destroy(other.gameObject); // Rimuove l’oggetto raccolto
            PointManager.Instance.AddPoint(); // Aggiunge 1 punto
            OnEventiChanged?.Invoke("Hai raccolto un frutto!"); // Mostra messaggio nella UI
        }
        // CIBO SPAZZATURA → Toglie punti
        else if (other.CompareTag("JunkFood"))
        {
            Destroy(other.gameObject);
            PointManager.Instance.menusPoints(); // Sottrae punti
            OnEventiChanged?.Invoke("Hai raccolto del cibo spazzatura!");
        }
        // MALUS (es. Alcool) → Inverte i controlli per alcuni secondi
        else if (other.CompareTag("Malus"))
        {
            Destroy(other.gameObject);

            // Recupera il componente Movimento2D sul giocatore
            Movimento2D movimento = GetComponent<Movimento2D>();
            if (movimento != null)
                movimento.InvertControls(effectDuration); // Inverte i controlli temporaneamente

            OnEventiChanged?.Invoke("Hai bevuto alcool! Comandi invertiti per 5 secondi!");
        }
        // BONUS → Raddoppia i punti
        else if (other.CompareTag("Bonus"))
        {
            Destroy(other.gameObject);
            PointManager.Instance.AddPointX2(); // Aggiunge punti doppi
            OnEventiChanged?.Invoke("Hai mangiato del cibo molto sano! Punti x2!");
        }
        // SPEEDUP (es. caffeina) → aumenta la velocità ma penalizza i punti
        else if (other.CompareTag("SpeedUp"))
        {
            Destroy(other.gameObject);

            // Raddoppia temporaneamente la velocità del movimento
            Movimento2D movimento = GetComponent<Movimento2D>();
            if (movimento != null)
                movimento.moveSpeed *= 2;

            // Penalizza i punti (comportamento definito nel PointManager)
            PointManager.Instance.menusPoints();

            OnEventiChanged?.Invoke("Hai raccolto qualcosa di sbagliato!");
        }
    }
}
