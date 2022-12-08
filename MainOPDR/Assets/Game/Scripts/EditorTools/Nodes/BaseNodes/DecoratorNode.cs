using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DecoratorNode : Node
{
    [HideInInspector]
    public Node m_Child;
    
    public override Node Clone()
    {
        DecoratorNode node = Instantiate(this);
        node.m_Child = m_Child.Clone();
        return node;
    }
}
