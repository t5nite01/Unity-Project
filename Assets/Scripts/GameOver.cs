using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    AudioController audioController;

    // Use this for initialization
    void Start()
    {
        audioController = GameObject.Find("AudioManager").GetComponent<AudioController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Restart game button
    public void RestartClick()
    {
      Scene scene = SceneManager.GetActiveScene();
      StartCoroutine(LoadScene(scene.name));
    }

    IEnumerator LoadScene(string scene)
    {
        // Unmute volume
        audioController.mixer.SetFloat("MainVolume", PlayerPrefs.GetFloat("MainVolume"));

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene);

        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }

    public void MainMenuClick()
    {
        string scene = "StartScreen";
        StartCoroutine(LoadScene(scene));
    }
}