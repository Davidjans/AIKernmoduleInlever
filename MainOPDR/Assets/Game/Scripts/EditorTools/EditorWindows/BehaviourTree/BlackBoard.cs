using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using Sirenix.Serialization;
using UnityEngine;

[System.Serializable]
[ShowOdinSerializedPropertiesInInspector]
public class BlackBoard
{   
    [SerializeReference] [OdinSerialize] [ShowInInspector]
    public Dictionary<string, object> m_BlackBoard = new Dictionary<string,object>();

    public void Set<T>(string key, T value)
    {
        if (m_BlackBoard.ContainsKey(key))
        {
            m_BlackBoard[key] = value as object;
        }
        else
        {
            m_BlackBoard.Add(key,value as object);
        }
    }

    public T Get<T>(string key)
    {
        if (m_BlackBoard.ContainsKey(key))
        {
            T value = (T)m_BlackBoard[key];
            return value;
        }
        else
        {
            m_BlackBoard.Add(key,default(T));
            Debug.LogError(key + ": Default has been created");
            return default(T);
        }
    }
    /*public GameObject m_AgentObject;
    public Transform m_WayPointParent;
    public bool m_SeesPlayer;
    public bool m_HasWeapon;
    public GameObject m_Player;*/
}
