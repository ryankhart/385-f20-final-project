using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateVillagerList : MonoBehaviour
{
    private GameObject header;
    private List<GameObject> villInfoLines;

    // Start is called before the first frame update
    void Start()
    {
        header = GameObject.Find("Text");
        villInfoLines = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddVillager(int index)
    {
        GameObject info = new GameObject("Text" + index);
        info.transform.SetParent(this.transform);
        float x = header.transform.localPosition.x;
        float y = header.transform.localPosition.y - 20;
        info.transform.localPosition = new Vector3(x, y, 0);
        info.transform.localScale = header.transform.localScale * 0.5f;
        villInfoLines.Add(info);
    }

    // TODO: public void RemoveVillager()
}
