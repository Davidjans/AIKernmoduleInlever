using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using Sirenix.OdinInspector;
using StackedBeans.Utils;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class BoidManager : MonoBehaviour
{
#if ODIN_INSPECTOR
    [FoldoutGroup("BOISpawnSettings")]
#endif
    [SerializeField]
    private int m_HowManyOfTheLittleBoisDoYouWant;
#if ODIN_INSPECTOR
    [FoldoutGroup("BOISpawnSettings")]
#endif
    [SerializeField]
    private GameObject m_BoidPrefab;
    
#if ODIN_INSPECTOR
    [FoldoutGroup("BOISpawnSettings")]
#endif
    [SerializeField]
    private Transform m_GoalTransform;
    
#if ODIN_INSPECTOR
    [HideInEditorMode]
#endif
    [SerializeField]
    private List<BOI> m_BoidList;

#if ODIN_INSPECTOR
    [FoldoutGroup("BOIMovementSettings")]
#endif
    [SerializeField]
    private float m_MassPercentagePerFrame = 0.005f;
#if ODIN_INSPECTOR
    [FoldoutGroup("BOIMovementSettings")]
#endif
    [SerializeField]
    private float m_DistanceToKeep = 0.25f;

#if ODIN_INSPECTOR
    [FoldoutGroup("BOIMovementSettings")]
#endif
    [SerializeField]
    private float m_SharedVelocityDivisionFactor = 8;
#if ODIN_INSPECTOR
    [FoldoutGroup("BOIMovementSettings")]
#endif
    [SerializeField]
    private float m_GoalPercentagePerFrame = 0.01f;

#if ODIN_INSPECTOR
    [FoldoutGroup("SphereSpawningValues")]
#endif
    public int m_SphereRadius;
#if ODIN_INSPECTOR
    [FoldoutGroup("SphereSpawningValues")]
#endif
    [SerializeField]
    private Vector3 m_StartCenterPosition;

    private Vector3 totalCenterOfMass = Vector3.zero;
    
    private void Start()
    {
        for (int i = m_HowManyOfTheLittleBoisDoYouWant - 1; i >= 0; i--)
        {
            Vector3 sphereLocation = Random.insideUnitSphere;
            sphereLocation.x *= Random.Range(0, m_SphereRadius);
            sphereLocation.y *= Random.Range(0, m_SphereRadius);
            sphereLocation.z *= Random.Range(0, m_SphereRadius);
            Vector3 spawnPos = sphereLocation + m_StartCenterPosition;

            GameObject spawnedBoid = Instantiate(m_BoidPrefab, spawnPos, Quaternion.identity, transform);
            spawnedBoid.GetComponent<MeshRenderer>().material.color = StackedBeansUtils.RandomColor();
            m_BoidList.Add(spawnedBoid.GetComponent<BOI>());
        }
    }

    private void Update()
    {
        SetTotalCenterOfMass();


        foreach (var boi in m_BoidList)
        {
            Vector3 centerMassResult = CenterMassRule(boi);
            Vector3 distancingResult = DistancingRule(boi);
            Vector3 sharedVelocityResult = SharedVelocityRule(boi);
            Vector3 goalResult = GoalRule(boi);
            //Vector3 rule3Result = Vector3.zero;

            boi.ChangeVelocity(boi.m_Velocity + centerMassResult + distancingResult + sharedVelocityResult + goalResult);
            boi.transform.position += boi.m_Velocity;
        }
    }

    private void SetTotalCenterOfMass()
    {
        totalCenterOfMass = Vector3.zero;
        foreach (var boi in m_BoidList)
        {
            totalCenterOfMass += boi.transform.position;
        }
    }

    private Vector3 CenterMassRule(BOI boi)
    {
        boi.m_PersonalizedCenterOfMass = ((totalCenterOfMass - boi.transform.position) / (m_BoidList.Count - 1));
        return (boi.m_PersonalizedCenterOfMass - boi.transform.position) * m_MassPercentagePerFrame;
    }

    private Vector3 DistancingRule(BOI boi)
    {
        Vector3 result = Vector3.zero;
        foreach (var currentBoi in m_BoidList)
        {
            if (currentBoi != boi && Vector3.Distance(boi.transform.position, currentBoi.transform.position) <
                m_DistanceToKeep)
            {
                result -= (currentBoi.transform.position - boi.transform.position);
            }
        }

        return result;
    }

    private Vector3 SharedVelocityRule(BOI boi)
    {
        Vector3 result = Vector3.zero;
        foreach (var currentBoi in m_BoidList)
        {
            if (currentBoi != boi)
            {
                result += currentBoi.m_Velocity;
            }
        }

        result /= (m_BoidList.Count - 1);
        return (result - boi.m_Velocity) / m_SharedVelocityDivisionFactor;
    }

    private Vector3 GoalRule(BOI boi)
    {
        return (m_GoalTransform.position - boi.transform.position) * m_GoalPercentagePerFrame;
    }
}