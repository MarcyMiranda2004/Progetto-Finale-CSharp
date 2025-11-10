using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

public class AnimationManager : MonoBehaviour
{
    [Header("Riferimenti Animator")]
    [SerializeField]
    private Animator tonyAnimator; // Animator collegato al personaggio "Tony"

    [SerializeField]
    private Animator baloonAnimator; // Animator collegato al fumetto / pannello del dialogo

    // Esegue l'animazione di ingresso di Tony da sopra verso il basso
    public IEnumerator PlayTonySlideIn()
    {
        // Controlla che l’Animator sia assegnato
        if (tonyAnimator == null)
        {
            Debug.LogWarning("Tony Animator non assegnato all'AnimationManager!");
            yield break; // Interrompe la coroutine se manca il riferimento
        }

        // Riproduce l’animazione chiamata “TonySlideInToBottom”
        tonyAnimator.Play("TonySlideInToBottom");

        // Attende la fine dell’animazione (circa 1 secondo)
        yield return new WaitForSeconds(1f);
    }

    // Esegue l'animazione di ingresso del fumetto / pannello di dialogo
    public IEnumerator PlayBaloonSlideIn()
    {
        // Controlla che l’Animator del balloon sia assegnato
        if (baloonAnimator == null)
        {
            Debug.LogWarning("Baloon Animator non assegnato all'AnimationManager!");
            yield break;
        }

        // Riproduce l’animazione chiamata “DialogTextSlideInToUp”
        baloonAnimator.Play("DialogTextSlideInToUp");

        // Aspetta mezzo secondo (durata stimata dell’animazione)
        yield return new WaitForSeconds(0.5f);
    }

    // Esegue l'animazione di uscita di Tony (esce dallo schermo)
    public IEnumerator PlayTonyExit()
    {
        if (tonyAnimator == null)
            yield break;

        // Riproduce l’animazione di uscita
        tonyAnimator.Play("TonySlideOut");

        // Attende 1 secondo per completare la transizione
        yield return new WaitForSeconds(1f);
    }

    // Esegue l'animazione di uscita del balloon (pannello di dialogo)
    public IEnumerator PlayBaloonExit()
    {
        if (baloonAnimator == null)
            yield break;

        // Riproduce l’animazione “DialogPanelSlideOut”
        baloonAnimator.Play("DialogPanelSlideOut");

        // Attende la durata stimata dell’animazione (1 secondo)
        yield return new WaitForSeconds(1f);
    }

    // Metodo generico per riprodurre un'animazione su un Animator qualunque
    public void PlayCustomAnimation(Animator animator, string animationName)
    {
        // Se manca l’animator o il nome dell’animazione, non fa nulla
        if (animator == null || string.IsNullOrEmpty(animationName))
            return;

        // Esegue l’animazione specificata
        animator.Play(animationName);
    }
}
