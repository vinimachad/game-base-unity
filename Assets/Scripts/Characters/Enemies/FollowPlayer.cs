using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{

    public Transform player;
    public float maxDistance;
    public float atackDistance;
    public float speed;

    private Vector3 _startPos;
    private bool _isAtacking;
    private bool _isWandering;
    private bool _isWalking;

    private int _randomHorizontalRotation;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        _isWandering = false;
        _isAtacking = false;
        _isWalking = false;
        _startPos = transform.position;
    }

    private void Update()
    {
        if (!_isWandering)
        {
            StartCoroutine(Wandering());
        }
        float distance = Vector3.Distance(transform.position, player.position);
        bool followingTarget = distance < maxDistance;

        if (_isWalking)
        {
            var targetRotation = Quaternion.AngleAxis(_randomHorizontalRotation, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, speed * Time.deltaTime);
            transform.position += transform.forward * speed * Time.deltaTime;
        }

        if (followingTarget && !_isAtacking)
        {
            RotateEnemy(player.position);
            transform.position += transform.forward * speed * Time.deltaTime;
        }
        else if (transform.position == _startPos)
        {
            return;
        }

        _isAtacking = distance <= atackDistance;
    }

    private void RotateEnemy(Vector3 targetPos)
    {
        var enemy = transform;
        var targetRotation = Quaternion.LookRotation(targetPos - enemy.position);
        transform.rotation = Quaternion.Slerp(enemy.rotation, targetRotation, speed * Time.deltaTime);
    }

    IEnumerator Wandering()
    {
        _randomHorizontalRotation = Random.Range(0, 360);
        var randWalkingTime = Random.Range(1, 3);
        var startWalkingTime = Random.Range(2, 10);
        _isWandering = true;
        yield return new WaitForSeconds(startWalkingTime);
        _isWalking = true;
        yield return new WaitForSeconds(randWalkingTime);
        _isWalking = false;
        _isWandering = false;
    }
}
