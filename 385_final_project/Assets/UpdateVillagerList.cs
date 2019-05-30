using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateVillagerList : MonoBehaviour
{
    private Transform header;

    // Start is called before the first frame update
    void Start()
    {
        header = gameObject.transform.Find("Text");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddVillager(int index)
    {
        Text newVillInfo = gameObject.AddComponent<Text>();
        //trans.anchoredPosition = new Vector2(x, y);
    }

    // TODO: public void RemoveVillager()
}
