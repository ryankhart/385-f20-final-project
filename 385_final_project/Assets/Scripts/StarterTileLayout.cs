using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarterTileLayout : MonoBehaviour
{
    public int mapSize = 10;
    public GameObject plainsTile;
    public GameObject waterTile;
    public GameObject tree;
    public Camera gameCamera;

    private GameObject[,] tileMap;

    // Start is called before the first frame update
    void Start()
    {
        // make map based on given size
        tileMap = new GameObject[mapSize, mapSize];

        GeneratePlains();
        GenerateWater();
        GenerateTrees();

        // position the camera in the middle of the map
        gameCamera.transform.position = new Vector3((mapSize / 2) * 0.86f, (mapSize / 2) * 0.86f, -10);
    }

    private void GenerateTrees()
    {
        System.Random rand = new System.Random((int)DateTime.Now.Ticks);

        // find a plains tile
        bool treeFound = false;
        while(!treeFound)
        {
            int nextTreeX = rand.Next(0, mapSize);
            int nextTreeY = rand.Next(0, mapSize);

            if (tileMap[nextTreeX, nextTreeY].gameObject.tag.Equals("PlainsTile"))
            {
                float tilePositionX = tileMap[nextTreeX, nextTreeY].transform.position.x;
                float tilePositionY = tileMap[nextTreeX, nextTreeY].transform.position.y;
                Vector3 treePosition = new Vector3(tilePositionX, tilePositionY, 0);
                Instantiate(tree, treePosition, Quaternion.Euler(-90,0,0));
                print(tilePositionX + " : " + tilePositionY);
                treeFound = true;
            }
        }
    }

    private void GeneratePlains()
    {
        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                Vector3 plainTilePosition = new Vector3(i * 0.86f, j * 0.86f, 0);
                tileMap[i, j] = Instantiate(plainsTile, plainTilePosition, Quaternion.identity);
            }
        }
    }

    private void GenerateWater()
    {
        // choose a random plains tile on the map
        System.Random rand = new System.Random();
        int waterIndexX = rand.Next(0,mapSize);
        float waterPositionX = waterIndexX * 0.86f;
        rand = new System.Random();
        int waterIndexY = rand.Next(0,mapSize);
        float waterPositionY = waterIndexY * 0.86f;

        // destroy the plains tile
        Destroy(tileMap[waterIndexX, waterIndexY]);

        // place new water tile - this will be the origin of the body of water
        Vector3 waterTilePosition = new Vector3(waterPositionX, waterPositionY, 0);
        tileMap[waterIndexX, waterIndexY] = Instantiate(waterTile, waterTilePosition, Quaternion.identity);
        print("Step 1");

        int waterCount = (int) ((mapSize * mapSize) * 0.20); // up to 20 procent of the surface can be water
        int nextWater;
        for(int i = 0; i < waterCount; )
        {
            rand = new System.Random((int)DateTime.Now.Ticks);
            nextWater = rand.Next(0,4);

            switch (nextWater)
            {
                case 1:
                    if (waterIndexX - 1 >= 0)  // check if we are past the map edge
                    {
                        waterIndexX -= 1;
                        waterPositionX = waterIndexX * 0.86f;
                        PlaceNewWaterTile(waterIndexX, waterPositionX, waterIndexY, waterPositionY);
                        i++;
                    }
                    break;
                case 2:
                    if (waterIndexX + 1 < mapSize)  // check if we are past the map edge
                    {
                        waterIndexX += 1;
                        waterPositionX = waterIndexX * 0.86f;
                        PlaceNewWaterTile(waterIndexX, waterPositionX, waterIndexY, waterPositionY);
                        i++;
                    }
                    break;
                case 3:
                    if (waterIndexY - 1 >= 0)  // check if we are past the map edge
                    {
                        waterIndexY -= 1;
                        waterPositionY = waterIndexY * 0.86f;
                        PlaceNewWaterTile(waterIndexX, waterPositionX, waterIndexY, waterPositionY);
                        i++;
                    }
                    break;
                default:
                    if (waterIndexY + 1 < mapSize)  // check if we are past the map edge
                    {
                        waterIndexY += 1;
                        waterPositionY = waterIndexY * 0.86f;
                        PlaceNewWaterTile(waterIndexX, waterPositionX, waterIndexY, waterPositionY);
                        i++;
                    }
                    break;
            }
        }
    }

    private void PlaceNewWaterTile(int waterX, float waterPositionX, int waterY, float waterPositionY)
    {
        Vector3 waterTilePosition;
        Destroy(tileMap[waterX, waterY]);
        waterTilePosition = new Vector3(waterPositionX, waterPositionY, 0);
        tileMap[waterX, waterY] = Instantiate(waterTile, waterTilePosition, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
