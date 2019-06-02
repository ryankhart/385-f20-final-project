using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateVillagerList : MonoBehaviour
{
    private GameObject header;
    private RectTransform panelTrans;
    private List<GameObject> villInfoLines;

    private List<GameObject> villagers;
    private int lastListLength;

    float timeToUpdate;

    // Start is called before the first frame update
    void Start()
    {
        header = GameObject.Find("Text");
        panelTrans = header.GetComponent<RectTransform>();
        villInfoLines = new List<GameObject>();
        villagers = GameObject.Find("VillagerSpawner").GetComponent<SpawnVillagers>().getVillagerList();

        // set the initual update time 5 seconds
        timeToUpdate = Time.fixedTime + 5.0f;
    }

    void FixedUpdate()
    {
        // Every 2 seconds
        // Get new villager list and check it for status and current resource task
        if(Time.fixedTime >= timeToUpdate)
        {
            if (villagers != null && villagers.Count > 0)
            {
                villagers = GameObject.Find("VillagerSpawner").GetComponent<SpawnVillagers>().getVillagerList();
                lastListLength = villagers.Count;

                int index;
                string task, status;
                foreach (GameObject villager in villagers)
                {
                    TownFolkAI script = villager.GetComponent<TownFolkAI>();
                    index = villagers.IndexOf(villager);
                    task = script.lastResource;
                    status = script.state;
                    villInfoLines[index].GetComponent<Text>().text = "Villager " + index + "\t\t\t\t" + task + "\t\t\t\t\t" + status;
                }
            }
            // update every 1 second
            timeToUpdate = Time.fixedTime + 1.0f;
        }
    }

    public void AddVillagerToMenu(int index, GameObject villager)
    {
        // make new object with text
        GameObject infoObj = new GameObject("Text " + index);
        infoObj.transform.SetParent(this.transform);

        // get info for the text
        string currentTask = villager.GetComponent<TownFolkAI>().lastResource;
        string currentState = villager.GetComponent<TownFolkAI>().state;
        infoObj.AddComponent<Text>().text = "Villager " + index + "\t\t\t" + currentTask + "\t\t\t" + currentState;
        Text infoText = infoObj.GetComponent<Text>();

        // position and scale
        infoText.GetComponent<RectTransform>().localScale = panelTrans.lossyScale;

        float x = panelTrans.position.x - 599;
        float y = header.transform.localScale.y + 170 - (30 * index);
        infoObj.transform.localPosition = new Vector3(x, y, 0);
        infoObj.transform.localScale = header.transform.lossyScale;

        // add font to make text actually appear on scree
        Font ArialFont = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
        infoText.font = ArialFont;
        infoText.material = ArialFont.material;
        infoText.fontSize = 16;
        infoText.horizontalOverflow = HorizontalWrapMode.Overflow;

        // infoObj index corresponds to villager index in the villager list
        villInfoLines.Add(infoObj);
        // get updated villager list
        villagers = GameObject.Find("VillagerSpawner").GetComponent<SpawnVillagers>().getVillagerList();
        lastListLength = villagers.Count;
    }

    // TODO: public void RemoveVillager()
}
