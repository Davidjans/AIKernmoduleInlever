using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepeatNode : DecoratorNode
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
        return State.Running;
    }
}
