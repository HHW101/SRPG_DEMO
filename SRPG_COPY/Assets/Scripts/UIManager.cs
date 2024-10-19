using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

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
    [Header("배틀씬 UI")]
    public GameObject battleScene;
    [SerializeField] private TMP_Text bsUnit_M;
    [SerializeField] private TMP_Text bsHP_M;
    [SerializeField] private TMP_Text bsskill_M;
    [SerializeField] private TMP_Text atk_M;
    [SerializeField] private Slider bshpS_M;

    [SerializeField] private TMP_Text bsUnit_P;
    [SerializeField] private TMP_Text bsHP_P;
    [SerializeField] private TMP_Text bsskill_P;
    [SerializeField] private TMP_Text atk_P;
    [SerializeField] private Slider bshpS_P;
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
        battleScene.SetActive(false);
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
        hpS.value = p.hp / p.maxhp;
        Bbtn.GetComponentInChildren<TMP_Text>().text = "Move";
        Bbtn.onClick.RemoveAllListeners();
        Bbtn.onClick.AddListener(GameManager.instance.startmove);
    }
    public void ShowBattleScene(UnitP m, UnitP p)
    {
        battleScene.SetActive(true);
        bsUnit_P.text = $"{p.name}";
        bshpS_P.value = p.hp / p.maxhp;
        bsHP_P.text = $"{p.hp}";
        bsUnit_M.text = $"{m.name}";
        bshpS_M.value = m.hp / m.maxhp;
        bsHP_M.text = $"{m.hp}";
    }
    public void getDamage(float f,float now,float max)
    {
        StartCoroutine(Damage(f, now,max));
    }
    IEnumerator Damage(float f, float now,float max)
    {

        while (f-now>=0.01f) {
            f -= 1f*Time.deltaTime;
            bshpS_M.value =  f/max;
            bsHP_M.text = $"{Mathf.Round(f)}";
            yield return null;
        }
        bshpS_M.value =  f/max;
        bsHP_M.text = $"{Mathf.Round(f)}";
    }
    public void HideBS()
    {
        battleScene.SetActive(false);
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
