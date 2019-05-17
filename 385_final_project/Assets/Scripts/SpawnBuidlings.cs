using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// extending EventTrigger to override dropdown functions
public class SpawnBuidlings : MonoBehaviour, ISelectHandler
{

    public GameObject housePrefab;

    private List<GameObject> houses = new List<GameObject>();

    void Start()
    {

    }

    void Update()
    {

    }

    public void OnSelect(BaseEventData eventData)
    {
        int selection = gameObject.GetComponent<Dropdown>().value;
        if(selection != 0)
        {
            if(Input.GetMouseButtonDown(0)) // on left click
            {
                Debug.Log("Selection" + selection);
                Vector3 cursorPosition = Input.mousePosition;
                cursorPosition.z = -2;
                Debug.Log("Mouse position" + cursorPosition);

                Vector3 housePosition = Camera.main.ScreenToWorldPoint(cursorPosition);
                Debug.Log("House position" + housePosition);
                GameObject newHouse = Instantiate(housePrefab, housePosition, Quaternion.identity);
                houses.Add(newHouse);
            }
        }
    }
}
