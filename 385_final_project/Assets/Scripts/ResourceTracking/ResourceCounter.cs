using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceCounter : MonoBehaviour
{
    public int numberOfResources;

    private float tileOffset = 0.860000f;
    private readonly float centerOfTileOffset = .5f;

    // Start is called before the first frame update
    void Start()
    {
        //numberOfResources = 10;
    }

    // Update is called once per frame
    void Update()
    {
       if(numberOfResources <= 0)
       {
            int tileX = (int)((transform.position.x - centerOfTileOffset) / tileOffset + 1);
            int tileZ = (int)((transform.position.z - centerOfTileOffset) / tileOffset + 1);
            Destroy(gameObject);
            GameObject.Find("TileLayoutStarter").GetComponent<StarterTileLayout>().setTileTag(tileX, tileZ, "PlainsTile");
        }
    }
}
