using UnityEngine;

public class Ball : MonoBehaviour
{
    // Hodnoty t�chto prom�nn�ch jsou nastaveny v Unity Editoru
    [Header("References")]
    [SerializeField] private GameValuesSO GV;
    [SerializeField] private Transform wallCheckBackupUpTransform;
    [SerializeField] private Transform wallCheckBackupDownTransform;

    [Header("Configuration")]
    [SerializeField] private LayerMask wallLayer;

    // Hodnoty t�chto prom�nn�ch jsou nastaveny v k�du
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
        // Pokud se m��ek z n�jak�ho d�vodu zpomal�, vr�tit mu spr�vnou rychlost
        if (RB.velocity.magnitude < movementSpeed) UpdateMovementDirection();
    }

    // Kolize se zd�/p�lkou
    private void OnCollisionEnter(Collision collision)
    {
        GameObject collisionGO = collision.gameObject;
        if (collisionGO.CompareTag("Wall")) {
            // Odraz od zdi � p�evr�tit vertik�ln� rychlost
            movementDirection.z *= -1;
        }
        if (collisionGO.CompareTag("Paddle")) {
            // Odraz od p�lky
            Vector3 ballPosition = this.transform.position;
            Vector3 paddlePosition = collision.transform.position;
            if (Mathf.Abs(ballPosition.x) - Mathf.Abs(paddlePosition.x) > 0) {
                // P��li� pozdn� odraz � m��ek polet� za p�lku
                movementDirection.z *= -1;
                movementDirection.x = ballPosition.x / Mathf.Abs(ballPosition.x);
            }
            else {
                // V�asn� odraz � nov� sm�r letu z�le�� na m�st� odrazu
                movementDirection = (ballPosition - paddlePosition); // (c�l - start) -> sm�r od p�lky k m��ku
                if (Physics.CheckSphere(wallCheckBackupUpTransform.position, .1f, wallLayer) || Physics.CheckSphere(wallCheckBackupDownTransform.position, .1f, wallLayer)) {
                    // Aby m��ek ne�el p�irazit ke zdi
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

    // P�esunout doprost�ed h�i�t� a vylet�t n�hodn�m sm�rem
    public void ResetPositionAndStartMoving()
    {
        // P�esunout doprost�ed h�i�t�
        this.transform.position = Vector3.zero;
        // Sm�r pohybu takov�, aby se nebl�il jedn� z os
        int angleDegrees = Random.Range(30, 61) + Random.Range(0, 4) * 90;
        float angleRadians = angleDegrees * Mathf.Deg2Rad;
        movementDirection = new Vector3(Mathf.Cos(angleRadians), 0, Mathf.Sin(angleRadians));
        // Uv�st do pohybu
        UpdateMovementDirection();
    }

    // Nastavit sm�r pohybu odpov�daj�c� `movementDirection` ve spr�vn� rychlosti odpov�daj�c� `movementSpeed`
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
