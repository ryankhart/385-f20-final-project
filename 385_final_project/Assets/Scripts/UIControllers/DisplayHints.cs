using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayHints : MonoBehaviour
{
    private Dictionary<string, Transform> hints;

    void Start()
    {
        hints = new Dictionary<string, Transform>();
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<CanvasGroup>())
            {
                hints.Add(transform.GetChild(i).name, transform.GetChild(i));
            }
        }
        StartCoroutine(WaitToDisplay());
    }

    public IEnumerator WaitToDisplay()
    {
        yield return new WaitForSeconds(10);
        CanvasGroup group = null;
        foreach (KeyValuePair<string, Transform> item in hints)
        {
            print(item.Key);
            if (item.Key == "BuildingHint")
            {
                group = item.Value.GetComponent<CanvasGroup>();
            }
        }
        if (group != null)
        {
            while (group.alpha < 1)
            {
                group.alpha += Time.deltaTime / 2;
                yield return null;
            }
        }

        yield return null;
    }
}
