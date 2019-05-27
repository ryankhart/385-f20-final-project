using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayHints : MonoBehaviour
{
    private CanvasGroup buildingHint;
    private CanvasGroup resourcesHint;
    private CanvasGroup villagerOverviewHint;

    void Start()
    { 
        buildingHint = transform.GetChild(0).GetComponent<CanvasGroup>();
        resourcesHint = transform.GetChild(1).GetComponent<CanvasGroup>();
        villagerOverviewHint = transform.GetChild(2).GetComponent<CanvasGroup>();

        StartCoroutine(WaitToDisplayBuildingHint());
    }

    private IEnumerator WaitToDisplayBuildingHint()
    {
        yield return new WaitForSeconds(10);
        if (buildingHint != null)
        {
            while (buildingHint.alpha < 1)
            {
                buildingHint.alpha += Time.deltaTime / 2;
                yield return null;
            }
        }

        yield return null;
    }

    public void HideHint(string hintName)
    {
        print("HIT TOO");
        CanvasGroup group = null;
        for(int i = 0; i < transform.childCount; i++)
        { 
            if(transform.GetChild(i).name == hintName)
            {
                group = transform.GetChild(i).GetComponent<CanvasGroup>();
            }
        }
        if (buildingHint != null)
        {
            while (buildingHint.alpha > 0)
            {
                buildingHint.alpha -= Time.deltaTime / 2;
            }
        }
    }
}
