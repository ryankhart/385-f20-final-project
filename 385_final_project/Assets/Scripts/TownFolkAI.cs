using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TownFolkAI : MonoBehaviour
{
    public GameObject targetObject;
    private Transform target;
    public string resourceTag = "Tree";
    public string lastResource;

    [SerializeField]
    private float movementSpeed = 5.0f;
    private float rotationSpeed = 10f;
    [SerializeField]
    private float toleranceRadius = .5f;

    private float currentSpeed;
    private Vector3 targetPoint;
    private Vector3 direction;
    private Quaternion targetRotation;

    public Node startNode { get; set; }
    public Node goalNode { get; set; }

    private ArrayList pathArray;

    private float elapsedTime = 0.0f;
    public float collectionTime = 0.0f;
    public float intervalTime = 1.0f;
    public GameObject GridCreator;
    public GridManager gridManager;

    public int nodesOfMovement = 1;
    public int inventory = 0;

    public StateMachine stateMachine = new StateMachine();
    public string state;

    // Start is called before the first frame update
    void Start()
    {
        GridCreator = GameObject.FindGameObjectsWithTag("GridCreator")[0];
        gridManager = GridCreator.GetComponent<GridManager>();
        //Calculate the path using our AStart code.
        pathArray = new ArrayList();
        setResource();
        stateMachine.ChangeState(new SearchState(this));
    }


    private void Update()
    {
        stateMachine.Update();
    }

    public void moveToDestination()
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
        return;
    }

    public void checkAction()
    {
        // Checks if target is too close
        Vector3 villagerPosition = transform.position;
        villagerPosition.y = 0;

        if(targetObject == null)
        {
            stateMachine.ChangeState(new SearchState(this));
        }
        else if(checkIfAtDestination(villagerPosition))
        {
            //If target is too close that means we need to peform an action!
            //Check to make sure object is there.
            if (targetObject != null)
            {
                if (resourceTag == "Home")
                {
                    //wait
                    if(waitTownFolk(1))
                    {
                       setTag("Tavern");
                       stateMachine.ChangeState(new SearchState(this));
                    }
                }
                else if (resourceTag == "Tree")
                {
                    ProcessResource();
                }
                else if (resourceTag == "VillageCenter")
                {
                    GoStoreCollectedResources(villagerPosition);
                }
                else if (resourceTag == "Stone")
                {
                    ProcessResource();
                }
                else if(resourceTag == "Tavern")
                {
                    if (waitTownFolk(20))
                    {
                        setTag(lastResource);
                        stateMachine.ChangeState(new SearchState(this));
                    }
                }
            }
        }
    }

    private bool waitTownFolk(int time)
    {
        elapsedTime += Time.deltaTime;
        if(elapsedTime > time)
        {
            elapsedTime = 0.0f;
            return true;
        }
        return false;
    }

    public bool checkIfAtDestination(Vector3 villagerPosition)
    {
        if (pathArray.Count == 0)
        {
            return true;
        }

        Node nextNode = (Node)pathArray[pathArray.Count - 1];
        Vector3 last = nextNode.position;
        if (Vector3.Distance(villagerPosition, last) < toleranceRadius)
        {
           
            return true;
        }

        return false;
    }

    public bool checkIfAtNode(Vector3 villagerPosition)
    {
        try
        {
            if (pathArray.Count == 0 || nodesOfMovement > pathArray.Count - 1)
            {
                return true;
            }

            Node nextNode = (Node)pathArray[nodesOfMovement];

            Vector3 last = nextNode.position;
          
            if (Vector3.Distance(villagerPosition, last) < .25)
            {
                return true;
            }

            
        }
        catch(Exception)
        {
            stateMachine.ChangeState(new SearchState(this));
            return false;
        }
        return false;
    }

    private void GoStoreCollectedResources(Vector3 villagerPosition)
    {
        if (checkIfAtDestination(villagerPosition))
        {
            if (targetObject.tag == "VillageCenter")
            {
                // drop the collected resources off at the village center
                targetObject.GetComponent<TrackStorageResources>().AddResourceUnits(lastResource, inventory);
                inventory = 0;
                targetObject = null;
                setResource();
                stateMachine.ChangeState(new SearchState(this));
            }
        }
    }

    public void setTag(String tag)
    {
        if(resourceTag.Equals("Tree"))
        {
            lastResource = "Tree";
        }
        else if(resourceTag.Equals("Stone"))
        {
            lastResource = "Stone";
        }
        else if (resourceTag.Equals("Copper"))
        {
            lastResource = "Copper";
        }
        resourceTag = tag;
    }

    private void setResource()
    {
        int rand = UnityEngine.Random.Range(1, 4);
        if(rand <= 2)
        {
            resourceTag = "Tree";
            lastResource = "Tree";
        }
        else
        {
            resourceTag = "Stone";
            lastResource = "Stone";
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
            targetObject = null;
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
                //nodesOfMovement = 1;
            }
        }
        catch (Exception)
        {

        }

    }

    //A* finds the path that the TownFolk should take.
    public void FindPath()
    {
        nodesOfMovement = 1;
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

    private void ProcessResource()
    {
        collectionTime += Time.deltaTime;
        if (collectionTime >= 10)
        {
            //They are waiting too long
            collectionTime = 0.0f;
            setTag("VillageCenter");
            stateMachine.ChangeState(new SearchState(this));
            return;
        }


        if (collectionTime >= 3)
        {
            targetObject.GetComponent<ResourceCounter>().numberOfResources -= 1;
            inventory += 1;
            collectionTime -= 3;
        }
       


        if(inventory == 1)
        {
            collectionTime = 0.0f;
            setTag("VillageCenter");
            stateMachine.ChangeState(new SearchState(this));
            return;
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