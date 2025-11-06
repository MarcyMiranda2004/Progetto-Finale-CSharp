using System.Collections;
using System.Text;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class TypewriterEffect : MonoBehaviour
{
    [Header("Riferimenti UI")]
    [SerializeField] private TextMeshProUGUI dialogoText1;

    [Header("Variabili")]
    [SerializeField] private float typeSpeed = 0.05f;

    private string fullText;
    private Coroutine typingCoroutine;

    private void Awake()
    {
        if (dialogoText1 == null)
        {
            dialogoText1 = GetComponent<TextMeshProUGUI>();
        }
    }

    public void ShowText(string textToShow)
    {
        fullText = textToShow;

        if (typingCoroutine != null) StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        var sb = new StringBuilder();
        dialogoText1.text = "";

        foreach (char c in fullText)
        {
            sb.Append(c);
            dialogoText1.text = sb.ToString();
            yield return new WaitForSecondsRealtime(typeSpeed);
        }
    }

    void Start()
    {
        ShowText(" Ciao! Io sono lo Chef Tony l'Healthy Chef e tu devi essere il mio nuovo commis, oggi ti insegnero le basi di una corretta alimentazione qui nella mia cucina. Dai Seguimi!");
    }
}