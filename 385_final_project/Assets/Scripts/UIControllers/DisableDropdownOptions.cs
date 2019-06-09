using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisableDropdownOptions : MonoBehaviour
{
    public GameObject housePrefab;
    public GameObject farmPrefab;
    public GameObject fortPrefab;
    public GameObject villCenterPrefab;
    public GameObject tavernPrefab;

    private ListBuildingTypePrices priceList;
    private Dropdown dropdown;
    private List<Dropdown.OptionData> smallList;
    private List<Dropdown.OptionData> biggerList;
    private List<Dropdown.OptionData> completeList;
    private bool displayFullList;

    // Start is called before the first frame update
    void Start()
    {
        priceList = GameObject.Find("BuildingSpawner").GetComponent<ListBuildingTypePrices>();

        smallList = new List<Dropdown.OptionData>();
        biggerList = new List<Dropdown.OptionData>();
        completeList = new List<Dropdown.OptionData>();

        dropdown = GetComponent<Dropdown>();

        smallList.Add(dropdown.options[0]);
        smallList.Add(dropdown.options[1]);

        biggerList.Add(dropdown.options[0]);
        biggerList.Add(dropdown.options[1]);
        biggerList.Add(dropdown.options[2]);    // farm

        completeList.Add(dropdown.options[0]);
        completeList.Add(dropdown.options[1]);
        completeList.Add(dropdown.options[2]);
        completeList.Add(dropdown.options[3]);
        completeList.Add(dropdown.options[4]);
        completeList.Add(dropdown.options[5]);

        dropdown.ClearOptions();
        dropdown.AddOptions(smallList);

        //displayFullList = false;
    }

    public void DisplayBiggerMenu()
    {
        dropdown.ClearOptions();
        dropdown.AddOptions(biggerList);
    }

    public void DisplayFullMenu()
    {
        dropdown.ClearOptions();
        dropdown.AddOptions(completeList);
        foreach (Dropdown.OptionData item in completeList)
        {
            if (!item.text.Contains("-------"))
            {
                item.text  = AddPriceTagsToMenu(item.text);
            }
        }
    }

    private string AddPriceTagsToMenu(string itemName)
    {
        GameObject prefab = null;
        string newText = "";
        if(itemName.ToLower().Contains("house"))
        {
            prefab = housePrefab;
        }
        else if(itemName.ToLower().Contains("farm"))
        {
            prefab = farmPrefab;
        }
        else if (itemName.ToLower().Contains("fort"))
        {
            prefab = fortPrefab;
        }
        else if (itemName.ToLower().Contains("village"))
        {
            prefab = villCenterPrefab;
        }
        else if (itemName.ToLower().Contains("tavern"))
        {
            prefab = tavernPrefab;
        }

        Dictionary<string, int> price = priceList.GetBuildingPrice(itemName);
        newText = itemName + " (";
        foreach (KeyValuePair<string, int> resource in price)
        {
            newText += " " + resource.Key + " : " + resource.Value + "; ";
        }
        // getting rid of the last ';'
        newText = newText.Substring(0, newText.Length - 2);
        newText += ")";
        return newText;
    }
}
