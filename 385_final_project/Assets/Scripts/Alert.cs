using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alert : MonoBehaviour
{
    public GameObject m_player;
    public GameObject m_target;
    
    private Vector3 m_difference;
    private float m_radiansToRotateZ;
    private float m_degreesToRotateZ;

    // Update is called once per frame
    void Update()
    {
        m_difference = m_player.transform.position - m_target.transform.position;

        // SOH CAH TOA
        // Arctan( opposite (y) over adjacent (x) ) = radian angle
        m_radiansToRotateZ = Mathf.Atan2(m_difference.z, m_difference.x);
        m_degreesToRotateZ = m_radiansToRotateZ * Mathf.Rad2Deg;

        // Not sure why, but without the "+ 90" here, it's always 90 degrees off.
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, m_degreesToRotateZ + 90);
    }
}
