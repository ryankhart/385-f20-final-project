using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LumberJackAI : MonoBehaviour
{
    private GameObject targetObject;
    private Transform target;
    public Transform home;
    private int state = 0;
    public string tag = "Tree";

    [SerializeField]
    private float movementSpeed = 5.0f;
    [SerializeField]
    private float rotationSpeed = 100.0f;
    [SerializeField]
    private float force = 50.0f;
    [SerializeField]
    private float minimumAvoidanceDistance = 20.0f;
    [SerializeField]
    private float toleranceRadius = 2.0f;

    private float currentSpeed;
    private Vector3 targetPoint;
    private RaycastHit mouseHit;
    private Camera mainCamera;
    private Vector3 direction;
    private Quaternion targetRotation;
    private RaycastHit avoidanceHit;
    private Vector3 hitNormal;


    private Transform startPosition;
    private Transform endPosition;

    public Node startNode { get; set; }
    public Node goalNode { get; set; }

    private ArrayList pathArray;

    private GameObject startCube;
    private GameObject endCube;

    private float elapsedTime = 0.0f;
    public float intervalTime = 1.0f;
    private GridManager gridManager;
    private int indexGob = 1;


    // Start is called before the first frame update
    void Start()
    {
        //navMeshAgent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        gridManager = FindObjectOfType<GridManager>();
        startCube = GameObject.FindGameObjectWithTag("Start");
        endCube = GameObject.FindGameObjectWithTag("End");
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
        transform.position += transform.forward * currentSpeed;

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
        if(indexGob < pathArray.Count)
        {
            Node nextNode = (Node)pathArray[indexGob];
            targetPoint = nextNode.position;
        }
        else
        {
            if(targetObject != null)
            Destroy(targetObject); 
        }
    }

    public Transform FindTarget()
    {
        //Get all the trees 
        //TODO: find a way to only find a few trees?
        GameObject[] targets = GameObject.FindGameObjectsWithTag(tag);

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
                targetPoint = targets[i].transform.position;
            }
        }

        return closest;

        

    }

    private void ApplyAvoidance(ref Vector3 direction)
    {
        //Only detect layer 8 (Obstacles)
        //We use bitshifting to create a layermask with a value of 
        //0100000000 where only the 8th position is 1, so only it is active.
        int layerMask = 1 << 8;

        //Check that the agent hit with the obstacles within it's minimum distance to avoid
        if (Physics.Raycast(transform.position, transform.forward, out avoidanceHit, minimumAvoidanceDistance, layerMask))
        {
            //Get the normal of the hit point to calculate the new direction
            hitNormal = avoidanceHit.normal;
            hitNormal.y = 0.0f; //Don't want to move in Y-Space

            //Get the new direction vector by adding force to agent's current forward vector
            direction = transform.forward + hitNormal * force;
        }

    }

    private void FindPath()
    {
        startPosition = startCube.transform;
        endPosition = FindTarget();

        if(endPosition != null)
        {
            startNode = new Node(gridManager.GetGridCellCenter(gridManager.GetGridIndex(startPosition.position)));
            goalNode = new Node(gridManager.GetGridCellCenter(gridManager.GetGridIndex(endPosition.position)));

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
