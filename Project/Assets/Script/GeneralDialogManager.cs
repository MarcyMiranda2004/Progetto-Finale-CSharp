using System.Collections;
using TMPro;
using UnityEngine;

public class GeneralDialogManager : MonoBehaviour
{
    [Header("Riferimenti")]
    [SerializeField]
    private AnimationManager animationManager;

    [SerializeField]
    private TextMeshProUGUI dialogoText1;

    [SerializeField]
    private TypewriterEffect twE;

    [Header("Dialoghi multipli")]
    [SerializeField]
    private TextAsset[] dialogueFiles;

    [SerializeField]
    private string nextSceneName;

    private int currentFileIndex = 0;
    private string[] currentDialogues;
    private int currentDialogueIndex = 0;
    private bool dialogueStarted = false;
    private bool waitingNextFile = false;

    void Start()
    {
        Time.timeScale = 1f;
        dialogoText1.text = "";

        StartCoroutine(SceneStartSequence());
    }

    IEnumerator SceneStartSequence()
    {
        if (animationManager != null)
        {
            yield return animationManager.PlayTonySlideIn();
            yield return animationManager.PlayBaloonSlideIn();
        }

        LoadNextFile();
    }

    void Update()
    {
        if (!dialogueStarted || twE == null)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            if (twE.IsTyping())
            {
                twE.SkipTypingAnimation();
            }
            else if (waitingNextFile)
            {
                waitingNextFile = false;
                LoadNextFile();
            }
            else
            {
                ShowNextDialogue();
            }
        }
    }

    private void LoadNextFile()
    {
        if (currentFileIndex >= dialogueFiles.Length)
        {
            StartCoroutine(EndSequence());
            return;
        }

        var file = dialogueFiles[currentFileIndex];
        if (file == null)
        {
            Debug.LogError($"File di dialogo {currentFileIndex + 1} non assegnato!");
            currentFileIndex++;
            LoadNextFile();
            return;
        }

        currentDialogues = file.text.Split(
            new string[] { "---" },
            System.StringSplitOptions.RemoveEmptyEntries
        );
        currentDialogueIndex = 0;
        dialogueStarted = true;

        ShowCurrentDialogue();
    }

    private void ShowCurrentDialogue()
    {
        if (currentDialogueIndex < currentDialogues.Length)
        {
            string text = currentDialogues[currentDialogueIndex].Trim();
            twE.ShowText(text);
        }
        else
        {
            currentFileIndex++;
            waitingNextFile = true;
        }
    }

    private void ShowNextDialogue()
    {
        currentDialogueIndex++;
        ShowCurrentDialogue();
    }

    IEnumerator EndSequence()
    {
        if (animationManager != null)
        {
            StartCoroutine(animationManager.PlayTonyExit());
            StartCoroutine(animationManager.PlayBaloonExit());

            yield return new WaitForSeconds(1.5f);
        }

        GameManager.Instance.ChangeScene(nextSceneName);
    }
}
