﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MonsterAI : MonoBehaviour
{
    public GameObject targetObject;
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

    public ArrayList pathArray;

    private float elapsedTime = 0.0f;
    public float intervalTime = 1.0f;
    public int nodesOfMovement = 1;
  
    public StateMachine stateMachine = new StateMachine();
    public string state;

    public float cooldown = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        GridCreator = GameObject.FindGameObjectsWithTag("GridCreator")[0];
        gridManager = GridCreator.GetComponent<GridManager>();
        pathArray = new ArrayList();
        stateMachine.ChangeState(new SearchStateMonster(this));
    }

    // Update is called once per frame
    void Update()
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
        catch (Exception)
        {
            stateMachine.ChangeState(new SearchStateMonster(this));
            return false;
        }
       
        return false;
    }

    public bool waitTownFolk(int time)
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime > time)
        {
            elapsedTime = 0.0f;
            return true;
        }
        return false;
    }

    //Finds a target with a tag
    public Transform FindTarget()
    {
        //Gets all objects with tag
        GameObject[] targets = GameObject.FindGameObjectsWithTag(townFolkTag);

        Transform closest;

        // If there are no trees
        if (targets.Length == 0)
        {
            return null;
        }

        int rand = UnityEngine.Random.Range(0, targets.Length);

        closest = targets[rand].transform;
        targetObject = targets[rand];


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
                    Debug.DrawLine(node.position, nextNode.position, Color.red);
                    index++;
                }
            };
        }
    }


}