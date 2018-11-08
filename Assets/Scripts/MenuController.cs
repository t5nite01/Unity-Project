using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

  private GameObject mainMenuPanel;
  private GameObject settingsPanel;
	// Use this for initialization
	void Start () {
		mainMenuPanel = GameObject.Find("PanelMenu");
    settingsPanel = GameObject.Find("PanelSettings");
    settingsPanel.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

  // Start game button
  public void StartClick(){
    StartCoroutine(LoadGameScene());
  }

  IEnumerator LoadGameScene()
  {
      AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Scene0");

      while (!asyncLoad.isDone)
      {
          yield return null;
      }
  }
  public void SettingsClick(){
    mainMenuPanel.SetActive(false);
    settingsPanel.SetActive(true);
  }
  public void QuitClick(){
    Application.Quit();
  }
  // return to main menu from settings
  public void BackClick(){
    mainMenuPanel.SetActive(true);
    settingsPanel.SetActive(false);

  }

}
