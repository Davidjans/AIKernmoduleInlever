using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
[NodeName("Attack")]
[StateDisplay("Attacking",50)]
public class AttackNode : ActionNode
{
    [HideInInspector]
    new public static string m_AdditionalCategory = "AI/Targetting/";

    public string m_TargetName = "Player";
    public string m_AgentName = "Agent";
    public float m_MaxDistance;
    private Entity m_AgentEntity;
    private Entity m_TargetEntity;
    protected override void OnStart()
    {
        m_AgentEntity = m_BlackBoard.Get<GameObject>(m_AgentName).GetComponent<Entity>();
        m_TargetEntity = m_BlackBoard.Get<GameObject>(m_TargetName).GetComponent<Entity>();
        m_TargetEntity.GettingAttacked();
        m_ParentTree.DisplayNodeState(this);
        
    }

    public override void OnStop()
    {
        m_ParentTree.RemoveStateDisplay();
        m_TargetEntity.NoLongerGettingAttacked();
        // Debug.Log($"OnStop{m_Message }");
    }

    protected override State OnUpdate()
    {
    
        m_AgentEntity.Attack();
        //Debug.LogError("it do the pewpew");
        return State.Running;
    }
}