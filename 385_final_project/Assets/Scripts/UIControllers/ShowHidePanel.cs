using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowHidePanel : MonoBehaviour
{
    private CanvasGroup group;

    // Start is called before the first frame update
    void Start()
    {
        group = GetComponent<CanvasGroup>();
    }

    // Update is called once per frame
    public void DisplayPanel()
    {
        while (group.alpha < 1)
        {
            group.alpha += Time.deltaTime;
        }
    }

    public void HidePanel()
    {
        while (group.alpha > 0)
        {
            group.alpha -= Time.deltaTime;
        }
    }
}
