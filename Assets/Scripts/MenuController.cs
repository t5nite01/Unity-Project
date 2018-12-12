using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;       

public class MenuController : MonoBehaviour {
  private string gameDataFileName = "data.json";
  private GameObject mainMenuPanel;
  private GameObject settingsPanel;
  private GameObject mapSelectPanel;
  
	void Start ()
    {
        Time.timeScale = 1;
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
  public void HighscoreClick(){

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

 private void LoadGameData()
    {
        // Path.Combine combines strings into a file path
        // Application.StreamingAssets points to Assets/StreamingAssets in the Editor, and the StreamingAssets folder in a build
        string filePath = Path.Combine(Application.streamingAssetsPath, gameDataFileName);

        if(File.Exists(filePath))
        {
            // Read the json from the file into a string
            string dataAsJson = File.ReadAllText(filePath); 
            // Pass the json to JsonUtility, and tell it to create a GameData object from it
            //GameData loadedData = JsonUtility.FromJson<GameData>(dataAsJson);

            // Retrieve the allRoundData property of loadedData
            //allRoundData = loadedData.allRoundData;
        }
        else
        {
            Debug.LogError("Cannot load game data!");
        }
    }

}
