using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public abstract class Node : ScriptableObject
{
    public enum State
    {
        Running,
        Failure,
        Success,
        Nothing
    }

    [ReadOnly] public State m_State = State.Nothing;
    [HideInInspector] public bool m_Started = false;
    [HideInInspector] public string m_Guid;
    [HideInInspector] public Vector2 m_Position;
    [HideInInspector] public Node m_Parent;
    [HideInInspector] public BehaviourTree m_ParentTree;
    [HideInInspector] public static string m_AdditionalCategory = "";
    [HideInInspector] public BlackBoard m_BlackBoard;
    public State Update()
    {
        if (!m_Started)
        {
            OnStart();
            m_Started = true;
            m_State = State.Running;
        }

        m_State = OnUpdate();

        if (m_State == State.Failure || m_State == State.Success)
        {
            OnStop();
            m_Started = false;
        }

        return m_State;
    }

    public virtual Node Clone()
    {
        return Instantiate(this);
    }

    protected abstract void OnStart();
    public abstract void OnStop();
    protected abstract State OnUpdate();
}