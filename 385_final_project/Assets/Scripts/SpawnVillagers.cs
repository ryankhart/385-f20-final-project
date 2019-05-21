using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnVillagers : MonoBehaviour
{
    public GameObject lumberjack;

    private int mapSize;
    private int currentNumHouses = 0;
    private List<GameObject> villagers;
    private StarterTileLayout mapStarterScript;

    private float tileOffset = 0.86f;
    private float centerOffset = 0.43f;

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
    }

    private void Update()
    {
        // one villager per house
        // TODO: come up with a better algorithm for ratio between houses and villagers
        currentNumHouses = GameObject.FindGameObjectsWithTag("Home").Length;
        if(villagers.Count < currentNumHouses && villagers.Count <= mapSize)
        {
            bool foundSpot = false;
            // find an empty plains tile
            for (int i = 0; i < mapSize; i++)
            {
                for (int j = 0; j < mapSize; j++)
                {
                    string tileTag = mapStarterScript.getTileTag(i,j);
                    if (tileTag.Equals("PlainsTile"))
                    {
                        //float lumberjackY = (float) (lumberjack.GetComponent<Head>().GetComponent<Renderer>().bounds.size.y + 0.5);
                        GameObject jack = Instantiate(lumberjack, new Vector3(i * tileOffset + centerOffset, 0.439f, j * tileOffset + centerOffset), Quaternion.identity);
                        villagers.Add(jack);
                        // TODO: ask Jonathan to add method to stop a position and not destroy the prefab
                        jack.GetComponent<TownFolkAI>().resourceTag = "Home";
                        foundSpot = true;
                        break;
                    }
                }
                if(foundSpot)
                {
                    break;
                }
            }
        }
    }
}
