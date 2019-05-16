using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScryMode : MonoBehaviour
{
    public GameObject m_ScryOverlay;
    public Button m_ScryToggleButton;

    // Start is called before the first frame update
    void Start()
    {
        m_ScryToggleButton.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        Toggle();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Toggle()
    {
        if (IsActive()) Deactivate();
        else Activate();
    }

    bool IsActive()
    {
        return (m_ScryOverlay.activeSelf);
    }

    void Activate()
    {
        m_ScryOverlay.SetActive(true);
    }

    void Deactivate()
    {
        m_ScryOverlay.SetActive(false);
    }
}
