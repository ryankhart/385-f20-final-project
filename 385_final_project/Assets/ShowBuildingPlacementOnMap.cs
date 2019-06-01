using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowBuildingPlacementOnMap : MonoBehaviour
{
    public GameObject plainsTile;

    // Start is called before the first frame update
    void Awake()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Ray myRay = new Ray(transform.position, Vector3.down);
        RaycastHit[] hits = Physics.RaycastAll(myRay);
        Debug.DrawRay(transform.position, new Vector3(0,-5,0), Color.green, 5f);

        foreach (RaycastHit hit in hits)
        {
            Debug.Log("IN");
            SpriteRenderer tileRenderer = hit.transform.GetComponent<SpriteRenderer>();

            Debug.Log(hit.collider.gameObject.tag);

            if (tileRenderer != null)
            {
                Debug.Log("ALMOST");
                Debug.Log(tileRenderer.gameObject.tag);
                if (tileRenderer.gameObject.tag == "PlainsTile")
                {
                    Debug.Log("HIT");
                }
            }
        }
    }
}
