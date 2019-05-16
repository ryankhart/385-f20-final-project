using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarterTileLayout : MonoBehaviour
{
    public int mapSize = 10;
    public GameObject plainsTile;
    public GameObject waterTile;
    public GameObject rockTile;
    public GameObject tree;
    public Camera gameCamera;

    private GameObject[,] tileMap;
    private float tileOffset = 0.86f; // Default offset

    private Vector3 TilePosition(float x, float y, float z)
    {
        return new Vector3(x * tileOffset, y * tileOffset, z);
    }

    private Vector3 TilePosition(int x, int y, int z, float offset_override)
    {
        return new Vector3(x * offset_override, y * offset_override, z);
    }

    // Start is called before the first frame update
    void Start()
    {
        // make map based on given size
        tileMap = new GameObject[mapSize, mapSize];

        GeneratePlains();
        GenerateOtherTileGroups(waterTile, 0.86f, (int)(mapSize * mapSize * 0.25)); // up to 25% of map is water
        GenerateOtherTileGroups(rockTile, 0.86f, (int)(mapSize * mapSize * 0.15)); // up to 15% of map is rock
        GenerateTrees();

        // position the camera in the middle of the map
        gameCamera.transform.position = TilePosition((mapSize / 2), (mapSize / 2), -10);
    }

    private void GeneratePlains()
    {
        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                tileMap[i, j] = Instantiate(plainsTile, TilePosition(i, j, 0), Quaternion.identity);
            }
        }
    }

    // can be used for water, mountains, desert...
    private void GenerateOtherTileGroups(GameObject tilePrefab, float offset, int percentage)
    {
        // choose a random plains tile on the map
        System.Random rand = new System.Random((int)DateTime.Now.Ticks);
        int indexX = rand.Next(0, mapSize);
        int indexY = rand.Next(0, mapSize);

        // destroy the plains tile
        Destroy(tileMap[indexX, indexY]);

        // place new terrain tile - this will be the origin of the group of terrain tiles
        tileMap[indexX, indexY] = Instantiate(tilePrefab, TilePosition(indexX, indexY, 0), Quaternion.identity);

        int nextRandValue;

        // randomly generate a group of water tiles
        // postcondition: the number of tiles placed into the map is generally less than the percentage
        for (int i = 0; i < percentage;)
        {
            rand = new System.Random((int)DateTime.Now.Ticks);
            nextRandValue = rand.Next(0, 4);

            switch (nextRandValue)
            {
                case 1: // west
                    if (indexX - 1 >= 0)  // check if we are past the map edge
                    {
                        indexX -= 1;
                        PlaceNewTile(tilePrefab, indexX, indexY);
                        i++;
                    }
                    break;
                case 2: // east
                    if (indexX + 1 < mapSize)  // check if we are past the map edge
                    {
                        indexX += 1;
                        PlaceNewTile(tilePrefab, indexX, indexY);
                        i++;
                    }
                    break;
                case 3: // south
                    if (indexY - 1 >= 0)  // check if we are past the map edge
                    {
                        indexY -= 1;
                        PlaceNewTile(tilePrefab, indexX, indexY);
                        i++;
                    }
                    break;
                default: // north
                    if (indexY + 1 < mapSize)  // check if we are past the map edge
                    {
                        indexY += 1;
                        PlaceNewTile(tilePrefab, indexX, indexY);
                        i++;
                    }
                    break;
            }
        }
    }

    private void PlaceNewTile(GameObject prefab, int indexX, int indexY)
    {
        Destroy(tileMap[indexX, indexY]);
        tileMap[indexX, indexY] = Instantiate(prefab, TilePosition(indexX, indexY, 0), Quaternion.identity);
    }

    private void GenerateTrees()
    {
        System.Random rand = new System.Random((int)DateTime.Now.Ticks);
        // seed position for a tree
        int nextTreeX;
        int nextTreeY;

        // find a plains tile
        int treeCount = (int)(mapSize * mapSize * 0.3); // ~30% of map will be trees
        while (treeCount > 0)
        {
            nextTreeX = rand.Next(0, mapSize);
            nextTreeY = rand.Next(0, mapSize);

            if (tileMap[nextTreeX, nextTreeY].gameObject.tag.Equals("PlainsTile"))
            {
                Vector3 treePosition = TilePosition(nextTreeX, nextTreeY, 0);
                Instantiate(tree, treePosition, Quaternion.Euler(-90, 0, 0)); // rotate to top down view
                treeCount--;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}