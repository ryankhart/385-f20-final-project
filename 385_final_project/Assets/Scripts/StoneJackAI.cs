using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoneJackAI : MonoBehaviour
{

    private Transform target;
    public Transform home;
    //public GameObject myGameObject;
    //private Rigidbody rb;
    public int speed = 2;
    private int state = 0;
    public UnityEngine.AI.NavMeshAgent navMeshAgent;



    // Start is called before the first frame update
    void Start()
    {
        //rb = GetComponent<Rigidbody>();
        navMeshAgent = gameObject.GetComponent<UnityEngine.AI.NavMeshAgent>();
        target = FindTarget();
    }


    private void FixedUpdate()
    {
        //print(target);
        if (target != null && state == 0)
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
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Stone")
        {
            Destroy(col.gameObject);
        }
        else
        {
            target = null;
            state = 1;
        }
    }




    public Transform FindTarget()
    {
        //Get all the trees 
        //TODO: find a way to only find a few trees?
        GameObject[] targets = GameObject.FindGameObjectsWithTag("Stone");

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
                closest = targets[i].transform;
                minDistance = distance;
            }
        }

        return closest;



    }


}
