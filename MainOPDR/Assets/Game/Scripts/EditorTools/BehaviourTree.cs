using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu()]
public class BehaviourTree : SerializedScriptableObject
{
    public Node m_RootNode;
    public Node.State m_TreeState = Node.State.Running;
    public List<Node> m_Nodes = new List<Node>();
    public BlackBoard m_BlackBoard = new BlackBoard();
    [HideInInspector] public List<Node> m_NodesRunning = new List<Node>();
    /*[HideInInspector] */public BehaviourTreeRunner m_ParentRunner;
    private Node m_CurrentDisplayingNode;
    public Node.State Update()
    {
        if (m_RootNode.m_State == Node.State.Running)
        {
            m_TreeState = m_RootNode.Update();
        }

        return m_TreeState;
    }

    public Node CreateNode(System.Type type,Vector2 nodePos)
    {
        Node node = ScriptableObject.CreateInstance(type) as Node;
        node.name = type.Name;
        node.m_Guid = GUID.Generate().ToString();
        node.m_Position = nodePos;
        
        Undo.RecordObject(this, "Behaviour Tree (CreateNode)");
        m_Nodes.Add(node);

        if (!Application.isPlaying)
        {
            node.m_ParentTree = this;
            AssetDatabase.AddObjectToAsset(node, this);
        }

        Undo.RegisterCreatedObjectUndo(node, "Behaviour Tree (CreateNode)");
        AssetDatabase.SaveAssets();
        return node;
    }

    public void DeleteNode(Node node)
    {
        Undo.RecordObject(this, "Behaviour Tree (CreateNode)");
        m_Nodes.Remove(node);

        //AssetDatabase.RemoveObjectFromAsset(node);
        Undo.DestroyObjectImmediate(node);
        AssetDatabase.SaveAssets();
    }

    public void AddChild(Node parent, Node child)
    {
        child.m_Parent = parent;

        DecoratorNode decorator = parent as DecoratorNode;
        if (decorator)
        {
            Undo.RecordObject(decorator, "Behaviour Tree (AddChild)");
            decorator.m_Child = child;
            EditorUtility.SetDirty(decorator);
            return;
        }

        CompositeNode composite = parent as CompositeNode;
        if (composite)
        {
            Undo.RecordObject(composite, "Behaviour Tree (AddChild)");
            composite.m_Children.Add(child);
            EditorUtility.SetDirty(composite);
            return;
        }

        RootNode root = parent as RootNode;
        if (root)
        {
            Undo.RecordObject(root, "Behaviour Tree (AddChild)");
            root.m_Child = child;
            EditorUtility.SetDirty(root);
            return;
        }
    }

    public void RemoveChild(Node parent, Node child)
    {
        DecoratorNode decorator = parent as DecoratorNode;
        if (decorator)
        {
            Undo.RecordObject(decorator, "Behaviour Tree (RemoveChild)");
            decorator.m_Child = null;
            EditorUtility.SetDirty(decorator);
        }

        CompositeNode composite = parent as CompositeNode;
        if (composite)
        {
            Undo.RecordObject(composite, "Behaviour Tree (RemoveChild)");
            composite.m_Children.Remove(child);
            EditorUtility.SetDirty(composite);
        }

        RootNode root = parent as RootNode;
        if (root)
        {
            Undo.RecordObject(root, "Behaviour Tree (RemoveChild)");
            root.m_Child = null;
            EditorUtility.SetDirty(root);
            return;
        }
    }

    public List<Node> GetChildren(Node parent)
    {
        List<Node> children = new List<Node>();

        DecoratorNode decorator = parent as DecoratorNode;
        if (decorator && decorator.m_Child != null)
        {
            children.Add(decorator.m_Child);
            return children;
        }

        CompositeNode composite = parent as CompositeNode;
        if (composite)
        {
            return composite.m_Children;
        }

        RootNode root = parent as RootNode;
        if (root && root.m_Child != null)
        {
            children.Add(root.m_Child);
            return children;
        }

        return children;
    }


    public void Traverse(Node node, System.Action<Node> visiter)
    {
        if (node)
        {
            visiter.Invoke(node);
            var children = GetChildren(node);
            children.ForEach((n) => Traverse(n, visiter));
        }
    }

    public BehaviourTree Clone()
    {
        BehaviourTree tree = Instantiate(this);
        tree.m_RootNode = tree.m_RootNode.Clone();
        tree.m_Nodes = new List<Node>();
        Traverse(tree.m_RootNode, (n) => { tree.m_Nodes.Add(n); });
        return tree;
    }

    public void Bind()
    {
        Traverse(m_RootNode, node  =>
        {
            node.m_BlackBoard = m_BlackBoard;
            node.m_ParentTree = this;
        });
    }

    

    public void DisplayNodeState(Node node)
    {
        StateDisplayAttribute[] customAttributes;
        customAttributes = node.GetType().GetCustomAttributes(typeof (StateDisplayAttribute), false) as StateDisplayAttribute[];
            
        if (customAttributes.Length > 0)
        {
            if (m_CurrentDisplayingNode == null)
            {
                m_CurrentDisplayingNode = node;
                m_ParentRunner.DisplayState(customAttributes[0].mState);
            }
            else
            {
                StateDisplayAttribute[] customAttributesCurrent;
                customAttributesCurrent = m_CurrentDisplayingNode.GetType().GetCustomAttributes(typeof (StateDisplayAttribute), false) as StateDisplayAttribute[];
                if (customAttributesCurrent[0].mPriority < customAttributes[0].mPriority)
                {
                    m_CurrentDisplayingNode = node;
                    m_ParentRunner.DisplayState(customAttributes[0].mState);
                }
            }
        }
    }
    public void RemoveStateDisplay()
    {
        m_CurrentDisplayingNode = null;
        m_ParentRunner.m_CurrentStateDisplayer.text = "Idle";
    }


    #region Retroactive fixing functionality that would break already made behaviourtrees if done otherwise so delete this when done.

    [Button]
    public void FixNodeNamesAccordingToAttribute()
    {
        
        foreach (var node in m_Nodes)
        {
            NodeNameAttribute[] customAttributes;
            customAttributes = node.GetType().GetCustomAttributes(typeof (NodeNameAttribute), false) as NodeNameAttribute[];
            
            if (customAttributes.Length > 0)
            {
                node.name = customAttributes[0].mName;
            }
            else
            {
                node.name = node.GetType().ToString();
            }
        }
    }

    #endregion
}