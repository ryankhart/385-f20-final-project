using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayBuildingHint : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(WaitToDisplay());
    }

    private IEnumerator WaitToDisplay()
    {
        yield return new WaitForSeconds(7);
        CanvasGroup group = GetComponentInChildren<CanvasGroup>();
        while(group.alpha < 1)
        {
            group.alpha += Time.deltaTime / 2;
            yield return null;
        }

        yield return null;
    }
}
