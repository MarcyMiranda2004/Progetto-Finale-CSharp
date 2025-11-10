using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class PyramidManager : MonoBehaviour
{
    // Elenco di tutti i cibi (oggetti GrabAndDrag) presenti nella scena
    //    Assegnali manualmente nell’Inspector o tramite script
    public List<GrabAndDrag> allFoods = new List<GrabAndDrag>();

    [SerializeField]
    private string nextSceneName; // Nome della scena successiva da caricare dopo la vittoria

    [SerializeField]
    private GameObject victorySprite; // Immagine o pannello di vittoria

    private void Start()
    {
        // All’avvio, assicura che l’immagine di vittoria sia nascosta
        if (victorySprite != null)
            victorySprite.SetActive(false);
    }

    // Metodo chiamato ogni volta che un alimento viene posizionato
    //    Controlla se la piramide è completa (tutti i cibi corretti)
    public async Task CheckCompletion()
    {
        // Controlla se TUTTI i cibi della lista sono attaccati correttamente
        foreach (GrabAndDrag food in allFoods)
        {
            if (!food.IsCorrectlyAttached)
            {
                // Se anche solo uno non è corretto, interrompe il controllo
                return;
            }
        }

        // Tutti i cibi sono stati collocati nella posizione giusta!

        // Mostra l’immagine o pannello di vittoria
        if (victorySprite != null)
        {
            victorySprite.SetActive(true);
        }

        // Attende 4 secondi (reali, non influenzati da Time.timeScale)
        await Task.Delay(4000);

        // Cambia scena (usa il GameManager per gestire la transizione)
        GameManager.Instance.ChangeScene(nextSceneName);
    }
}
