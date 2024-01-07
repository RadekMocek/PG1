using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameValuesSO GV;
    [SerializeField] private InputHandler IH;

    [Header("Configuration")]
    [SerializeField] private bool isPlayer2;

    private Rigidbody RB;

    private int inputMovementDirection;
    private int lastMovementDirection;
    private float maxMovementSpeed;
    private float currentMovementSpeed;
    private float movementAcceleration;

    private void Awake()
    {
        RB = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        maxMovementSpeed = GV.PlayerMaxMovementSpeed;
        currentMovementSpeed = 0;

        movementAcceleration = GV.PlayerMovementAcceleration;
    }

    private void Update()
    {
        Movement();
    }

    private void Movement()
    {
        inputMovementDirection = (isPlayer2) ? IH.MovementPlayer2 : IH.MovementPlayer1;

        if (inputMovementDirection != 0) lastMovementDirection = inputMovementDirection;
        RB.velocity = Vector3.forward * (lastMovementDirection * currentMovementSpeed);

        // Akcelerace, zpomalení
        float thisFrameMovementAcceleration = Time.deltaTime * movementAcceleration;
        if (inputMovementDirection != 0 && currentMovementSpeed < maxMovementSpeed) {
            currentMovementSpeed += thisFrameMovementAcceleration;
            if (currentMovementSpeed > maxMovementSpeed) currentMovementSpeed = maxMovementSpeed;
        }
        else if (inputMovementDirection == 0 && currentMovementSpeed > 0) {
            currentMovementSpeed -= thisFrameMovementAcceleration;
            if (currentMovementSpeed < 0) currentMovementSpeed = 0;
        }
    }
}
