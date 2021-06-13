using System.Collections;
using UnityEngine;

public class Pulse : MonoBehaviour {

    public float approachSpeed = 0.005f;
    public float growthBound = 1.05f;
    public float shrinkBound = 0.95f;
    public float currentRatio = 1;

    private Coroutine routine;
    private bool keepGoing = true;
    private bool closeEnough = false;

    private void OnEnable()
    {
        routine = StartCoroutine(PulseEffect());
    }

    IEnumerator PulseEffect()
    {
        // Run this indefinitely
        while (keepGoing)
        {
            // Get bigger for a few seconds
            while (currentRatio != growthBound)
            {
                // Determine the new ratio to use
                currentRatio = Mathf.MoveTowards( currentRatio, growthBound, approachSpeed);
 
                // Update our text element
                transform.localScale = Vector3.one * currentRatio;

                yield return new WaitForEndOfFrame();
            }
 
            // Shrink for a few seconds
            while (currentRatio != shrinkBound)
            {
                // Determine the new ratio to use
                currentRatio = Mathf.MoveTowards( currentRatio, shrinkBound, approachSpeed);
 
                // Update our text element
                transform.localScale = Vector3.one * currentRatio;
 
                yield return new WaitForEndOfFrame();
            }
        }
    }

    public void Stop()
    {
        StopCoroutine(routine);
    }
}
