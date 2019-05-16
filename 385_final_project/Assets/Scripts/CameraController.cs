using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float m_panSpeed = 2f;
    public float m_zoomSpeed = 0.5f;
    public float m_fastModeMultiplier = 2;
    public Vector3 m_position;

    private bool m_isInFastMode = false;

    // Start is called before the first frame update
    private void Start()
    {
        // Set camera position to an easier to read variable
        m_position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Zoom();
        UpdatePosition();
    }

    void Move()
    {
        // Move camera relative to time, not to framerate by using Time.deltaTime.
        // if-else is not used because player should be able to press 2 separate
        // keys to go diagonally.
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            m_position.y += m_panSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            m_position.x -= m_panSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            m_position.y -= m_panSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            m_position.x += m_panSpeed * Time.deltaTime;
        }
    }

    void Zoom()
    {
        // Basic camera zoom by adjusting camera position
        // Requires perspective mode to work
        m_position.z += Input.mouseScrollDelta.y * m_zoomSpeed;
    }

    void UpdatePosition()
    {
        transform.position = m_position;
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
