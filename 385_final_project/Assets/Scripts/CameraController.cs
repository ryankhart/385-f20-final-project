using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float panSpeed = 20f;
    public float zoomSpeed = 0.5f;

    // Update is called once per frame
    void Update()
    {
        // Move camera relative to time, not to framerate by using Time.deltaTime.
        // if-else is not used because player should be able to press 2 separate
        // keys to go diagonally.
        Vector3 position = transform.position;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            position.y += panSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            position.x -= panSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            position.y -= panSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            position.x += panSpeed * Time.deltaTime;
        }

        // Basic camera zoom by adjusting camera position
        position.z += Input.mouseScrollDelta.y * zoomSpeed;

        // Update camera position
        transform.position = position;
    }
}
