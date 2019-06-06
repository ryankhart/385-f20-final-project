using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateVillagerUIList : MonoBehaviour
{
    // text prefabs
    public Text villID;
    public Text villTask;
    public Text villStatus;

    // UI manipulation
    private GameObject container;
    private RectTransform containerTrans;
    private List<Text> villIDs;
    private List<Text> villTasks;
    private List<Text> villStatuses;

    // outside objects
    private List<GameObject> villagers;
    private int lastListLength;

    // math stuff
    float timeToUpdate;

    void Start()
    {
        container = GameObject.Find("TextContainerPanel");
        containerTrans = container.GetComponent<RectTransform>();
        villIDs = new List<Text>();
        villTasks = new List<Text>();
        villStatuses = new List<Text>();

        // set the initual update time 5 seconds
        timeToUpdate = Time.fixedTime + 5.0f;
    }

    void FixedUpdate()
    {
        // Every 1 second
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
                    int tableID = index + 1;
                    task = script.lastResource;
                    status = script.state;

                    villTasks[index].text = task;
                    villStatuses[index].text = status;
                }
            }
            // update every 1 second
            timeToUpdate = Time.fixedTime + 1.0f;
        }

        //foreach(GameObject line in villInfoLines)
        //{ 
        //    // make the object invisible somehow
        //}
    }

    public void AddVillagerToMenu(int index, GameObject villager)
    {
        Text id = Instantiate(villID, containerTrans);
        float x = container.transform.localScale.x - 890;
        float y = containerTrans.localScale.y + 805 - (40 * index);
        id.transform.localPosition = new Vector3(x, y, 0);
        id.text = "Villager " + index;
        villIDs.Add(id);

        Text task = Instantiate(villTask, containerTrans);
        x = container.transform.localScale.x + 30;
        task.transform.localPosition = new Vector3(x, y, 0);
        task.text = villager.GetComponent<TownFolkAI>().lastResource;
        villTasks.Add(task);

        Text status = Instantiate(villID, containerTrans);
        x = container.transform.localScale.x + 900;
        status.transform.localPosition = new Vector3(x, y, 0);
        status.text = villager.GetComponent<TownFolkAI>().state;
        villStatuses.Add(status);

        villagers = GameObject.Find("VillagerSpawner").GetComponent<SpawnVillagers>().getVillagerList();
        lastListLength = villagers.Count;
    }

    // TODO: public void RemoveVillager()
}
