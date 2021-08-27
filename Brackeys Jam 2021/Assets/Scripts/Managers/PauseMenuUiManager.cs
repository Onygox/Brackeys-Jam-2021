﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuUiManager : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public GameObject pauseMenuCanvas;
    public GameObject optionsMenu;
    public GameObject optionsFileSystem;
    public GameObject pauseMenuUi;
    public GameObject musicManager;
    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)){
            if (gameIsPaused){
                ResumeGame();
            }
            else{
                PauseGame();
            }
        }
    }

    public void PauseGame() {
        gameIsPaused = true;
        pauseMenuCanvas.SetActive(true);
        Time.timeScale = 0f;
        musicManager.GetComponent<MusicManager>().OnPauseGame();
    }
    public void ResumeGame() {
        gameIsPaused = false;
        pauseMenuCanvas.SetActive(false);
        Time.timeScale = 1f;
        // make sure if you resume game on options menu it will save stuff
        optionsFileSystem.GetComponent<OptionsMenuFileSystem>().SaveData();
        // make sure to disable options ui and enable default ui
        pauseMenuUi.SetActive(true);
        optionsMenu.SetActive(false);
        musicManager.GetComponent<MusicManager>().OnResumeGame();

    }
    public void QuitGame() {
        // Application.Quit();
        PersistentManager.Instance.LoadSceneByIndex(0);
    }
}
