using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
public class InspectorView : VisualElement
{
    public new class UxmlFactory : UxmlFactory<InspectorView, VisualElement.UxmlTraits> {
    }

    private Editor m_Editor;
    public InspectorView()
    {
        
    }

    public void UpdateSelection(NodeView node)
    {
        Clear();
        UnityEngine.Object.DestroyImmediate(m_Editor);
        m_Editor = Editor.CreateEditor(node.m_Node);
        IMGUIContainer container = new IMGUIContainer(() =>
        {
            if(m_Editor.target)
                m_Editor.OnInspectorGUI();
        });
        Add(container);
    }
}
