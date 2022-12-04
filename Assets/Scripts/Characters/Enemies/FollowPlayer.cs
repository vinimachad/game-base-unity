using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AI;

public class FollowPlayer : MonoBehaviour
{

    public Transform target;
    public float maxDistance;
    public float atackDistance;
    public float speed;
    public float sightRange;
    public float atackRange;
    public float walkPointRange;
    private Vector3 walkPoint;
    private bool walkPointSet;

    public NavMeshAgent agent;
    public LayerMask playerLayer;
    public LayerMask groundLayer;

    public UnityEvent<bool> onAtacking;
    public UnityEvent<bool> onFollowing;

    private Vector3 _startPos;
    private bool _isAtacking;


    private void Start()
    {
        walkPointSet = false;
        _isAtacking = false;
        _startPos = transform.position;
    }

    private void Update()
    {
        ChekRange();
    }

    private void ChekRange()
    {
       bool playerInPatrolingRange = Physics.CheckSphere(transform.position, sightRange, playerLayer);
       bool playerInAtackRange = Physics.CheckSphere(transform.position, atackRange, playerLayer);

        if (!playerInPatrolingRange && !playerInAtackRange) Patroling();
        if (playerInPatrolingRange && !playerInAtackRange) ChasePlayer();
        if (playerInPatrolingRange && playerInAtackRange) Atacking();

        onFollowing.Invoke(playerInPatrolingRange && !playerInAtackRange);
        onAtacking.Invoke(playerInPatrolingRange && playerInAtackRange);
    }

    private void Patroling()
    {

        if (!walkPointSet) SearchWalkPoint();

        if (walkPointSet)
            agent.SetDestination(walkPoint);
        Vector3 distanceToWalkPoint = transform.position - walkPoint;

        if (distanceToWalkPoint.magnitude < 1)
            walkPointSet = false;
    }

    private void SearchWalkPoint()
    {
        float randomX = Random.Range(-walkPointRange, walkPointRange);
        float randomZ = Random.Range(-walkPointRange, walkPointRange);

        walkPoint = new Vector3(transform.position.x + randomX, transform.position.y, transform.position.z + randomZ);
        if (Physics.Raycast(walkPoint, -transform.up, 2f, groundLayer))
            walkPointSet = true;
    }

    private void ChasePlayer()
    {
        RotateEnemy(target.position);
        agent.SetDestination(target.position);
    }

    private void Atacking()
    {
    }

    private void RotateEnemy(Vector3 targetPos)
    {
        var enemy = transform;
        var targetRotation = Quaternion.LookRotation(targetPos - enemy.position);
        transform.rotation = Quaternion.Slerp(enemy.rotation, targetRotation, speed * Time.deltaTime);
    }
}
