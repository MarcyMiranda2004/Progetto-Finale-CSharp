using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    // Elenco di prefab che verranno generati (da assegnare nel pannello Inspector)
    public GameObject[] oggettiDaSpawnare;

    // Intervallo di tempo (in secondi) tra una generazione e l’altra
    public float intervalloSpawn = 3f;

    // Timer interno per misurare il tempo trascorso dall’ultimo spawn
    private float timer;

    // Area di spawn definita da un Collider2D (ad esempio un BoxCollider2D invisibile nella scena)
    public Collider2D areaSpawn;

    void Update()
    {
        // Incrementa il timer ad ogni frame
        timer += Time.deltaTime;

        // Quando il timer supera l’intervallo impostato...
        if (timer >= intervalloSpawn)
        {
            // ... genera un nuovo oggetto
            SpawnOggetto();

            // e azzera il timer
            timer = 0f;
        }
    }

    // Metodo che si occupa di generare un nuovo oggetto casuale
    void SpawnOggetto()
    {
        // Controlla che ci siano prefab assegnati e che l’area di spawn esista
        if (oggettiDaSpawnare.Length == 0 || areaSpawn == null)
        {
            Debug.LogWarning("Prefab o area di spawn non assegnati!");
            return;
        }

        // Ottiene i limiti del Collider (bounding box)
        Bounds bounds = areaSpawn.bounds;

        // Genera una posizione casuale all’interno dei limiti
        float randomX = Random.Range(bounds.min.x, bounds.max.x);
        float randomY = Random.Range(bounds.min.y, bounds.max.y);
        Vector2 posizioneCasuale = new Vector2(randomX, randomY);

        // Sceglie casualmente un prefab dall’array
        int index = Random.Range(0, oggettiDaSpawnare.Length);

        // Istanzia il prefab nella posizione generata, con rotazione neutra
        Instantiate(oggettiDaSpawnare[index], posizioneCasuale, Quaternion.identity);
    }
}
