using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Hodnoty těchto proměnných jsou nastaveny v Unity Editoru
    [Header("References")]
    [SerializeField] private GameValuesSO GV;
    [SerializeField] private InputHandler IH;
    [SerializeField] private HUDManager HUD;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private GameObject ballGO;
    [SerializeField] private GameObject player1GO;
    [SerializeField] private GameObject player2GO;

    // Hodnoty těchto proměnných jsou nastaveny v kódu
    private Ball ballScript;
    private Player player1Script;
    private Player player2Script;

    private int player1Score;
    private int player2Score;

    private void Awake()
    {
        ballScript = ballGO.GetComponent<Ball>();
        ballScript.GM = this;
        ballScript.GV = GV;

        HUD.GM = this;

        player1Script = player1GO.GetComponent<Player>();
        player1Script.ballTransform = ballGO.transform;
        player1Script.GV = GV;
        player1Script.IH = IH;

        player2Script = player2GO.GetComponent<Player>();
        player2Script.ballTransform = ballGO.transform;
        player2Script.GV = GV;
        player2Script.IH = IH;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) {
            ballScript.NewRound();
        }
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
        ballScript.NewRound();
    }

    // Po stisknutí tlačítka "Play" v hlavním menu
    public void StartGame(bool isPlayer1AI, bool isPlayer2AI)
    {
        // Reset skóre
        player1Score = 0;
        player2Score = 0;
        // Přepnout režim pálek člověk/AI podle vybraných položek v hlavním menu
        player1Script.isAI = isPlayer1AI;
        player2Script.isAI = isPlayer2AI;
        //
        StartCoroutine(StartGameCoroutine());
    }

    private IEnumerator StartGameCoroutine()
    {
        // Smooth rotace kamery na hřiště
        var target = Quaternion.Euler(70, 0, 0);
        while (cameraTransform.eulerAngles.x < 66) {
            cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, target, 1 * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }
        // Začít první kolo
        ballScript.NewRound();
    }
}
