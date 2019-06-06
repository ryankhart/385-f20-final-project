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

    private Dropdown dropdown;
    private List<Dropdown.OptionData> smallList;
    private List<Dropdown.OptionData> biggerList;
    private List<Dropdown.OptionData> completeList;
    private bool displayFullList;

    // Start is called before the first frame update
    void Start()
    {
        smallList = new List<Dropdown.OptionData>();
        biggerList = new List<Dropdown.OptionData>();
        completeList = new List<Dropdown.OptionData>();

        dropdown = GetComponent<Dropdown>();

        smallList.Add(dropdown.options[0]);
        smallList.Add(dropdown.options[1]);

        biggerList.AddRange(smallList);
        biggerList.Add(dropdown.options[2]);    // farm

        completeList.AddRange(biggerList);
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
        foreach (Dropdown.OptionData item in completeList)
        {
            if (!item.text.Contains("-------"))
            {
                print(item.text);
                item.text  = AddPriceTagsToMenu(item.text);
                print(item.text);
            }
        }
        dropdown.AddOptions(completeList);
    }

    private string AddPriceTagsToMenu(string itemName)
    {
        //GameObject prefab = null;
        string newText = "";
        if(itemName.ToLower().Contains("house"))
        {
            //prefab = housePrefab;
            newText = itemName + " (Tree : 5)";
        }
        else if(itemName.ToLower().Contains("farm"))
        {
            //prefab = farmPrefab;
            newText = itemName + " (Tree : 3)";
        }
        else if (itemName.ToLower().Contains("fort"))
        {
            //prefab = fortPrefab;
            newText = itemName + " (Stone : 10)";
        }
        else if (itemName.ToLower().Contains("vill"))
        {
            //prefab = villCenterPrefab;
            newText = itemName + " (Tree : 50; Stone : 20; Copper : 10)";
        }
        else if (itemName.ToLower().Contains("tavern"))
        {
            //prefab = tavernPrefab;
            newText = itemName + " (Tree : 20; Stone : 10)";
        }

        //Dictionary<string, int> price = prefab.GetComponent<BuildingPrice>().GetPrice();
        //foreach (KeyValuePair<string, int> resource in price)
        //{
        //    newText = itemName + " " + resource.Key + " : " + resource.Value + "; ";
        //}
        return newText;
    }
}
