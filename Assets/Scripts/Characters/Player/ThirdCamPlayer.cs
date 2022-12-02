using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdCamPlayer : MonoBehaviour
{
    public float turnSpeed = 1f;

    private float _horizontalInput;
    private float _verticalInput;

    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    public Rigidbody rb;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void FixedUpdate()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");
        ChangeViewDirOfPlayer();
    }


    private void ChangeViewDirOfPlayer()
    {
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        Vector3 inputDir = orientation.forward * _verticalInput + orientation.right * _horizontalInput;

        if (inputDir != Vector3.zero)
        {
            player.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * turnSpeed);
        }
    }
}
