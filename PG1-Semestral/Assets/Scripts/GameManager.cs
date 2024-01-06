using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject ballGO;

    private Ball ballScript;

    private int player1Score;
    private int player2Score;

    private void Awake()
    {
        ballScript = ballGO.GetComponent<Ball>();

        ballScript.GM = this;
    }

    private void Start()
    {
        player1Score = 0;
        player2Score = 0;
    }

    public void Goal(Collider goal)
    {
        if (goal.CompareTag("Goal1")) {
            player2Score++;
        }
        else {
            player1Score++;
        }
        print($"Skóre: {player1Score}:{player2Score}");
        ballScript.ResetPositionAndStartMoving();
    }
}
