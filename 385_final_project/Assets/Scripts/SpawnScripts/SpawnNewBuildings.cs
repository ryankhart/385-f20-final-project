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
    public GameObject tavernPrefab;
    public GameObject fortPrefab;
    public GameObject farmPrefab;
    public new Camera camera;   // new is neccessary because this camera overrides some inherited camera

    // buidlings
    private List<GameObject> houses = new List<GameObject>();
    private List<GameObject> taverns = new List<GameObject>();
    private GameObject villageCenter;

    // building manipulation
    private GameObject buildingToDrag;
    private bool draggingNewBuilding;
    private bool selectedAndFloating;

    // building cost and resource spending
    private TrackStorageResources resourceCounterScript;

    // offsets and other math stuff
    private Vector3 cameraMouseOffset;
    private Vector3 screenPoint;
    private StarterTileLayout tileLayoutScript;
    private readonly float tileOffset = 0.86f;
    private readonly float centerOffset = 0.43f;

    void Start()
    {
        draggingNewBuilding = false;
        selectedAndFloating = false;
    }

    private void Awake()
    {
        GameObject tileLayoutStarter = GameObject.Find("TileLayoutStarter");
        tileLayoutScript = tileLayoutStarter.GetComponent<StarterTileLayout>();
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
            float posX = Input.mousePosition.x;
            float posY = Input.mousePosition.y;
            // 10 units below the camera, so that the player can see where the building is
            if (camera.transform.position.y > 8.99)
            {
                buildingToDrag.transform.position = camera.ScreenToWorldPoint(new Vector3(posX, posY, 9));
            }
            else
            {
                // nearer the camera so that the building doesn't go under the map
                buildingToDrag.transform.position = camera.ScreenToWorldPoint(new Vector3(posX, posY, camera.transform.position.y * 0.8f));
            }
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

    public void SelectBuildingFromDropdown(int index)
    {
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
            else if (index == 2)
            {
                GameObject newFarm = Instantiate(farmPrefab, buildingPosition, Quaternion.identity);
                newFarm.tag = "MovingBuilding";
                //houses.Add(newHouse);
                buildingToDrag = newFarm;
            }
            else if (index == 3)
            {
                GameObject newHouse = Instantiate(housePrefab, buildingPosition, Quaternion.identity);
                newHouse.tag = "MovingBuilding";
                houses.Add(newHouse);
                buildingToDrag = newHouse;
            }
            else if(index == 4)
            {
                GameObject newFort = Instantiate(fortPrefab, buildingPosition, Quaternion.identity);
                newFort.tag = "MovingBuilding";
                //houses.Add(newHouse);
                buildingToDrag = newFort;
            }
            else if(index == 5)
            {
                GameObject newTavern = Instantiate(tavernPrefab, buildingPosition, Quaternion.identity);
                newTavern.tag = "MovingBuilding";
                // TODO do I need these lists?
                taverns.Add(newTavern);
                buildingToDrag = newTavern;
            }
            draggingNewBuilding = true;
        }
    }

    private void DragBuilding(GameObject building)
    {
        // if user clicks on the left mouse button
        if (Input.GetMouseButtonDown(0))
        {
            if (selectedAndFloating)
            {
                StopDraggingBuidling();
            }
            else
            {
                StartCoroutine(WaitInsteadOfDropping());
            }
        }
        buildingToDrag = building;
        float posX = Input.mousePosition.x;
        float posY = Input.mousePosition.y;
        // 10 units below the camera, so that the player can see where the bwwwuilding is
        if (camera.transform.position.y > 8.99)
        {
            buildingToDrag.transform.position = camera.ScreenToWorldPoint(new Vector3(posX, posY, 9));
        }
        else
        {
            // move building closer to camera, so that the building doesn't go below surface
            buildingToDrag.transform.position = camera.ScreenToWorldPoint(new Vector3(posX, posY, camera.transform.position.y * 0.8f));
        }
    }

    private void StopDraggingBuidling()
    {
        // stop the dragging process
        draggingNewBuilding = false;
    }

    private IEnumerator WaitInsteadOfDropping()
    {
        yield return new WaitForSeconds(0.7f);
        selectedAndFloating = true;
    }

    private void PlaceBuildingOnFreePlainsTile()
    {
        // place building into a tile on the grid
        int tileXIndex = (int)(buildingToDrag.transform.position.x / tileOffset);
        int tileZIndex = (int)(buildingToDrag.transform.position.z / tileOffset);
        string tileTag = tileLayoutScript.getTileTag(tileXIndex, tileZIndex);
        if(tileTag == null)
        {
            Destroy(buildingToDrag);
            StopDraggingBuidling();
            return;
        }

        // drop the building down onto a free plains tile
        if (tileTag.Equals("PlainsTile"))
        {
            if (!buildingToDrag.tag.Equals("VillageCenter"))
            {
                // if village center exists - village center stores resources
                if (resourceCounterScript != null)
                {
                    if(buildingToDrag.name.Contains("House"))
                    {
                        buildingToDrag.tag = "Home";
                    }
                    else if(buildingToDrag.name.Contains("Tavern"))
                    {
                        buildingToDrag.tag = "Tavern";
                    }
                    else if (buildingToDrag.name.Contains("Fort"))
                    {
                        buildingToDrag.tag = "Fort";
                    }
                    else if (buildingToDrag.name.Contains("Farm"))
                    {
                        buildingToDrag.tag = "Farm";
                    }

                    // pay for the building
                    Dictionary<string, int> price = buildingToDrag.GetComponent<BuildingPrice>().GetPrice();
                    foreach (KeyValuePair<string, int> resource in price)
                    {
                        if(resourceCounterScript.SubtractResourceUnits(resource.Key, resource.Value) == false)
                        {
                            StartCoroutine(GameObject.Find("Hints").GetComponent<DisplayHints>().DisplayHint("NotEnoughHint"));
                            Destroy(buildingToDrag);
                            return;
                        }
                    }

                    if (GameObject.FindGameObjectsWithTag("Farm").Length == 1)
                    {
                        StartCoroutine(GameObject.Find("Hints").GetComponent<DisplayHints>().DisplayHint("FarmFunctionHint", 5));
                    }
                } 
                else
                {
                    StartCoroutine(GameObject.Find("Hints").GetComponent<DisplayHints>().DisplayHint("NotEnoughHint"));
                    Destroy(buildingToDrag);
                    return;
                }
            }
            else
            {
                if(GameObject.FindGameObjectsWithTag("VillageCenter").Length == 1)
                {
                    StartCoroutine(GameObject.Find("Hints").GetComponent<DisplayHints>().DisplayHint("BuildFarmsHint", 5));
                }
                else
                {
                    Dictionary<string, int> price = buildingToDrag.GetComponent<BuildingPrice>().GetPrice();

                    foreach (KeyValuePair<string, int> resource in price)
                    {
                        if (resourceCounterScript.SubtractResourceUnits(resource.Key, resource.Value) == false)
                        {
                            StartCoroutine(GameObject.Find("Hints").GetComponent<DisplayHints>().DisplayHint("NotEnoughHint"));
                            Destroy(buildingToDrag);
                            return;
                        }
                    }
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
