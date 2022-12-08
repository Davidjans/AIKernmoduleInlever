using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertNode : DecoratorNode
{
    protected override void OnStart()
    {
        
    }

    public override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        m_Child.Update();
        if (m_Child.m_State == State.Success)
        {
            return State.Failure;
        }
        else if (m_Child.m_State == State.Failure)
        {
            return State.Success;
        }
        return State.Running;
    }
}
