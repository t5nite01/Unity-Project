using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    GameObject player;
    PlayerMovementScript playerMovement;
    ScoreManager scoreManager;
    AudioController audioController;

    Text pauseScoreText, pauseKillsText;
    bool justEntered;

    // Use this for initialization
    void Start()
    {
        justEntered = true;

        player = GameObject.Find("Player");
        playerMovement = player.GetComponent<PlayerMovementScript>();
        scoreManager = player.GetComponent<ScoreManager>();
        audioController = GameObject.Find("AudioManager").GetComponent<AudioController>();
    }

    // Update is called once per frame
    void Update()
    {
        // Update only when game is paused
        if (playerMovement.gamePaused == true)
        {
            // Only allow the script to pass here once in one shopping time
            if (justEntered)
            {
                justEntered = false;
                // Pause game
                Time.timeScale = 0;
                // Mute volume
                audioController.mixer.SetFloat("MainVolume", -80);
                // Get score text components
                pauseScoreText = GameObject.Find("PauseScoreText").GetComponent<Text>();
                pauseKillsText = GameObject.Find("PauseKillsText").GetComponent<Text>();
                // Stop score script
                scoreManager.stop();
                // Show the cursor
                Cursor.lockState = CursorLockMode.None;
            }
            // Set score to screen
            pauseScoreText.text = "Score: " + Mathf.RoundToInt(scoreManager.getScore()).ToString();
            pauseKillsText.text = "Kills: " + Mathf.RoundToInt(scoreManager.getKills()).ToString();
        }
    }

    // Resume game button
    public void ResumeClick()
    {
        // Resume game
        Time.timeScale = 1;
        // Unmute volume
        audioController.mixer.SetFloat("MainVolume", PlayerPrefs.GetFloat("MainVolume"));

        playerMovement.gamePaused = false;
        // Resume score script
        scoreManager.resume();
        // Hide the cursor
        Cursor.lockState = CursorLockMode.Locked;
        // Allow 
        justEntered = true;
    }

    // Restart game button
    public void RestartClick()
    {
        Time.timeScale = 1;
        // Unmute volume
        audioController.mixer.SetFloat("MainVolume", PlayerPrefs.GetFloat("MainVolume"));

        playerMovement.gamePaused = false;
        Scene scene = SceneManager.GetActiveScene();
        StartCoroutine(LoadScene(scene.name));
    }

    IEnumerator LoadScene(string scene)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public void MainMenuClick()
    {
        // Unmute volume
        audioController.mixer.SetFloat("MainVolume", PlayerPrefs.GetFloat("MainVolume"));

        string scene = "StartScreen";
        StartCoroutine(LoadScene(scene));
    }
}
