using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StackedBeans.Utils;
using UnityEngine;
using UnityEngine.AI;
[NodeName("MoveToCover")]
[StateDisplay("GettingCover",30)]
public class MoveToCover : ActionNode
{
    [HideInInspector] new public static string m_AdditionalCategory = "AI/Targetting/";

    public string m_EnemyName;
    public string m_AgentName = "Agent";
    public float m_RecheckClosestPointTime;
    [SerializeField] private LayerMask LayersToCheckIfHit;
    private GameObject m_Agent;
    private GameObject m_enemyGameObject;
    private List<Transform> m_CoverPoints = new List<Transform>();
    private NavMeshAgent m_NavAgent;
    private Transform m_ClosestCover;
    protected override void OnStart()
    {
        m_Agent = m_BlackBoard.Get<GameObject>(m_AgentName);
        m_NavAgent = m_Agent.GetComponent<NavMeshAgent>();
        m_enemyGameObject = m_BlackBoard.Get<GameObject>(m_EnemyName);
        GetClosestValidCover();
        
        m_NavAgent.SetDestination(m_ClosestCover.position);
    }

    public override void OnStop()
    {
        // Debug.Log($"OnStop{m_Message }");
    }

    protected override State OnUpdate()
    {
        if (m_ClosestCover == null)
        {
            Debug.LogError("Returned because no valid cover found");
            m_BlackBoard.Set("InCover", false);
            return State.Failure;
        }
            
            
        if (!m_NavAgent.pathPending && m_NavAgent.remainingDistance <= m_NavAgent.stoppingDistance 
                                    && (!m_NavAgent.hasPath || m_NavAgent.velocity.sqrMagnitude == 0f))
        {
            m_BlackBoard.Set("InCover", true);
            return State.Success;
        }
        else
        {
            m_BlackBoard.Set("InCover", false);
        }
        
        return State.Running;
    }

    private async void GetClosestValidCover()
    {
        m_CoverPoints.Clear();
        CoverPoint[] coverPoints = FindObjectsOfType<CoverPoint>();

        foreach (var cover in coverPoints)
        {
            m_CoverPoints.Add(cover.transform);
        }

        m_ClosestCover = null;
        while (m_ClosestCover == null && m_CoverPoints.Count > 0)
        {
            Transform closestCoverTransform = m_CoverPoints.GetClosestTransform(m_Agent.transform);
            RaycastHit hit;
            Vector3 direction = closestCoverTransform.position -  m_enemyGameObject.transform.position;
            float distance = Vector3.Distance(m_enemyGameObject.transform.position , closestCoverTransform.position);
            if (Physics.Raycast(m_enemyGameObject.transform.position, direction, out hit,distance,LayersToCheckIfHit))
            {
                //Debug.DrawRay(m_enemyGameObject.transform.position,direction,Color.green,5);
                m_ClosestCover = closestCoverTransform;
            }
            else
            {
                //Debug.DrawRay(m_enemyGameObject.transform.position,direction,Color.red,5);
                m_CoverPoints.Remove(closestCoverTransform);
            }
        }
        if(m_ClosestCover == null)
            Debug.LogError("No valid cover points found");

        if (m_State == State.Running)
        {
            await Task.Delay((int)(m_RecheckClosestPointTime * 1000));
            GetClosestValidCover();
        }
            
    }
}