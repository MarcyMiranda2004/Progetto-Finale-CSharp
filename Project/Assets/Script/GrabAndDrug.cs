using UnityEngine;

public class GrabAndDrag : MonoBehaviour
{
    // Stato del trascinamento
    private bool isDragging = false; // True mentre l'oggetto è tenuto col mouse
    private bool attached = false; // True quando l’oggetto è stato “attaccato” a una zona valida
    private Vector3 offset; // Offset tra il punto cliccato e il centro dell’oggetto

    // Componenti e riferimenti
    private Rigidbody2D rb; // RigidBody2D per la fisica
    private FoodType foodType; // Tipo di cibo (usato per verificare compatibilità con la zona)
    private Attach currentZone; // Zona attuale a cui l’oggetto è attaccato
    private PyramidManager manager; // Riferimento al gestore generale della piramide

    // Stato di attacco corretto (visibile da altri script)
    [HideInInspector]
    public bool IsCorrectlyAttached = false; // Indica se l'oggetto è nella posizione corretta

    // Componenti grafici
    private SpriteRenderer sr; // Renderer dello sprite
    private Color originalColor; // Colore originale per feedback visivi

    private void Start()
    {
        // Recupera i componenti richiesti
        rb = GetComponent<Rigidbody2D>();
        foodType = GetComponent<FoodType>();
        manager = FindFirstObjectByType<PyramidManager>();

        // Copia il materiale per evitare che cambi colore anche sugli altri oggetti che condividono lo stesso
        sr = GetComponent<SpriteRenderer>();
        sr.material = new Material(sr.material);
        originalColor = sr.color;
    }

    // Quando l'utente clicca sull’oggetto con il mouse
    void OnMouseDown()
    {
        // Se è già attaccato a una zona, non può essere trascinato
        if (attached)
            return;

        isDragging = true;
        // Calcola l’offset per mantenere la distanza corretta durante il drag
        offset = transform.position - GetMouseWorldPos();
    }

    // Mentre l’utente tiene premuto il mouse e trascina
    void OnMouseDrag()
    {
        // Se non sta trascinando o è attaccato → non fa nulla
        if (!isDragging || attached)
            return;

        // Aggiorna la posizione seguendo il mouse (considerando l’offset)
        transform.position = GetMouseWorldPos() + offset;
    }

    // Quando il tasto del mouse viene rilasciato
    void OnMouseUp()
    {
        if (attached)
            return;

        // Termina il trascinamento
        isDragging = false;
    }

    // Converte la posizione del mouse in coordinate del mondo 2D
    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        // Calcola la distanza z in base alla camera
        mousePoint.z = Mathf.Abs(Camera.main.transform.position.z - transform.position.z);
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    // Quando entra in contatto con un collider "Attach"
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (attached)
            return;

        // Controlla se il collider ha un componente Attach
        Attach zone = other.GetComponent<Attach>();
        if (zone != null)
        {
            // Se la zona può accettare questo tipo di cibo
            if (zone.CanAccept(foodType))
            {
                // Posiziona perfettamente al centro della zona
                transform.position = zone.transform.position;

                // Blocca la fisica (non più influenzato da gravità o forze)
                rb.bodyType = RigidbodyType2D.Static;

                // Imposta stato di attacco
                attached = true;
                IsCorrectlyAttached = true;

                // Registra la zona corrente
                currentZone = zone;
                currentZone.SetCurrentFood(this);

                // Avvisa il manager per controllare se la piramide è completa
                if (manager != null)
                    manager.CheckCompletion();
            }
            else
            {
                // Se è stato messo nella zona sbagliata → feedback visivo (colore rosso)
                StartCoroutine(WrongPlacementFeedback());
            }
        }
    }

    //  Metodo pubblico per staccare manualmente l’oggetto
    public void Detach()
    {
        if (attached)
        {
            attached = false;
            IsCorrectlyAttached = false;

            // Rende di nuovo dinamico l’oggetto
            rb.bodyType = RigidbodyType2D.Dynamic;

            // Libera la zona a cui era attaccato
            if (currentZone != null)
            {
                currentZone.ClearCurrentFood();
                currentZone = null;
            }
        }
    }

    //  Feedback visivo per il posizionamento errato
    private System.Collections.IEnumerator WrongPlacementFeedback()
    {
        // Cambia temporaneamente il colore in rosso
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        Color original = sr.color;
        sr.color = Color.red;

        // Attende brevemente e poi ripristina il colore originale
        yield return new WaitForSeconds(0.3f);
        sr.color = originalColor;
    }
}
