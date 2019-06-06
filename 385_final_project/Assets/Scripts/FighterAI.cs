using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterAI : MonoBehaviour
{
    public GameObject targetObject;
    private Transform target;
    public string resourceTag = "Monsters";
    public string lastResource;

    [SerializeField]
    private float movementSpeed = 5.0f;
    private float rotationSpeed = 10f;
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
        stateMachine.ChangeState(new SearchStateFighter(this));
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

        if (targetObject == null)
        {
            stateMachine.ChangeState(new SearchStateFighter(this));
        }
        else if (checkIfAtDestination(villagerPosition))
        {
            //If target is too close that means we need to peform an action!
            //Check to make sure object is there.
            if (targetObject != null)
            {
                if (resourceTag == "Monsters")
                {
                    Destroy(targetObject);
                    stateMachine.ChangeState(new SearchStateFighter(this));
                }
                else if (resourceTag == "Fort")
                {
                    setTag("Monsters");
                    stateMachine.ChangeState(new SearchStateFighter(this));
                }
            }
        }
    }

    private bool waitTownFolk(int time)
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > time)
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

    public void setTag(String tag)
    {
        resourceTag = tag;
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
           
            if (Vector3.Distance(villagerPosition, last) < 1)
            {
                return true;
            }


        }
        catch (Exception)
        {
            stateMachine.ChangeState(new SearchStateFighter(this));
            return false;
        }
       
        return false;
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
        catch
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