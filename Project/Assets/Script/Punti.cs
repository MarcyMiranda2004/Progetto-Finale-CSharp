using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Punti : MonoBehaviour
{
    // Riferimento al componente TextMeshPro dove verranno mostrati i punti
    [SerializeField]
    private TextMeshProUGUI puntiText;

    private void Update()
    {
        // Aggiorna il testo ad ogni frame
        // (mostra sempre il punteggio corrente)
        stampaPunti();
    }

    // Metodo che scrive il punteggio sul testo UI
    private void stampaPunti()
    {
        // Controlla che il riferimento al testo e al GameManager esistano
        if (puntiText != null && GameManager.Instance != null)
        {
            // Ottiene il punteggio corrente dal PointManager e lo mostra nella UI
            puntiText.text = "Punti: " + PointManager.Instance.GetPoints();
        }
    }
}
