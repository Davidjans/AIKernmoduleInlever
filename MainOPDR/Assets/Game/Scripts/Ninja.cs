using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ninja : Entity
{
    public string m_AgentName = "Agent";
    public string m_PlayerName = "Player";
    public string m_EnemyName = "Enemy";
    [HideInInspector]
    public BehaviourTreeRunner m_Runner;
    public GameObject m_Player;
    public GameObject m_Enemy;
    void Start()
    {
        m_Runner = GetComponent<BehaviourTreeRunner>();
        m_Runner.m_Tree.m_BlackBoard.Set(m_AgentName,gameObject);
        m_Runner.m_Tree.m_BlackBoard.Set(m_PlayerName, m_Player);
        m_Runner.m_Tree.m_BlackBoard.Set(m_EnemyName,m_Enemy);
    }
}
