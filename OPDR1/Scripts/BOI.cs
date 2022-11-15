using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BOI : MonoBehaviour
{
    public Vector3 m_PersonalizedCenterOfMass;
    public Vector3 m_Velocity = Vector3.zero;

    public Vector2 m_MinMaxVelocity;
    public void ChangeVelocity(Vector3 newVelocity)
    {
        m_Velocity.x = Mathf.Clamp(newVelocity.x, m_MinMaxVelocity.x, m_MinMaxVelocity.y);
        m_Velocity.y = Mathf.Clamp(newVelocity.y, m_MinMaxVelocity.x, m_MinMaxVelocity.y);
        m_Velocity.z = Mathf.Clamp(newVelocity.z, m_MinMaxVelocity.x, m_MinMaxVelocity.y);
    }
}
