using System.Collections;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    [Header("Riferimenti Animator")]
    [SerializeField] private Animator tonyAnimator;
    [SerializeField] private Animator baloonAnimator;

    public IEnumerator PlayTonySlideIn()
    {
        if (tonyAnimator == null)
        {
            Debug.LogWarning("Tony Animator non assegnato all'AnimationManager!");
            yield break;
        }

        tonyAnimator.Play("TonySlideInToBottom");
        yield return new WaitForSeconds(1f);
    }

    public IEnumerator PlayBaloonSlideIn()
    {
        if (baloonAnimator == null)
        {
            Debug.LogWarning("Baloon Animator non assegnato all'AnimationManager!");
            yield break;
        }

        baloonAnimator.Play("DialogTextSlideInToUp");
        yield return new WaitForSeconds(0.5f);
    }

    public IEnumerator PlayTonyExit()
    {
        if (tonyAnimator == null) yield break;
        tonyAnimator.Play("TonySlideOut");
        yield return new WaitForSeconds(1f);
    }

    public IEnumerator PlayBaloonExit()
    {
        if (baloonAnimator == null) yield break;
        baloonAnimator.Play("DialogPanelSlideOut");
        yield return new WaitForSeconds(1f);
    }

    public void PlayCustomAnimation(Animator animator, string animationName)
    {
        if (animator == null || string.IsNullOrEmpty(animationName)) return;
        animator.Play(animationName);
    }
}
