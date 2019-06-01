﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnVillagers : MonoBehaviour
{
    public GameObject lumberjack;
    public GameObject monster;

    private int mapSize;
    private int currentNumHouses = 0;
    private List<GameObject> villagers;
    private List<GameObject> monsters;
    private GameObject[] houses;
    private StarterTileLayout mapStarterScript;

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
    }

    private void Update()
    {
        // one villager per house
        // TODO: come up with a better algorithm for ratio between houses and villagers
        houses = GameObject.FindGameObjectsWithTag("Home");
        currentNumHouses = houses.Length;
        if(currentNumHouses > 0 && villagers.Count < currentNumHouses && villagers.Count <= mapSize)
        {
            // spawn villager at current home
            float jackX = houses[currentNumHouses - 1].transform.position.x;
            float jackZ = houses[currentNumHouses - 1].transform.position.z;
            GameObject jack = Instantiate(lumberjack, new Vector3(jackX, 0.439f, jackZ), Quaternion.identity);
            villagers.Add(jack);
            GameObject.Find("Panel").GetComponent<UpdateVillagerList>().AddVillager(villagers.Count);
        }
        // monster spawns after every 4 villagers
        if (villagers.Count > 0 && villagers.Count % 4 == 0 && monsters.Count < (int) (villagers.Count / 4))
        {
            GameObject[] trees = GameObject.FindGameObjectsWithTag("Tree");
            if(trees.Length > 0)
            {
                float monsterX = trees[trees.Length / 2].transform.position.x;
                float monsterZ = trees[trees.Length / 2].transform.position.z;
                GameObject mon = Instantiate(monster, new Vector3(monsterX, 0.439f, monsterZ), Quaternion.identity);
                monsters.Add(mon);
            }
        }
    }
}
