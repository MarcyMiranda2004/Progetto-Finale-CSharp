using System.Collections;
using TMPro;
using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public static MusicPlayer Instance; // Singleton: accesso globale all'istanza del MusicPlayer

    [Header("Audio")]
    public AudioSource audioSource; // Componente AudioSource che riproduce le tracce
    public AudioClip[] tracks; // Lista delle tracce musicali disponibili

    private int currentTrack = 0; // Indice della traccia attualmente in riproduzione
    private bool isPaused = false; // Indica se la riproduzione è in pausa
    private bool isMuted = false; // Indica se l’audio è in stato di muto
    private bool isFading = false; // Indica se è in corso un effetto di fade-in/out
    private float userVolume = 0.2f; // Volume preferito dell’utente (salvato nei PlayerPrefs)

    void Awake()
    {
        // Implementazione del pattern Singleton: assicura che esista una sola istanza del MusicPlayer
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Distrugge eventuali duplicati
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Mantiene l’oggetto tra i caricamenti di scena

        // Assicura che esista un componente AudioSource
        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
                audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Permette di continuare a riprodurre anche se il gioco è messo in pausa
        audioSource.ignoreListenerPause = true;

        // Recupera il volume salvato in precedenza (default: 0.2)
        float savedVolume = PlayerPrefs.GetFloat("MusicVolume", 0.2f);
        audioSource.volume = savedVolume;
        userVolume = savedVolume;

        // Impostazioni di base dell’AudioSource
        audioSource.loop = false; // Non loopa automaticamente (passa al prossimo brano da codice)
        audioSource.playOnAwake = false; // Non suona subito all’avvio
    }

    void Start()
    {
        // Se non ci sono tracce, avvisa e interrompi
        if (tracks == null || tracks.Length == 0)
        {
            Debug.LogWarning("MusicPlayer: nessuna traccia trovata!");
            return;
        }

        // Imposta la prima traccia e avvia la riproduzione
        audioSource.clip = tracks[currentTrack];
        audioSource.Play();

        // Applica un fade-in all’inizio per un effetto più morbido
        StartCoroutine(FadeIn(audioSource, userVolume, 1f));
    }

    void Update()
    {
        if (audioSource == null)
            return;

        // Quando la traccia termina naturalmente e non è in pausa, passa automaticamente alla successiva
        if (
            !audioSource.isPlaying
            && !isPaused
            && !isMuted
            && audioSource.clip != null
            && !isFading
        )
        {
            NextTrack();
        }
    }

    public void TogglePlayPause()
    {
        if (audioSource == null)
            return;

        // Alterna tra pausa e riproduzione
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
            isPaused = true;
        }
        else
        {
            audioSource.Play();
            isPaused = false;
        }
    }

    public bool IsPlaying()
    {
        // Ritorna true se l’audio è in riproduzione
        return audioSource != null && audioSource.isPlaying;
    }

    public void NextTrack()
    {
        if (tracks == null || tracks.Length == 0 || audioSource == null)
            return;

        // Interrompe eventuali fade in corso per evitare conflitti
        StopAllCoroutines();
        isFading = false;

        // Incrementa l’indice e torna all’inizio se supera il numero di tracce
        currentTrack = (currentTrack + 1) % tracks.Length;

        Debug.Log($"▶ NextTrack(): {tracks[currentTrack].name}");

        // Esegue un fade-out della traccia attuale e un fade-in della successiva
        StartCoroutine(FadeOutIn(tracks[currentTrack], 0.6f));
    }

    public void PreviousTrack()
    {
        if (tracks == null || tracks.Length == 0 || audioSource == null)
            return;

        StopAllCoroutines();
        isFading = false;

        // Decrementa l’indice e torna all’ultima traccia se va sotto zero
        currentTrack = (currentTrack - 1 + tracks.Length) % tracks.Length;

        Debug.Log($"⏮ PreviousTrack(): {tracks[currentTrack].name}");

        // Esegue il cambio brano con effetto di transizione
        StartCoroutine(FadeOutIn(tracks[currentTrack], 0.6f));
    }

    public void ToggleMute()
    {
        if (audioSource == null)
            return;

        // Alterna tra muto e non muto
        isMuted = !isMuted;
        audioSource.mute = isMuted;
    }

    public void ChangeVolume(float value)
    {
        if (audioSource == null)
            return;

        // Clampa il valore tra 0 e 1 e aggiorna il volume
        userVolume = Mathf.Clamp01(value);
        audioSource.volume = userVolume;

        // Salva la preferenza dell’utente
        PlayerPrefs.SetFloat("MusicVolume", userVolume);
        PlayerPrefs.Save();
    }

    public string GetCurrentTrackName()
    {
        // Ritorna il nome della traccia attuale o "(none)" se non disponibile
        return audioSource != null && audioSource.clip != null ? audioSource.clip.name : "(none)";
    }

    IEnumerator FadeOutIn(AudioClip newClip, float duration)
    {
        if (audioSource == null || newClip == null)
            yield break;

        isFading = true;
        float startVol = audioSource.volume;

        // FADE OUT — indipendente dal TimeScale
        float t = 0f;
        while (t < duration * 0.5f)
        {
            if (audioSource == null)
                yield break;

            t += Time.unscaledDeltaTime; // uso di unscaledDeltaTime per funzionare anche in pausa
            audioSource.volume = Mathf.Lerp(startVol, 0f, t / (duration * 0.5f));
            yield return null;
        }

        // Cambio clip e riproduzione
        audioSource.clip = newClip;
        audioSource.time = 0f;
        audioSource.Play();

        // FADE IN
        t = 0f;
        while (t < duration * 0.5f)
        {
            if (audioSource == null)
                yield break;

            t += Time.unscaledDeltaTime;
            audioSource.volume = Mathf.Lerp(0f, userVolume, t / (duration * 0.5f));
            yield return null;
        }

        audioSource.volume = userVolume;
        isFading = false;
    }

    IEnumerator FadeIn(AudioSource source, float targetVolume, float duration)
    {
        if (source == null)
            yield break;

        isFading = true;
        source.volume = 0f;
        float t = 0f;

        // Esegue un fade-in graduale indipendente dal TimeScale
        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            source.volume = Mathf.Lerp(0f, userVolume, t / duration);
            yield return null;
        }

        source.volume = userVolume;
        isFading = false;
    }

    public void UpdateTrackNameUI(TMP_Text label)
    {
        if (label == null)
            return;

        // Aggiorna un'etichetta di testo con il nome della traccia corrente
        label.text =
            audioSource != null && audioSource.clip != null
                ? $"Now Playing: {audioSource.clip.name}"
                : "Now Playing: (none)";
    }
}
