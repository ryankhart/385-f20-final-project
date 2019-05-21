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
    private readonly float tileOffset = 0.86f;
    private readonly float centerOffset = 0.43f;

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
                PlaceBuildingOnFreePlainsTile();
            }
            else
            {
                StopDraggingBuidling();
            }
        }
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

    private void DragBuilding()
    {
        // if user clicks on the left mouse button
        if (Input.GetMouseButtonDown(0))
        {
            StopDraggingBuidling();
        }
        buildingToDrag = houses[houses.Count - 1];
        float posX = Input.mousePosition.x;
        float posY = Input.mousePosition.y;
        // 10 units below the camera, so that the player can see where the building is
        buildingToDrag.transform.position = camera.ScreenToWorldPoint(new Vector3(posX, posY, 9));
    }

    private void StopDraggingBuidling()
    {
        // stop the dragging process
        draggingNewBuilding = false;
    }

    private void PlaceBuildingOnFreePlainsTile()
    {
        // TODO: check that we aren't outside of the map - may not be necessary?, so far all buildings
        // just happend to be dropped on the map - that may change when the player character is able to move
        // around, though

        // place building into a tile on the grid
        // TODO: for now, place to the tile where the lower left corner of the house is
        // get the index of the tiles from the tile map
        int tileXIndex = (int)(buildingToDrag.transform.position.x / tileOffset);
        int tileZIndex = (int)(buildingToDrag.transform.position.z / tileOffset);

        // get the tile tag
        GameObject tileLayoutStarter = GameObject.Find("TileLayoutStarter");
        StarterTileLayout tileLayoutScript = tileLayoutStarter.GetComponent<StarterTileLayout>();
        string tileTag = tileLayoutScript.getTileTag(tileXIndex, tileZIndex);

        // drop the buidling down onto a free plains tile
        if (tileTag.Equals("PlainsTile"))
        {
            buildingToDrag.transform.position = new Vector3(tileXIndex * tileOffset + centerOffset, 0.25f, tileZIndex * tileOffset + centerOffset);
            tileLayoutScript.setTileTag(tileXIndex, tileZIndex, "PlainsTileWithBuilding");
            buildingToDrag.tag = "Home";
            // stop holding onto this building
            buildingToDrag = null;
            StopDraggingBuidling();
        }
        else
        {
            // continue dragging the building
            draggingNewBuilding = true;
        }
    }
}
