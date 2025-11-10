using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PresentationSceneManager : MonoBehaviour
{
    private PresentationSceneManager _instance;
    public PresentationSceneManager Instance
    {
        get => _instance;
    }

    [Header("Tony Slide In Reference")]
    [SerializeField]
    private Animator tonySprite;

    [Header("Baloon 1")]
    [SerializeField]
    private Animator baloonSprite1;

    [SerializeField]
    private TextMeshProUGUI dialogoText1; // definiamo il o i file di testo da usare

    [SerializeField]
    private TextAsset dialogueFile;

    [SerializeField]
    private TypewriterEffect twE;

    [Header("Buttons")]
    [SerializeField]
    private Button nextBtn;
    private bool dialogueStarted = false;

    void Start()
    {
        nextBtn.gameObject.SetActive(false); // rende il bottone con la scritta "Andiamo! >>" invisibile

        Time.timeScale = 1f; // si assicura che la scala di tempo sia ad 1
        dialogoText1.text = ""; // svuota il text mesh in modo che alla partenza sia vuoto

        StartCoroutine(TonySlideIn()); // avvia l'animazione di tony che appare dal basso
        StartCoroutine(BaloonSlideIn()); // avvia l'animazione del baloon che appare dall'alto

        nextBtn.onClick.AddListener(() => ChangingScene("AlimentazioneSalutareScene"));
    }

    void Update()
    {
        if (twE == null)
            return; // controlla la presenza di un TypewriterEffect

        if (Input.GetMouseButtonDown(0) && twE.IsTyping())
            twE.SkipTypingAnimation(); // se clicchiamo sulla canvas skippa l'animazione di scrittura

        if (dialogueStarted && !twE.IsTyping())
            nextBtn.gameObject.SetActive(true); // attiva il bottone "Andiamo! >>" solo quando il testo è completo (dialogueStarted == true && twE.IsType == false)
    }

    IEnumerator TonySlideIn()
    {
        tonySprite.Play("TonySlideInToBottom"); // fa partire l'animazione del png di tony
        yield return new WaitForSeconds(1f); // aspetta 1s prima di fare altro
    }

    IEnumerator BaloonSlideIn()
    {
        baloonSprite1.Play("DialogTextSlideInToUp");
        yield return new WaitForSeconds(0.5f); // aspetta 0.5s prima di fare altro

        LoadAndShowDialogue(); // avvia il caricamento del testo dal file
    }

    private void LoadAndShowDialogue()
    {
        if (dialogueFile == null) // controlla se il dialogueFile
        {
            Debug.LogError("Nessun file di dialogo collegato!");
            return;
        }

        if (twE == null) // controlla se il twE funziona
        {
            Debug.LogError("TypewriterEffect non assegnato al Manager!");
            return;
        }

        nextBtn.gameObject.SetActive(false); // rende attivo e visibile il bottone "Andiamo! >>"
        dialogueStarted = true; // rende attivo il button
        twE.ShowText(dialogueFile.text); // attiva la scrittura del testo in twE | prende il testo contenuto nel file txt e lo manda come stringa al metodo ShowText di TypewriterEffect
    }

    void ChangingScene(string sceneName) // cambia scena
    {
        GameManager.Instance.ChangeScene(sceneName);
    }
}
