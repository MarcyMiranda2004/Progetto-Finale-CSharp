using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Classe di supporto che rappresenta una singola domanda del quiz
public class Question
{
    public string question; // Testo della domanda
    public string[] answers; // Array delle possibili risposte
    public int correctIndex; // Indice della risposta corretta
}

public class QuizManager : MonoBehaviour
{
    // Riferimenti UI
    public TMP_Text DomandeText; // Campo di testo che mostra la domanda
    public Button[] BottoneRisposta; // Array di bottoni per le risposte
    public TMP_Text feedbackText; // Testo che mostra il messaggio di feedback (corretto/sbagliato)

    // File di testo contenente le domande (da assegnare nel pannello Inspector)
    public TextAsset txtFile;

    // Dati del quiz
    private List<Question> Domande = new List<Question>(); // Lista di tutte le domande caricate
    private Question domandaCorrente; // Domanda attualmente mostrata
    private int indiceDomanda = 0; // Indice della domanda corrente
    private int risposteCorrette = 0; // Contatore risposte corrette

    void Start()
    {
        // Carica le domande dal file
        LoadQuestions();

        // Mescola l’ordine delle domande per ogni partita
        Domande = Domande.OrderBy(q => Random.value).ToList();

        // Mostra la prima domanda
        ShowNextQuestion();
    }

    // Carica le domande dal file di testo
    void LoadQuestions()
    {
        // Divide il testo in righe
        string[] lines = txtFile.text.Split('\n');

        foreach (string line in lines)
        {
            // Salta le righe vuote
            if (string.IsNullOrWhiteSpace(line))
                continue;

            // Divide ogni riga in parti separate da “;”
            string[] parts = line.Trim().Split(';');
            if (parts.Length < 3)
                continue;

            // Crea un nuovo oggetto domanda
            Question q = new Question();
            q.question = parts[0]; // La prima parte è la domanda

            // Le parti intermedie sono le risposte
            q.answers = new string[parts.Length - 2];
            for (int i = 1; i < parts.Length - 1; i++)
            {
                q.answers[i - 1] = parts[i];
            }

            // L’ultima parte è l’indice della risposta corretta
            int.TryParse(parts[parts.Length - 1], out q.correctIndex);

            // Aggiunge la domanda alla lista
            Domande.Add(q);
        }
    }

    //  Mostra la domanda successiva sullo schermo
    async void ShowNextQuestion()
    {
        // Se non ci sono più domande → quiz terminato
        if (indiceDomanda >= Domande.Count)
        {
            DomandeText.text =
                $"Quiz completato!\nHai risposto correttamente a {risposteCorrette} su {Domande.Count}.";
            feedbackText.text = "";

            // Nasconde tutti i bottoni
            foreach (var btn in BottoneRisposta)
                btn.gameObject.SetActive(false);

            // Attende 2 secondi e cambia scena
            await Task.Delay(2000);
            GameManager.Instance.ChangeScene("PiramideAlimentare");
            return;
        }

        // Prende la prossima domanda
        domandaCorrente = Domande[indiceDomanda];
        DomandeText.text = domandaCorrente.question;

        //  Mescola le risposte mantenendo traccia di quella corretta
        int correctIndexOriginale = domandaCorrente.correctIndex;
        string[] risposteOriginali = domandaCorrente.answers;

        // Crea una lista di indici (0,1,2,3...) e la mescola
        List<int> indici = new List<int>();
        for (int i = 0; i < risposteOriginali.Length; i++)
            indici.Add(i);

        for (int i = 0; i < indici.Count; i++)
        {
            int rnd = Random.Range(0, indici.Count);
            (indici[i], indici[rnd]) = (indici[rnd], indici[i]);
        }

        // Calcola il nuovo indice della risposta corretta dopo la mescolatura
        int nuovoIndiceCorretto = indici.IndexOf(correctIndexOriginale);

        // Imposta i bottoni con le nuove risposte
        for (int i = 0; i < BottoneRisposta.Length; i++)
        {
            if (i < risposteOriginali.Length)
            {
                BottoneRisposta[i].gameObject.SetActive(true);
                BottoneRisposta[i].interactable = true;

                // Aggiorna il testo del bottone
                BottoneRisposta[i].GetComponentInChildren<TMP_Text>().text = risposteOriginali[
                    indici[i]
                ];

                // Aggiunge l’evento click
                int indiceSelezionato = i;
                BottoneRisposta[i].onClick.RemoveAllListeners();
                BottoneRisposta[i]
                    .onClick.AddListener(() => CheckAnswer(indiceSelezionato, nuovoIndiceCorretto));
            }
            else
            {
                BottoneRisposta[i].gameObject.SetActive(false);
            }
        }

        // Resetta il feedback
        feedbackText.text = "";
        feedbackText.alpha = 0;
    }

    // Controlla se la risposta selezionata è corretta
    void CheckAnswer(int selectedIndex, int correctIndex)
    {
        // Disabilita tutti i bottoni per evitare clic multipli
        foreach (var btn in BottoneRisposta)
            btn.interactable = false;

        // Verifica la risposta
        if (selectedIndex == correctIndex)
        {
            feedbackText.text = "Corretto!";
            feedbackText.color = Color.green;
            risposteCorrette++;
        }
        else
        {
            feedbackText.text = "Sbagliato!";
            feedbackText.color = Color.red;
        }

        // Mostra il feedback con effetto di fade
        StopAllCoroutines();
        StartCoroutine(FadeInFeedback());

        // Passa alla prossima domanda dopo 1,5 secondi
        indiceDomanda++;
        Invoke(nameof(ShowNextQuestion), 1.5f);
    }

    // Effetto visivo per far comparire dolcemente il testo di feedback
    IEnumerator FadeInFeedback()
    {
        float duration = 0.5f; // Durata dell’effetto
        float elapsed = 0f;

        feedbackText.alpha = 0f;
        feedbackText.transform.localScale = Vector3.one * 0.8f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // Graduale apparizione e ingrandimento del testo
            feedbackText.alpha = Mathf.Lerp(0f, 1f, t);
            feedbackText.transform.localScale = Vector3.Lerp(Vector3.one * 0.8f, Vector3.one, t);

            yield return null;
        }

        feedbackText.alpha = 1f;
        feedbackText.transform.localScale = Vector3.one;
    }
}
