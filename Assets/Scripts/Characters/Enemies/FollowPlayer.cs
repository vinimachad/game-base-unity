using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FollowPlayer : MonoBehaviour
{

    public Transform target;
    public float maxDistance;
    public float atackDistance;
    public float speed;

    public UnityEvent<bool> onAtacking;
    public UnityEvent<bool> onFollowing;

    private Vector3 _startPos;
    private bool _isAtacking;

    private void Start()
    {
        _isAtacking = false;
        _startPos = transform.position;
    }

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        bool followingTarget = distance < maxDistance;

        if (followingTarget && !_isAtacking)
        {
            RotateEnemy(target.position);
            transform.position += transform.forward * speed * Time.deltaTime;
        }
        else if (transform.position == _startPos)
        {
            return;
        }

        _isAtacking = distance <= atackDistance;
        onAtacking.Invoke(_isAtacking);
        onFollowing.Invoke(followingTarget);
    }

    private void RotateEnemy(Vector3 targetPos)
    {
        var enemy = transform;
        var targetRotation = Quaternion.LookRotation(targetPos - enemy.position);
        transform.rotation = Quaternion.Slerp(enemy.rotation, targetRotation, speed * Time.deltaTime);
    }
}
