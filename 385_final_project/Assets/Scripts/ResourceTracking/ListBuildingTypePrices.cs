using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListBuildingTypePrices : MonoBehaviour
{
    private Dictionary<string, int> farmPrice;
    private Dictionary<string, int> fortPrice;
    private Dictionary<string, int> housePrice;
    private Dictionary<string, int> tavernPrice;
    private Dictionary<string, int> villCenterPrice;

    // Start is called before the first frame update
    void Start()
    {
        housePrice = new Dictionary<string, int>();
        housePrice.Add("Tree", 5);

        tavernPrice = new Dictionary<string, int>();
        tavernPrice.Add("Tree", 20);
        tavernPrice.Add("Stone", 10);

        villCenterPrice = new Dictionary<string, int>();
        villCenterPrice.Add("Tree", 50);
        villCenterPrice.Add("Stone", 20);
        villCenterPrice.Add("Copper", 10);

        fortPrice = new Dictionary<string, int>();
        fortPrice.Add("Stone", 10);

        farmPrice = new Dictionary<string, int>();
        farmPrice.Add("Tree", 3);
    }

    public Dictionary<string, int> GetBuildingPrice(string building)
    {
        if (building.ToLower().Contains("center"))
        {
            return villCenterPrice;
        }
        else if(building.ToLower().Contains("house"))
        {
            return housePrice;
        }
        else if(building.ToLower().Contains("farm"))
        {
            return farmPrice;
        }
        else if(building.ToLower().Contains("fort"))
        {
            return fortPrice;
        }
        else if(building.ToLower().Contains("tavern"))
        {
            return tavernPrice;
        }
        else
        {
            return null;
        }
    }
}
