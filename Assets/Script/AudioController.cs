using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public AudioSource audioSource; // Reference to the AudioSource component
    public AudioClip introMusic;    // Clip for the intro music
    public AudioClip ghostMusic;    // Clip for the ghost's background music

    void Start()
    {
        PlayIntroMusic();
    }

    void PlayIntroMusic()
    {
        audioSource.clip = introMusic;  // Set the intro music clip
        audioSource.Play();             // Play the intro music
        Invoke(nameof(PlayGhostMusic), introMusic.length);  // Schedule the next music
    }

    void PlayGhostMusic()
    {
        audioSource.clip = ghostMusic;  // Set the ghost music clip
        audioSource.Play();             // Play the ghost music
    }
}
