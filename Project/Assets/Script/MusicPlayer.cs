using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MusicPlayer : MonoBehaviour
{
    // Istanza unica (singleton) del MusicPlayer
    // Permette di avere un solo player persistente tra le scene
    public static MusicPlayer Instance;

    // Riferimento all'AudioSource che riproduce la musica
    public AudioSource audioSource;

    // Elenco delle tracce musicali disponibili
    public AudioClip[] tracks;

    // Riferimenti ai controlli UI (opzionali)
    [Header("UI Controls (optional)")]
    public Button playPauseButton;
    public Button nextButton;
    public Button muteButton;
    public Slider volumeSlider;
    public Text trackNameText;

    // Indice della traccia attualmente in riproduzione
    private int currentTrack = 0;

    // Stato di "mute" (vero = silenziato)
    private bool isMuted = false;

    // Metodo Awake()
    // Eseguito prima di Start(): gestisce la persistenza e il singleton
    void Awake()
    {
        // Se esiste già un'altra istanza del MusicPlayer, distrugge il duplicato
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        // Imposta questa come istanza principale
        Instance = this;

        // Mantiene l'oggetto tra le scene (non viene distrutto)
        DontDestroyOnLoad(gameObject);
    }

    // Metodo Start()
    // Inizializza il player e collega la UI
    void Start()
    {
        // Controlla che ci siano tracce assegnate
        if (tracks == null || tracks.Length == 0)
        {
            Debug.LogWarning("MusicPlayer: nessuna traccia assegnata!");
            return;
        }

        // Se il volume è 0, imposta un valore di default (0.5)
        if (audioSource.volume == 0)
            audioSource.volume = 0.5f;

        // Se esiste lo slider, sincronizzalo con il volume corrente
        if (volumeSlider != null)
            volumeSlider.value = audioSource.volume;

        // Imposta e avvia la prima traccia
        audioSource.clip = tracks[currentTrack];
        audioSource.Play();

        // Aggiorna il nome della traccia nella UI
        UpdateTrackName();

        // Collega i pulsanti UI, se presenti nella scena
        if (playPauseButton != null)
            playPauseButton.onClick.AddListener(TogglePlayPause);

        if (nextButton != null)
            nextButton.onClick.AddListener(NextTrack);

        if (muteButton != null)
            muteButton.onClick.AddListener(ToggleMute);

        if (volumeSlider != null)
            volumeSlider.onValueChanged.AddListener(ChangeVolume);
    }

    // Metodo TogglePlayPause()
    // Gestisce la riproduzione o la pausa del brano corrente
    void TogglePlayPause()
    {
        if (audioSource == null)
            return;

        if (audioSource.isPlaying)
            audioSource.Pause();
        else
            audioSource.Play();
    }

    // Metodo NextTrack()
    // Passa alla traccia successiva con un effetto di fade
    void NextTrack()
    {
        if (tracks == null || tracks.Length == 0)
            return;

        currentTrack = (currentTrack + 1) % tracks.Length;
        StartCoroutine(FadeOutIn(tracks[currentTrack], 1f)); // 1 secondo di transizione
    }

    // Metodo ToggleMute()
    // Attiva o disattiva il mute del suono
    void ToggleMute()
    {
        if (audioSource == null)
            return;

        isMuted = !isMuted;
        audioSource.mute = isMuted;
    }

    // Metodo ChangeVolume()
    // Aggiorna il volume in base al valore dello slider
    void ChangeVolume(float value)
    {
        if (audioSource == null)
            return;
        audioSource.volume = value;
    }

    // Metodo UpdateTrackName()
    // Aggiorna il testo del nome brano nella UI
    void UpdateTrackName()
    {
        if (trackNameText != null && tracks != null && tracks.Length > 0)
            trackNameText.text = "Now Playing: " + tracks[currentTrack].name;
    }

    // Coroutine FadeOutIn()
    // Effettua una transizione morbida tra due brani (fade-out e fade-in)
    IEnumerator FadeOutIn(AudioClip newClip, float fadeTime)
    {
        if (audioSource == null || newClip == null)
            yield break;

        float startVolume = audioSource.volume;

        // Fade-out: riduce progressivamente il volume
        for (float v = startVolume; v > 0; v -= Time.deltaTime / fadeTime)
        {
            audioSource.volume = v;
            yield return null;
        }

        // Cambia clip e riproduce la nuova traccia
        audioSource.clip = newClip;
        audioSource.Play();
        UpdateTrackName();

        // Fade-in: aumenta gradualmente il volume al valore target
        float targetVolume = (volumeSlider != null) ? volumeSlider.value : startVolume;
        for (float v = 0; v < targetVolume; v += Time.deltaTime / fadeTime)
        {
            audioSource.volume = v;
            yield return null;
        }

        // Assicura che il volume finale sia preciso
        audioSource.volume = targetVolume;
    }
}
