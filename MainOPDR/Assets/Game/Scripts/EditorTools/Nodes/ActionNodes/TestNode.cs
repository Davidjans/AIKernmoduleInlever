
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[NodeName("Test")]
public class TestNode : ActionNode
{

    protected override void OnStart()
    {
        throw new System.NotImplementedException();
    }

    public override void OnStop()
    {
        throw new System.NotImplementedException();
    }

    protected override State OnUpdate()
    {
        return State.Success;
    }
}
