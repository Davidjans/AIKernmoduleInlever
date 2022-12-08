using System.Collections;
using System.Collections.Generic;
using Sirenix.Serialization;
using UnityEngine;
[NodeName("SetVariable")]
public class SetVariableNode : ActionNode
{
    public string m_VariableName;
    /*[OdinSerialize] */[SerializeReference]
    public object m_VariableValue;
    
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
        m_BlackBoard.Set(m_VariableName,m_VariableValue);
        
        return State.Success;
    }
}
