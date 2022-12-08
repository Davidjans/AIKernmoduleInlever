
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompareFieldValueNode : ConditionalNode
{
    new public static string m_AdditionalCategory = "Reflection/";

    protected override void OnStart() 
    {
    }

    public override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (true)
        {
            Debug.LogError("Add actual condition checking here.");
            return State.Success;
        }
        /*else
        {
            return State.Failure;
        }
        return State.Running;*/
    }
}
