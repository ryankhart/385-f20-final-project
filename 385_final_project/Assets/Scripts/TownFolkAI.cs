using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TownFolkAI : MonoBehaviour
{
    private GameObject targetObject;
    private Transform targetPosition;
    private Vector3 targetPoint;
    public string resourceTag = "Tree";

    private float currentSpeed;
    private Vector3 direction;
    private Quaternion targetRotation;
    private RaycastHit avoidanceHit;
    private Vector3 hitNormal;

    //private Transform startPosition;
    //private Transform endPosition;

    public Node startNode { get; set; }
    public Node goalNode { get; set; }

    private ArrayList pathArray;

    [SerializeField]
    private float movementSpeed = 5.0f;
    private float rotationSpeed = 100.0f;
    [SerializeField]
    private float toleranceRadius = .25f;

    private float elapsedTime = 0.0f;
    private float collectionTime = 0.0f;
    public float intervalTime = 1.0f;
    public GameObject GridCreator;
    public GridManager gridManager;

    private int nodesOfMovement = 1;
    private int inventory = 0;


    // Start is called before the first frame update
    void Start()
    {
        GridCreator = GameObject.FindGameObjectsWithTag("GridCreator")[0];
        gridManager = GridCreator.GetComponent<GridManager>();
        //Calculate the path using our AStart code.
        pathArray = new ArrayList();
        FindPath();
    }


    private void FixedUpdate()
    {
        elapsedTime += Time.deltaTime;

        //How much time it takes to find a new path.
        if (elapsedTime >= intervalTime)
        {
            elapsedTime = 0.0f;
            FindPath();
        }

         FindNode();

        // Checks if target is too close
        // WARNING, YOU ARE FORGETTING THE VILLAGER HAS Y POSITION 0.439
        Vector3 villagerPosition = transform.position;
        villagerPosition.y = 0;
        if (Vector3.Distance(targetPoint, villagerPosition) < toleranceRadius)
        {
            //If target is too close that means we need to peform an action!
            //Check to make sure object is there.
            if (targetObject != null)
            {
                if (targetObject.tag == "Home")
                {
                    //wait at home
                }
                else if (targetObject.tag == "Tree")
                {
                    ProcessResource();
                }
                else if (resourceTag == "VillageCenter")
                {
                    GoStoreCollectedResources();
                }
            }
            return;
        }

        currentSpeed = movementSpeed * Time.deltaTime;

        //Rotate the agent towards its target direction
        direction = (targetPoint - transform.position).normalized;
        direction.y = 0;

        // look
        // the if statement prevents turning when at target and that stupid zero vector warning
        if (Vector3.Distance(direction, Vector3.zero) > 0.01)
        {
            targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        //Move the agent forard
        transform.position += new Vector3(direction.x * currentSpeed, direction.y, direction.z * currentSpeed);
    }

    private void GoStoreCollectedResources()
    {
        GameObject villCenter = GameObject.Find("VillageCenter(Clone)");
        Vector3 centerPosition = villCenter.transform.position;
        centerPosition.y = 0;   // center of the towncenter prefab is not on y = 0
        if (Vector3.Distance(targetPoint, centerPosition) < toleranceRadius)
        {
            // drop the collected resources off at the village center
            villCenter.GetComponent<TrackStorageResources>().AddResourceUnits("Tree", inventory);
            inventory = 0;
            targetObject = null;
            resourceTag = "Tree"; // go find a tree to chop
        }
    }

    //Finds a target with a tag
    public Transform FindTarget()
    {
        //Gets all objects with tag
        GameObject[] targets = GameObject.FindGameObjectsWithTag(resourceTag);

        float minDistance = Mathf.Infinity;
        Transform closest;

        // If there are no trees
        if (targets.Length == 0)
        {
            return null;
        }

        closest = targets[0].transform;
        for (int i = 0; i < targets.Length; i++)
        {
            float distance = (targets[i].transform.position - transform.position).sqrMagnitude;

            if (distance < minDistance)
            {
                targetObject = targets[i];
                closest = targets[i].transform;
                minDistance = distance;
            }
        }
        return closest;
    }

    //Finds the next node to move towards
    public void FindNode()
    {
        try
        {
            //Check if we can get the next node or if we are too close
            if (nodesOfMovement < pathArray.Count && pathArray.Count != 1 && nodesOfMovement > 0)
            {
                //Gets the next node
                Node nextNode = (Node)pathArray[nodesOfMovement];
                //Sets the target Point to the nodes position
                targetPoint = nextNode.position;
                //Sets the node to 1;
                nodesOfMovement = 1;
            }
        }
        catch (Exception e)
        {

        }

    }

    //A* finds the path that the TownFolk should take.
    private void FindPath()
    {
        Transform startPosition = transform;

        if (endPosition != null)
        {
            startNode = new Node(gridManager.GetGridCellCenter(gridManager.GetGridIndex(startPosition.position)));
            goalNode = new Node(gridManager.GetGridCellCenter(gridManager.GetGridIndex(endPosition.position)));

            if (goalNode != null)
                pathArray = AStar.FindPath(startNode, goalNode);
        }
    }

    private void ProcessResource()
    {
        collectionTime += Time.deltaTime;
        if (collectionTime >= 2)
        {
            collectionTime = 0.0f;

            targetObject.GetComponent<ResourceCounter>().numberOfResources -= 1;
            inventory += 1;

<<<<<<< HEAD



            if(inventory == 5)
=======
            if (inventory == 5)
>>>>>>> villager brings collected resources to village center and goes on to next resource
            {
                resourceTag = "VillageCenter";
                targetObject = null;
                return;
            }
        }
    }

    //Drawing debug line
    private void OnDrawGizmos()
    {
        if (pathArray == null)
        {
            return;
        }

        if (pathArray.Count > 0)
        {
            int index = 1;
            foreach (Node node in pathArray)
            {
                if (index < pathArray.Count)
                {
                    Node nextNode = (Node)pathArray[index];
                    Debug.DrawLine(node.position, nextNode.position, Color.green);
                    index++;
                }
            };
        }
    }
}