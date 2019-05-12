using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarterTileLayout : MonoBehaviour
{
    public int mapSize = 10;
    public GameObject plainsTile;
    public Camera gameCamera;

    // Start is called before the first frame update
    void Start()
    {
        // start with plains
        for(int i = 0; i < mapSize; i++)
        { 
            for(int j = 0; j < mapSize; j++)
            {
                Vector3 tilePosition = new Vector3(i * 0.86f, j * 0.86f, 0);
                print(tilePosition);
                Instantiate(plainsTile, tilePosition, Quaternion.identity);
            }
        }

        // position the camera in the middle of the map
        gameCamera.transform.position = new Vector3((mapSize/2) * 0.86f, (mapSize/2) * 0.86f, -20);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
