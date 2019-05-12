using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarterTileLayout : MonoBehaviour
{
    public int mapSize = 10;
    public GameObject plainsTile;
    public GameObject waterTile;
    public Camera gameCamera;

    private GameObject[,] tileMap;

    // Start is called before the first frame update
    void Start()
    {
        // make map based on given size
        tileMap = new GameObject[mapSize, mapSize];

        GeneratePlains();
        GenerateWater();

        // position the camera in the middle of the map
        gameCamera.transform.position = new Vector3((mapSize / 2) * 0.86f, (mapSize / 2) * 0.86f, -20);
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
        System.Random rand = new System.Random();
        int waterPositionX = rand.Next(mapSize);
        rand = new System.Random();
        int waterPositionY = rand.Next(mapSize);
        print(waterPositionX + " " + waterPositionY);
        Destroy(tileMap[waterPositionX, waterPositionY]);
        Vector3 waterTilePosition = new Vector3(waterPositionX * 0.86f, waterPositionY * 0.86f, 0);
        tileMap[waterPositionX, waterPositionY] = Instantiate(waterTile, waterTilePosition, Quaternion.identity);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
