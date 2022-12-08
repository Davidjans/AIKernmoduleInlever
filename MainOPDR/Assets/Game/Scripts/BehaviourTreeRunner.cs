using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class BehaviourTreeRunner : MonoBehaviour
{
    public BehaviourTree m_Tree;

    public TextMeshPro m_CurrentStateDisplayer;
    // Start is called before the first frame update
    void Awake()
    {
        m_Tree = m_Tree.Clone();
        m_Tree.Bind();
        m_Tree.m_RootNode.m_State = Node.State.Running;
        m_Tree.m_ParentRunner = this;
    }

    // Update is called once per frame
    void Update()
    {
        m_Tree.Update();
    }

    public void DisplayState(string text)
    {
        m_CurrentStateDisplayer.text = text;
    }
}