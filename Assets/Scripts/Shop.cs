using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Shop : MonoBehaviour
{
    private GameObject shopPanel;

    // Use this for initialization
    void Start()
    {
        shopPanel = GameObject.Find("ShopPanel");
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Start game button
    public void RestartClick()
    {
        string scene = "Scene0";
        StartCoroutine(LoadScene(scene));
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
