using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] protected Transform m_FirePoint;
    public float m_Cooldown = 3;
    public GameObject m_ProjectilePrefab;
    public float m_ProjectileForce = 100;
    protected bool m_CanFire = true;
    [HideInInspector] public bool m_ViewBlocked;
    public virtual void Fire()
    {
        if (m_CanFire)
        {
            Cooldown();
            if(!m_ViewBlocked)
                ShootProjectile();
        }
    }

    public async void Cooldown()
    {
        m_CanFire = false;
        await Task.Delay((int)(m_Cooldown * 1000));
        m_CanFire = true;
    }
    public virtual void ShootProjectile()
    {
        GameObject projectile = Instantiate(m_ProjectilePrefab, m_FirePoint);
        projectile.transform.localPosition = Vector3.zero;
        projectile.transform.localEulerAngles = Vector3.zero;
        projectile.transform.parent = null;
        projectile.GetComponent<Rigidbody>().AddForce(projectile.transform.forward * m_ProjectileForce);
        Destroy(projectile,10);
    }
}
