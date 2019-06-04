using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FarmFoodGenerator : MonoBehaviour
{
    public float foodSpawnRate = 2f;

    private TrackStorageResources resourceTracker;

    // Start is called before the first frame update
    void Start()
    {
        GameObject villageCenter = GameObject.FindWithTag("VillageCenter");
        resourceTracker = villageCenter.GetComponent<TrackStorageResources>();
        InvokeRepeating("GenerateFarmFood", 5.0f, foodSpawnRate);
    }

    private void GenerateFarmFood()
    {
        resourceTracker.AddFoodUnits(1);
    }
}
