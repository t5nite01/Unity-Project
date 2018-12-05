using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreManager : MonoBehaviour
{
  private Text text;        // Reference to the Text component.
  private float score;        // The player's score.
  private float timer;
  private float timeScaler; // Gain score expotentially overtime
  private bool running; // To stop script on player death.
  private int kills;
  void Awake ()
  {
      // Set up the reference.
      text = GameObject.Find("ScoreText").GetComponent<Text>();
    
      // Reset.
      score = 0;
      timer = 0;
      timeScaler = 0;
      running = true;
  }

  void Update ()
  {
    if (running)
    {
      timer += Time.deltaTime;
      if (timer >= 1 )
      {
        score += 1+timeScaler;
        timeScaler += 0.2f;
        timer = 0;
      }
      // Set the displayed text to be the word "Score" followed by the score value.
      text.text = "Score: " + Mathf.RoundToInt(score).ToString();
    }
  }
  
  public bool removeScore(int amount){
    if(score >= amount){
      score -= amount;
      return true;
    }
    return false;
  }

  public void addKillAndScore(int amount){
    score += amount;
    kills += 1;
  }

  public void setScore(int amount){
    score = amount;
  }

  public float getScore(){
    return score;
  }

  public int getKills(){
    return kills;
  }

  public void stop(){
    running = false;
  }

  public void resume(){
    running = true;
  }
}