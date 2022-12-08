using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[NodeName("MoveToTarget")]
[StateDisplay("Following",25)]
public class MoveToTargetNode : ActionNode
{
    [HideInInspector]
    new public static string m_AdditionalCategory = "AI/Movement/";
    public string m_TargetName = "Player";
    public string m_AgentName = "Agent";
    private GameObject m_Agent;
    private Transform m_TargetTransform;
    private NavMeshAgent m_NavAgent;

    protected override void OnStart()
    {
        m_TargetTransform = m_BlackBoard.Get<GameObject>(m_TargetName).transform;
        m_Agent = m_BlackBoard.Get<GameObject>(m_AgentName);
        m_NavAgent = m_Agent.GetComponent<NavMeshAgent>();
        m_ParentTree.DisplayNodeState(this);
    }

    public override void OnStop()
    {
        m_ParentTree.RemoveStateDisplay();
        m_NavAgent.isStopped = true;
        // Debug.Log($"OnStop{m_Message }");
    }

    protected override State OnUpdate()
    { 
        if(m_NavAgent.destination != m_TargetTransform.position)
            m_NavAgent.SetDestination(m_TargetTransform.position);
        if (!m_NavAgent.pathPending && m_NavAgent.remainingDistance <= m_NavAgent.stoppingDistance 
                                    && (!m_NavAgent.hasPath || m_NavAgent.velocity.sqrMagnitude == 0f))
        {
            return State.Success;
        }
        
        return State.Running;
    }
    
}
