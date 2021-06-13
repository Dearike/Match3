using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/*
    This class is responsible for a post game logic. Here we see final score and can restart game or go back to menu.
*/

public class LevelEnd : MonoBehaviour
{
    public static LevelEnd instance;

    public Text messageText;
    public Text scoreText;
    public Text highscoreText;

    void Start()
    {
        instance = this;

        int score = Storage.LoadScore();
        int highscore = Storage.LoadHighscore();

        if (score >= highscore)
            messageText.gameObject.SetActive(true);

        scoreText.text = "SCORE" + System.Environment.NewLine + score;
        highscoreText.text = "HIGHSCORE" + System.Environment.NewLine + highscore;

        Sound.instance.PlayLevelEnd();
    }

    // Restart current level
    public void Restart()
    {
        Sound.instance.PlayTap();
        GameManager.instance.LoadScene("Game");
    }

    // Back to menu
    public void BackToMenu()
    {
        Sound.instance.PlayTap();
        GameManager.instance.LoadScene("Menu");
    }
}
