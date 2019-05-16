using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScryMode : MonoBehaviour
{
    public GameObject m_Overlay;
    public Button m_ToggleButton;
    public Camera m_Camera;

    private CameraController m_CameraController;
    private Vector3 m_OriginalCameraPosition;

    // Start is called before the first frame update
    void Start()
    {
        // If the the toggle button is clicked, run TaskOnClick().
        m_ToggleButton.onClick.AddListener(TaskOnClick);

        // Set the CameraController to an easy to read variable
        m_CameraController = m_Camera.GetComponent<CameraController>();
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
        return (m_Overlay.activeSelf);
    }

    void Activate()
    {
        m_OriginalCameraPosition = m_CameraController.m_position;
        m_Overlay.SetActive(true);
        m_CameraController.EnableFastMode();
    }

    void Deactivate()
    {
        m_Overlay.SetActive(false);
        m_CameraController.DisableFastMode();
        m_CameraController.m_position = m_OriginalCameraPosition;
    }
}
