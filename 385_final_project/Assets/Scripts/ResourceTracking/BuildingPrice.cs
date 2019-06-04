using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPrice : MonoBehaviour
{
    private Dictionary<string, int> price;

    // Start is called before the first frame update
    void Start()
    {
        // can't use tags because upon initiation, building have MovingBuilding tag
        // TODO resolve
        price = new Dictionary<string, int>();
        if(gameObject.name.Contains("House"))
        {
            price.Add("Tree",5);
            //price.Add("Stone", 0);
            //price.Add("Copper", 0);
        }
        else if (gameObject.name.Contains("Tavern"))
        {
            price.Add("Tree", 20);
            price.Add("Stone", 10);
            //price.Add("Copper", 0);
        }
        else if (gameObject.name.Contains("VillageCenter"))
        {
            price.Add("Tree", 50);
            price.Add("Stone", 20);
            price.Add("Copper", 0);
        }
        else if (gameObject.name.Contains("Fort"))
        {
            price.Add("Tree", 0);
            price.Add("Stone", 10);
            price.Add("Copper", 0);
        }
    }

    public Dictionary<string, int> GetPrice()
    {
        return price;
    }

    // TODO method for canAfford?

    // Update is called once per frame
    void Update()
    {
        
    }
}
