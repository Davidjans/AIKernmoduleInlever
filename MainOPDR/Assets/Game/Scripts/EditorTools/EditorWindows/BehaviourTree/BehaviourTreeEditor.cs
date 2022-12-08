using System;
using System.Net;
using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities.Editor;
using Unity.VisualScripting;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;
using UnityEngine.UIElements;

public class BehaviourTreeEditor : OdinEditorWindow
{
    public BehaviourTreeView m_Treeview;
    private InspectorView m_InspectorView;
    private IMGUIContainer m_BlackBoardView;
    private Label m_NodeNameLabel;
    private Label m_DescriptionLabel;
    
    private SerializedObject m_TreeObject;
    private SerializedProperty m_BlackBoardProperty;
    [MenuItem("DavidTools/BehaviourTree")]
    public static void OpenWindow()
    {
        BehaviourTreeEditor wnd = GetWindow<BehaviourTreeEditor>();
        wnd.titleContent = new GUIContent("BehaviourTreeEditor");
    }

    [OnOpenAsset]
    public static bool OnOpenAsset(int instanceID, int line)
    {
        if (Selection.activeObject is BehaviourTree)
        {
            OpenWindow();
            return true;
        }

        return false;
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;


        // Instantiate UXML
        var visualTree =
            AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                "Assets/Game/Scripts/EditorTools/EditorWindows/BehaviourTree/BehaviourTreeEditor.uxml");
        visualTree.CloneTree(root);

        var styleSheet =
            AssetDatabase.LoadAssetAtPath<StyleSheet>(
                "Assets/Game/Scripts/EditorTools/EditorWindows/BehaviourTree/BehaviourTreeEditor.uss");
        root.styleSheets.Add(styleSheet);

        m_Treeview = root.Q<BehaviourTreeView>();
        m_InspectorView = root.Q<InspectorView>();
        m_BlackBoardView = root.Q<IMGUIContainer>();
        m_DescriptionLabel = root.Q<Label>("TaskDescription");
        m_NodeNameLabel = root.Q<Label>("NodeName");
        m_BlackBoardView.onGUIHandler = () =>
        {
            if (m_TreeObject == null)
                return;
            m_TreeObject.Update();
            GUILayoutOption[] EmptyLayoutOption = new GUILayoutOption[0];
            EditorGUILayout.PropertyField(m_BlackBoardProperty);
            m_TreeObject.ApplyModifiedProperties();
        };
        m_Treeview.m_OnNodeSelected = OnNodeSelectionChanged;

        OnSelectionChange();
    }

    private new void OnEnable()
    {
        EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private new void OnDisable()
    {
        EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
    }
    
    private void OnPlayModeStateChanged(PlayModeStateChange obj)
    {
        switch (obj)
        {
            case PlayModeStateChange.EnteredEditMode:
                OnSelectionChange();
                break;
            case PlayModeStateChange.ExitingEditMode:
                break;
            case PlayModeStateChange.EnteredPlayMode:
                OnSelectionChange();
                break;
            case PlayModeStateChange.ExitingPlayMode:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(obj), obj, null);
        }
    }

    private void OnSelectionChange()
    {
        BehaviourTree tree = Selection.activeObject as BehaviourTree;
        if (!tree && Selection.activeGameObject)
        {
            BehaviourTreeRunner runner = Selection.activeGameObject.GetComponent<BehaviourTreeRunner>();
            if (runner)
            {
                tree = runner.m_Tree;
            }
        }
        
        // don't check id if it can run during runtime because it is a clone.
        if (tree && (Application.isPlaying || AssetDatabase.CanOpenAssetInEditor(tree.GetInstanceID())))
        {
                m_Treeview.PopulateView(tree);
        }

        if (tree != null)
        {
            m_TreeObject = new SerializedObject(tree);
            m_BlackBoardProperty = m_TreeObject.FindProperty("m_BlackBoard");
        }
        
    }

    void OnNodeSelectionChanged(NodeView node)
    {
        m_InspectorView.UpdateSelection(node);
        DrawSelectedTaskDescription(node);
    }

    private void OnInspectorUpdate()
    {
        m_Treeview?.UpdateNodeState();
    }
    private void DrawSelectedTaskDescription(NodeView node)
    {
        TaskDescriptionAttribute[] customAttributes;
        customAttributes = node.m_Node.GetType().GetCustomAttributes(typeof (TaskDescriptionAttribute), false) as TaskDescriptionAttribute[];
        m_NodeNameLabel.text = node.m_Node.name;
        if (customAttributes.Length > 0)
        {
            m_DescriptionLabel.text = customAttributes[0].Description;
        }
        else
        {
            m_DescriptionLabel.text = "No description given";
        }
    }

}