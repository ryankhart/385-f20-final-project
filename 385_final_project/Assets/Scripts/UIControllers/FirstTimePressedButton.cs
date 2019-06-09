using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstTimePressedButton : MonoBehaviour
{
    private bool pressedFirstTime;
    private float waitTime;

    void Start()
    {
        pressedFirstTime = false;
        Invoke("DontDisplayHint", 50);
    }

    public void UserPressedButton()
    {
        // do this exactly once
        if(!pressedFirstTime)
        {
            // after closing the panel for the very first time, show the last hint
            StartCoroutine(GameObject.Find("Hints").GetComponent<DisplayHints>().DisplayHint("HaveFunHint", 5));
            pressedFirstTime = true;
        }
    }

    private void DontDisplayHint()
    { 
        // if after this time user didn't use the close button, don't show the last hint
        // it would seem out of context
        if(!pressedFirstTime)
        {
            pressedFirstTime = true;
        }
    }
}
