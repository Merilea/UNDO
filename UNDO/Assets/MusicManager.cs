using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    public AudioClip[] musicClips; // Array to hold the music clips
    public AudioMixerGroup masterGroup; // Reference to the master audio mixer group

    private AudioSource audioSource; // Reference to the AudioSource component
    private int currentClipIndex = 0; // Index to track the current clip

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.outputAudioMixerGroup = masterGroup; // Set output to master group

        if (musicClips.Length > 0)
        {
            PlayNextClip();
        }
    }

    void Update()
    {
        // Check if the current clip has finished playing
        if (!audioSource.isPlaying && audioSource.clip != null)
        {
            PlayNextClip();
        }
    }

    void PlayNextClip()
    {
        if (musicClips.Length == 0)
        {
            Debug.LogWarning("No music clips assigned.");
            return;
        }

        // Check if we have more clips to play
        audioSource.clip = musicClips[currentClipIndex];
        audioSource.Play();
        Debug.Log($"Playing clip: {musicClips[currentClipIndex].name}");
        currentClipIndex = (currentClipIndex + 1) % musicClips.Length; // Loop back to the first clip
    }
}
