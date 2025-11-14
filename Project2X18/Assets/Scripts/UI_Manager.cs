using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI_Manager : MonoBehaviour
{
    [Header("Menu Panel")]
    public GameObject pauseMenuPanel;
    public GameObject settingsPanel;
    private bool isPaused = false;

    [Header("Audio")]
    public AudioSource bgmSource;
    public AudioSource settingMusic;

    public void TogglePause()
    {
        isPaused = !isPaused;
        pauseMenuPanel.SetActive(isPaused);

        if (isPaused)
        {
            Time.timeScale = 0f; // game time stop
            if (bgmSource != null)
            {
                bgmSource.Stop();
                settingMusic.Play();
            }
        }
        else
        {
            Time.timeScale = 1f; // run game time
            if (bgmSource != null)
            {
                bgmSource.Play();
                settingMusic.Stop();
            }
        }

        Debug.Log("Game Paused: " + isPaused);
    }

    public void ResumeGame()
    {
        if (isPaused)
        {
            TogglePause(); // TogglePause
        }
    }

    public void ResetGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void OpenSettings()
    {
        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(false);
        }

        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
        }
    }

    public void CloseSettings()
    {
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }

        if (pauseMenuPanel != null)
        {
            pauseMenuPanel.SetActive(true);
        }

        Debug.Log("Settings Panel Closed. Back to Pause Menu.");
    }
}
