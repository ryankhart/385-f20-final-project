using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DecrementFoodCount : MonoBehaviour
{
    public int decrementsInSeconds = 15;

    Text foodCount;
    int villagersCount;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("DecrementFoodCounter", 10, decrementsInSeconds);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void DecrementFoodCounter()
    {
        foodCount = GameObject.Find("FarmFoodCount").GetComponent<Text>();
        villagersCount = GameObject.Find("VillagerSpawner").GetComponent<SpawnVillagers>().villagers.Count;
        int currCount = Int32.Parse(foodCount.text);
        int result = currCount - villagersCount * 5;
        foodCount.text = result.ToString();
    }
}
