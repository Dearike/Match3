using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public static Game instance;

    public Slider timerSlider;
    public Text scoreText;

    private int score;
    private int highscore;

    private float time;
    private int matches;
    private int totalscore;

    private bool isPlaying;

    private void Start()
    {
        instance = this;
        scoreText.text = score.ToString();
        highscore = Storage.LoadHighscore();

        time = Storage.LoadTime();
        matches = Storage.LoadMatches();
        totalscore = Storage.LoadTotalscore();

        isPlaying = true;
    }

    private void Update()
    {
        if (isPlaying)
        {
            Timer();
            time += Time.deltaTime;
        }
    }
    
    private void Timer()
    {
        timerSlider.value -= Time.deltaTime;

        if (timerSlider.value <= 0)
            EndGame();
    }
    
    public void AddPoints(int amount)
    {
        score += amount;
        timerSlider.value += amount;
        scoreText.text = score.ToString();

        if (score >= highscore)
            highscore = score;

        totalscore += amount;
        matches += 1;
    }
    
    private void SaveStats()
    {
        Storage.SaveMatches(matches);
        Storage.SaveTime(time);
        Storage.SaveTotalscore(totalscore);
        Storage.SaveHighscore(highscore);
        Storage.SaveScore(score);
    }
    
    private void EndGame()
    {
        isPlaying = false;
        SaveStats();
        GameManager.instance.LoadScene("Level End");
        Application.ExternalCall("ShowAd");
    }

    public void BackToMenu()
	{
        SaveStats();
        Sound.instance.PlayTap();
        GameManager.instance.LoadScene("Menu");
    }
}
