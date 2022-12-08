using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[NodeName("WithinDistance")]
public class WithinDistanceNode : ConditionalNode
{
    public string m_OriginBlackBoardGameObjectVariable;
    public string m_TargetBlackBoardGameObjectVariable;
    public float m_WithinDistance = 3;
    protected override void OnStart() 
    {
    }

    public override void OnStop()
    {
    }

    protected override State OnUpdate()
    {
        GameObject originPos = m_BlackBoard.Get<GameObject>(m_OriginBlackBoardGameObjectVariable);
        GameObject targetPos = m_BlackBoard.Get<GameObject>(m_TargetBlackBoardGameObjectVariable);
        if (Vector3.Distance(originPos.transform.position,targetPos.transform.position) < m_WithinDistance)
        {
            return State.Success;
        }
        else
        {
            return State.Failure;
        }
    }
}
