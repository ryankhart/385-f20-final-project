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
        GenerateOtherTileGroups(waterTile, 0.86f); // water
        GenerateTrees();

        // position the camera in the middle of the map
        gameCamera.transform.position = new Vector3((mapSize / 2) * 0.86f, (mapSize / 2) * 0.86f, -10);
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

    // can be used for water, mountains, desert...
    private void GenerateOtherTileGroups(GameObject tilePrefab, float offset)
    {
        // choose a random plains tile on the map
        System.Random rand = new System.Random((int)DateTime.Now.Ticks);
        int indexX = rand.Next(0,mapSize);
        float tilePositionX = indexX * offset;
        int indexY = rand.Next(0,mapSize);
        float tilePositionY = indexY * offset;

        // destroy the plains tile
        Destroy(tileMap[indexX, indexY]);

        // place new water tile - this will be the origin of the body of water
        Vector3 waterTilePosition = new Vector3(tilePositionX, tilePositionY, 0);
        tileMap[indexX, indexY] = Instantiate(waterTile, waterTilePosition, Quaternion.identity);
        print("Step 1");

        // up to 25 procent of the surface can be water, in practice it is always less
        int waterCount = (int) ((mapSize * mapSize) * 0.25);
        int nextWater;

        // randomly generate a group of water tiles
        for(int i = 0; i < waterCount; )
        {
            rand = new System.Random((int)DateTime.Now.Ticks);
            nextWater = rand.Next(0,4);

            switch (nextWater)
            {
                case 1:
                    if (indexX - 1 >= 0)  // check if we are past the map edge
                    {
                        indexX -= 1;
                        tilePositionX = indexX * offset;
                        PlaceNewTile(tilePrefab, indexX, tilePositionX, indexY, tilePositionY);
                        i++;
                    }
                    break;
                case 2:
                    if (indexX + 1 < mapSize)  // check if we are past the map edge
                    {
                        indexX += 1;
                        tilePositionX = indexX * offset;
                        PlaceNewTile(tilePrefab, indexX, tilePositionX, indexY, tilePositionY);
                        i++;
                    }
                    break;
                case 3:
                    if (indexY - 1 >= 0)  // check if we are past the map edge
                    {
                        indexY -= 1;
                        tilePositionY = indexY * offset;
                        PlaceNewTile(tilePrefab, indexX, tilePositionX, indexY, tilePositionY);
                        i++;
                    }
                    break;
                default:
                    if (indexY + 1 < mapSize)  // check if we are past the map edge
                    {
                        indexY += 1;
                        tilePositionY = indexY * offset;
                        PlaceNewTile(tilePrefab, indexX, tilePositionX, indexY, tilePositionY);
                        i++;
                    }
                    break;
            }
        }
    }

    private void PlaceNewTile(GameObject prefab, int indexX, float tilePositionX, int indexY, float tilePositionY)
    {
        Vector3 newTilePosition;
        Destroy(tileMap[indexX, indexY]);
        newTilePosition = new Vector3(tilePositionX, tilePositionY, 0);
        tileMap[indexX, indexY] = Instantiate(prefab, newTilePosition, Quaternion.identity);
    }

    private void GenerateTrees()
    {
        System.Random rand = new System.Random((int)DateTime.Now.Ticks);
        // seed position for a tree
        int nextTreeX;
        int nextTreeY;

        // find a plains tile
        int treeCount = (int) (mapSize * mapSize * 0.3); // ~30% of map will be trees
        while (treeCount > 0)
        {
            nextTreeX = rand.Next(0, mapSize);
            nextTreeY = rand.Next(0, mapSize);

            if (tileMap[nextTreeX, nextTreeY].gameObject.tag.Equals("PlainsTile"))
            {
                float tilePositionX = tileMap[nextTreeX, nextTreeY].transform.position.x;
                float tilePositionY = tileMap[nextTreeX, nextTreeY].transform.position.y;
                Vector3 treePosition = new Vector3(tilePositionX + 0.43f, tilePositionY + 0.43f, 0);
                Instantiate(tree, treePosition, Quaternion.Euler(-90, 0, 0)); // rotatet to top down view
                print(tilePositionX + " : " + tilePositionY);
                treeCount--;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
