using UnityEngine;

public class Player : MonoBehaviour
{
    // Hodnoty těchto proměnných jsou nastaveny v Unity Editoru
    [Header("Configuration")]
    [SerializeField] private bool isPlayer2;
    public bool isAI;

    // Hodnoty těchto proměnných jsou nastaveny v kódu nebo ve ScriptableObjektu (v editoru)
    [HideInInspector] public GameValuesSO GV;
    [HideInInspector] public InputHandler IH;

    private Rigidbody RB;

    private int inputMovementDirection;
    private int lastMovementDirection;
    private float maxMovementSpeed;
    private float currentMovementSpeed;
    private float movementAcceleration;

    [HideInInspector] public Transform ballTransform;

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
        inputMovementDirection = (isAI) ? AIMovement() : KeyboardMovement();

        if (inputMovementDirection != 0) lastMovementDirection = inputMovementDirection;
        RB.velocity = Vector3.forward * (lastMovementDirection * currentMovementSpeed);

        // Akcelerace, zpomalení
        float thisFrameMovementAcceleration = Time.deltaTime * movementAcceleration;
        if (inputMovementDirection != 0 && currentMovementSpeed < maxMovementSpeed) {
            // Postupné zrychlování, dokud se nedostaneme na `maxMovementSpeed`
            currentMovementSpeed += thisFrameMovementAcceleration;
            if (currentMovementSpeed > maxMovementSpeed) currentMovementSpeed = maxMovementSpeed;
        }
        else if (inputMovementDirection == 0 && currentMovementSpeed > 0) {
            // Postupné zpomalování, dokud se nedostaneme na nulu
            currentMovementSpeed -= thisFrameMovementAcceleration;
            if (currentMovementSpeed < 0) currentMovementSpeed = 0;
        }
    }

    private int KeyboardMovement() => (isPlayer2) ? IH.MovementPlayer2 : IH.MovementPlayer1;

    private float thisZ, ballZ;
    private readonly float AIZThreshold = 0.5f;
    private int AIMovement()
    {
        thisZ = this.transform.position.z;
        ballZ = ballTransform.position.z;
        if (ballZ - thisZ > AIZThreshold) return 1;
        else if (thisZ - ballZ > AIZThreshold) return -1;
        return 0;
    }
}
