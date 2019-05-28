using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayHints : MonoBehaviour
{
    private CanvasGroup buildingHint;
    //private CanvasGroup resourcesHint;
    //private CanvasGroup villagerOverviewHint;

    void Start()
    { 
        buildingHint = transform.GetChild(0).GetComponent<CanvasGroup>();
        // TODO: maybe won't be needed
        //resourcesHint = transform.GetChild(1).GetComponent<CanvasGroup>();
        //villagerOverviewHint = transform.GetChild(2).GetComponent<CanvasGroup>();

        StartCoroutine(WaitToDisplayBuildingHint());
    }

    private IEnumerator WaitToDisplayBuildingHint()
    {
        yield return new WaitForSeconds(9);
        if (buildingHint != null)
        {
            while (buildingHint.alpha < 1)
            {
                buildingHint.alpha += Time.deltaTime;
                yield return null;
            }
        }

        yield return null;
    }

    public IEnumerator DisplayHint(string hintName)
    {
        CanvasGroup group = GetCanvasGroup(hintName);
        if (group != null)
        {
            while (group.alpha < 1)
            {
                group.alpha += Time.deltaTime;
                yield return null;
            }
        }
        // special case for game start - display player hints one after another
        if (hintName == "PlayerActionHint (1)")
        {
            yield return new WaitForSeconds(3);
            StartCoroutine(HideHint(hintName));
            yield return new WaitForSeconds(2);
            StartCoroutine(DisplayHint("PlayerActionHint (2)"));
            yield return new WaitForSeconds(5);
            StartCoroutine(HideHint("PlayerActionHint (2)"));
        }
        if(hintName == "NotEnoughHint")
        {
            yield return new WaitForSeconds(3);
            StartCoroutine(HideHint(hintName));
        }
    }

    public IEnumerator HideHint(string hintName)
    {
        CanvasGroup group = GetCanvasGroup(hintName);
        if (group != null)
        {
            while (group.alpha > 0)
            {
                group.alpha -= Time.deltaTime;
                yield return null;
            }
        }
    }

    private CanvasGroup GetCanvasGroup(string hintName)
    {
        CanvasGroup group = null;
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).name == hintName)
            {
                group = transform.GetChild(i).GetComponent<CanvasGroup>();
            }
        }
        return group;
    }
}
