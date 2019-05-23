using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrackStorageResources : MonoBehaviour
{
    private Dictionary<string, int> resources = new Dictionary<string, int>();
<<<<<<< HEAD
    private GameObject woodCount;
    private GameObject stoneCount;
    private GameObject copperCount;
    // TODO:
    private Text herbCount;
=======
>>>>>>> villager brings collected resources to village center and goes on to next resource

    // Start is called before the first frame update
    void Start()
    {
        resources.Add("Tree", 0);
        resources.Add("Stone", 0);
        resources.Add("Copper", 0);
        resources.Add("Herb", 0);

        woodCount = GameObject.Find("WoodCount");
        stoneCount = GameObject.Find("StoneCount");
        copperCount = GameObject.Find("CopperCount");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddResourceUnits(string resourceTag, int numUnits)
    {
        resources[resourceTag] += numUnits;
        if (resourceTag == "Tree")
        {
            woodCount.GetComponent<UpdateResourceCounter>().SetCount(numUnits);
        } 
        else if (resourceTag == "Stone")
        {
            stoneCount.GetComponent<UpdateResourceCounter>().SetCount(numUnits);
        }
        else if (resourceTag == "Copper")
        {
            copperCount.GetComponent<UpdateResourceCounter>().SetCount(numUnits);
        }
    }

    public void SubtractResourceUnits(string resourceTag, int numUnits)
    {
        // TODO: can't subtract below 0
        resources[resourceTag] -= numUnits;
        if (resourceTag == "Tree")
        {
            woodCount.GetComponent<UpdateResourceCounter>().SetCount(- numUnits);
        }
        else if (resourceTag == "Stone")
        {
            stoneCount.GetComponent<UpdateResourceCounter>().SetCount(- numUnits);
        }
        else if (resourceTag == "Copper")
        {
            copperCount.GetComponent<UpdateResourceCounter>().SetCount(- numUnits);
        }
    }
}
