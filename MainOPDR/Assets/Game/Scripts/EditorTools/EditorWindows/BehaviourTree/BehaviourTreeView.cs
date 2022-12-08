using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
public class BehaviourTreeView : GraphView
{
    public new class UxmlFactory : UxmlFactory<BehaviourTreeView, GraphView.UxmlTraits> {
    }
    public Action<NodeView> m_OnNodeSelected;
    [HideInInspector]
    public BehaviourTree m_Tree;
    public BehaviourTreeView()
    {
        Insert(0,new GridBackground());
        
        // this is the basic functionality from graphview things
        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());
        
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Game/Scripts/EditorTools/EditorWindows/BehaviourTree/BehaviourTreeEditor.uss");
        styleSheets.Add(styleSheet);

        Undo.undoRedoPerformed += OnUndoRedo;
    }

    private void OnUndoRedo()
    {
        PopulateView(m_Tree);
        AssetDatabase.SaveAssets();
    }

    NodeView FindNodeView(Node node)
    {
        return GetNodeByGuid(node.m_Guid) as NodeView;
    }
    
    public void PopulateView(BehaviourTree tree)
    {
        m_Tree = tree;
        graphViewChanged -= OnGraphViewChanged;
        DeleteElements(graphElements);
        graphViewChanged += OnGraphViewChanged;

        if (tree.m_RootNode == null)
        {
            tree.m_RootNode = tree.CreateNode(typeof(RootNode),Vector2.zero) as RootNode;
            EditorUtility.SetDirty(tree);
            AssetDatabase.SaveAssets();
        }
        
        // puts the nodes down
        tree.m_Nodes.ForEach(n=>  CreateNodeView(n));
        
        
        // creates the edges
        tree.m_Nodes.ForEach(n =>
        {
            var children = tree.GetChildren(n);
            children.ForEach(c =>
            {
                NodeView parentView = FindNodeView(n);
                NodeView childView = FindNodeView(c);

                Edge edge = parentView.m_OutputPort.ConnectTo(childView.m_InputPort);
                AddElement(edge);
            });
        });
        
    }

    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter)
    {
        return ports.ToList().Where(endPort => endPort.direction != startPort.direction && endPort.node != startPort.node).ToList();
    }

    private GraphViewChange OnGraphViewChanged(GraphViewChange graphviewchange)
    {
        if (graphviewchange.elementsToRemove != null)
        {
            graphviewchange.elementsToRemove.ForEach(elem =>
            {
                NodeView nodeview = elem as NodeView;
                if (nodeview != null)
                {
                    m_Tree.DeleteNode(nodeview.m_Node);
                }

                Edge edge = elem as Edge;
                if (edge != null)
                {
                    NodeView parentView = edge.output.node as NodeView;
                    NodeView childView = edge.input.node as NodeView;
                    m_Tree.RemoveChild(parentView.m_Node, childView.m_Node);
                }
            });
        }

        if (graphviewchange.edgesToCreate != null)
        {
            graphviewchange.edgesToCreate.ForEach(edge =>
            {
                NodeView parentView = edge.output.node as NodeView;
                NodeView childView = edge.input.node as NodeView;
                m_Tree.AddChild(parentView.m_Node, childView.m_Node);
            });
        }

        if (graphviewchange.movedElements != null)
        {
            nodes.ForEach((n) =>
            {
                NodeView view = n as NodeView;
                view.SortChildren();
            });
        }
        return graphviewchange;
    }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt)
    {
        
        Vector2 nodePosition = this.ChangeCoordinatesTo(contentViewContainer, evt.localMousePosition);
        string category = "";
        {
            var types = TypeCache.GetTypesDerivedFrom(typeof(ActionNode));
            foreach (var type in types)
            {
                category = (string) (type.GetField("m_AdditionalCategory")?.GetValue(null) ?? typeof(Node).GetField("m_AdditionalCategory").GetValue(null));
                evt.menu.AppendAction("Action/"+ $"{category}" + $"{type.Name}", (a) => CreateNode(type,nodePosition));
            }
        }
        {
            var types = TypeCache.GetTypesDerivedFrom(typeof(CompositeNode));
            foreach (var type in types)
            {
                category = (string) (type.GetField("m_AdditionalCategory")?.GetValue(null) ?? typeof(Node).GetField("m_AdditionalCategory").GetValue(null));
                evt.menu.AppendAction("Composite/"+ $"{category}" +$"{type.Name}", (a) => CreateNode(type,nodePosition));
            }
        }
        {
            var types = TypeCache.GetTypesDerivedFrom(typeof(DecoratorNode));
            foreach (var type in types)
            {
                category = (string) (type.GetField("m_AdditionalCategory")?.GetValue(null) ?? typeof(Node).GetField("m_AdditionalCategory").GetValue(null));
                evt.menu.AppendAction("Decorator/"+ $"{category}" +$"{type.Name}", (a) => CreateNode(type,nodePosition));
            }
        }
        {
            var types = TypeCache.GetTypesDerivedFrom(typeof(ConditionalNode));
            foreach (var type in types)
            {
                category = (string) (type.GetField("m_AdditionalCategory")?.GetValue(null) ?? typeof(Node).GetField("m_AdditionalCategory").GetValue(null));
                evt.menu.AppendAction("Conditional/"+ $"{category}" +$"{type.Name}", (a) => CreateNode(type,nodePosition));
            }
        }
    }

    void CreateNode(System.Type type, Vector2 nodePos)
    {
        Node node = m_Tree.CreateNode(type,nodePos);
        CreateNodeView(node);
    }
    
    void CreateNodeView(Node node)
    {
        NodeView nodeView = new NodeView(node);
        nodeView.m_OnNodeSelected = m_OnNodeSelected;
        AddElement(nodeView);
    }

    public void UpdateNodeState()
    {
        nodes.ForEach(n =>
        {
            NodeView view = n as NodeView;
            view.UpdateState();
        });
    }
    
}
