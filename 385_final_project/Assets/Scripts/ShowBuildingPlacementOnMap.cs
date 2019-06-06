using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowBuildingPlacementOnMap : MonoBehaviour
{
    public GameObject buildingRadicalPrefab;
    private GameObject buildingRadical;
    private Color32 green;
    private Color32 red;

    private void Awake()
    {
        green = new Color32(61, 255, 61, 130);
        red = new Color32(255, 61, 61, 130);
        Vector3 radicalPosition = transform.position;
        radicalPosition.y = 0.2f;
        buildingRadical = Instantiate(buildingRadicalPrefab, radicalPosition, Quaternion.Euler(90, 0, 0));
        buildingRadical.GetComponent<MeshRenderer>().material.color = red;
    }

    // Update is better than FixedUpdate - in FixedUpdate, the radical sometimes doesn't change color when it should
    void Update()
    {
        //// if building gets placed
        //if(transform.position.y < 0.2)
        //{
        //    Destroy(buildingRadical);
        //}

        Vector3 radicalPosition = transform.position;
        radicalPosition.y = 0.1f;
        buildingRadical.transform.position = radicalPosition;
        Ray myRay = new Ray(transform.position, Vector3.down);
        RaycastHit[] hits = Physics.RaycastAll(myRay);

        foreach (RaycastHit hit in hits)
        {
            SpriteRenderer tileRenderer = hit.collider.GetComponent<SpriteRenderer>();

            if (tileRenderer != null)
            {
                if (tileRenderer.gameObject.tag == "PlainsTile")
                {
                    buildingRadical.GetComponent<MeshRenderer>().material.color = green;
                }
                else
                {
                    buildingRadical.GetComponent<MeshRenderer>().material.color = red;
                }
            }
        }
    }

    public void DestroyRadical()
    {
        Destroy(buildingRadical);
    }

    void OnDestroy()
    {
        Destroy(buildingRadical);
    }
}
