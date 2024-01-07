using UnityEngine;

public class Ball : MonoBehaviour
{
    // Hodnoty tìchto promìnných jsou nastaveny v Unity Editoru
    [Header("References")]
    [SerializeField] private GameValuesSO GV;
    [SerializeField] private Transform wallCheckBackupUpTransform;
    [SerializeField] private Transform wallCheckBackupDownTransform;

    [Header("Configuration")]
    [SerializeField] private LayerMask wallLayer;

    // Hodnoty tìchto promìnných jsou nastaveny v kódu
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

    private void Update()
    {
        // Pokud se míèek z nìjakého dùvodu zpomalí, vrátit mu správnou rychlost
        if (RB.velocity.magnitude < movementSpeed) UpdateMovementDirection();
    }

    // Kolize se zdí/pálkou
    private void OnCollisionEnter(Collision collision)
    {
        GameObject collisionGO = collision.gameObject;
        if (collisionGO.CompareTag("Wall")) {
            // Odraz od zdi – pøevrátit vertikální rychlost
            movementDirection.z *= -1;
        }
        if (collisionGO.CompareTag("Paddle")) {
            // Odraz od pálky
            Vector3 ballPosition = this.transform.position;
            Vector3 paddlePosition = collision.transform.position;
            if (Mathf.Abs(ballPosition.x) - Mathf.Abs(paddlePosition.x) > 0) {
                // Pøíliš pozdní odraz – míèek poletí za pálku
                movementDirection.z *= -1;
                movementDirection.x = ballPosition.x / Mathf.Abs(ballPosition.x);
            }
            else {
                // Vèasný odraz – nový smìr letu záleží na místì odrazu
                movementDirection = (ballPosition - paddlePosition); // (cíl - start) -> smìr od pálky k míèku
                if (Physics.CheckSphere(wallCheckBackupUpTransform.position, .1f, wallLayer) || Physics.CheckSphere(wallCheckBackupDownTransform.position, .1f, wallLayer)) {
                    // Aby míèek nešel pøirazit ke zdi
                    movementDirection.z *= -1;
                }
            }
        }
        UpdateMovementDirection();
    }

    // "Kolize" s "brankou"
    private void OnTriggerEnter(Collider other)
    {
        GM.Goal(other);
    }

    // Pøesunout doprostøed høištì a vyletìt náhodným smìrem
    public void ResetPositionAndStartMoving()
    {
        // Pøesunout doprostøed høištì
        this.transform.position = Vector3.zero;
        // Smìr pohybu takový, aby se neblížil jedné z os
        int angleDegrees = Random.Range(30, 61) + Random.Range(0, 4) * 90;
        float angleRadians = angleDegrees * Mathf.Deg2Rad;
        movementDirection = new Vector3(Mathf.Cos(angleRadians), 0, Mathf.Sin(angleRadians));
        // Uvést do pohybu
        UpdateMovementDirection();
    }

    // Nastavit smìr pohybu odpovídající `movementDirection` ve správné rychlosti odpovídající `movementSpeed`
    private void UpdateMovementDirection()
    {
        RB.velocity = movementDirection.normalized * movementSpeed;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(wallCheckBackupUpTransform.position, 0.1f);
        Gizmos.DrawWireSphere(wallCheckBackupDownTransform.position, 0.1f);
    }
}
