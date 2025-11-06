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
    [SerializeField] private float typeSpeed = 0.03f;

    private string fullText;
    private Coroutine typingCoroutine;
    private bool isTyping;
    private bool skipRequested;

    private void Awake()
    {
        if (dialogoText1 == null) dialogoText1 = GetComponent<TextMeshProUGUI>();

        if (dialogoText1 == null) Debug.LogError("Nessun TextMeshProUGUI assegnato al TypewriterEffect!");
        else dialogoText1.text = "";
    }

    public void ShowText(string textToShow)
    {
        fullText = textToShow;

        if (typingCoroutine != null) StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TypeText());
    }

    private IEnumerator TypeText()
    {
        if (dialogoText1 == null) yield break;

        isTyping = true;
        skipRequested = false;

        var sb = new StringBuilder();
        dialogoText1.text = "";

        foreach (char c in fullText)
        {
            if (skipRequested)
            {
                dialogoText1.text = fullText;
                break;
            }

            sb.Append(c);
            dialogoText1.text = sb.ToString();

            while (Time.timeScale == 0f) yield return null;

            yield return new WaitForSeconds(typeSpeed);
        }

        isTyping = false;
        skipRequested = false;
    }

    public void SkipTypingAnimation()
    {
        if (isTyping) skipRequested = true;
    }

    public bool IsTyping()
    {
        return isTyping;
    }
}