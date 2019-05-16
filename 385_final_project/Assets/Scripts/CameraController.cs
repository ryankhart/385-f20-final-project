using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float m_panSpeed = 2f;
    public float m_zoomSpeed = 0.5f;
    public float m_fastModeMultiplier = 2;

    private bool m_isInFastMode = false;

    // Update is called once per frame
    void Update()
    {
        // Move camera relative to time, not to framerate by using Time.deltaTime.
        // if-else is not used because player should be able to press 2 separate
        // keys to go diagonally.
        Vector3 position = transform.position;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            position.y += m_panSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            position.x -= m_panSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            position.y -= m_panSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            position.x += m_panSpeed * Time.deltaTime;
        }

        // Basic camera zoom by adjusting camera position
        // Requires perspective mode to work
        position.z += Input.mouseScrollDelta.y * m_zoomSpeed;

        // Update camera position
        transform.position = position;
    }

    public void EnableFastMode()
    {
        if (!m_isInFastMode)
        {
            print("Enabling Camera Fast Mode");
            m_isInFastMode = true;
            m_panSpeed *= m_fastModeMultiplier;
        }
    }

    public void DisableFastMode()
    {
        if (m_isInFastMode)
        {
            print("Disabling Camera Fast Mode");
            m_isInFastMode = false;
            m_panSpeed /= m_fastModeMultiplier;
        }
    }
}
