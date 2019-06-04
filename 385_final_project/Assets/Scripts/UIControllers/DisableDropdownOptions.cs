using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisableDropdownOptions : MonoBehaviour
{
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

        displayFullList = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!displayFullList && GameObject.FindWithTag("VillageCenter") && GameObject.FindWithTag("Farm"))
        {
            dropdown.ClearOptions();
            dropdown.AddOptions(completeList);
        }
        else if(!displayFullList && GameObject.FindWithTag("VillageCenter"))
        {
            dropdown.ClearOptions();
            dropdown.AddOptions(biggerList);
        }
    }
}
