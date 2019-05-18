using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// extending EventTrigger to override dropdown functions
public class SpawnNewBuildings : MonoBehaviour
{
    public GameObject housePrefab;
    public new Camera camera;   // new is neccessary because this camera overrides some inherited camera

    private List<GameObject> houses = new List<GameObject>();
    private GameObject buildingToDrag;
    private bool draggingNewBuilding;
    private Vector3 cameraMouseOffset;
    private Vector3 screenPoint;

    void Start()
    {
        draggingNewBuilding = false; // set initial value
    }

    void Update()
    {
        if (draggingNewBuilding)
        {
            DragBuilding();
        }
        else
        {
            if (buildingToDrag != null)
            {
                // drop the buidling down onto the map surface
                buildingToDrag.transform.position = new Vector3(buildingToDrag.transform.position.x, buildingToDrag.transform.position.y, 0);
                
            }
            // stop hodling onto this building
            buildingToDrag = null;
            // stop the dragging process
            draggingNewBuilding = false;
        }
    }

    private void DragBuilding()
    {
        // if user clicks on the left mouse button
        if (Input.GetMouseButtonDown(0))
        {
            // stop holding onto this building
            buildingToDrag = null;
            // stop the dragging process
            draggingNewBuilding = false;
        }
        buildingToDrag = houses[houses.Count - 1];
        float posX = Input.mousePosition.x;
        float posY = Input.mousePosition.y;
        // 10 units below the camera, so that the player can see where the building is
        buildingToDrag.transform.position = camera.ScreenToWorldPoint(new Vector3(posX, posY, 10));
    }

    public void SelectBuildingFromDropdown(int index)
    {
        // 0 = Building Menu - unselectable item, 1-3 = building options
        if (index != 0)
        {
            // position the building to the mouse cursor position
            Vector3 mousePosition = Input.mousePosition;
            // camera is positioned at z = -10 => z = 9 means the object will appear 1 unit above the ground
            Vector3 housePosition = camera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 9.0f));

            GameObject newHouse = Instantiate(housePrefab, housePosition, Quaternion.identity);
            houses.Add(newHouse);
            draggingNewBuilding = true;
        }
        else
        {
            Debug.Log("No building selection made");
        }
    }
}
