using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

/*
    This class is used as a scenes switcher. You can also implement here any logic you want to pass through all game.
*/

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject faderObject;
    public Image faderImage;

    private float fadeSpeed = 0.02f;

    private Color fadeTransparency = new Color(0, 0, 0, 0.04f);
    private AsyncOperation async;

    private void Start()
    {
        Application.ExternalCall("ShowAd");

        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = GetComponent<GameManager>();
            SceneManager.sceneLoaded += OnLevelFinishedLoading;

			// Load saved data from the storage
			Storage.Init();

			Sound.instance.PlayMusic();

            Menu.instance.SetSettings();

            DOTween.SetTweensCapacity(500, 312);
		}
		else
        {
            Destroy(gameObject);
        }
    }

    // Load a scene with a specified string name
    public void LoadScene(string sceneName)
    {
        instance.StartCoroutine(Load(sceneName));
        instance.StartCoroutine(FadeOut(instance.faderObject, instance.faderImage));
    }

    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        instance.StartCoroutine(FadeIn(instance.faderObject, instance.faderImage));
    }

    //Iterate the fader transparency to 100%
    IEnumerator FadeOut(GameObject faderObj, Image fader)
    {
        faderObj.SetActive(true);
        while (fader.color.a < 1)
        {
            fader.color += fadeTransparency;
            yield return new WaitForSeconds(fadeSpeed);
        }
        //Activate the scene when the fade ends
        ActivateScene();
    }

    // Iterate the fader transparency to 0%
    IEnumerator FadeIn(GameObject faderObj, Image fader)
    {
        while (fader.color.a > 0)
        {
            fader.color -= fadeTransparency;
            yield return new WaitForSeconds(fadeSpeed);
        }
        faderObj.SetActive(false);
    }

    // Begin loading a scene with a specified string asynchronously
    IEnumerator Load(string sceneName)
    {
        async = SceneManager.LoadSceneAsync(sceneName);
        async.allowSceneActivation = false;
        yield return async;
    }

    // Allows the scene to change once it is loaded
    public void ActivateScene()
    {
        async.allowSceneActivation = true;
    }
}
