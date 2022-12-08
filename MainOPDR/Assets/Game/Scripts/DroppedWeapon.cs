using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedWeapon : MonoBehaviour
{
    public string m_HasWeaponName = "HasWeapon";
    public GameObject m_DroppedWeaponPrefab;
    private void OnTriggerEnter(Collider other)
    {
        Guard guard = other.GetComponent<Guard>();
        if (guard)
        {
            guard.m_Runner.m_Tree.m_BlackBoard.Set("HasWeapon",true);
            guard.GiveWeapon(m_DroppedWeaponPrefab);
            Destroy(gameObject);
        }
    }
}
