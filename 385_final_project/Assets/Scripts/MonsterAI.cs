using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MonsterAI : MonoBehaviour
{
    private GameObject targetObject;
    private GameObject GridCreator;
    private GridManager gridManager;
    public string townFolkTag = "TownFolk";
    [SerializeField]
    private float movementSpeed = 5.0f;
    private float rotationSpeed = 100.0f;
    [SerializeField]
    private float toleranceRadius = .25f;

    private float currentSpeed;
    private Vector3 targetPoint;
    private Vector3 direction;
    private Quaternion targetRotation;

    public Node startNode { get; set; }
    public Node goalNode { get; set; }

    private ArrayList pathArray;

    private float elapsedTime = 0.0f;
    public float intervalTime = 1.0f;
    private int nodesOfMovement = 1;
    private int range = 5;

    public float cooldown = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        GridCreator = GameObject.FindGameObjectsWithTag("GridCreator")[0];
        gridManager = GridCreator.GetComponent<GridManager>();

        pathArray = new ArrayList();
        FindPath();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        elapsedTime += Time.deltaTime;

        //How much time it takes to find a new path.
        if (elapsedTime >= intervalTime)
        {
            if(cooldown <= 0.0f)
            {
                elapsedTime = 0.0f;
                FindPath();
            }
            if(cooldown > 0.0f)
            {
                cooldown -= Time.deltaTime;
            }
        }


        FindNode();

        // Checks if target is too close
        if (Vector3.Distance(targetObject.transform.position, transform.position) < toleranceRadius)
        {
            print("Monster Attack");
            //If target is too close that means we need to peform an action!

            //Check to make sure object is there.
            if (targetObject != null)
            {
                // "Attacks" townfolk
                if (targetObject.tag == townFolkTag && cooldown <= 0)
                {
                    targetObject.GetComponent<TownFolkAI>().flee = true;
                    cooldown = 20.0f;
                }
            }

            return;
        }

        if (pathArray.Count < range)
        {
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
        
    }


    public Transform FindTarget()
    {
        //Gets all objects with tag
        GameObject[] targets = GameObject.FindGameObjectsWithTag(townFolkTag);

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
            print("Error: " + e);
        }

    }

    //A* finds the path that the TownFolk should take.
    private void FindPath()
    {
        Transform startPosition = transform;
        Transform endPosition = FindTarget();

        if (endPosition != null)
        {
            startNode = new Node(gridManager.GetGridCellCenter(gridManager.GetGridIndex(startPosition.position)));
            goalNode = new Node(gridManager.GetGridCellCenter(gridManager.GetGridIndex(endPosition.position)));

            if (goalNode != null)
                pathArray = AStar.FindPath(startNode, goalNode);
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
                    Debug.DrawLine(node.position, nextNode.position, Color.red);
                    index++;
                }
            };
        }
    }



}
