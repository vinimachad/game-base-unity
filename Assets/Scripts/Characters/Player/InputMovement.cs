using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputMovement : MoveBase
{
    [Header("Input")]
    [SerializeField] private PlayerInput playerInput;
    public Camera cam;

    public override void Update()
    {
        movInput = playerInput.actions["Move"].ReadValue<Vector2>();
        base.Update();
    }
}
