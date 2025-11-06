using System.Collections;
using TMPro;
using UnityEngine;

public class PresentationSceneManager : MonoBehaviour
{
    private PresentationSceneManager _instance;
    public PresentationSceneManager Instance { get => _instance; }

    [Header("Tony Slide In Reference")]
    [SerializeField] private Animator tonySprite;

    [Header("Baloon 1")]
    [SerializeField] private Animator baloonSprite1;
    [SerializeField] private TextMeshProUGUI dialogoText1;

    void Start()
    {
        StartCoroutine(TonySlideIn());
        StartCoroutine(BaloonSlideIn());
    }

    IEnumerator TonySlideIn()
    {
        tonySprite.Play("TonySlideInToBottom");
        yield return new WaitForSeconds(1f);
    }

    IEnumerator BaloonSlideIn()
    {
        baloonSprite1.Play("DialogTextSlideInToUp");
        yield return new WaitForSeconds(2f);
    }
}
