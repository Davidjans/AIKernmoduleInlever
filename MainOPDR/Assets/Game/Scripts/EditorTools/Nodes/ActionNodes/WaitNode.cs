using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[NodeName("Wait")]
public class WaitNode : ActionNode
{
    public float m_Duration = 1;
    protected float m_CurrentTime;
    protected override void OnStart()
    {
        m_CurrentTime = 0;
    }

    public override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        m_CurrentTime += Time.deltaTime;
        if (m_CurrentTime >= m_Duration)
        {
            return State.Success;
        }

        return State.Running;
    }
}
