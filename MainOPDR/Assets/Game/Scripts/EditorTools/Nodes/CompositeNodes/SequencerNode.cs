using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental;
using UnityEngine;
[TaskDescription("The sequence task is similar to an \"and\" operation. It will return failure as soon as one of its child tasks return failure. " +
                 "If a child task returns success then it will sequentially run the next task. If all child tasks return success then it will return success.")]
[NodeName("Sequencer")]
public class SequencerNode : CompositeNode
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
                return State.Failure;
            case State.Success:
                m_Current++;
                break;
        }

        return m_Current == m_Children.Count ? State.Success : State.Running;
    }
}
