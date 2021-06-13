using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    This class allows you to handle camera size properly
*/

public class Aspect : MonoBehaviour
{
	public GameObject UI;

    void Awake()
    {
        float windowaspect = (float)Screen.width / (float)Screen.height;

        // 12.9 inches iPads
        if (windowaspect < 0.8f)
            Camera.main.orthographicSize = 7.5f;

        // 5.5 inches iPhones
        if (windowaspect < 0.6f)
                Camera.main.orthographicSize = 10;

		// 6.5 inches iPhones
		if (windowaspect < 0.5f)
        {
            Camera.main.orthographicSize = 12;
            // We should lower UI a bit to deal with notch
            UI.transform.localPosition -= new Vector3(0, 75, 0);
        }
    }
}
