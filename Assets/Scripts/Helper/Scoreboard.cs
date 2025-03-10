using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Scoreboard : MonoBehaviour
{
    public static Scoreboard instance {get; private set;}

    int highScore;
    string highScoreName;
    // Start is called before the first frame update
    private void Awake()
    {
        if(instance != null)
        {
            Debug.Log("A Scoreboard manager instance already exists in the scene");
        }
        instance = this;
    }

    public void GetHighScore()
    {
        highScore = PlayerPrefs.GetInt("HighScore");
        highScoreName = PlayerPrefs.GetString("HighScoreName");
    }
   public void UpdateHighScore(int score, string playerName)
   {
        if(score > highScore)
        {
            PlayerPrefs.SetInt("HighScore", score);
            PlayerPrefs.SetString("HighScoreName", playerName);
        }
   }
}
