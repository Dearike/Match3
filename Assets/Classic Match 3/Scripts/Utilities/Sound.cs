using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    This class is responsible for all sounds in the game
    It provides sound for taps, level end, swipes, clear and background music
*/

public class Sound : MonoBehaviour
{
    private AudioSource background;
    private AudioSource tap;
    private AudioSource levelEnd;
    private AudioSource swipe;
    private AudioSource clear;

    public GameObject _tap;

    public static Sound instance;

    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);

            instance = this;

            background = transform.Find("Background").GetComponent<AudioSource>();
            tap = transform.Find("Tap").GetComponent<AudioSource>();
            levelEnd = transform.Find("Level End").GetComponent<AudioSource>();
            swipe = transform.Find("Swipe").GetComponent<AudioSource>();
            clear = transform.Find("Clear").GetComponent<AudioSource>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic()
    {
        if (background.isPlaying) return;
        background.Play();
    }

    // Play swipe sound
    public void PlaySwipe()
    {
        swipe.Play();
    }

    // Level end sound
    public void PlayLevelEnd()
    {
        levelEnd.Play();
    }

    // Buttons sound
    public void PlayTap()
    {
        tap.Play();
    }

    // Play match sound
    public void PlayClear()
    {
        clear.Play();
    }

    // Mute or unmute music
    public void MuteMusic(bool mute)
    {
        if (mute)
            background.mute = true;
        else
            background.mute = false;
    }

    // Mute or unmute sounds
    public void MuteSounds(bool mute)
    {
        if (mute)
        {
            clear.mute = true;
            levelEnd.mute = true;
            tap.mute = true;
            swipe.mute = true;
        }
        else
        {
            clear.mute = false;
            levelEnd.mute = false;
            tap.mute = false;
            swipe.mute = false;
        }
    }
}
