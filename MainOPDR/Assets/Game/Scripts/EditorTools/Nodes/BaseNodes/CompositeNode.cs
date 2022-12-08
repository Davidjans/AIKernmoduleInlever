using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class CompositeNode : Node
{
    public List<Node> m_Children = new List<Node>();
    
    public override Node Clone()
    {
        CompositeNode node = Instantiate(this);
        node.m_Children = m_Children.ConvertAll(c => c.Clone());
        return node;
    }
}
