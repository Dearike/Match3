using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class Menu : MonoBehaviour
{
    public static Menu instance;

    public GameObject settingsContainer;

    public Button soundButton;
    public Button musicButton;

    public GameObject start;

    private List<GameObject> blocks;

    void Start()
    {
        instance = this;
    }
    
    public void StartGame()
    {
        Sound.instance.PlayTap();
        GameManager.instance.LoadScene("Game");
    }
    
    public void Leaderboard()
    {
        Sound.instance.PlayTap();
        // Implemenet your leaderboard logic here 
    }
    
    public void OpenAchievements()
    {
        Sound.instance.PlayTap();
        GameManager.instance.LoadScene("Stats");
    }
    
    public void ShowSettings()
    {
        Sound.instance.PlayTap();

        if (!settingsContainer.activeSelf)
            settingsContainer.SetActive(true);
        else
            settingsContainer.SetActive(false);
    }
    
    public void SetSettings()
    {
        if (Storage.LoadMusic())
        {
            musicButton.transform.GetChild(0).gameObject.SetActive(false);
            Sound.instance.MuteMusic(false);
        }
        else
        {
            musicButton.transform.GetChild(0).gameObject.SetActive(true);
            Sound.instance.MuteMusic(true);
        }

        if (Storage.LoadSound())
        {
            soundButton.transform.GetChild(0).gameObject.SetActive(false);
            Sound.instance.MuteSounds(false);
        }
        else
        {
            soundButton.transform.GetChild(0).gameObject.SetActive(true);
            Sound.instance.MuteSounds(true);
        }
    }
    
    public void EnableMusic()
    {
        if (Storage.LoadMusic())
        {
            musicButton.transform.GetChild(0).gameObject.SetActive(true);
            Storage.SaveMusic(false);
            Sound.instance.MuteMusic(true);
        }
        else
        {
            musicButton.transform.GetChild(0).gameObject.SetActive(false);
            Storage.SaveMusic(true);
            Sound.instance.MuteMusic(false);
        }
    }
    
    public void EnableSound()
    {
        if (Storage.LoadSound())
        {
            soundButton.transform.GetChild(0).gameObject.SetActive(true);
            Storage.SaveSound(false);
            Sound.instance.MuteSounds(true);

        }
        else
        {
            soundButton.transform.GetChild(0).gameObject.SetActive(false);
            Storage.SaveSound(true);
            Sound.instance.MuteSounds(false);
        }
    }
}