using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MusicUI : MonoBehaviour
{
    [Header("UI Elements")]
    public Slider volumeSlider; // Riferimento allo slider che controlla il volume della musica
    public Button nextButton; // Bottone per passare alla traccia successiva
    public Button prevButton; // Bottone per tornare alla traccia precedente
    public Button playPauseButton; // Bottone per avviare o mettere in pausa la musica
    public Button muteButton; // Bottone per attivare/disattivare il muto
    public TextMeshProUGUI trackNameText; // Testo che mostra il nome della traccia attualmente in riproduzione

    public Sprite playIcon; // Icona da mostrare quando la musica è ferma (pulsante Play)
    public Sprite pauseIcon; // Icona da mostrare quando la musica è in riproduzione (pulsante Pausa)

    private Image playPauseImage; // Componente Image del bottone play/pause, usato per cambiare l’icona

    void Start()
    {
        // Controlla che il MusicPlayer esista nella scena
        if (MusicPlayer.Instance == null)
        {
            Debug.LogWarning("MusicPlayer non trovato!");
            return; // Interrompe l’esecuzione se non c’è un MusicPlayer
        }

        // Imposta il volume iniziale dallo stato salvato nei PlayerPrefs (default: 0.2)
        volumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.2f);

        // Aggiunge un listener allo slider per aggiornare il volume nel MusicPlayer in tempo reale
        volumeSlider.onValueChanged.AddListener(MusicPlayer.Instance.ChangeVolume);

        // Collega i bottoni ai rispettivi metodi di controllo della musica
        nextButton.onClick.AddListener(OnNext); // Avanti
        prevButton.onClick.AddListener(OnPrev); // Indietro
        muteButton.onClick.AddListener(MusicPlayer.Instance.ToggleMute); // Muto
        playPauseButton.onClick.AddListener(OnPlayPause); // Play/Pausa

        // Recupera il componente Image del pulsante Play/Pausa per poterne cambiare lo sprite
        playPauseImage = playPauseButton.GetComponent<Image>();

        // Aggiorna subito l’icona e il nome del brano al caricamento
        UpdatePlayPauseIcon();
        UpdateTrackName();
    }

    void Update()
    {
        // Aggiorna il nome del brano e l’icona in tempo reale (utile se la traccia cambia automaticamente)
        if (trackNameText != null && MusicPlayer.Instance != null)
            trackNameText.text = $"Now Playing: {MusicPlayer.Instance.GetCurrentTrackName()}";

        UpdatePlayPauseIcon();
    }

    void OnNext()
    {
        // Passa alla traccia successiva e aggiorna il testo
        MusicPlayer.Instance.NextTrack();
        UpdateTrackName();
    }

    void OnPrev()
    {
        // Torna alla traccia precedente e aggiorna il testo
        MusicPlayer.Instance.PreviousTrack();
        UpdateTrackName();
    }

    void OnPlayPause()
    {
        // Alterna tra play e pausa
        MusicPlayer.Instance.TogglePlayPause();

        // Aggiorna l’icona del pulsante per riflettere il nuovo stato
        UpdatePlayPauseIcon();
    }

    void UpdateTrackName()
    {
        // Aggiorna il testo con il nome del brano attuale
        if (trackNameText == null)
            return;

        trackNameText.text = $"Now Playing: {MusicPlayer.Instance.GetCurrentTrackName()}";
    }

    void UpdatePlayPauseIcon()
    {
        // Verifica che tutte le risorse necessarie siano collegate
        if (playPauseImage == null || playIcon == null || pauseIcon == null)
            return;

        // Se la musica è in riproduzione → mostra l’icona di pausa
        // Altrimenti → mostra l’icona di play
        playPauseImage.sprite = MusicPlayer.Instance.IsPlaying() ? pauseIcon : playIcon;
    }
}
