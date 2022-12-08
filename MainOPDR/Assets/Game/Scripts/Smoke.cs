using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Smoke : MonoBehaviour
{
    public float m_MaxScale = 3;

    public float m_StartScale = 0.2f;

    public float m_ScaleIncrement;

    private List<Weapon> m_WeaponsTurnedOff = new List<Weapon>();
    // Start is called before the first frame update
    void Start()
    {
        Vector3 scale = new Vector3(m_StartScale,m_StartScale,m_StartScale);
        transform.localScale = scale;
        Destroy(gameObject,10);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localScale.x < m_MaxScale)
        {
            Vector3 scale = transform.localScale;
            scale.x += m_ScaleIncrement * Time.deltaTime;
            scale.y += m_ScaleIncrement * Time.deltaTime;
            scale.z += m_ScaleIncrement * Time.deltaTime;
            transform.localScale = scale;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Weapon weapon = other.GetComponentInChildren<Weapon>();
        if (weapon != null)
        {
            weapon.m_ViewBlocked = true;
            m_WeaponsTurnedOff.Add(weapon);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Weapon weapon = other.GetComponentInChildren<Weapon>();
        if (weapon != null)
        {
            weapon.m_ViewBlocked = false;
            m_WeaponsTurnedOff.Remove(weapon);
        }
    }

    private void OnDestroy()
    {
        foreach (var weapon in m_WeaponsTurnedOff)
        {
            if (weapon == null)
                continue;
            weapon.m_ViewBlocked = false;
            m_WeaponsTurnedOff.Remove(weapon);
        }
    }
}
