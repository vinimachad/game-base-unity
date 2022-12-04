using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wandering : MonoBehaviour
{
    // Start is called before the first frame update
    private int _randomHorizontalRotation;
    private bool _isWandering;
    private bool _isWalking;
    private bool _isFollowing;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _speed;

    void Start()
    {
        _isWandering = false;
        _isWalking = false;
        _isFollowing = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isWandering && !_isFollowing)
        {
            StartCoroutine(WanderingEnemy());
        }

        if (_isWalking)
        {
            var targetRotation = Quaternion.AngleAxis(_randomHorizontalRotation, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
            transform.position += transform.forward * _speed * Time.deltaTime;
        }
    }

    public void OnFollowing(bool isFollowing)
    {
        _isFollowing = isFollowing;
    }

    IEnumerator WanderingEnemy()
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
