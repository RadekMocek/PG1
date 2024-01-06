using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameValuesSO GV;
    [SerializeField] private InputHandler IH;

    [Header("Configuration")]
    [SerializeField] private bool isPlayer2;

    private Rigidbody RB;

    private int movementDirection;
    private float movementSpeed;

    private void Awake()
    {
        RB = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        movementSpeed = GV.PlayerMovementSpeed;
    }

    private void Update()
    {
        movementDirection = (isPlayer2) ? IH.MovementPlayer2 : IH.MovementPlayer1;

        RB.velocity = Vector3.forward * (movementDirection * movementSpeed);
    }
}
