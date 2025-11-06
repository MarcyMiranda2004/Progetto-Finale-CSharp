using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PresentationSceneManager : MonoBehaviour
{
    private PresentationSceneManager _instance;
    public PresentationSceneManager Instance { get => _instance; }

    [Header("Tony Slide In Reference")]
    [SerializeField] private Animator tonySprite;

    [Header("Baloon 1")]
    [SerializeField] private Animator baloonSprite1;
    [SerializeField] private TextMeshProUGUI dialogoText1;
    [SerializeField] private TextAsset dialogueFile;
    [SerializeField] private TypewriterEffect twE;

    [Header("Buttons")]
    [SerializeField] private Button nextBtn;
    private bool dialogueStarted = false;

    void Start()
    {
        nextBtn.gameObject.SetActive(false);

        Time.timeScale = 1f;
        dialogoText1.text = "";

        StartCoroutine(TonySlideIn());
        StartCoroutine(BaloonSlideIn());

        nextBtn.onClick.AddListener(() => ChangingScene("EquilibrioAlimentareScene"));
    }

    void Update()
    {
        if (twE == null) return;

        if (Input.GetMouseButtonDown(0) && twE.IsTyping()) twE.SkipTypingAnimation();

        if (dialogueStarted && !twE.IsTyping()) nextBtn.gameObject.SetActive(true);
    }

    IEnumerator TonySlideIn()
    {
        tonySprite.Play("TonySlideInToBottom");
        yield return new WaitForSeconds(1f);
    }

    IEnumerator BaloonSlideIn()
    {
        baloonSprite1.Play("DialogTextSlideInToUp");
        yield return new WaitForSeconds(0.5f);

        LoadAndShowDialogue();
    }

    private void LoadAndShowDialogue()
    {
        if (dialogueFile == null)
        {
            Debug.LogError("Nessun file di dialogo collegato!");
            return;
        }

        if (twE == null)
        {
            Debug.LogError("TypewriterEffect non assegnato al Manager!");
            return;
        }

        nextBtn.gameObject.SetActive(false);
        dialogueStarted = true;
        twE.ShowText(dialogueFile.text);
    }

    void ChangingScene(string sceneName)
    {
        GameManager.Instance.ChangeScene(sceneName);
    }
}
