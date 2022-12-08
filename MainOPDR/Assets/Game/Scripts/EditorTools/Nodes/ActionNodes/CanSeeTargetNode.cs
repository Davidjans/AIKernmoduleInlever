using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
[NodeName("CanSeeTarget")]
public class CanSeeTargetNode : ActionNode
{
    [HideInInspector]
    new public static string m_AdditionalCategory = "AI/Targetting/";
    
    public float m_MaxDistance;
    public float m_WithinAngle = 60;
    public string m_AgentName = "Agent";
    public string m_TargetName = "Player";
    public string m_SeesTargetName = "SeesTarget";
    
    private GameObject m_Agent;
    private GameObject m_Player;
    protected override void OnStart()
    {
        m_Player = m_BlackBoard.Get<GameObject>(m_TargetName);
        m_Agent = m_BlackBoard.Get<GameObject>(m_AgentName);
    }

    public override void OnStop()
    {
       // Debug.Log($"OnStop{m_Message }");
    }

    protected override State OnUpdate()
    {
        float angle = Vector3.Angle(m_Agent.transform.forward,
            m_Player.transform.position - m_Agent.transform.position);
        //Debug.LogError("Angle: " + angle  );
        if (angle < m_WithinAngle)
        {
            RaycastHit hit;
            Vector3 direction = m_Player.transform.position - m_Agent.transform.position;
            if (Physics.Raycast(m_Agent.transform.position, direction, out hit, m_MaxDistance))
            {
                if (hit.transform.gameObject == m_Player)
                {
                    m_BlackBoard.Set(m_SeesTargetName, true);
                    return State.Success;
                }
            }
        }
        m_BlackBoard.Set(m_SeesTargetName, false);
        return State.Failure;
    }
}
