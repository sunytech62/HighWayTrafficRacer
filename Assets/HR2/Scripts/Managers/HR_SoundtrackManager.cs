using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HR_SoundtrackManager : MonoBehaviour
{
    private static HR_SoundtrackManager instance;

    public static HR_SoundtrackManager Instance;

    public AudioSource SoundtrackSource
    {
        get
        {
            if (soundtrackSource == null)
                soundtrackSource = GetComponent<AudioSource>();

            if (soundtrackSource == null)
                soundtrackSource = RCCP_AudioSource.NewAudioSource(gameObject, "HR_SountrackSource", 0f, 0f, HR_Settings.Instance.defaultMusicVolume, null, true, false, false);

            return soundtrackSource;
        }
    }

    private AudioSource soundtrackSource;

    public List<AudioClip> showroomSoundtracks = new List<AudioClip>();

    public List<AudioClip> gameplaySoundtracks = new List<AudioClip>();

    public bool ignorePause = false;

    [Range(.1f, 1f)] public float maximumVolume = 1f;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        SoundtrackSource.ignoreListenerPause = ignorePause;
        Stop();
        HR_Events_OnAudioChanged();
    }

    private void OnEnable()
    {
        HR_Events.OnOptionsChanged += HR_Events_OnAudioChanged;
    }

    private void HR_Events_OnAudioChanged()
    {
        SetMusicVolume(HR_API.GetMusicVolume());
    }

    private void Update()
    {
        if (SoundtrackSource.volume > maximumVolume)
            SoundtrackSource.volume = maximumVolume;

        // Check if the current scene is the main menu and play the appropriate soundtrack.
        if (SceneManager.GetActiveScene().buildIndex == HR_Settings.Instance.mainMenuSceneIndex)
        {
            if (showroomSoundtracks.Count > 0)
            {
                // Select a random audio clip from the showroom soundtracks list.
                AudioClip randomClip = showroomSoundtracks[Random.Range(0, showroomSoundtracks.Count)];

                // Play the selected audio clip if it's not already playing.
                if (!showroomSoundtracks.Contains(SoundtrackSource.clip))
                    PlayClip(randomClip);
            }

            return;
        }

        // Play a random gameplay soundtrack if the active scene is not the main menu.
        if (gameplaySoundtracks.Count > 0)
        {
            // Select a random audio clip from the gameplay soundtracks list.
            AudioClip randomClip = gameplaySoundtracks[Random.Range(0, gameplaySoundtracks.Count)];

            // Play the selected audio clip if it's not already playing.
            if (!gameplaySoundtracks.Contains(SoundtrackSource.clip))
                PlayClip(randomClip);
        }
    }

    public void PlayClip(AudioClip newClip)
    {
        SoundtrackSource.clip = newClip;
        SoundtrackSource.Play();
    }

    public void SetMusicVolume(float newVolume)
    {
        SoundtrackSource.volume = newVolume;
    }

    public void Stop()
    {
        SoundtrackSource.clip = null;
        SoundtrackSource.Stop();
    }

    private void OnDisable()
    {
        HR_Events.OnOptionsChanged -= HR_Events_OnAudioChanged;
    }
}
