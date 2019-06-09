using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingPrice : MonoBehaviour
{
    private Dictionary<string, int> price;

    void Start()
    {
        ListBuildingTypePrices priceScript = GameObject.Find("BuildingSpawner").GetComponent<ListBuildingTypePrices>();
        price = priceScript.GetBuildingPrice(gameObject.name.ToLower());
    }

    public Dictionary<string, int> GetPrice()
    {
        return price;
    }

    // TODO method for canAfford?
}
