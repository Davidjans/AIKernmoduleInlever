using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class SmokeBomb : Weapon
{
    [Button]
    public override void ShootProjectile()
    {
        GameObject projectile = Instantiate(m_ProjectilePrefab, m_FirePoint);
        projectile.transform.localPosition = Vector3.zero;
        projectile.transform.localEulerAngles = Vector3.zero;
        projectile.transform.parent = null;
        projectile.GetComponent<SmokeBombProjectile>().m_Target = GameObject.Find("Guard").transform.position;
        projectile.GetComponent<SmokeBombProjectile>().m_Target.y =- 0.814f;
    }
}
