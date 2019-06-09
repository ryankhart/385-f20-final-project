using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UpdateResourceCounter : MonoBehaviour
{
    private Text count;

    // Start is called before the first frame update
    void Start()
    {
        count = gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int SetCount(int addValue)
    {
        int oldValue = 0;
        try
        {
            oldValue = Int32.Parse(count.text);
        }
        catch (FormatException)
        {
            print("Unable to parse input");
        }
        int newValue = oldValue + addValue;
        //if(newValue >= 0)
        //{
            count.text = newValue.ToString();
        //}
        return newValue;
    }
}
