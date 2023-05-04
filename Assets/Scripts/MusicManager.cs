using UnityEngine;
using System.Collections.Generic;

public class MusicManager : MonoBehaviour
{
    public List<AudioClip> musicList;
    public AudioSource audioSource;
    public float fadeTime = 1f;

    private float currentVolume = 0f;
    private bool isFading = false;

    void Start()
    {
        LoadMusic();
        PlayRandomSong();
    }

    void LoadMusic()
    {
        // Load all audio clips from the Sounds/Music folder in Resources
        AudioClip[] clips = Resources.LoadAll<AudioClip>("Sounds/Music");

        foreach (AudioClip clip in clips)
        {
            musicList.Add(clip);
        }
    }

    void PlayRandomSong()
    {
        if (musicList.Count > 0)
        {
            // Pick a random song from the musicList
            int index = Random.Range(0, musicList.Count);
            AudioClip clip = musicList[index];

            // Set the AudioClip of the AudioSource component
            audioSource.clip = clip;
            audioSource.loop = true;

            // Start the fade-in effect
            StartCoroutine(FadeIn());

            // Play the song
            audioSource.Play();
        }
        else
        {
            Debug.LogError("Music list is empty!");
        }
    }

    IEnumerator<float> FadeIn()
    {
        isFading = true;
        currentVolume = 0f;

        while (currentVolume < 1f)
        {
            currentVolume += Time.deltaTime / fadeTime;
            audioSource.volume = currentVolume;
            yield return currentVolume;
        }

        isFading = false;
    }

    IEnumerator<float> FadeOut()
    {
        isFading = true;
        currentVolume = 1f;

        while (currentVolume > 0f)
        {
            currentVolume -= Time.deltaTime / fadeTime;
            audioSource.volume = currentVolume;
            yield return currentVolume;
        }

        isFading = false;
        PlayRandomSong();
    }
    

    void Update()
    {
        // Check if the current song has finished playing
        if (!isFading && !audioSource.isPlaying)
        {
            // Start the fade-out effect
            StartCoroutine(FadeOut());
        }
    }
}
