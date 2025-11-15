using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using System.IO;
using UnityEngine.InputSystem;

public class AudioPlayer : MonoBehaviour
{
    public string audioPath;
    private AudioClip audioClip;
    private AudioSource audioSource;
    private LevelConfigurator levelConfig;
    private GameManager gameManager;
    private bool started = false;
    private bool paused = true;
    private float currentTime = 0f;

    private void Start()
    {
        GameObject levelRenderer = GameObject.Find("LevelRenderer");
        levelConfig = levelRenderer.GetComponent<LevelConfigurator>();
        audioPath = levelConfig.musicPath;
        audioSource = GetComponent<AudioSource>();
        gameManager = GameManager.instance;

        StartCoroutine(WaitASecond());

        if (!string.IsNullOrEmpty(audioPath) && audioSource != null)
        {
            if (gameManager.isDataDownloaded)
            {
                StartCoroutine(LoadAudioClip());
            }
            else
            {
                audioClip = Resources.Load<AudioClip>(audioPath);
                if (audioClip != null)
                {
                    audioSource.clip = audioClip;
                }
                else
                {
                    Debug.LogError("Audio clip not found: " + audioPath);
                }
            }
        }
    }

    IEnumerator WaitASecond()
    {
        yield return new WaitForSeconds(1f);
    }

    public IEnumerator LoadAudioClip()
    {
        string audioFilePath = "file://" + Path.Combine(Application.persistentDataPath, audioPath + ".mp3");
        UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(audioFilePath, AudioType.MPEG);

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
        {
            audioClip = DownloadHandlerAudioClip.GetContent(www);

            if (audioClip != null)
            {
                audioSource.clip = audioClip;
            }
            else
            {
                Debug.LogError("Failed to load audio clip.");
            }
        }
        else
        {
            Debug.LogError("Failed to load audio file: " + www.error);
        }
    }

    public void UpdateAudioClip() {
        StartCoroutine(LoadAudioClip());
    }

    private void Update()
    {
        audioPath = levelConfig.musicPath;

        if (gameManager.isDataDownloaded)
        {
            if (((Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame) || (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.wasPressedThisFrame)) && audioSource.clip == null)
            {
                StartCoroutine(LoadAudioClip());
            }
        }
        else
        {
            audioClip = Resources.Load<AudioClip>(audioPath);
            if (audioClip != null)
            {
                audioSource.clip = audioClip;
            }
        }
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
        audioSource.time = 0f;
        started = false;
    }

    public void SeekToTime(float time) {
        audioSource.time = time;
    }
}