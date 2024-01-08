using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour
{
    // Hodnoty těchto proměnných jsou nastaveny v Unity Editoru
    [Header("References")]
    [SerializeField] private Transform wallCheckBackupUpTransform;
    [SerializeField] private Transform wallCheckBackupDownTransform;

    [Header("Configuration")]
    [SerializeField] private LayerMask wallLayer;

    // Hodnoty těchto proměnných jsou nastaveny v kódu nebo ve ScriptableObjektu (v editoru)
    [HideInInspector] public GameManager GM;
    [HideInInspector] public GameValuesSO GV;
    private Rigidbody RB;

    private Vector3 movementDirection;
    private float initialMovementSpeed;
    private float movementSpeed;
    private float movementSpeedIncrement;
    private bool isMoving;

    private void Awake()
    {
        RB = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        initialMovementSpeed = GV.BallInitialMovementSpeed;
        movementSpeed = initialMovementSpeed;
        movementSpeedIncrement = GV.BallMovementSpeedIncrement;

        NewRound();
    }

    private void Update()
    {        
        // Pokud se míček z nějakého důvodu zpomalí, vrátit mu správnou rychlost
        if (isMoving && RB.velocity.magnitude < movementSpeed) UpdateMovementDirection();
    }

    // Kolize se zdí/pálkou
    private GameObject collisionGO;
    private Vector3 ballPosition, paddlePosition;
    private void OnCollisionEnter(Collision collision)
    {
        collisionGO = collision.gameObject;
        if (collisionGO.CompareTag("Wall")) {
            // Odraz od zdi – převrátit vertikální rychlost
            movementDirection.z *= -1;
            // Pozměnit směr pokud míček lítá příliš vertikálně (a dlouho by trvalo než doletí k pálce)
            if (Mathf.Abs(movementDirection.normalized.x) < 0.3f) movementDirection.x = Mathf.Sign(movementDirection.x);
        }
        if (collisionGO.CompareTag("Paddle")) {
            // Odraz od pálky
            ballPosition = this.transform.position;
            paddlePosition = collision.transform.position;
            if (Mathf.Abs(ballPosition.x) - Mathf.Abs(paddlePosition.x) > 0) {
                // Příliš pozdní odraz – míček poletí za pálku
                movementDirection.z *= -1;
                movementDirection.x = Mathf.Sign(movementDirection.x);
            }
            else {
                // Včasný odraz – nový směr letu záleží na místě odrazu
                movementDirection = (ballPosition - paddlePosition); // (cíl - start) -> směr od pálky k míčku
                if (Physics.CheckSphere(wallCheckBackupUpTransform.position, .1f, wallLayer) || Physics.CheckSphere(wallCheckBackupDownTransform.position, .1f, wallLayer)) {
                    // Aby míček nešel přirazit ke zdi
                    movementDirection.z *= -1;
                }
                movementSpeed += movementSpeedIncrement; // Po odrazu pálkou míček zrychlit
            }
        }
        UpdateMovementDirection();
    }

    // "Kolize" s "brankou"
    private void OnTriggerEnter(Collider other)
    {
        GM.Goal(other);
    }

    // Přesunout doprostřed hřiště a vyletět náhodným směrem
    public void NewRound()
    {
        // Zastavit
        isMoving = false;
        RB.velocity = Vector3.zero;
        // Přesunout doprostřed hřiště
        this.transform.position = Vector3.zero;
        // Resetovat rychlost
        movementSpeed = initialMovementSpeed;
        // Směr pohybu takový, aby se neblížil jedné z os
        int angleDegrees = Random.Range(30, 61) + Random.Range(0, 4) * 90;
        float angleRadians = angleDegrees * Mathf.Deg2Rad;
        movementDirection.Set(Mathf.Cos(angleRadians), 0, Mathf.Sin(angleRadians));
        // Odpočet a následné uvedení do pohybu
        StopAllCoroutines();
        StartCoroutine(StartMovingAfterCountDown());
    }

    private IEnumerator StartMovingAfterCountDown()
    {
        // 3 2 1
        for (int i = 3; i > 0; i--) {
            print(i);
            yield return new WaitForSeconds(.5f);
        }
        print("Start");
        // Uvést do pohybu
        UpdateMovementDirection();
        isMoving = true;
    }

    // Nastavit směr pohybu odpovídající `movementDirection` ve správné rychlosti odpovídající `movementSpeed`
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
