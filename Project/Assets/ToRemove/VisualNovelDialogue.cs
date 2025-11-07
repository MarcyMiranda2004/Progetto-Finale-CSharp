using UnityEngine;
using TMPro;
public class VisualNovelDialogue : MonoBehaviour
{
    [SerializeField] private GameObject dialogPanel;

    [SerializeField] private TextMeshProUGUI dialogText;

    private string[] lines;
    private int currentLineIndex = 0;
    private bool isActive = false;

    void Start()
    {
        //PASTE HERE THE DIALOGUE TO SHOW
        lines = new string[] {
            "Un'adeguata alimentazione è fondamentale per il benessere fisico, mentale e sociale, rappresentando un diritto umano fondamentale.",
            "Una dieta equilibrata, che include frutta, verdura, cereali integrali, proteine magre e grassi salutari, contribuisce alla prevenzione di numerose patologie croniche come tumori, malattie cardiovascolari e disturbi muscolo-scheletrici",
            "Un’alimentazione equilibrata significa mangiare in modo vario e proporzionato",
            "fornendo all’organismo tutti i nutrienti necessari (carboidrati, proteine, grassi, vitamine, sali minerali e acqua) nelle giuste quantità."
        };

        StartDialogue();
    }

    void StartDialogue()
    {
        dialogPanel.SetActive(true);
        isActive = true;
        currentLineIndex = 0;
        ShowLine();
    }

    void Update()
    {
        if (isActive && Input.GetMouseButtonDown(0))
        {
            NextLine();
        }
    }
/// <summary>
/// This method allow to show the sentences into the panel. 
/// </summary>
    void ShowLine()
    {
        dialogText.text = lines[currentLineIndex];
    }
/// <summary>
/// Use this method to increment the value of the index.
/// </summary> 
    void NextLine()
    {
        currentLineIndex++;

        if (currentLineIndex >= lines.Length)
        {
            EndDialogue();
        }
        else
        {
            ShowLine();
        }
    }

    void EndDialogue()
    {
        dialogPanel.SetActive(false);
        isActive = false;
        Debug.Log("Dialogo terminato!");
    }
}
