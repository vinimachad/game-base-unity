using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    private Rigidbody rb;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float patrolingRange;
    [SerializeField] private float atackRange;

    private Wandering wandering;
    private Atack atack;
    private FollowPlayer chasePlayer;

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        wandering = GetComponent<Wandering>();
        atack = GetComponent<Atack>();
        chasePlayer = GetComponent<FollowPlayer>();
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    void Update()
    {
        ChekRange();
    }

    private void ChekRange()
    {
        bool playerInAtackRange = Physics.CheckSphere(transform.position, atackRange, playerLayer);
        bool playerInPatrolingRange = Physics.CheckSphere(transform.position, patrolingRange, playerLayer);

        if (!playerInPatrolingRange && !playerInAtackRange) wandering.Patroling();
        if (playerInPatrolingRange && !playerInAtackRange) chasePlayer.ChasePlayer();
        if (playerInPatrolingRange && playerInAtackRange) atack.Atacking();
    }
}
