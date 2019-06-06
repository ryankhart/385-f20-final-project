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

        StartCoroutine(WaitToDisplayBuildingHint());
    }

    private IEnumerator WaitToDisplayBuildingHint()
    {
        yield return new WaitForSeconds(1);
        if (GameObject.FindGameObjectWithTag("VillageCenter") == null && GameObject.FindGameObjectWithTag("MovingBuilding") == null)
        {
            if (buildingHint != null)
            {
                while (buildingHint.alpha < 1)
                {
                    buildingHint.alpha += Time.deltaTime;
                    yield return null;
                }
            }
        }
        HideHint("BuildingHint");
        yield return null;
    }

    public IEnumerator DisplayHint(string hintName, int forTime)
    {
        if (hintName == "BuildHousesHint")
        {
            yield return new WaitForSeconds(5);
        }
        if (hintName == "VillagersOverview")
        {
            // 5 seconds is too long for this hint
            yield return new WaitForSeconds(4);
        }

        CanvasGroup group = GetCanvasGroup(hintName);
        if (group != null)
        {
            while (group.alpha < 1)
            {
                group.alpha += Time.deltaTime;
                yield return null;
            }
        }
        yield return new WaitForSeconds(forTime);
        StartCoroutine(HideHint(hintName));
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
