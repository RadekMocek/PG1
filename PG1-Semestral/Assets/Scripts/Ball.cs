using UnityEngine;

public class Ball : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameValuesSO GV;

    [HideInInspector] public GameManager GM;
    private Rigidbody RB;

    private Vector3 movementDirection;
    private float movementSpeed;

    private void Awake()
    {
        RB = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        movementSpeed = GV.BallMovementSpeed;

        ResetPositionAndStartMoving();
    }

    private void OnCollisionEnter(Collision collision)
    {
        GameObject collisionGO = collision.gameObject;
        if (collisionGO.CompareTag("Wall")) {
            movementDirection.z *= -1;
        }
        else if (collisionGO.CompareTag("Paddle")) {
            movementDirection.x *= -1;
        }
        UpdateMovementDirection();
    }

    private void OnTriggerEnter(Collider other)
    {
        GM.Goal(other);
    }

    public void ResetPositionAndStartMoving()
    {
        this.transform.position = Vector3.zero;
        movementDirection = new Vector3(1, 0, 1);
        UpdateMovementDirection();
    }

    private void UpdateMovementDirection()
    {
        RB.velocity = movementDirection * movementSpeed;
    }
}
