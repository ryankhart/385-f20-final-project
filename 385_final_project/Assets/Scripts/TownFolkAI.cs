using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TownFolkAI : MonoBehaviour
{
    private GameObject targetObject;
    private Transform target;
    public string resourceTag = "Tree";

    [SerializeField]
    private float movementSpeed = 5.0f;
    private float rotationSpeed = 10f;
    [SerializeField]
    private float toleranceRadius = .25f;

    private float currentSpeed;
    private Vector3 targetPoint;
    private Vector3 direction;
    private Quaternion targetRotation;
    private RaycastHit avoidanceHit;
    private Vector3 hitNormal;

    //private Transform startPosition;
    //private Transform endPosition;

    public Node startNode { get; set; }
    public Node goalNode { get; set; }

    private ArrayList pathArray;

    private float elapsedTime = 0.0f;
    private float collectionTime = 0.0f;
    public float intervalTime = 1.0f;
    public GameObject GridCreator;
    public GridManager gridManager;

    private int nodesOfMovement = 1;
    private int inventory = 0;
    private string currentResource;


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
        if (Vector3.Distance(targetPoint, transform.position) < toleranceRadius)
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
                    //Process tree
                    ProcessResource();

                }
            }




            return;
        }

        currentSpeed = movementSpeed * Time.deltaTime;

        //Rotate the agent towards its target direction
        direction = (targetPoint - transform.position).normalized;
        direction.y = 0;

        // look
        targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        //Move the agent forard
        transform.position += new Vector3(direction.x * currentSpeed, direction.y, direction.z * currentSpeed);

        /*
        //print(target);
        if(target != null && state == 0)
        {
            navMeshAgent.SetDestination(target.position);
        }
        else
        {
           // print(state);
            //Going home
            if (state == 0)
            {
                target = home;
            }
            else
            {
                target = FindTarget();
                state = 0;
            }
            
        }
        */
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
        if (collectionTime >= 5)
        {

            collectionTime = 0.0f;

            targetObject.GetComponent<ResourceCounter>().numberOfResources -= 1;
            inventory += 1;
<<<<<<< HEAD



=======
>>>>>>> added script to village center




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