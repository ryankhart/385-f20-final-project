using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// extending EventTrigger to override dropdown functions
public class SpawnNewBuildings : MonoBehaviour
{
    public GameObject villCenterPrefab;
    public GameObject housePrefab;
    public new Camera camera;   // new is neccessary because this camera overrides some inherited camera

    // buidlings
    private List<GameObject> houses = new List<GameObject>();
    private GameObject villageCenter;

    // building manipulation
    private GameObject buildingToDrag;
    private bool draggingNewBuilding;

    // building cost and resource spending
    private TrackStorageResources resourceCounterScript;
    private Dictionary<string, int> homePrice = new Dictionary<string, int>()
        {{"Tree", 5}};

    // offsets and other math stuff
    private Vector3 cameraMouseOffset;
    private Vector3 screenPoint;
    private StarterTileLayout tileLayoutScript;
    private readonly float tileOffset = 0.86f;
    private readonly float centerOffset = 0.43f;

    void Start()
    {
        draggingNewBuilding = false;
    }

    void Update()
    {
        // check if user created village center yet
        if(resourceCounterScript == null)
        { 
            if(GameObject.Find("VillageCenter(Clone)") != null)
            {
                resourceCounterScript = GameObject.Find("VillageCenter(Clone)").GetComponent<TrackStorageResources>();
            }
        }

        if (draggingNewBuilding)
        {
            DragBuilding(buildingToDrag);
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

    private void Awake()
    {
        GameObject tileLayoutStarter = GameObject.Find("TileLayoutStarter");
        tileLayoutScript = tileLayoutStarter.GetComponent<StarterTileLayout>();
    }

    public void SelectBuildingFromDropdown(int index)
    {
        // 0 = Building Menu - unselectable item, 1-4 = building options
        if (index != 0)
        {
            // position the building to the mouse cursor position
            Vector3 mousePosition = Input.mousePosition;
            // camera is positioned at z = -10 => z = 9 means the object will appear 1 unit above the ground
            Vector3 buildingPosition = camera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, 9.0f));

            if (index == 1)
            {
                villageCenter = Instantiate(villCenterPrefab, buildingPosition, Quaternion.identity);
                villageCenter.tag = "VillageCenter";
                buildingToDrag = villageCenter;
                
            }
            else
            {
                GameObject newHouse = Instantiate(housePrefab, buildingPosition, Quaternion.identity);
                houses.Add(newHouse);
                buildingToDrag = newHouse;
            }
            draggingNewBuilding = true;
        }
        // else do nothing - just the heading of the menu was selected
    }

    private void DragBuilding(GameObject building)
    {
        // if user clicks on the left mouse button
        if (Input.GetMouseButtonDown(0))
        {
            StopDraggingBuidling();
        }
        buildingToDrag = building;
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
        string tileTag = tileLayoutScript.getTileTag(tileXIndex, tileZIndex);

        // drop the buidling down onto a free plains tile
        if (tileTag.Equals("PlainsTile"))
        {
            if (!buildingToDrag.tag.Equals("VillageCenter"))
            {
                // if village center exists
                if (resourceCounterScript != null)
                {
                    buildingToDrag.tag = "Home";
                    // pay with resources
                    bool canBuild = true;
                    foreach (KeyValuePair<string, int> resource in homePrice)
                    {
                        if(resourceCounterScript.SubtractResourceUnits(resource.Key, resource.Value) == false)
                        {
                            canBuild = false;
                        }
                    }
                    if(!canBuild)
                    {
                        print("You have no resources to build with!");
                        Destroy(buildingToDrag);
                        return;
                    }
                } 
                else
                {
                    print("You have no resources to build with!");
                    Destroy(buildingToDrag);
                    return;
                }
            }

            buildingToDrag.transform.position = new Vector3(tileXIndex * tileOffset + centerOffset, 0.25f, tileZIndex * tileOffset + centerOffset);
            tileLayoutScript.setTileTag(tileXIndex, tileZIndex, "PlainsTileWithBuilding");

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
