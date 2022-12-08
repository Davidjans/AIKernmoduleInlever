using System;
using System.Collections;
using System.Collections.Generic;
using BehaviorDesigner.Runtime.Tasks;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

public class NodeView : UnityEditor.Experimental.GraphView.Node
{
    public Action<NodeView> m_OnNodeSelected;
    public Node m_Node;
    public Port m_InputPort;
    public Port m_OutputPort;
    public NodeView(Node node) : base("Assets/Game/Scripts/EditorTools/EditorWindows/BehaviourTree/NodeView.uxml")
    {
        this.m_Node = node;
        this.title = node.name;
        this.viewDataKey = node.m_Guid;
        
        style.left = node.m_Position.x;
        style.top = node.m_Position.y;

        CreateInputPorts();
        CreateOutputPorts();
        SetupClasses();
    }

    private void SetupClasses()
    {
        switch (m_Node)
        {
            case ActionNode actionNode:
                AddToClassList("action");
                break;
            case CompositeNode compositeNode:
                AddToClassList("composite");
                break;
            case ConditionalNode conditionalNode:
                AddToClassList("conditional");
                break;
            case DecoratorNode decoratorNode:
                AddToClassList("decorator");
                break;
            case RootNode rootNode:
                AddToClassList("root");
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(m_Node));
        }
    }

    private void CreateInputPorts()
    {
        if (m_Node is ActionNode)
        {
            m_InputPort = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
        }
        else if (m_Node is CompositeNode)
        {
            m_InputPort = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
        }
        else if (m_Node is DecoratorNode)
        {
            m_InputPort = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
        }
        else if (m_Node is ConditionalNode)
        {
            m_InputPort = InstantiatePort(Orientation.Vertical, Direction.Input, Port.Capacity.Single, typeof(bool));
        }
        else if (m_Node is RootNode)
        {
        }
        
        if(m_InputPort != null){
            m_InputPort.portName = "";
            m_InputPort.style.flexDirection = FlexDirection.Column;
            inputContainer.Add(m_InputPort);
        }
    }

    private void CreateOutputPorts()
    {
        if (m_Node is ActionNode)
        {
            
        }
        else if (m_Node is ConditionalNode)
        {
            
        }
        else if (m_Node is CompositeNode)
        {
            m_OutputPort = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Multi, typeof(bool));
        }
        else if (m_Node is DecoratorNode)
        {
            m_OutputPort = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(bool));
        }
        else if (m_Node is RootNode)
        {
            m_OutputPort = InstantiatePort(Orientation.Vertical, Direction.Output, Port.Capacity.Single, typeof(bool));
        }
        
        if(m_OutputPort != null){
            m_OutputPort.portName = "";
            m_OutputPort.style.flexDirection = FlexDirection.ColumnReverse;
            outputContainer.Add(m_OutputPort);
        }
    }
    
    public override void SetPosition(Rect newPos)
    {
        base.SetPosition(newPos);
        Undo.RecordObject(m_Node,"Behaviour Tree(Set  Position)");
        m_Node.m_Position.x = newPos.xMin;
        m_Node.m_Position.y = newPos.yMin;
        EditorUtility.SetDirty(m_Node);
    }

    public void SortChildren()
    {
        if (m_Node is CompositeNode)
        {
            CompositeNode node = m_Node as  CompositeNode;
            node.m_Children.Sort(SortByHorizontalPosition);
            
            // this was the old method i did the sorting by x pos.
            /*List<Node> children = node.m_Children;
            List<Node> newOrder = new List<Node>();
            foreach (var child in children)
            {
                if (newOrder.Count == 0)
                {
                    newOrder.Add(child);
                    continue;
                }

                for (int i = 0; i < newOrder.Count; i++)
                {
                    if (child.m_Position.x < newOrder[i].m_Position.x)
                    {
                        newOrder.Insert(i,child);
                        break;
                    }
                }

                if (!newOrder.Contains(child))
                {
                    newOrder.Add(child);
                    continue;
                }
            }

            node.m_Children = newOrder;*/
        }
    }

    private int SortByHorizontalPosition(Node left, Node right)
    {
        return left.m_Position.x < right.m_Position.x ? -1 : 1;
    }

    public override void OnSelected()
    {
        base.OnSelected();
        if (m_OnNodeSelected != null)
        {
            m_OnNodeSelected.Invoke(this);
        }
    }

    public void UpdateState()
    {
        RemoveFromClassList("running");
        RemoveFromClassList("failure");
        RemoveFromClassList("success");
        RemoveFromClassList("nothing");
        if (Application.isPlaying)
        {
            switch (m_Node.m_State)
            {
                case Node.State.Running:
                    AddToClassList("running");
                    break;
                case Node.State.Failure:
                    AddToClassList("failure");
                    break;
                case Node.State.Success:
                    AddToClassList("success");
                    break;
                case Node.State.Nothing:
                    AddToClassList("nothing");
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
