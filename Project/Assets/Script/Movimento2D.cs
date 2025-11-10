using System.Collections;
using UnityEngine;

public class Movimento2D : MonoBehaviour
{
    // Velocità di movimento orizzontale del personaggio
    public float moveSpeed = 40f;

    // Valore dell’input orizzontale (da tastiera o controller)
    private float moveInput;

    // Moltiplicatore per invertire i controlli (1 = normale, -1 = invertito)
    private float directionMultiplier = 1f;

    // Componenti di riferimento
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Animator animator;

    // Riferimento alla camera principale per calcolare i limiti di movimento
    public Camera mainCamera;

    // Metà larghezza e altezza della visuale, per limitare il movimento
    private float halfWidth;
    private float halfHeight;

    // Hash per i parametri dell’Animator (ottimizza le chiamate)
    private readonly int isWalkingHash = Animator.StringToHash("isWalking");
    private readonly int isStoppingHash = Animator.StringToHash("isStopping");

    void Start()
    {
        // Recupera i componenti principali
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        // Calcola le dimensioni visibili della camera per bloccare il movimento ai bordi
        halfHeight = mainCamera.orthographicSize;
        halfWidth = halfHeight * mainCamera.aspect;
    }

    void Update()
    {
        // Se il gioco è in pausa (Time.timeScale = 0), non gestisce input né animazioni
        if (Time.timeScale == 0f)
            return;

        // Ottiene l’input orizzontale (A/D, Frecce o stick) e applica l’eventuale inversione
        moveInput = Input.GetAxisRaw("Horizontal") * directionMultiplier;

        // Gestisce la direzione dello sprite (flip a sinistra o destra)
        if (moveInput > 0)
            spriteRenderer.flipX = false; // Verso destra
        else if (moveInput < 0)
            spriteRenderer.flipX = true; // Verso sinistra

        // Aggiorna le animazioni del personaggio
        HandleAnimations();
    }

    void FixedUpdate()
    {
        // Applica il movimento fisico al Rigidbody2D
        // Usa linearVelocity (nuova API Unity Physics 2D)
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    private void HandleAnimations()
    {
        // Se c’è input di movimento → personaggio sta camminando
        if (Mathf.Abs(moveInput) > 0.01f)
        {
            animator.ResetTrigger(isStoppingHash); // Resetta eventuale trigger di stop
            animator.SetBool(isWalkingHash, true); // Imposta lo stato "camminando"
        }
        else
        {
            // Se era in movimento e si ferma → attiva animazione di stop
            if (animator.GetBool(isWalkingHash))
                animator.SetTrigger(isStoppingHash);

            animator.SetBool(isWalkingHash, false); // Ferma l’animazione di cammino
        }

        //  Limita la posizione del personaggio ai bordi dello schermo
        Vector3 pos = transform.position;
        pos.x = Mathf.Clamp(
            pos.x,
            mainCamera.transform.position.x - halfWidth + 0.5f, // Limite sinistro
            mainCamera.transform.position.x + halfWidth - 0.5f // Limite destro
        );
        transform.position = pos;
    }

    //  Metodo pubblico per invertire temporaneamente i controlli orizzontali
    public void InvertControls(float duration)
    {
        // Ferma eventuali inversioni già in corso e ne avvia una nuova
        StopAllCoroutines();
        StartCoroutine(InvertTemporarily(duration));
    }

    // Coroutine che inverte i controlli per un certo periodo
    private IEnumerator InvertTemporarily(float duration)
    {
        directionMultiplier = -1f; // Inverte i controlli
        Debug.Log("Controlli invertiti per " + duration + " secondi!");

        // Aspetta il tempo specificato
        yield return new WaitForSeconds(duration);

        // Ripristina i controlli normali
        directionMultiplier = 1f;
        Debug.Log("Controlli tornati normali!");
    }
}
