
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[TaskDescription("Similar to the selector task, the parallel selector task will return success as soon as a child task returns success. " +
                 "The difference is that the parallel task will run all of its children tasks simultaneously versus running each task one at a time. " +
                 "If one tasks returns success the parallel selector task will end all of the child tasks and return success. " +
                 "If every child task returns failure then the parallel selector task will return failure.")]
[NodeName("ParallelSelector")]
public class ParalelSelectorNode : CompositeNode
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
                case State.Success:
                    m_State = State.Success;
                    break;
            }

            if (m_State == State.Success)
                break;
        }

        if (m_State == State.Success)
        {
            foreach (var child in m_Children)
            {
                if (child.m_State == State.Success)
                    continue;
                else
                {
                    child.m_State = State.Nothing;
                }
            }

            return State.Success;
        }

        bool onlyFailure = true;
        foreach (var child in m_Children)
        {
            if (child.m_State is State.Running or State.Success)
            {
                onlyFailure = false;
            }
        }

        if (onlyFailure)
        {
            return State.Failure;
        }

        return State.Running;
    }
}
