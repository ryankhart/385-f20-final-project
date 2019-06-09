using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    private Vector3 creationPosition;

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
    }

    private void Awake()
    {
        GameObject tileLayoutStarter = GameObject.Find("TileLayoutStarter");
        tileLayoutScript = tileLayoutStarter.GetComponent<StarterTileLayout>();

        InvokeRepeating("DestroyRogueFloaterBuildings", 10, 0.5f);
    }

    void Update()
    {
        // check if user created village center yet
        if (resourceCounterScript == null)
        {
            if (GameObject.Find("VillageCenter(Clone)") != null)
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
                if (buildingToDrag == null)
                {
                    return;
                }
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
            // player can place a building only if mouse has been moved away from the creation position
            // because the creation position is usually the UI position
            if (Vector3.Distance(creationPosition, Input.mousePosition) > 7)
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
            else
            {
                draggingNewBuilding = true;
            }
        }
    }

    private void DestroyRogueFloaterBuildings()
    {
        GameObject[] floaters = GameObject.FindGameObjectsWithTag("MovingBuilding");
        foreach (GameObject floater in floaters)
        {
            if (floater.transform.position.y > 0.27)
            {
                if (!floater.Equals(buildingToDrag))
                {
                    Destroy(floater);
                }
            }
        }
        floaters = GameObject.FindGameObjectsWithTag("VillageCenter");
        foreach (GameObject floater in floaters)
        {
            if (floater.transform.position.y > 0.27)
            {
                if (!floater.Equals(buildingToDrag))
                {
                    Destroy(floater);
                }
            }
        }
    }

    public void SelectBuildingFromDropdown(int index)
    {
        if (index != 0)
        {
            // position the building to the mouse cursor position
            creationPosition = Input.mousePosition;
            // camera is positioned at z = -10 => z = 9 means the object will appear 1 unit above the ground
            Vector3 buildingPosition = camera.ScreenToWorldPoint(new Vector3(creationPosition.x, creationPosition.y, 9.0f));

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
            else if (index == 4)
            {
                GameObject newFort = Instantiate(fortPrefab, buildingPosition, Quaternion.identity);
                newFort.tag = "MovingBuilding";
                //houses.Add(newHouse);
                buildingToDrag = newFort;
            }
            else if (index == 5)
            {
                GameObject newTavern = Instantiate(tavernPrefab, buildingPosition, Quaternion.identity);
                newTavern.tag = "MovingBuilding";
                taverns.Add(newTavern);
                buildingToDrag = newTavern;
            }
            draggingNewBuilding = true;
        }
    }

    // methods for destroying building if user clicks on building menu to re-select building
    // triggered by PointerEnter and PointerExit on BuildingMenuDropDown object
    public void MakeDestroyable()
    {
        GameObject floating = GameObject.FindWithTag("MovingBuilding");
        if (floating)
        {
            floating.tag = "DestroyThis";
        }
    }

    public void MakeUndestroyable()
    {
        GameObject floating = GameObject.FindWithTag("DestroyThis");
        if (floating)
        {
            floating.tag = "MovingBuilding";
        }
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
        // stop dragging, building can now be placed on map (or destroyed)
        draggingNewBuilding = false;

        // Since player is no longer dragging building, destry the radical showing where
        // building will be placed.
        //ShowBuildingPlacementOnMap script = buildingToDrag.GetComponent<ShowBuildingPlacementOnMap>();
       // script.DestroyRadical();
        //script.enabled = false;
    }

    private void PlaceBuildingOnFreePlainsTile()
    {
        // place building into a tile on the grid
        int tileXIndex = (int)(buildingToDrag.transform.position.x / tileOffset);
        int tileZIndex = (int)(buildingToDrag.transform.position.z / tileOffset);
        string tileTag = tileLayoutScript.getTileTag(tileXIndex, tileZIndex);
        if (tileTag == null || buildingToDrag.tag == "DestroyThis")
        {
            Destroy(buildingToDrag);
            buildingToDrag = null;
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
                    if (buildingToDrag.name.Contains("House"))
                    {
                        buildingToDrag.tag = "Home";

                        SpawnVillagers script = GameObject.Find("VillagerSpawner").GetComponent<SpawnVillagers>();
                        int villagerNum = 0;
                        if (script == null || script.villagers?.Any() != true)
                        {
                            villagerNum = 0;
                        }
                        else
                        {
                            villagerNum = script.getVillagerList().Count;
                        }
                        Text foodCount = GameObject.Find("FarmFoodCount").GetComponent<Text>(); 
                        int currFoodCount = int.Parse(foodCount.text);
                        if (currFoodCount < 5 * villagerNum + 5)
                        {
                            StartCoroutine(GameObject.Find("Hints").GetComponent<DisplayHints>().DisplayHint("NotEnoughFood", 4));
                        }
                    }
                    else if (buildingToDrag.name.Contains("Tavern"))
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
                        // if this is the first farm in the game
                        if (GameObject.FindGameObjectsWithTag("Farm").Length == 1)
                        {
                            StartCoroutine(GameObject.Find("Hints").GetComponent<DisplayHints>().DisplayHint("FarmFunctionHint", 5));
                            // BuildHousesHint will be delayed
                            StartCoroutine(GameObject.Find("Hints").GetComponent<DisplayHints>().DisplayHint("BuildHousesHint", 5));
                        }
                        GameObject.Find("BuildMenuDropDown").GetComponent<DisableDropdownOptions>().DisplayFullMenu();
                    }

                    PayThePrice();
                }
                else
                {
                    StartCoroutine(GameObject.Find("Hints").GetComponent<DisplayHints>().DisplayHint("NotEnoughHint", 4));
                    Destroy(buildingToDrag);
                    return;
                }
            }
            else
            {
                // if this is the first village center
                if (GameObject.FindGameObjectsWithTag("VillageCenter").Length == 1)
                {
                    StartCoroutine(GameObject.Find("Hints").GetComponent<DisplayHints>().DisplayHint("BuildFarmsHint", 5));
                    GameObject.Find("BuildMenuDropDown").GetComponent<DisableDropdownOptions>().DisplayBiggerMenu();
                }
                else
                {
                    PayThePrice();
                }
            }

            buildingToDrag.transform.position = new Vector3(tileXIndex * tileOffset + centerOffset, 0, tileZIndex * tileOffset + centerOffset);
            tileLayoutScript.setTileTag(tileXIndex, tileZIndex, "PlainsTileWithBuilding");

            buildingToDrag.GetComponent<ShowBuildingPlacementOnMap>().MakeRadicalInvisible();
            buildingToDrag = null;
            StopDraggingBuidling();
        }
        else
        {
            // continue dragging the building
            draggingNewBuilding = true;
        }
    }

    private void PayThePrice()
    {
        Dictionary<string, int> price = buildingToDrag.GetComponent<BuildingPrice>().GetPrice();

        foreach (KeyValuePair<string, int> resource in price)
        {
            if (resourceCounterScript.SubtractResourceUnits(resource.Key, resource.Value) == false)
            {
                StartCoroutine(GameObject.Find("Hints").GetComponent<DisplayHints>().DisplayHint("NotEnoughHint", 4));
                Destroy(buildingToDrag);
                return;
            }
        }
    }
}