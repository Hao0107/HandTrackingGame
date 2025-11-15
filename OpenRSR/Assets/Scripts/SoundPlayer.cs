using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundPlayer : MonoBehaviour
{
    public AudioClip audioClip;
    private AudioSource audioSource;
    private bool started = false;
    private bool paused = true;
    private float currentTime = 0f;
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = audioClip;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayAudio()
    {
        if (audioSource != null)
        {
            currentTime = audioSource.time;
            if (!started) {
                audioSource.Play();
                started = true;
            } else {
                audioSource.time = currentTime;
                audioSource.UnPause();
                paused = false;
            }
        }
    }

    public void PauseAudio()
    {
        if (audioSource != null && !paused)
        {
            audioSource.Pause();
        }
    }

    public void StopAudio() {
        if (audioSource != null) {
            audioSource.Stop();
        }
    }

    public void SeekToZero() {
        currentTime = 0f;
        if (audioSource != null) {
            audioSource.time = 0f;
        }
        started = false;
    }

    public void SeekToTime(float time) {
        audioSource.time = time;
    }

    public bool AudioPlayHasEnded() {
        return currentTime >= audioSource.clip.length;
    }

    public bool CompareTime(float time) {
        return audioSource.time >= time;
    }
}
