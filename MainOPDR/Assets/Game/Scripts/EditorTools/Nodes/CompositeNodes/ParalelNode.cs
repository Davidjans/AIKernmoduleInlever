using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[TaskDescription("Executes children at the same time. Only stops when everything has returned a success.")]
[NodeName("Parallel")]
public class ParalelNode : CompositeNode
{
    public bool m_RerunChildrenOnFail = true;
    protected override void OnStart()
    {
    }

    public override void OnStop()
    {
        foreach (var child in m_Children)
        {
            if (child.m_State == State.Running)
            {
                child.OnStop();
            }
        }
    }

    protected override State OnUpdate()
    {
        
        foreach (var child in m_Children)
        {
            if((m_State == State.Running || m_State == State.Running) || (child.m_State == State.Failure && m_RerunChildrenOnFail))
                child.Update();
        }
        bool onlySuccess = true;
        foreach (var child in m_Children)
        {
            if (child.m_State == State.Running || child.m_State == State.Failure)
            {
                onlySuccess = false;
            }
        }

        if (onlySuccess)
        {
            return State.Success;
        }

        return State.Running;
    }
}
