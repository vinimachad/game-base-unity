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
        if (movInput.y > 0)
        {
            movInput = new Vector2(0, 1);
        } else if (movInput.y < 0)
        {
            movInput = new Vector2(0, -1);
        }
        base.Update();
    }
}
