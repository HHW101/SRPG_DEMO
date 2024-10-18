using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private TMP_Text turn;
    [SerializeField] private TMP_Text tile;
    [SerializeField] private TMP_Text moveC;
    public static UIManager instance;
    public GameObject battleMenu;
    [SerializeField] private TMP_Text MUnit;
    [SerializeField] private TMP_Text MHP;
    [SerializeField] private Button Bbtn;
    [SerializeField] private Slider hpS;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }
    public void SelectSkill()
    {

    }
    void Start()
    {
        tile.enabled = false;
        battleMenu.SetActive(false);
    }
    public void ShowTile(Tile t)
    {
      
        tile.enabled = true;
        tile.text = $"[{t.getX()},{t.getY()}]\n비용:{t.moveCost}";
    }
    
    public void ShowC(UnitP p)
    {
        //switch (TurnManager.instance.turn)
        //{
        //    case TurnManager.TurnState.pMoveTurn:
        //        break;
        //}
    }
    
    public void ShowBMenu(UnitP p) {
        battleMenu.SetActive(true);
        MHP.text = $"{p.hp}";
        hpS.value = p.maxhp / p.hp;
        Bbtn.GetComponentInChildren<TMP_Text>().text = "Move";
        Bbtn.onClick.RemoveAllListeners();
        Bbtn.onClick.AddListener(GameManager.instance.startmove);
    }
    public void HideBMenu()
    {
        battleMenu.SetActive(false);
    }
    public void ShowBAMenu(UnitP p)
    {
        battleMenu.SetActive(true);
        MHP.text = $"{p.hp}";
        hpS.value = p.maxhp / p.hp;
        Bbtn.GetComponentInChildren<TMP_Text>().text = "Attack";
        Bbtn.onClick.RemoveAllListeners();
        Bbtn.onClick.AddListener(GameManager.instance.Attackstart);
    }
    void Showturn()
    {
        switch (GameManager.instance.turn)
        {
            case GameManager.TurnState.start:
                turn.text = $"{GameManager.instance.turnN}: 시작";
               
                break;
            case GameManager.TurnState.playerTurn:
                turn.text = $"{GameManager.instance.turnN}:플레이어 턴";
                break;
            case GameManager.TurnState.end:
                turn.text = "종료";
                break;
            case GameManager.TurnState.enemyTurn:
                turn.text = $"{GameManager.instance.turnN}: 적 턴";
                break;
        }
    }
    // Update is called once per frame
    void Update()
    {
        Showturn();
    }
}
