using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[TaskDescription("The selector task is similar to an \"or\" operation. It will return success as soon as one of its child tasks return success. " +
                 "If a child task returns failure then it will sequentially run the next task. If no child task returns success then it will return failure.")]
[NodeName("Selector")]
public class SelectorNode : CompositeNode
{
    private int m_Current;
    protected override void OnStart()
    {
        m_Current = 0;
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
        var child = m_Children[m_Current];
        switch (child.Update())
        {
            case State.Running:
                return State.Running;
            case State.Failure:
                m_Current++;
                break;
            case State.Success:
                return State.Success;
        }

        return m_Current == m_Children.Count ? State.Failure : State.Running;
    }
}
