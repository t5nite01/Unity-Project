using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class HighscoreManager : MonoBehaviour
{
  private int[] highScore;
  List<GameObject> scores;
  private GameObject highScorePanel;
  private GameObject highScoreleftPanel;
  private static HighscoreManager highscoreManager;

  void Start()
  {
    DontDestroyOnLoad(gameObject);
    if (highscoreManager == null) {
      highscoreManager = this;
      highScore = new int[5];
      scores = new List<GameObject>();
      highScorePanel = GameObject.Find("PanelHighscore");
      highScoreleftPanel = GameObject.Find("PanelleftHighscore");
    
      LoadHighscores();

      highScoreleftPanel.SetActive(false);
      highScorePanel.SetActive(false);
    } else {
      highScorePanel = GameObject.Find("PanelHighscore");
      highScoreleftPanel = GameObject.Find("PanelleftHighscore");
      highScoreleftPanel.SetActive(false);
      highScorePanel.SetActive(false);
      Object.Destroy(gameObject);
    }
  }

  public void SubmitNewPlayerScore(int newScore)
  {
    for (int i = 0; i < highScore.Length; i++)
    {
      //Get the highScore from 1 - 5
      string highScoreKey = "HighScore" + (i + 1).ToString();
      int highScore = PlayerPrefs.GetInt(highScoreKey , 0);
      // Check if score submitted is high enough to make it to leaderboard.
      if (newScore > highScore)
      {
        // Move earlier scores down when adding in between.
        if (i < 5)
        {
          for (int a = 5; a >= i && (a+2) < 6; a--)
          {
            string highScoreKeyfrom = "HighScore" + (a + 1).ToString();
            string highScoreKeySetTo = "HighScore" + (a + 2).ToString();
            PlayerPrefs.SetInt(highScoreKeySetTo, PlayerPrefs.GetInt(highScoreKeyfrom, 0));
          }
        }
        // Set new score.
        PlayerPrefs.SetInt(highScoreKey, newScore);
        break;
      }
    }
  }

  public int GetHighestPlayerScore()
  {
    return 1;
  }

  public void LoadHighscores()
  {
    scores = new List<GameObject>();
    for (int a = 0; a < 5; a++)
    {
      scores.Add(GameObject.Find("TextScore" + (a+1).ToString()));
    }

    int i = 0;
    foreach(GameObject score in scores){
      string highScoreKey = "HighScore" + (i + 1).ToString();
      score.GetComponent<Text>().text = (i+1)+". "+PlayerPrefs.GetInt(highScoreKey, 0).ToString();
      i++;
    }
  }
}