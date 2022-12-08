
#if  UNITY_EDITOR
using System;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using Sirenix.Utilities.Editor;
using Sirenix.OdinInspector.Editor;
using UnityEditor;



public class StateMachineWindow : OdinEditorWindow
{
    public string Hello;

    public static StateMachineWindow Instance
    {
        get
        {
            m_ThisWindow = GetWindow<StateMachineWindow>();
            return m_ThisWindow;
        }
    }

    private static StateMachineWindow m_ThisWindow;

    private int m_LeftRectWidth = 100;
    
    
    //[MenuItem("DavidTools/StateMachineEditor")]
    private static void OpenWindow()
    {
        Instance.Show();
    }

    void OnGUI()
    {
        float height = Instance.position.height;
        Rect rectRight = EditorGUILayout.GetControlRect(true, height);
        rectRight = rectRight.AlignRight(Instance.position.width - m_LeftRectWidth);
        SirenixEditorGUI.DrawSolidRect(rectRight, new Color(0.5f, 0.5f, 0.5f, 1f));
        
        Rect rectLeft = EditorGUILayout.GetControlRect(true, height);
        rectLeft = rectLeft.AlignLeft(m_LeftRectWidth);
        SirenixEditorGUI.DrawSolidRect(rectLeft, new Color(0.5f, 0.8f, 0.8f, 1f));
        
        Event e = Event.current;
        GUILayout.Label("Mouse pos: " + e.mousePosition);

        /*
        // Toolbar
        {
            SirenixEditorGUI.DrawSolidRect(rect.AlignTop(20), new Color(0.5f, 0.5f, 0.5f, 1f));
            SirenixEditorGUI.DrawBorders(rect.AlignTop(20).SetHeight(21).SetWidth(rect.width + 1), 1);
        }*/
        GUIHelper.RequestRepaint();
    }
}
#endif