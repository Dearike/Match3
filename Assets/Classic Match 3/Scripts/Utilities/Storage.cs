using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    This class is responsible for data persitance
    I have used default PlayerPrefs as a storage, but you should think about more secure solution to prevent cheating among players
*/

public static class Storage
{

	// Initialize function
	// It's called every time game is launched
	// It checks if the game is launched at the first time. If it's true it creates default data
	public static void Init()
	{
		if (!PlayerPrefs.HasKey("score"))
			PlayerPrefs.SetInt("score", 0);

        if (!PlayerPrefs.HasKey("highscore"))
            PlayerPrefs.SetInt("highscore", 0);

        if (!PlayerPrefs.HasKey("totalscore"))
            PlayerPrefs.SetInt("totalscore", 0);

        if (!PlayerPrefs.HasKey("matches"))
            PlayerPrefs.SetInt("matches", 0);

        if (!PlayerPrefs.HasKey("time"))
            PlayerPrefs.SetFloat("time", 0);

        if (!PlayerPrefs.HasKey("sound"))
			PlayerPrefs.SetInt("sound", 1);

		if (!PlayerPrefs.HasKey("music"))
			PlayerPrefs.SetInt("music", 1);
	}

	// Retrieves information about music to check if it's enabled or disabled
	public static bool LoadMusic()
	{
		return PlayerPrefs.GetInt("music") == 1;
	}

	// Retrieves information about sound to check if it's enabled or disabled
	public static bool LoadSound()
	{
		return PlayerPrefs.GetInt("sound") == 1;
	}

	// Save music status
	public static void SaveMusic(bool value)
	{
		PlayerPrefs.SetInt("music", value ? 1 : 0);
	}

	// Save sound status
	public static void SaveSound(bool value)
	{
		PlayerPrefs.SetInt("sound", value ? 1 : 0);
	}

    // Get current highscore
    public static int LoadHighscore()
    {
        return PlayerPrefs.GetInt("highscore");
    }

    // Save new highscore
    public static void SaveHighscore(int score)
    {
        PlayerPrefs.SetInt("highscore", score);
    }

    // Get current score
    public static int LoadScore()
    {
        return PlayerPrefs.GetInt("score");
    }

    // Set new score
    public static void SaveScore(int score)
    {
        PlayerPrefs.SetInt("score", score);
    }

    // Get total score amount
    public static int LoadTotalscore()
    {
        return PlayerPrefs.GetInt("totalscore");
    }

    // Set new score total amount
    public static void SaveTotalscore(int score)
    {
        PlayerPrefs.SetInt("totalscore", score);
    }

    // Get total matches amount
    public static int LoadMatches()
    {
        return PlayerPrefs.GetInt("matches");
    }

    // Set new matches amount
    public static void SaveMatches(int matches)
    {
        PlayerPrefs.SetInt("matches", matches);
    }

    // Get total time played
    public static float LoadTime()
    {
        return PlayerPrefs.GetFloat("time");
    }

    // Set new time played
    public static void SaveTime(float time)
    {
        PlayerPrefs.SetFloat("time", time);
    }
}
