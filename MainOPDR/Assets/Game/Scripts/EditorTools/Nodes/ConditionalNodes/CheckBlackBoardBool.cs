using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[NodeName("CheckBool")]
public class CheckBlackBoardBool : ConditionalNode
{
    public string m_BlackBoardVariableName;
    public CheckIf m_Check;
    protected override void OnStart() 
    {
    }

    public override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        if (m_BlackBoard.Get<bool>(m_BlackBoardVariableName))
        {
            if(m_Check == CheckIf.True)
                return State.Success;
            else
                return State.Failure;
        }
        else
        {
            if(m_Check == CheckIf.Nottrue)
                return State.Success;
            else
                return State.Failure;
        }
    }
    
    public enum CheckIf
    {
        True,
        Nottrue
    }
}
