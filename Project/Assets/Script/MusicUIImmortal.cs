using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MusicUIImmortal : MonoBehaviour
{
    [Header("UI Elements")]
    public Slider volumeSlider; // Slider per controllare il volume della musica
    public Button nextButton; // Bottone per passare alla traccia successiva
    public Button prevButton; // Bottone per tornare alla traccia precedente
    public Button playPauseButton; // Bottone per avviare o mettere in pausa la musica
    public Button muteButton; // Bottone per attivare/disattivare il mute
    public TextMeshProUGUI trackNameText; // Testo che mostra il nome della traccia corrente

    public Sprite playIcon; // Icona da mostrare quando la musica è in pausa (pulsante play)
    public Sprite pauseIcon; // Icona da mostrare quando la musica è in riproduzione (pulsante pausa)

    private Image playPauseImage; // Componente Image del bottone play/pause per poter cambiare l’icona

    void Start()
    {
        // Inizializza e collega la UI con il MusicPlayer all'avvio
        ConnectUI();
    }

    void ConnectUI()
    {
        // Verifica che esista un'istanza del MusicPlayer nella scena
        if (MusicPlayer.Instance == null)
        {
            Debug.LogWarning("MusicPlayer non trovato nella scena!");
            return;
        }

        // Ottiene il componente Image del bottone play/pause per modificare l’icona
        playPauseImage = playPauseButton.GetComponent<Image>();

        // Imposta il valore iniziale del volume dallo PlayerPrefs (default: 0.2)
        volumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 0.2f);

        // Aggiunge un listener per aggiornare il volume nel MusicPlayer quando cambia lo slider
        volumeSlider.onValueChanged.AddListener(MusicPlayer.Instance.ChangeVolume);

        // Aggiunge le funzioni dei bottoni ai relativi metodi del MusicPlayer
        nextButton.onClick.AddListener(() => MusicPlayer.Instance.NextTrack());
        prevButton.onClick.AddListener(() => MusicPlayer.Instance.PreviousTrack());
        muteButton.onClick.AddListener(() => MusicPlayer.Instance.ToggleMute());

        // Collega il bottone play/pause al metodo locale OnPlayPause()
        playPauseButton.onClick.AddListener(OnPlayPause);

        // Aggiorna subito l'interfaccia in base allo stato corrente della musica
        UpdateUI();
    }

    void Update()
    {
        // Se non esiste un MusicPlayer, esce
        if (MusicPlayer.Instance == null)
            return;

        // Aggiorna continuamente la UI (utile per aggiornare il nome della traccia o lo stato)
        UpdateUI();
    }

    void OnPlayPause()
    {
        // Se non esiste il MusicPlayer, esce
        if (MusicPlayer.Instance == null)
            return;

        // Alterna tra play e pausa nel MusicPlayer
        MusicPlayer.Instance.TogglePlayPause();

        // Aggiorna la UI per riflettere il nuovo stato (play/pause)
        UpdateUI();
    }

    void UpdateUI()
    {
        // Se non esiste il MusicPlayer, esce
        if (MusicPlayer.Instance == null)
            return;

        // Aggiorna il testo del nome della traccia corrente
        if (trackNameText != null)
            trackNameText.text = $"Now Playing: {MusicPlayer.Instance.GetCurrentTrackName()}";

        // Aggiorna l’icona del bottone play/pause in base allo stato del player
        if (playPauseImage != null && playIcon != null && pauseIcon != null)
            playPauseImage.sprite = MusicPlayer.Instance.IsPlaying() ? pauseIcon : playIcon;
    }
}
