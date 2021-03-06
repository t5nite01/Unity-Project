using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AudioController : MonoBehaviour
{
  public AudioMixer mixer;
  public Slider volSlider;
  private void Start()
  {
      LoadPref();
  }

  public void SetMusixLevel(float value)
  {
      mixer.SetFloat("MainVolume", value);
      PlayerPrefs.SetFloat("MainVolume", value);
      PlayerPrefs.Save();
  }

  private void LoadPref()
  {
    if(SceneManager.GetActiveScene().name == "StartScreen"){
      volSlider.value = PlayerPrefs.GetFloat("MainVolume");  
    }
    mixer.SetFloat("MainVolume", PlayerPrefs.GetFloat("MainVolume"));
  }
}