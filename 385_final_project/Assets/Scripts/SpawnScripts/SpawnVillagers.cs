﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SpawnVillagers : MonoBehaviour
{
    public GameObject lumberjack;
    public GameObject monster;
    public GameObject fighter;

    private int mapSize;
    private int currentNumHouses = 0;
    private int currentNumForts = 0;
    public List<GameObject> villagers;
    private List<GameObject> monsters;
    private List<GameObject> fighters;
    private GameObject[] houses;
    private GameObject[] forts;
    private StarterTileLayout mapStarterScript;
    private float elapsedTime = 0.0f;

    private Text foodCount;

    //private float tileOffset = 0.86f;
    //private float centerOffset = 0.43f;

    private void Start()
    {
        // get map size
        GameObject mapStarter = GameObject.Find("TileLayoutStarter");
        mapStarterScript = mapStarter.GetComponent<StarterTileLayout>();
        GameObject gridCreator = GameObject.Find("GridCreator(Clone)");
        mapSize = gridCreator.GetComponent<GridManager>().numberOfColumns;

        // limit max num of villagers to a number based on map size
        // TODO: come up with a better number selection
        villagers = new List<GameObject>(mapSize);
        monsters = new List<GameObject>(mapSize);
        fighters = new List<GameObject>(mapSize);

        foodCount = GameObject.Find("FarmFoodCount").GetComponent<Text>();
    }

    private void Update()
    {
        // one villager per house
        // TODO: come up with a better algorithm for ratio between houses and villagers
        houses = GameObject.FindGameObjectsWithTag("Home");
        forts = GameObject.FindGameObjectsWithTag("Fort");
        currentNumHouses = houses.Length;
        currentNumForts = forts.Length;
        if (currentNumHouses > 0 && villagers.Count < currentNumHouses && villagers.Count <= mapSize)
        {
            int currFoodCount = int.Parse(foodCount.text);
            if (currFoodCount > 5 * villagers.Count + 5)
            {
                // spawn villager at current home
                float jackX = houses[currentNumHouses - 1].transform.position.x;
                float jackZ = houses[currentNumHouses - 1].transform.position.z;
                GameObject jack = Instantiate(lumberjack, new Vector3(jackX, 0.439f, jackZ), Quaternion.identity);
                string currentResource = jack.GetComponent<TownFolkAI>().lastResource;
                string state = jack.GetComponent<TownFolkAI>().state;
                villagers.Add(jack);
                if (villagers.Count == 1)
                {
                    DisplayHints script = GameObject.Find("Hints").GetComponent<DisplayHints>();
                    StartCoroutine(script.DisplayHint("VillagersFunctionHint", 5));
                    StartCoroutine(script.DisplayHint("VillagersOverview", 5));
                }
                GameObject.Find("VillagerInfo").GetComponent<UpdateVillagerUIList>().AddVillagerToMenu(villagers.Count, jack);
            }
        }

        if (currentNumForts > 0 && fighters.Count < currentNumForts && fighters.Count <= mapSize)
        {
            // spawn villager at current home
            float jackX = forts[currentNumForts - 1].transform.position.x;
            float jackZ = forts[currentNumForts - 1].transform.position.z;
            GameObject jack = Instantiate(fighter, new Vector3(jackX, 0.439f, jackZ), Quaternion.identity);
            fighters.Add(jack);
        }

        // monster spawns after every 4 villagers
        if (villagers.Count > 4)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime > 10)
            {
                elapsedTime = 0.0f;
                GameObject[] trees = GameObject.FindGameObjectsWithTag("Tree");
                if (trees.Length > 0)
                {
                    int rand = UnityEngine.Random.Range(0, trees.Length);
                    float monsterX = trees[rand].transform.position.x;
                    float monsterZ = trees[rand].transform.position.z;

                    GameObject mon = Instantiate(monster, new Vector3(monsterX, 0.439f, monsterZ), Quaternion.identity);
                    monsters.Add(mon);
                }
            }
           
        }
    }

    public List<GameObject> getVillagerList()
    {
        return villagers;
    }
}
