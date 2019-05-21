using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TownFolkAI : MonoBehaviour
{
    private GameObject targetObject;
    private Transform target;
    //public Transform home;
    private int state = 0;
    // renamed because if it's named just "tag" it may cause problems if you
    // ever need to access the tag of this game object
    public string resourceTag = "Tree";

    [SerializeField]
    private float movementSpeed = 5.0f;
    private float rotationSpeed = 100.0f;
    private float force = 50.0f;
    private float minimumAvoidanceDistance = 20.0f;
    [SerializeField]
    private float toleranceRadius = .25f;

    private float currentSpeed;
    private Vector3 targetPoint;
    private RaycastHit mouseHit;
    private Camera mainCamera;
    private Vector3 direction;
    private Quaternion targetRotation;
    private RaycastHit avoidanceHit;
    private Vector3 hitNormal;

    //private Transform startPosition;
    //private Transform endPosition;

    public Node startNode { get; set; }
    public Node goalNode { get; set; }

    private ArrayList pathArray;

    private GameObject startCube;
    private GameObject endCube;

    private float elapsedTime = 0.0f;
    public float intervalTime = 1.0f;
    public GameObject GridCreator;
    public GridManager gridManager;
    private int indexGob = 1;


    // Start is called before the first frame update
    void Start()
    {
        //navMeshAgent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        GridCreator = GameObject.Find("GridCreator(Clone)");
        gridManager = GridCreator.GetComponent<GridManager>();
        startCube = GameObject.FindGameObjectWithTag("Start");
        //Calculate the path using our AStart code.
        pathArray = new ArrayList();
        FindPath();
        //target = FindTarget();
        
    }


    private void FixedUpdate()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= intervalTime)
        {
            elapsedTime = 0.0f;
            FindPath();
        }

        FindTarget2();

        direction = (targetPoint - transform.position);
        direction.Normalize();

        if (Vector3.Distance(targetPoint, transform.position) < toleranceRadius)
        {
            return;
        }

        currentSpeed = movementSpeed * Time.deltaTime;

        //Rotate the agent towards its target direction 
        direction = (targetPoint - transform.position);
        direction.Normalize();
        targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        //Move the agent forard
        //transform.position += transform.forward * currentSpeed;
        transform.position += new Vector3((transform.forward * currentSpeed).x, 0, (transform.forward * currentSpeed).z);
        transform.rotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

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

    public void FindTarget2()
    {
        try
        {
            if (indexGob < pathArray.Count && pathArray.Count != 1)
            {
                Node nextNode = (Node)pathArray[indexGob];
                targetPoint = nextNode.position;
            }
            else
            {
                if (targetObject != null)
                    Destroy(targetObject);
            }
        }
        catch(Exception e)
        {

        }
       
    }

    public Transform FindTarget()
    {
        //Get all the trees 
        //TODO: find a way to only find a few trees?
        GameObject[] targets = GameObject.FindGameObjectsWithTag(resourceTag);

        float minDistance = Mathf.Infinity;
        Transform closest;

        // If there are no trees
        if(targets.Length == 0)
        {
            return null;
        }

        closest = targets[0].transform;
        for(int i = 0; i < targets.Length; i++)
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

    private void FindPath()
    {
        Transform startPosition = startCube.transform;
        Transform endPosition = FindTarget();

        if(endPosition != null)
        {
            startNode = new Node(gridManager.GetGridCellCenter(gridManager.GetGridIndex(startPosition.position)));
            goalNode = new Node(gridManager.GetGridCellCenter(gridManager.GetGridIndex(endPosition.position)));

            if(goalNode != null)
            pathArray = AStar.FindPath(startNode, goalNode);
        }
       
    }

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
