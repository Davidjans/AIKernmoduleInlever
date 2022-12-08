using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[NodeName("WayPointMove")]
[StateDisplay("Patrolling",20)]
public class WaypointMoveNode : ActionNode
{
    [HideInInspector]
    new public static string m_AdditionalCategory = "AI/Movement/";
    public string m_AgentName = "Agent";
    public string m_WayPointParentName = "WayPointParent";
    public int m_CurrentWaypointPoint;
    public FollowMethod m_FollowMethod;
    public bool m_Reverse = false;
    private GameObject m_Agent;
    private NavMeshAgent m_NavAgent;
    private List<Transform> m_WaypointPoints = new List<Transform>();
    
    protected override void OnStart()
    {
        m_Agent = m_BlackBoard.Get<GameObject>(m_AgentName);
        m_NavAgent = m_Agent.GetComponent<NavMeshAgent>();
        m_WaypointPoints.Clear();
        foreach (Transform child in m_BlackBoard.Get<Transform>(m_WayPointParentName))
        { 
            m_WaypointPoints.Add(child);   
        }
        m_ParentTree.DisplayNodeState(this);
    }

    public override void OnStop()
    {
        m_ParentTree.RemoveStateDisplay();
       // Debug.Log($"OnStop{m_Message }");
    }

    protected override State OnUpdate()
    { 
        if (!m_NavAgent.pathPending && m_NavAgent.remainingDistance <= m_NavAgent.stoppingDistance 
                                    && (!m_NavAgent.hasPath || m_NavAgent.velocity.sqrMagnitude == 0f))
        {
            if (m_CurrentWaypointPoint == m_WaypointPoints.Count - 1 && !m_Reverse)
            {
                if(m_FollowMethod == FollowMethod.Loop && !m_Reverse)
                    m_CurrentWaypointPoint = 0;
                else if (m_FollowMethod == FollowMethod.WalkBack)
                    m_Reverse = !m_Reverse;
                else if (m_FollowMethod == FollowMethod.SuccessOnLastPointReached)
                    return State.Success;
            }
            else if (m_CurrentWaypointPoint == 0 && m_Reverse)
            {
                if(m_FollowMethod == FollowMethod.Loop && !m_Reverse)
                    m_CurrentWaypointPoint = m_WaypointPoints.Count -1;
                else if (m_FollowMethod == FollowMethod.WalkBack)
                    m_Reverse = !m_Reverse;
                else if (m_FollowMethod == FollowMethod.SuccessOnLastPointReached)
                    return State.Success;
            }

            if (m_Reverse)
            {
                m_CurrentWaypointPoint--;
            }
            else
            {
                m_CurrentWaypointPoint++;
            }
            m_NavAgent.SetDestination(m_WaypointPoints[m_CurrentWaypointPoint].position);
        }
        
        return State.Running;
    }

    public enum FollowMethod
    {
        Loop,
        WalkBack,
        SuccessOnLastPointReached
    }
    
}
