
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[TaskDescription("Paralel Sequencer")]
[NodeName("Parallel sequencer")]
public class ParalelSequencerNode : CompositeNode
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
                    m_State = State.Failure;
                    break;
            }

            if (m_State == State.Failure)
                break;
        }

        if (m_State == State.Failure)
        {
            foreach (var child in m_Children)
            {
                if (child.m_State == State.Failure)
                    continue;
                else
                {
                    child.m_State = State.Nothing;
                }
            }

            return State.Failure;
        }

        bool onlySucces = true;
        foreach (var child in m_Children)
        {
            if (child.m_State is State.Running or State.Failure)
            {
                onlySucces = false;
            }
        }

        if (onlySucces)
        {
            return State.Success;
        }

        return State.Running;
    }
}
