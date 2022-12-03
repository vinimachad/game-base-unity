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

    private void Awake()
    {
        _isAtacking = false;
        _startPos = transform.position;
    }

    private void Update()
    {

        float distance = Vector3.Distance(transform.position, player.position);
        bool followingTarget = distance < maxDistance;

        if (followingTarget && !_isAtacking)
        {
            RotateEnemy(player.position);
            transform.position += transform.forward * speed * Time.deltaTime;
        }
        else if (transform.position == _startPos)
        {
            return;
        }
        else if (!followingTarget && !_isAtacking)
        {
            RotateEnemy(_startPos);
            transform.position += transform.forward * speed * Time.deltaTime;
        }

        _isAtacking = distance <= atackDistance;
    }

    private void RotateEnemy(Vector3 targetPos)
    {
        var enemy = transform;
        var targetRotation = Quaternion.LookRotation(targetPos - enemy.position);
        transform.rotation = Quaternion.Slerp(enemy.rotation, targetRotation, speed * Time.deltaTime);
    }
}
