using System.Collections;
using System.Text;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class TypewriterEffect : MonoBehaviour
{
    [Header("Riferimenti UI")]
    [SerializeField]
    private TextMeshProUGUI dialogoText1;

    [Header("Variabili")]
    [SerializeField]
    private float typeSpeed = 0.03f;

    private string fullText;
    private Coroutine typingCoroutine;
    private bool isTyping;
    private bool skipRequested;

    private void Awake()
    {
        if (dialogoText1 == null)
            dialogoText1 = GetComponent<TextMeshProUGUI>(); // recupera il Text Mesh Pro se non viene assegnato

        if (dialogoText1 == null)
            Debug.LogError("Nessun TextMeshProUGUI assegnato al TypewriterEffect!");
        else
            dialogoText1.text = "";
    }

    public void ShowText(string textToShow) // prende il contenuto del fil txt come stringa
    {
        fullText = textToShow; // prende il testo come stringa

        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine); // se la typingCoroutine non esiste ferma la Coroutine

        typingCoroutine = StartCoroutine(TypeText()); // avvia la Coroutine e lancia il metodo TypeText
    }

    private IEnumerator TypeText()
    {
        if (dialogoText1 == null)
            yield break; // si ferma se il testo è null

        isTyping = true;
        skipRequested = false;

        var sb = new StringBuilder(); // crea lo StringBuilder che serve per costruire le stringhe nel text mesh pro
        dialogoText1.text = ""; // si assicura che il text mesh pro sia vuoto prima di iniziare a scrivere

        foreach (char c in fullText) // fa un foreach basato sula testo intero
        {
            if (skipRequested) // controlla se c'è la skipRequested (skipRequested == true) e in caso ferma l'animazione
            {
                dialogoText1.text = fullText;
                break;
            }

            sb.Append(c); // "appende" ogni carattere per stamparlo
            dialogoText1.text = sb.ToString(); // converte ogni stringa per stamparla

            while (Time.timeScale == 0f)
                yield return null; // quando il gioco è in pausa la scrittura si ferma

            yield return new WaitForSeconds(typeSpeed); // aspetta typeSpeed secondi prima di stampare il carattere successivo
        }

        isTyping = false; // ferma la scrittura che
        skipRequested = false; // ferma la skipRequested
    }

    public void SkipTypingAnimation() // se vene chiamato mentre isTyping è attivo (isTyping == true) allora rende attiva la skipRequested
    {
        if (isTyping)
            skipRequested = true;
    }

    public bool IsTyping() // ritorna lo stato della isTyping
    {
        return isTyping;
    }
}
