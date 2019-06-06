using System;
using System.Collections;
using UnityEngine;

public class StarterTileLayout : MonoBehaviour
{
    public int mapSize = 10;
    public GameObject plainsTile;
    public GameObject waterTile;
    public GameObject rockTile;
    public GameObject tree1;
    public GameObject tree2;
    public GameObject tree3;
    public GameObject stone;
    public GameObject copper;
    public GameObject GridCreator;
    public GameObject TownMan;
    public GameObject Sparkles;
    public GameObject Player;
    public Camera gameCamera;

    private GameObject[,] tileMap;
    private Vector3 objPosition;
    private float tileOffset = 0.860000f; // Default offset

    private readonly float centerOfTileOffset = .50f;

    private Vector3 TilePosition(float x, float y, float z)
    {
        return new Vector3(x * tileOffset, y, z * tileOffset);
    }

    private Vector3 TilePosition(int x, int y, int z, float offset_override)
    {
        return new Vector3(x * offset_override, y, z * tileOffset);
    }

    // Start is called before the first frame update
    void Awake()
    {
        // instantiate the grid creator, get the map size from it
        GameObject gridCreator = Instantiate(GridCreator, new Vector3(0, 0, 0), Quaternion.identity);
        mapSize = gridCreator.GetComponent<GridManager>().GetComponent<GridManager>().numberOfColumns;

        // make map based on given size
        tileMap = new GameObject[mapSize, mapSize];

        GeneratePlains();
        GenerateOtherTileGroups(waterTile, tileOffset, (int)(mapSize * mapSize * 0.25)); // up to 25% of map is water
        GenerateOtherTileGroups(rockTile, tileOffset, (int)(mapSize * mapSize * 0.15)); // up to 15% of map is rock
        GenerateOtherTileGroups(rockTile, tileOffset, (int)(mapSize * mapSize * 0.09)); // up to 9% of map is rock
        GenerateEnvironObjs(stone, 0.03f); // up to 3% of map has stone
        GenerateEnvironObjs(copper, 0.01f); // up to 1% of map has stone
        GenerateEnvironObjs(tree1, 0.066f); // up to 6.6% of map has tree1 trees
        GenerateEnvironObjs(tree2, 0.066f); // up to 6.6% of map has tree2 trees
        GenerateEnvironObjs(tree3, 0.066f); // up to 6.6% of map has tree3 trees

        // position camera at the center
        gameCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        gameCamera.transform.position = new Vector3((mapSize / 2), 12, (mapSize / 2));
        gameCamera.transform.rotation = Quaternion.Euler(90, 0, 0);
    }

