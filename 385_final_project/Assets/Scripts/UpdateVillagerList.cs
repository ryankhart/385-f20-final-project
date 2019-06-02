using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateVillagerList : MonoBehaviour
{
    private GameObject header;
    private RectTransform panelTrans;
    private List<GameObject> villInfoLines;

    // Start is called before the first frame update
    void Start()
    {
        header = GameObject.Find("Text");
        panelTrans = header.GetComponent<RectTransform>();
        villInfoLines = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddVillagerToMenu(int index)
    {
        GameObject info = new GameObject("Text " + index);
        info.transform.SetParent(this.transform);
        info.AddComponent<Text>().text = "Hello " + index;
        Text infoText = info.GetComponent<Text>();

        infoText.GetComponent<RectTransform>().localScale = panelTrans.lossyScale;

        float x = panelTrans.position.x - 599;
        float y = header.transform.localScale.y + 170 - (30 * index);
        info.transform.localPosition = new Vector3(x, y, 0);
        info.transform.localScale = header.transform.lossyScale;

        Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
        infoText.font = ArialFont;
        infoText.material = ArialFont.material;
        infoText.fontSize = 16;
        infoText.horizontalOverflow = HorizontalWrapMode.Overflow;

        villInfoLines.Add(info);
    }

    // TODO: public void RemoveVillager()
}
