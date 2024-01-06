using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    private PlayerInput PI;

    private InputAction p1UpAction;
    private InputAction p1DownAction;
    private InputAction p2UpAction;
    private InputAction p2DownAction;

    public int MovementPlayer1 { get; private set; }
    public int MovementPlayer2 { get; private set; }

    private void Awake()
    {
        PI = GetComponent<PlayerInput>();

        p1UpAction = PI.actions["p1_up"];
        p1DownAction = PI.actions["p1_down"];
        p2UpAction = PI.actions["p2_up"];
        p2DownAction = PI.actions["p2_down"];
    }

    private void Update()
    {
        MovementPlayer1 = Convert.ToInt32(p1UpAction.IsPressed()) - Convert.ToInt32(p1DownAction.IsPressed());
        MovementPlayer2 = Convert.ToInt32(p2UpAction.IsPressed()) - Convert.ToInt32(p2DownAction.IsPressed());
    }
}
