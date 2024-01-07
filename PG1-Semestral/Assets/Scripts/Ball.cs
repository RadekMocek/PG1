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

        int angleDegrees = Random.Range(30, 61) + Random.Range(0, 4) * 90;
        float angleRadians = angleDegrees * Mathf.Deg2Rad;
        movementDirection = new Vector3(Mathf.Cos(angleRadians), 0, Mathf.Sin(angleRadians)).normalized;
        UpdateMovementDirection();
    }

    private void UpdateMovementDirection()
    {
        RB.velocity = movementDirection * movementSpeed;
    }
}
