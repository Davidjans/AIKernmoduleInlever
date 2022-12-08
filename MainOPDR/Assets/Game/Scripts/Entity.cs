using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks.Triggers;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public Transform m_WeaponHolder;
    public Weapon m_HeldWeapon;
    protected bool m_GettingAttacked;
    public virtual void GiveWeapon(GameObject weaponPrefab)
    {
        GameObject spawnedWeapon = Instantiate(weaponPrefab, m_WeaponHolder);
        spawnedWeapon.transform.localPosition = Vector3.zero;
        spawnedWeapon.transform.localEulerAngles = Vector3.zero;
        m_HeldWeapon = spawnedWeapon.GetComponent<Weapon>();
    }

    public virtual void Attack()
    {
        m_HeldWeapon.Fire();
    }

    public virtual void GettingAttacked()
    {
        m_GettingAttacked = true;
    }

    public virtual void NoLongerGettingAttacked()
    {
        m_GettingAttacked = false;
    }
    
}
