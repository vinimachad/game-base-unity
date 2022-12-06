using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Wandering : MonoBehaviour
{

    [Header("Ranges")]
    [SerializeField] private float walkPointRange;

    [Header("Layers")]
    [SerializeField] private LayerMask groundLayer;

    [Header("Agent")]
    [SerializeField] private NavMeshAgent agent;

    private bool walkPointSet;
    private Vector3 walkPoint;

    private bool startCoroutine;
    private bool canPatroling;

    private float canWalkAgainTimer;
    private float cantWalkTimer;

    void Start()
    {
        startCoroutine = true;
        canPatroling = false;
        walkPointSet = false;
    }

    public void Patroling()
    {

        if (startCoroutine)
        {
            StartCoroutine(Wander());
        }

        if (canPatroling)
        {
            CanPatroling();
        }
    }

    public void CanPatroling()
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

    IEnumerator Wander()
    {
        canWalkAgainTimer = Random.Range(2, 4);
        cantWalkTimer = Random.Range(1, 3);

        startCoroutine = false;
        yield return new WaitForSeconds(canWalkAgainTimer);
        canPatroling = true;
        yield return new WaitForSeconds(cantWalkTimer);
        canPatroling = false;
        startCoroutine = true;
    }
}
