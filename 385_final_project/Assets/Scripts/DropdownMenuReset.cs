using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropdownMenuReset : MonoBehaviour
{
    Dropdown dropdown;
    int originalState;

    private void Awake()
    {
        dropdown = gameObject.GetComponent<Dropdown>();
        originalState = dropdown.value;
    }

    void Update()
    { 
        if(dropdown.value != originalState && Input.GetMouseButtonDown(0))
        {
            dropdown.value = originalState;
        }
    }
}
