using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Alert : MonoBehaviour
{
    public GameObject m_player;
    public GameObject m_target;
    
    private Vector3 m_difference;
    private float m_rotationZ;

    // Update is called once per frame
    void Update()
    {
        m_difference = m_player.transform.position - m_target.transform.position;
        print("Difference: X=" + m_difference.x + "  Y=" + m_difference.y + "  Z=" + m_difference.z);
        m_rotationZ = Mathf.Atan2(m_difference.z, m_difference.x) * Mathf.Rad2Deg;
        print("Z rotation is " + m_rotationZ);
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, m_rotationZ);
    }
}
