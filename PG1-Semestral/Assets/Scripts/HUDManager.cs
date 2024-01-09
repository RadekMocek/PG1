using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [Header("UI Parents")]
    [SerializeField] private GameObject mainMenuGO;
    [SerializeField] private GameObject inGameGO;

    [Header("Main menu")]
    [SerializeField] private ToggleGroup group1;
    [SerializeField] private ToggleGroup group2;

    [Header("InGame")]
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private GameObject countdownGO;
    [SerializeField] private TMP_Text countdownText;

    [Header("Settings")]
    [SerializeField] private GameObject soundManagerGO;
    [SerializeField] private GameObject waterGO;

    [HideInInspector] public GameManager GM;

    private void Start()
    {
        // Hra začíná v hlavním menu
        mainMenuGO.SetActive(true);
        inGameGO.SetActive(false);
    }

    public void OnClickPlay()
    {
        // Předat GM nastavení člověk/AI a skrýt menu UI
        bool isPlayer1AI = group1.ActiveToggles().FirstOrDefault().name[0] == '1';
        bool isPlayer2AI = group2.ActiveToggles().FirstOrDefault().name[0] == '1';
        GM.StartGame(isPlayer1AI, isPlayer2AI);
        mainMenuGO.SetActive(false);
    }

    public void OnClickExit() => Application.Quit();

    public void OnClickResetBall() => GM.ResetBall();

    public void OnClickMainMenu()
    {
        inGameGO.SetActive(false);
        GM.BackToMainMenu();
    }

    public void OnClickSettingsSounds() => soundManagerGO.SetActive(!soundManagerGO.activeSelf);

    public void OnClickSettingsWater() => waterGO.SetActive(!waterGO.activeSelf);

    public void ShowScore(int score1, int score2)
    {
        inGameGO.SetActive(true);
        scoreText.text = $"{score1,2} : {score2,2}";
    }

    public void ShowCountdown(int i)
    {
        countdownGO.SetActive(true);
        countdownText.text = $"{i}";
    }

    public void HideCountdown() => countdownGO.SetActive(false);

    public void ShowMainMenu() => mainMenuGO.SetActive(true);
}
