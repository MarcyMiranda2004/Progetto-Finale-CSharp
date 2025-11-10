using UnityEngine;

public class PropLife : MonoBehaviour
{
    // Durata di vita dell’oggetto (in secondi)
    // Dopo questo tempo, il GameObject verrà distrutto automaticamente
    public float lifetime = 5f;

    void Start()
    {
        // Distrugge automaticamente questo GameObject dopo "lifetime" secondi
        // Unity gestisce internamente un timer e chiama Destroy() quando scade
        Destroy(gameObject, lifetime);
    }
}
