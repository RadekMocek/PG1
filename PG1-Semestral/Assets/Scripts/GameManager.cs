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

    private bool isMainMenu;

    private void Awake()
    {
        ballScript = ballGO.GetComponent<Ball>();
        ballScript.GM = this;
        ballScript.GV = GV;
        ballScript.HUD = HUD;

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

    private void Start()
    {
        isMainMenu = true;
    }

    public void Goal(Collider goal)
    {
        if (isMainMenu) {
            ballScript.StopAndCenter();
            return;
        }
        // Po gólu přičíst skóre patřičnému hráči a začít nové kolo; skóre zobrazit v HUD
        if (goal.CompareTag("Goal1")) {
            player2Score++;
        }
        else {
            player1Score++;
        }
        ballScript.NewRound();
        HUD.ShowScore(player1Score, player2Score);
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
        isMainMenu = false;
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
        HUD.ShowScore(0, 0);
    }

    // Po stisknutí tlačítka "Reset the ball"
    public void ResetBall() => ballScript.NewRound();

    // Po stisknutí tlačítka "Back to menu"
    public void BackToMainMenu() => StartCoroutine(BackToMainMenuCoroutine());

    private IEnumerator BackToMainMenuCoroutine()
    {
        isMainMenu = true;
        // Kamera do původní pozice
        var target = Quaternion.Euler(-3, 0, 0);
        while (cameraTransform.eulerAngles.x > 1) {
            cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, target, 1 * Time.deltaTime);
            yield return new WaitForEndOfFrame();
        }

        ballScript.StopAndCenter();
        player1Script.ResetPosition();
        player2Script.ResetPosition();
        HUD.ShowMainMenu();
    }
}
