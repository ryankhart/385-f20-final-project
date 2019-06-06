using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DecrementFoodCount : MonoBehaviour
{
    Text foodCount;
    int villagersCount;

    // Start is called before the first frame update
    void Start()
    {
        foodCount = GameObject.Find("FarmFoodCount").GetComponent<Text>();
        InvokeRepeating("DecrementFoodCounter", 10, 10);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void DecrementFoodCounter()
    {
        villagersCount = GameObject.Find("VillagerSpawner").GetComponent<SpawnVillagers>().villagers.Count;
        int currCount = System.Convert.ToInt32(foodCount.ToString());
        int result = currCount - villagersCount * 5;
        foodCount.text = result.ToString();
    }
}
