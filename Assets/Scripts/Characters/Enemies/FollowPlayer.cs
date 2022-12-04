using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public class FollowPlayer : MonoBehaviour
{

    private Transform target;
    [SerializeField] private float speed;
     
    [SerializeField] private NavMeshAgent agent;

    public void ChasePlayer()
    {
        target = GameObject.Find("Player").transform;
        RotateEnemy(target.position);
        agent.SetDestination(target.position);
    }

    private void RotateEnemy(Vector3 targetPos)
    {
        var enemy = transform;
        var targetRotation = Quaternion.LookRotation(targetPos - enemy.position);
        transform.rotation = Quaternion.Slerp(enemy.rotation, targetRotation, speed * Time.deltaTime);
    }
}