    private void GeneratePlains()
    {
        for (int i = 0; i < mapSize; i++)
        {
            for (int j = 0; j < mapSize; j++)
            {
                tileMap[i, j] = Instantiate(plainsTile, TilePosition(i+centerOfTileOffset, 0, j+centerOfTileOffset), Quaternion.Euler(90,0,0));
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

        // destroy the previous tile (may not be plains -> makes more interesting maps)
        Destroy(tileMap[indexX, indexZ]);

        // place new terrain tile - this will be the origin of the group of terrain tiles
        tileMap[indexX, indexZ] = Instantiate(tilePrefab, TilePosition(indexX+centerOfTileOffset, 0, indexZ+centerOfTileOffset), Quaternion.Euler(90, 0, 0));
       
        int nextRandValue;

        // randomly generate a group of tiles
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
        tileMap[indexX, indexZ] = Instantiate(prefab, TilePosition(indexX+centerOfTileOffset, 0, indexZ+centerOfTileOffset), Quaternion.Euler(90, 0, 0));
    }

    private void GenerateEnvironObjs(GameObject prefab, float surfaceCoverage)
    {
        System.Random rand = new System.Random(Guid.NewGuid().GetHashCode());
        // seed position for a collectible group
        int nextX;
        int nextZ;

        // produce random-sized groupings of collectibles
        int groupSize = 0;
        int nextTile;

        int objCount = (int)(mapSize * mapSize * surfaceCoverage); // num objects in the scene

        // find a plains tile
        while (objCount > 0)
        {
            nextX = rand.Next(0, mapSize);
            nextZ = rand.Next(0, mapSize);

            if (tileMap[nextX, nextZ].gameObject.tag.Equals("PlainsTile"))
            {
                if (prefab.tag == "Tree")
                {
                    objPosition = InstaPrefab(prefab, nextX, nextZ);
                    tileMap[nextX, nextZ].gameObject.tag = "PlainsTileWithTree";
                }
                else
                {
                    // seed position for a collectible group
                    objPosition = InstaPrefab(prefab, nextX, nextZ);
                    tileMap[nextX, nextZ].gameObject.tag = "PlainsTileWithTree";

                    rand = new System.Random(Guid.NewGuid().GetHashCode());
                    groupSize = rand.Next(1, mapSize / 4);

                    while (groupSize > 0)
                    {
                        nextTile = CreateResourceGroup(prefab, rand, ref nextX, ref nextZ, ref groupSize, ref objCount);
                        // TODO: either make it PlainsWithResource or add a PlainsWithStoneTile
                    }
                }
                objCount--;
            }
        }
    }

    private int CreateResourceGroup(GameObject prefab, System.Random rand, ref int nextX, ref int nextZ, ref int groupSize, ref int objCount)
    {
        int nextTile = rand.Next(0, 4);
        switch (nextTile)
        {
            case 1: // west
                if (nextX - 1 >= 0)  // check if we are past the map edge
                {
                    nextX -= 1;
                    if (tileMap[nextX, nextZ].tag == "PlainsTile")
                    {
                        objPosition = InstaPrefab(prefab, nextX, nextZ);
                        objCount--;
                        groupSize--;
                    }
                }
                break;
            case 2: // east
                if (nextX + 1 < mapSize)  // check if we are past the map edge
                {
                    nextX += 1;
                    if (tileMap[nextX, nextZ].tag == "PlainsTile")
                    {
                        objPosition = InstaPrefab(prefab, nextX, nextZ);
                        objCount--;
                        groupSize--;
                    }
                }
                break;
            case 3: // south
                if (nextZ - 1 >= 0)  // check if we are past the map edge
                {
                    nextZ -= 1;
                    if (tileMap[nextX, nextZ].tag == "PlainsTile")
                    {
                        objPosition = InstaPrefab(prefab, nextX, nextZ);
                        objCount--;
                        groupSize--;
                    }
                }
                break;
            default: // north
                if (nextZ + 1 < mapSize)  // check if we are past the map edge
                {
                    nextZ += 1;
                    if (tileMap[nextX, nextZ].tag == "PlainsTile")
                    {
                        objPosition = InstaPrefab(prefab, nextX, nextZ);
                        objCount--;
                        groupSize--;
                    }
                }
                break;
        }

        return nextTile;
    }

    private Vector3 InstaPrefab(GameObject prefab, int nextX, int nextZ)
    {
        objPosition = TilePosition(nextX + centerOfTileOffset, prefab.transform.localScale.y / 2, nextZ + centerOfTileOffset);
        Instantiate(prefab, objPosition, Quaternion.Euler(0, 0, 0)); // rotate to top down view
        tileMap[nextX, nextZ].gameObject.tag = "PlainsTileWithTree";
        return objPosition;
    }

    public string getTileTag(int x, int z)
    {
        try
        {
            return tileMap[x, z].gameObject.tag;
        }
        catch (IndexOutOfRangeException)
        {
            return null;
        }
    }

    public void setTileTag(int x, int z, string newTag)
    {
        try
        {
            tileMap[x, z].gameObject.tag = newTag;
        }
        catch (IndexOutOfRangeException)
        { 
            // nothing is set
        }
    }
}