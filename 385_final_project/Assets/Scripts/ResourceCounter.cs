using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceCounter : MonoBehaviour
{

    public int numberOfResources;

    // Start is called before the first frame update
    void Start()
    {
        numberOfResources = 10;
    }

    // Update is called once per frame
    void Update()
    {
       if(numberOfResources <= 0)
       {
            Destroy(gameObject);
       }
    }
}
