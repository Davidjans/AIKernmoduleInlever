using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[TaskDescription("Similar to the parallel selector task, except the parallel complete task will return the child status as soon as the child returns success or failure." + 
                 "The child tasks are executed simultaneously.")]
[NodeName("ParallelComplete")]
public class ParalelCompleteNode : CompositeNode
{
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
            switch (child.Update())
            {
                case State.Failure:
                    return State.Failure;
                case State.Success:
                    return State.Success;
            }
        }
        return State.Running;
    }
}
