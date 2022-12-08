using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    public float m_Speed = 0.1f;
    public override void GettingAttacked()
    {
        base.GettingAttacked();
        // i am doing this because i don't feel like setting it up properly.
        GameObject.Find("Ninja").GetComponent<BehaviourTreeRunner>().m_Tree.m_BlackBoard.Set("GettingAttacked",m_GettingAttacked);
    }

    public override void NoLongerGettingAttacked()
    {
        base.NoLongerGettingAttacked();
        // i am doing this because i don't feel like setting it up properly.
        GameObject.Find("Ninja").GetComponent<BehaviourTreeRunner>().m_Tree.m_BlackBoard.Set("GettingAttacked",m_GettingAttacked);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            Move(new Vector2(0,m_Speed * Time.deltaTime));
            
        }
        if (Input.GetKey(KeyCode.S))
        {
            Move(new Vector2(0,-m_Speed * Time.deltaTime));
            
            
        }
        if (Input.GetKey(KeyCode.A))
        {
            Move(new Vector2(-m_Speed * Time.deltaTime,0));
            
        }
        if (Input.GetKey(KeyCode.D))
        {
            Move(new Vector2(m_Speed * Time.deltaTime,0));
        }
    }

    public void Move(Vector2 dir)
    {
        Vector3 newPos = transform.position;
        newPos.x += dir.x;
        newPos.z += dir.y;
        transform.position = newPos;
    }
}
