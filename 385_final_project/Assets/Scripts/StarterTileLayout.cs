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
    public GameObject GridCreator;
    public GameObject TownMan;
    public Camera gameCamera;

    private GameObject[,] tileMap;
    private float tileOffset = 0.860000f; // Default offset

    private float testVal = .50f;

    private Vector3 TilePosition(float x, float y, float z)
    {
        return new Vector3(x * tileOffset, y, z * tileOffset);
    }

    private Vector3 TilePosition(int x, int y, int z, float offset_override)
    {
        return new Vector3(x * offset_override, y, z * tileOffset);
    }

    // Start is called before the first frame update
    void Start()
    {
        // instantiate the grid creator, get the map size from it
        GameObject gridCreator = Instantiate(GridCreator, new Vector3(0, 0, 0), Quaternion.identity);
        mapSize = gridCreator.GetComponent<GridManager>().GetComponent<GridManager>().numberOfColumns;
        print(mapSize);

        // make map based on given size
        tileMap = new GameObject[mapSize, mapSize];

        GeneratePlains();
        GenerateOtherTileGroups(waterTile, tileOffset, (int)(mapSize * mapSize * 0.25)); // up to 25% of map is water
        GenerateOtherTileGroups(rockTile, tileOffset, (int)(mapSize * mapSize * 0.15)); // up to 15% of map is rock
        GenerateTrees();

        // position the camera in the middle of the map
        gameCamera.transform.position = TilePosition((mapSize / 2f), 10, (mapSize / 2f));
        gameCamera.transform.rotation = Quaternion.Euler(90,0,0);

        Instantiate(TownMan, new Vector3(1, .2f, 1), Quaternion.identity);
    }

    private void GeneratePlains()
    {
        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                tileMap[i, j] = Instantiate(plainsTile, TilePosition(i+testVal, 0, j+testVal), Quaternion.Euler(90,0,0));
            }
        }
    }

    // can be used for water, mountains, desert...
    private void GenerateOtherTileGroups(GameObject tilePrefab, float offset, int percentage)
    {
        // choose a random plains tile on the map
        System.Random rand = new System.Random(Guid.NewGuid().GetHashCode());
        int indexX = rand.Next(0, mapSize);
        int indexZ = rand.Next(0, mapSize);

        // destroy the plains tile
        Destroy(tileMap[indexX, indexZ]);

        // place new terrain tile - this will be the origin of the group of terrain tiles
        tileMap[indexX, indexZ] = Instantiate(tilePrefab, TilePosition(indexX+testVal, 0, indexZ+testVal), Quaternion.Euler(90, 0, 0));

        int nextRandValue;

        // randomly generate a group of water tiles
        // postcondition: the number of tiles placed into the map is generally less than the percentage
        for (int i = 0; i < percentage;)
        {
            rand = new System.Random(Guid.NewGuid().GetHashCode());
            nextRandValue = rand.Next(0, 4);

            switch (nextRandValue)
            {
                case 1: // west
                    if (indexX - 1 >= 0)  // check if we are past the map edge
                    {
                        indexX -= 1;
                        PlaceNewTile(tilePrefab, indexX, indexZ);
                        i++;
                    }
                    break;
                case 2: // east
                    if (indexX + 1 < mapSize)  // check if we are past the map edge
                    {
                        indexX += 1;
                        PlaceNewTile(tilePrefab, indexX, indexZ);
                        i++;
                    }
                    break;
                case 3: // south
                    if (indexZ - 1 >= 0)  // check if we are past the map edge
                    {
                        indexZ -= 1;
                        PlaceNewTile(tilePrefab, indexX, indexZ);
                        i++;
                    }
                    break;
                default: // north
                    if (indexZ + 1 < mapSize)  // check if we are past the map edge
                    {
                        indexZ += 1;
                        PlaceNewTile(tilePrefab, indexX, indexZ);
                        i++;
                    }
                    break;
            }
        }
    }

    private void PlaceNewTile(GameObject prefab, int indexX, int indexZ)
    {
        Destroy(tileMap[indexX, indexZ]);
        tileMap[indexX, indexZ] = Instantiate(prefab, TilePosition(indexX+testVal, 0, indexZ+testVal), Quaternion.Euler(90, 0, 0));
    }

    private void GenerateTrees()
    {
        System.Random rand = new System.Random(Guid.NewGuid().GetHashCode());
        // seed position for a tree
        int nextTreeX;
        int nextTreeZ;

        // find a plains tile
        int treeCount = (int)(mapSize * mapSize * 0.3); // ~30% of map will be trees
        while (treeCount > 0)
        {
            nextTreeX = rand.Next(0, mapSize);
            nextTreeZ = rand.Next(0, mapSize);

            if (tileMap[nextTreeX, nextTreeZ].gameObject.tag.Equals("PlainsTile"))
            {
                Vector3 treePosition = TilePosition(nextTreeX+testVal, 0, nextTreeZ+testVal);
                Instantiate(tree, treePosition, Quaternion.Euler(0, 0, 0)); // rotate to top down view
                tileMap[nextTreeX, nextTreeZ].gameObject.tag = "PlainsTileWithTree";
                treeCount--;
            }
        }
    }

    public string getTileTag(int x, int z)
    { 
        return tileMap[x,z].gameObject.tag;
    }

    public void setTileTag(int x, int z, string newTag)
    {
        tileMap[x, z].gameObject.tag = newTag;
    }
}