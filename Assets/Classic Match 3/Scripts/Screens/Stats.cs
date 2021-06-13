using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
    This class is responsible for a post game logic. Here we see final score and can restart game or go back to menu.
*/

public class Stats : MonoBehaviour
{
    public static Stats instance;

    public Text highscoreText;
    public Text totalscoreText;
    public Text timeText;
    public Text matchesText;

    void Start()
    {
        instance = this;

        int totalscore = Storage.LoadTotalscore();
        int highscore = Storage.LoadHighscore();
        float time = Storage.LoadTime();
        int matches = Storage.LoadMatches();

        highscoreText.text = "HIGHSCORE" + System.Environment.NewLine + highscore;
        totalscoreText.text = "TOTAL SCORE" + System.Environment.NewLine + totalscore;
        matchesText.text = "MATCHES" + System.Environment.NewLine + matches;
        timeText.text = "TIME PLAYED" + System.Environment.NewLine + GetTimeString(time);

        Sound.instance.PlayLevelEnd();
    }

    // Back to menu
    public void BackToMenu()
    {
        Sound.instance.PlayTap();
        GameManager.instance.LoadScene("Menu");
    }

    public static string GetTimeString(float value)
    {
        int minutes = (int)value / 60;
        int seconds = (int)value % 60;

        string time = "";

        if (minutes > 9)
            time += minutes;
        else
            time += "0" + minutes;

        time += ":";

        if (seconds > 9)
            time += seconds;
        else
            time += "0" + seconds;

        return time;
    }
}
