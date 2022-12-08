using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StackedBeans.Utils;
using UnityEngine;
using UnityEngine.AI;
[NodeName("GetClosestWeapon")]
[StateDisplay("GettingArmed",30)]
public class GetClosestWeapon : ActionNode
{
    [HideInInspector] new public static string m_AdditionalCategory = "AI/Targetting/";

    
    public string m_HasWeaponName = "HasWeapon";
    public string m_AgentName = "Agent";
    private GameObject m_Agent;
    private List<Transform> m_Weapons = new List<Transform>();
    private NavMeshAgent m_NavAgent;
    private Transform m_ClosestWeapon;
    private bool m_CanFail;
    private bool m_FailCheckRunning;
    
    protected override void OnStart()
    {
        m_Agent = m_BlackBoard.Get<GameObject>(m_AgentName);
        m_NavAgent = m_Agent.GetComponent<NavMeshAgent>();

        m_Weapons.Clear();
        DroppedWeapon[] droppedWeapons = FindObjectsOfType<DroppedWeapon>();

        foreach (var weapon in droppedWeapons)
        {
            m_Weapons.Add(weapon.transform);
        }
        m_ParentTree.DisplayNodeState(this);
    }

    public override void OnStop()
    {
        m_ParentTree.RemoveStateDisplay();
        // Debug.Log($"OnStop{m_Message }");
    }

    protected override State OnUpdate()
    {
        m_ClosestWeapon = m_Weapons.GetClosestTransform(m_Agent.transform);

        if (m_ClosestWeapon.position != m_NavAgent.destination)
        {
            m_NavAgent.SetDestination(m_ClosestWeapon.position);
        }

        if (m_BlackBoard.Get<bool>(m_HasWeaponName))
        {
            return State.Success;
        }
        else if(!m_NavAgent.pathPending && m_NavAgent.remainingDistance <= m_NavAgent.stoppingDistance
                                        && (!m_NavAgent.hasPath || m_NavAgent.velocity.sqrMagnitude == 0f))
        {
            if (m_CanFail)
            {
                return State.Failure;
            }
            else if(!m_FailCheckRunning)
            {
                m_FailCheckRunning = true;
                
            }
        }
        
        return State.Running;
    }

    public async void CanFailCheck()
    {
        await Task.Delay(1000);
        if(!m_BlackBoard.Get<bool>(m_HasWeaponName))
            m_CanFail = true;
    }
    
    public enum FollowMethod
    {
        Loop,
        WalkBack,
        SuccessOnLastPointReached
    }
}