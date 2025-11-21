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

    private void Awake()
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
        string fullPath = Path.Combine(Application.persistentDataPath, audioPath + ".mp3");

        if (!File.Exists(fullPath))
        {
            Debug.LogWarning($"Audio file not found at: {fullPath}. Waiting for Resources copy...");
            yield break;
        }

        string url = "file://" + fullPath;

        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Failed to load audio file: {www.error} | URL: {url}");
            }
            else
            {
                audioSource.clip = DownloadHandlerAudioClip.GetContent(www);
                Debug.Log("Loaded audio from external file: " + fullPath);
            }
        }
    }

    public void UpdateAudioClip()
    {
        AudioClip clip = Resources.Load<AudioClip>(audioPath);

        if (clip != null)
        {
            audioSource.clip = clip;
            //audioSource.Play();
            Debug.Log("Loaded audio from Resources: " + audioPath);
        }
        else
        {
            StartCoroutine(LoadAudioClip());
        }
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

    public void SeekToTime(float time) {
        audioSource.time = time;
    }

    public void PlayMusic()
    {
        if (audioSource.clip != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }
    }

    public void PauseMusic()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
        }
    }

    public void StopMusic()
    {
        audioSource.Stop();
    }

    public void SeekToZero()
    {
        audioSource.time = 0;
    }
}