﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    private Renderer m_renderer;

    // Start is called before the first frame update
    void Start()
    {
        // TODO: Come back later and replace these static coordinates with a
        // raycast (?) of where the camera is pointed at at the start of the game.
        transform.position = new Vector3(10.75f, 0f, 10.75f);
        m_renderer = GetComponent<Renderer>();
        m_renderer.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}