using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class HighscoreManager : MonoBehaviour
{
  [SerializeField]
  private Hashtable highscores;
  
  private int playerscore = 5000;

  private string gameDataFileName = "data.json";

  void Start()
  {
    DontDestroyOnLoad(gameObject);
    highscores = new Hashtable();
    LoadGameData();

    LoadPlayerProgress();
    SubmitNewPlayerScore(playerscore);
    SaveGameData();
  }

  public void SubmitNewPlayerScore(int newScore)
  {
    highscores.Add("score0", newScore);
    Debug.Log(highscores.ContainsKey("score0"));
    // If newScore is greater than playerProgress.highestScore, update playerProgress with the new value and call SavePlayerProgress()
    if (newScore > 0)
    {
      
    }
  }

  public int GetHighestPlayerScore()
  {
    return 1;
  }

  private void LoadGameData()
  {
    // Path.Combine combines strings into a file path
    // Application.StreamingAssets points to Assets/StreamingAssets in the Editor, and the StreamingAssets folder in a build
    string filePath = Path.Combine(Application.streamingAssetsPath, gameDataFileName);

    if (File.Exists(filePath))
    {
      // Read the json from the file into a string
      string dataAsJson = File.ReadAllText(filePath);
      Debug.Log("loaded = " + dataAsJson);
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

  // This function could be extended easily to handle any additional data we wanted to store in our PlayerProgress object
  private void LoadPlayerProgress()
  {
    // Create a new PlayerProgress object
    //playerProgress = new PlayerProgress();

    // If PlayerPrefs contains a key called "highestScore", set the value of playerProgress.highestScore using the value associated with that key
    if (PlayerPrefs.HasKey("highestScore"))
    {
      //playerProgress.highestScore = PlayerPrefs.GetInt("highestScore");
    }
  }

  private void SaveGameData()
  {
    string dataAsJson = JsonUtility.ToJson(highscores);
    Debug.Log(dataAsJson+" ");
    string filePath = Path.Combine(Application.streamingAssetsPath, gameDataFileName);
    File.WriteAllText(filePath, dataAsJson);
  }
}