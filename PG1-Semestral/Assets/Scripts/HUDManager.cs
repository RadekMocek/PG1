using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [SerializeField] private ToggleGroup group1;
    [SerializeField] private ToggleGroup group2;

    [SerializeField] private GameObject mainMenuGO;

    [HideInInspector] public GameManager GM;

    public void OnClickPlay()
    {
        bool isPlayer1AI = group1.ActiveToggles().FirstOrDefault().name[0] == '1';
        bool isPlayer2AI = group2.ActiveToggles().FirstOrDefault().name[0] == '1';
        GM.StartGame(isPlayer1AI, isPlayer2AI);

        mainMenuGO.SetActive(false);
    }

    public void OnClickExit()
    {
        Application.Quit();
    }
}
