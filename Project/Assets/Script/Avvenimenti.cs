using TMPro;
using UnityEngine;

public class Avvenimenti : MonoBehaviour
{
    //  Riferimento al testo UI in cui verranno mostrati i messaggi degli eventi
    [SerializeField]
    private TextMeshProUGUI eventiText;

    private void OnEnable()
    {
        //  Si sottoscrive all’evento statico OnEventiChanged della classe Raccolta
        // In questo modo, ogni volta che Raccolta genera un nuovo messaggio,
        // il metodo ShowEventi() verrà automaticamente chiamato.
        Raccolta.OnEventiChanged += ShowEventi;
    }

    private void OnDisable()
    {
        //  Rimuove la sottoscrizione quando l’oggetto viene disattivato
        // (fondamentale per evitare memory leak o errori quando la scena cambia)
        Raccolta.OnEventiChanged -= ShowEventi;
    }

    //  Mostra il messaggio ricevuto dall’evento nel campo di testo UI
    private void ShowEventi(string message)
    {
        if (eventiText != null)
            eventiText.text = message; // Aggiorna il testo visibile nella UI
    }
}
