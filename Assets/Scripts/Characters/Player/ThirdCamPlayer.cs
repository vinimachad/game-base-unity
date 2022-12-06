using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class ThirdCamPlayer : MonoBehaviour
{
    public float turnSpeed = 10f;

    private float _horizontalInput;
    private float _verticalInput;

    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    public Rigidbody rb;

    [SerializeField] private PlayerInput playerInput;
    [SerializeField] private CinemachineFreeLook cinemachine;
    [SerializeField] private Transform combatLook;
    [SerializeField] private TouchPanel touchPanel;

    public CameraStyle currentCam;

    public enum CameraStyle
    {
        BASIC,
        COMBAT
    }

    private void Update()
    {

        if (currentCam == CameraStyle.BASIC)
        {
            ChangeViewDirOfPlayer();
        } else if (currentCam == CameraStyle.COMBAT)
        {
            CombatCam();
        }
    }

    private void CombatCam()
    {
        Debug.Log(touchPanel.VectorOutput());
        cinemachine.m_XAxis.Value = touchPanel.VectorOutput().x * Time.deltaTime * 130f;
        Vector3 dirToCombatLookAt = combatLook.position - new Vector3(transform.position.x, combatLook.position.y, transform.position.z);
        orientation.forward = dirToCombatLookAt.normalized;

        playerObj.forward = dirToCombatLookAt.normalized;
    }

    private void ChangeViewDirOfPlayer()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");

        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        Vector3 inputDir = orientation.forward * _verticalInput + orientation.right * _horizontalInput;

        if (inputDir != Vector3.zero)
        {
            player.forward = Vector3.Slerp(playerObj.forward, inputDir.normalized, Time.deltaTime * turnSpeed);
        }
    }
}
