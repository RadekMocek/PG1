using UnityEngine;

public class Ball : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameValuesSO GV;

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

        movementDirection = new Vector3(1, 0, 1);
        UpdateMovementDirection();
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

    private void UpdateMovementDirection()
    {
        RB.velocity = movementDirection * movementSpeed;
    }
}
