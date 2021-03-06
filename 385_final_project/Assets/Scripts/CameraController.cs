using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float m_panSpeed = 2f;
    public float m_zoomSpeed = 0.5f;
    public float m_fastModeMultiplier = 10;
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
        //print("Pan Speed is now " + m_panSpeed);
        Zoom();
        UpdatePosition();
    }

    void Move()
    {
        // Move camera relative to time, not to framerate by using Time.deltaTime.
        // if-else is not used because player should be able to press 2 separate
        // keys to go diagonally.
        float panSpeed = Time.deltaTime * 10;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            m_position.z += m_panSpeed * panSpeed;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            m_position.x -= m_panSpeed * panSpeed;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            m_position.z -= m_panSpeed * panSpeed;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            m_position.x += m_panSpeed * panSpeed;
        }
    }

    void Zoom()
    {
        // Basic camera zoom by adjusting camera position
        // Requires perspective mode to work
        float zoomY = Input.mouseScrollDelta.y * m_zoomSpeed;
        if(zoomY == 0)
        {
            return;
        }
        m_position.y = Mathf.Clamp(m_position.y - (zoomY * m_zoomSpeed), 5, 15);
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
            print("Pan Speed is now " + m_panSpeed);
        }
    }

    public void DisableFastMode()
    {
        if (m_isInFastMode)
        {
            print("Disabling Camera Fast Mode");
            m_isInFastMode = false;
            m_panSpeed /= m_fastModeMultiplier;
            print("Pan Speed is now " + m_panSpeed);
        }
    }
}
