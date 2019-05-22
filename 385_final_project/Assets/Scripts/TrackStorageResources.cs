using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrackStorageResources : MonoBehaviour
{
    private Dictionary<string, int> resources;

    // Start is called before the first frame update
    void Start()
    {
        resources.Add("Tree", 0);
        resources.Add("Stone", 0);
        resources.Add("Copper", 0);
        resources.Add("Herb", 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddResourceUnits(string resourceTag, int numUnits)
    {
        resources[resourceTag] += numUnits;
    }

    public void SubtractResourceUnits(string resourceTag, int numUnits)
    {
        // TODO: can't subtract below 0
        resources[resourceTag] -= numUnits;
    }
}
