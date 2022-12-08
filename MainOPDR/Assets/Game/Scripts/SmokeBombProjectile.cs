using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class SmokeBombProjectile : MonoBehaviour
{
    // yoinked code and modified it from
    //https://community.gamedev.tv/t/projectile-parabola-motion-and-rotation-arrows-fly-on-an-arc/180130
	[SerializeField] private float speed = 0.4f;
    //private Health target = null;
    [HideInInspector] public Vector3 m_Target;
    private float damage = 0f;
    private Vector3 initialPosition;
    private Quaternion initialRotation;
    private float launchAngle = 20f;
    private Rigidbody rigidbody;
    public GameObject m_SmokePrefab;
    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        initialPosition = transform.position;
        initialRotation = transform.rotation;
    }

    void Update()
    {
        //if(target==null) return;
        transform.LookAt(GetAimLocation());
        //transform.Translate(Vector3.forward*speed*Time.deltaTime);
        Launch();
    }

    /*public void SetTarget(Health target,float damage)
    {
        this.target = target;
        this.damage = damage;
    }*/

    private Vector3 GetAimLocation()
    {

        Vector3 aimLocation = m_Target; /*new Vector3(target.GetComponent<Collider>().bounds.center.x, target.GetComponent<Collider>().bounds.center.y,
            m_Target);*/

        return aimLocation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ally") || other.CompareTag("Projectile") || other.CompareTag("Player") || other.CompareTag("Wall")) return;
        /*if (other.GetComponent<Health>()!=target) return;
        target.TakeDamage(damage);*/
        Instantiate(m_SmokePrefab, transform.position, quaternion.identity);
        Destroy(gameObject);
    }

    private void Launch()
    {
        Vector3 projectileXZPos = new Vector3(transform.position.x, 0.0f, transform.position.z);
        float R = Vector3.Distance(projectileXZPos, GetAimLocation());
        float G = Physics.gravity.y;
        float tanAlpha = Mathf.Tan(launchAngle * Mathf.Deg2Rad);
        float H = m_Target.y - transform.position.y;

        float Vz = Mathf.Sqrt(G * R * R / (speed * (H - R * tanAlpha)));
        float Vy = tanAlpha * Vz;

        Vector3 localVelocity = new Vector3(0f, Vy, Vz);
        Vector3 globalVelocity = transform.TransformDirection(localVelocity);

        //This makes the projectile go forward
        rigidbody.velocity = globalVelocity;
        //This rotates the projectile correctly on the arc
        transform.rotation = Quaternion.LookRotation(rigidbody.velocity) * initialRotation;
        
    }
	
	
	
	
}
