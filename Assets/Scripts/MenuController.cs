using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

  private GameObject mainMenuPanel;
  private GameObject settingsPanel;
  private GameObject mapSelectPanel;
  
	void Start () {
		mainMenuPanel = GameObject.Find("PanelMenu");
    mapSelectPanel = GameObject.Find("PanelMapSelect");
    settingsPanel = GameObject.Find("PanelSettings");
    settingsPanel.SetActive(false);
    mapSelectPanel.SetActive(false);
	}
	
	void Update () {
		
	}

  public void StartClickValley(){
    StartCoroutine(LoadGameScene(1));
  }
  public void StartClickPit(){
    StartCoroutine(LoadGameScene(2));
  }

  IEnumerator LoadGameScene(int sceneId)
  {
      AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneId);

      while (!asyncLoad.isDone)
      {
          yield return null;
      }
  }
  public void StartGameClick(){
    mainMenuPanel.SetActive(false);
    mapSelectPanel.SetActive(true);
  }
  public void SettingsClick(){
    mainMenuPanel.SetActive(false);
    settingsPanel.SetActive(true);
  }
  public void QuitClick(){
    Application.Quit();
  }

  // return to main menu from mapselect
  public void BackClickMapSelect(){
    mainMenuPanel.SetActive(true);
    mapSelectPanel.SetActive(false);
  }

  // return to main menu from settings
  public void BackClickSettings(){
    mainMenuPanel.SetActive(true);
    settingsPanel.SetActive(false);
  }

}
