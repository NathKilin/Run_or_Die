using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    
    private List<AudioSource> activeAudioSources;
    
    public int maxConcurrentSounds = 25;
    

    private void Awake()
    {
        if (Instance == null) {
            Instance = this;
            activeAudioSources = new List<AudioSource>();
        } else {
            Destroy(gameObject);
        }
    }

    
    public AudioSource PlaySound(AudioClip clip, float volume = 1.0f)
    {
        if (activeAudioSources.Count < maxConcurrentSounds) {
            AudioSource audioSource = CreateAudioSource(clip, volume);
            audioSource.Play();
            activeAudioSources.Add(audioSource);
            return audioSource;
        } 
        
        return null;
    }
    
    private AudioSource CreateAudioSource(AudioClip clip, float volume)
    {
        GameObject audioObject = new GameObject("AudioSource_" + clip.name);
        AudioSource newSource = audioObject.AddComponent<AudioSource>();

        newSource.clip = clip;
        newSource.volume = volume;
        newSource.spatialBlend = 0.0f; 
        StartCoroutine(DestroyAudioSourceAfterPlayback(newSource));
        return newSource;
    }

    
    private IEnumerator DestroyAudioSourceAfterPlayback(AudioSource audioSource)
    {
        yield return new WaitForSeconds(audioSource.clip.length);
        activeAudioSources.Remove(audioSource);
        Destroy(audioSource.gameObject);
    }

    
    public void StopSound(AudioSource source)
    {
        if (activeAudioSources.Contains(source)) {
            source.Stop();
            activeAudioSources.Remove(source);
            Destroy(source.gameObject);
        }
    }

}
