using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Guard : Entity
{
    public string m_AgentName = "Agent";
    public string m_WaypointParentName = "WayPointParent";
    public string m_PlayerName = "Player";
    public string m_NinjaName = "Ninja";
    [HideInInspector]
    public BehaviourTreeRunner m_Runner;
    public Transform m_WayPointParent;
    public GameObject m_Player;
    public Entity m_Ninja;
    void Start()
    {
        m_Runner = GetComponent<BehaviourTreeRunner>();
        m_Runner.m_Tree.m_BlackBoard.Set(m_AgentName,gameObject);
        m_Runner.m_Tree.m_BlackBoard.Set(m_WaypointParentName,m_WayPointParent);
        m_Runner.m_Tree.m_BlackBoard.Set(m_PlayerName, m_Player);
        m_Runner.m_Tree.m_BlackBoard.Set(m_NinjaName, m_Ninja);
    }
}
