using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameFreeze : MonoBehaviour
{
    public bool gamePaused = true;
    GameObject pausedText;
    GameObject pauseButton;
    GameObject exitButton;
    GameObject editButton;
    GameObject settingsButton;
    float timer;
    public GameObject balus;
    private SphereMovement sphm;
    private SphereDragger sphd;
    private AudioPlayer audioPlayer;
    private GameManager manager;

    void Start()
    {
        timer = 0f;
        sphm = balus.GetComponent<SphereMovement>();
        sphd = balus.GetComponent<SphereDragger>();
        audioPlayer = balus.GetComponent<AudioPlayer>();
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        gamePaused = true;
        pausedText = GameObject.Find("PausedText");
        pauseButton = GameObject.Find("PauseButton");
        editButton = GameObject.Find("EditButton");
        settingsButton = GameObject.Find("SettingsButton");
        exitButton = GameObject.Find("ExitButton");
    }
    
    void Update()
    {
        if (gamePaused)
        {
            if (sphm != null) {
            sphm.enabled = false;
            }
            sphd.enabled = false;
            if (pausedText != null && editButton != null && settingsButton != null) {
            pausedText.SetActive(true);
            editButton.SetActive(true);
            settingsButton.SetActive(true);
            pauseButton.SetActive(false);
            exitButton.SetActive(true);
            }
        }
        else
        {
            //sphm.enabled = true;
            //sphd.enabled = true;
            if (pausedText != null && editButton != null && settingsButton != null && pauseButton != null && exitButton != null) {
            pausedText.SetActive(false);
            editButton.SetActive(false);
            settingsButton.SetActive(false);
            pauseButton.SetActive(true);
            exitButton.SetActive(false);
            }
        }

        if (!manager.isGameOver) {
            if (( (Mouse.current != null && Mouse.current.leftButton.isPressed) || (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed) ))
            {
                // gamePaused = false;
                timer += 0.01f;
            }
            if (!((Mouse.current != null && Mouse.current.leftButton.isPressed) || (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed))) {
                timer = 0f;
            }
            if (timer >= 1f) {
                gamePaused = false;
                manager.isGamePaused = false;
                if (GameManager.instance != null) {
                    if (GameManager.instance.currentScene.name == "DebugScene") {
                        //sphm.enabled = false;
                        sphd.enabled = false;
                    } else {
                        audioPlayer.PlayAudio();
                        sphm.enabled = true;
                        sphd.enabled = true;
                    }
                }
            }
        }
    }

    public void PauseGame(bool stopAudio = true)
    {
        gamePaused = true;
        if (sphm != null && sphd != null) {
            sphm.isNotFalling = true;
            sphm.enabled = false;
            sphd.enabled = false;
        }
        timer = 0f;
        if (audioPlayer != null && stopAudio) {
            audioPlayer.PauseAudio();
        }
    }
}