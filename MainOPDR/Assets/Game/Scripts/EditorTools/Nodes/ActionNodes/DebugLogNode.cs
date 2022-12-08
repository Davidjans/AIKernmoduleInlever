using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[NodeName("DebugLog")]
public class DebugLogNode : ActionNode
{
    public string m_Message;
    protected override void OnStart()
    {
        //Debug.Log($"OnStart{m_Message }");
    }

    public override void OnStop()
    {
       // Debug.Log($"OnStop{m_Message }");
    }

    protected override State OnUpdate()
    { 
        Debug.Log($"{m_Message }");
        
        //blackboard test
        //Debug.Log($"Blackboard:{m_BlackBoard.moveToPosition}");
        //m_BlackBoard.moveToPosition.x = 5;
        
        return State.Success;
    }
}
