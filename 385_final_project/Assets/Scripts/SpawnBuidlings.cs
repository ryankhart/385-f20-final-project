using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

// extending EventTrigger to override dropdown functions
public class SpawnBuidlings : MonoBehaviour
{
    public Dropdown m_Dropdown;

    void Start()
    {
        m_Dropdown = GetComponent<Dropdown>();
        m_Dropdown.onValueChanged.AddListener(delegate {
            DropdownValueChanged(m_Dropdown);
        });
    }

    //Ouput the new value of the Dropdown into Text
    void DropdownValueChanged(Dropdown change)
    {
        Debug.Log("Selection performed" + change);
    }
}
