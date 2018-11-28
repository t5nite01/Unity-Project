using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    private GameObject gameOverPanel;

    // Use this for initialization
    void Start()
    {
        gameOverPanel = GameObject.Find("GameOverPanel");
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Start game button
    public void RestartClick()
    {
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
        string scene = "StartScreen";
        StartCoroutine(LoadScene(scene));
    }
}