using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuizManagerAspettiSociali : MonoBehaviour
{
    // Riferimenti UI
    public TMP_Text DomandeText; // Testo che mostra la domanda corrente
    public Button[] BottoneRisposta; // Pulsanti delle risposte
    public TMP_Text feedbackText; // Testo di feedback (Corretto/Sbagliato)

    public TextAsset txtFile; // File di testo con le domande (una per riga, campi separati da ';')

    // Stato del quiz
    private List<Question> Domande = new List<Question>(); // Lista domande caricate
    private Question domandaCorrente; // Domanda attualmente mostrata
    private int indiceDomanda = 0; // Indice della domanda corrente
    private int risposteCorrette = 0; // Contatore risposte corrette

    void Start()
    {
        // Carica domande dal file
        LoadQuestions();

        // Mescola l'ordine delle domande per varietà di gioco
        Domande = Domande.OrderBy(q => Random.value).ToList();

        // Mostra la prima domanda
        ShowNextQuestion();
    }

    void LoadQuestions()
    {
        // Divide il testo in righe
        string[] lines = txtFile.text.Split('\n');

        foreach (string line in lines)
        {
            // Salta righe vuote o di sola spaziatura
            if (string.IsNullOrWhiteSpace(line))
                continue;

            // Ogni riga: Domanda;Risposta1;Risposta2;...;IndiceCorretta
            string[] parts = line.Trim().Split(';');
            if (parts.Length < 3) // Minimo: 1 domanda, 1 risposta, 1 indice
                continue;

            // Crea la struttura della domanda
            Question q = new Question();
            q.question = parts[0];

            // Le risposte sono tutte le parti tranne la prima (domanda) e l'ultima (indice)
            q.answers = new string[parts.Length - 2];
            for (int i = 1; i < parts.Length - 1; i++)
            {
                q.answers[i - 1] = parts[i];
            }

            // L'ultima parte è l'indice della risposta corretta
            int.TryParse(parts[parts.Length - 1], out q.correctIndex);

            // Aggiungi alla lista
            Domande.Add(q);
        }
    }

    async void ShowNextQuestion()
    {
        // Se abbiamo finito le domande → schermata di fine e cambio scena
        if (indiceDomanda >= Domande.Count)
        {
            DomandeText.text =
                $"Quiz completato!\nHai risposto correttamente a {risposteCorrette} su {Domande.Count}.";
            feedbackText.text = "";

            // Nasconde i bottoni delle risposte
            foreach (var btn in BottoneRisposta)
                btn.gameObject.SetActive(false);

            // Piccola attesa di 2s, poi cambio scena
            await Task.Delay(2000);
            Debug.LogError("METTETE IL NOME DELLA SCENA IN CHANGE SCENE SOTTO QUESTO ERROR LOG");
            Debug.Log("sono qui;");
            GameManager.Instance.ChangeScene("SalutiScene");
            Debug.Log("non sono qui;");
            return;
        }

        // Prende la domanda corrente e la mostra
        domandaCorrente = Domande[indiceDomanda];
        DomandeText.text = domandaCorrente.question;

        // Mescola le risposte mantenendo traccia di quella corretta
        int correctIndexOriginale = domandaCorrente.correctIndex;
        string[] risposteOriginali = domandaCorrente.answers;

        // Costruisce lista indici [0..n) e li mescola manualmente
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

        // Popola i bottoni con le risposte (mostra solo quelli necessari)
        for (int i = 0; i < BottoneRisposta.Length; i++)
        {
            if (i < risposteOriginali.Length)
            {
                BottoneRisposta[i].gameObject.SetActive(true);
                BottoneRisposta[i].interactable = true;

                // Imposta il testo della risposta i-esima (mappata tramite indici mescolati)
                BottoneRisposta[i].GetComponentInChildren<TMP_Text>().text = risposteOriginali[
                    indici[i]
                ];

                // Registra il click con il corretto indice catturato
                int indiceSelezionato = i;
                BottoneRisposta[i].onClick.RemoveAllListeners();
                BottoneRisposta[i]
                    .onClick.AddListener(() => CheckAnswer(indiceSelezionato, nuovoIndiceCorretto));
            }
            else
            {
                // Se ci sono meno risposte del numero di bottoni, nasconde quelli in eccesso
                BottoneRisposta[i].gameObject.SetActive(false);
            }
        }

        // Reset del feedback (trasparente e senza testo)
        feedbackText.text = "";
        feedbackText.alpha = 0;
    }

    void CheckAnswer(int selectedIndex, int correctIndex)
    {
        // Disabilita tutti i bottoni per evitare risposte multiple
        foreach (var btn in BottoneRisposta)
            btn.interactable = false;

        // Verifica la risposta selezionata
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

        // Avvia l'animazione del feedback (fade + pop)
        StopAllCoroutines();
        StartCoroutine(FadeInFeedback());

        // Dopo un breve ritardo, passa alla prossima domanda
        indiceDomanda++;
        Invoke(nameof(ShowNextQuestion), 1.5f);
    }

    // Effetto visivo di comparsa per il feedback (fade-in + piccola scalatura)
    IEnumerator FadeInFeedback()
    {
        float duration = 0.5f; // Durata dell'animazione
        float elapsed = 0f;
        feedbackText.alpha = 0f; // Parte trasparente...
        feedbackText.transform.localScale = Vector3.one * 0.8f; // ...e leggermente più piccola

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            // Interpola trasparenza e scala
            feedbackText.alpha = Mathf.Lerp(0f, 1f, t);
            feedbackText.transform.localScale = Vector3.Lerp(Vector3.one * 0.8f, Vector3.one, t);

            yield return null;
        }

        // Assicura i valori finali
        feedbackText.alpha = 1f;
        feedbackText.transform.localScale = Vector3.one;
    }
}
