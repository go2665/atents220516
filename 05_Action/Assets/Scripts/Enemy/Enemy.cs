using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public Transform patrolRoute;
    NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        if(patrolRoute)
        {
            agent.SetDestination( patrolRoute.GetChild(0).position );
        }
    }

    private void Update()
    {
        //if(patrolRoute!=null)
        //{
        //    agent.SetDestination(patrolRoute.position);  // 길찾기는 연산량이 많은 작업. SetDestination을 자주하면 안된다.
        //}
        if(agent.remainingDistance <= agent.stoppingDistance)
        {            
            Debug.Log("도착");
        }    
    }
}
