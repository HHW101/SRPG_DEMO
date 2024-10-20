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

    public static UIManager instance;
    public GameObject InfoUI;
    [SerializeField] private TMP_Text MUnit;
    [SerializeField] private TMP_Text MHP;
    [SerializeField] private TMP_Text atkT;
    [SerializeField] private TMP_Text moveT;
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
    public GameObject camP;
    public GameObject camM;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        camP.SetActive(false);
        camM.SetActive(false);
    }
    public void SelectSkill()
    {

    }
    void Start()
    {
        tile.enabled = false;
        InfoUI.SetActive(false);
        battleScene.SetActive(false);
    }
    public void ShowTile(Tile t)
    {
      
        tile.enabled = true;
        tile.text = $"[{t.getX()},{t.getY()}]\n비용:{t.moveCost}";
    }
    
    public void ShowC(FUnit p)
    {
        //switch (TurnManager.instance.turn)
        //{
        //    case TurnManager.TurnState.pMoveTurn:
        //        break;
        //}
    }
    
    public void ShowInfoMenu(FUnit p) {
        InfoUI.SetActive(true);
        MHP.text = $"{p.hp}";
        hpS.value = p.hp / p.maxhp;
        atkT.text = $"공격기회:{p.atkC}";
        moveT.text = $"공격기회:{p.moveC}";
        Bbtn.GetComponentInChildren<TMP_Text>().text = "Move";
        Bbtn.onClick.RemoveAllListeners();
     //   Bbtn.onClick.AddListener(GameManager.instance.startmove);
    }
    public void ShowMonster(FUnit p)
    {
        InfoUI.SetActive(true);
        MHP.text = $"{p.hp}";
        hpS.value = p.hp / p.maxhp;
        Bbtn.GetComponentInChildren<TMP_Text>().text = "상세";
        Bbtn.onClick.RemoveAllListeners();
    }
    public void HideInfo()
    {
        InfoUI.SetActive(false);
        tile.enabled = false;
    }
    public void ShowBattleScene(FUnit m, FUnit p)
    {   
         camP.SetActive(true) ;
        camM.SetActive(true) ;
        Vector3 temp = new Vector3(0f,5f,-4f);
        camP.transform.position=p.transform.position+temp;
        camP.transform.rotation=Quaternion.Euler(new Vector3(-p.transform.rotation.x+10,p.transform.rotation.y,p.transform.rotation.z+30));
        camM.transform.position=m.transform.position+temp;
        camM.transform.rotation = Quaternion.Euler(new Vector3(-m.transform.rotation.x-10, m.transform.rotation.y, m.transform.rotation.z+30));
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
        camP.SetActive(false);
        camM.SetActive(false);
    }
    public void HideBMenu()
    {
        InfoUI.SetActive(false);
    }
    public void ShowBAMenu(FUnit p)
    {
        InfoUI.SetActive(true);
        MHP.text = $"{p.hp}";
        hpS.value = p.maxhp / p.hp;
        atkT.text = $"공격기회:{p.atkC}";
        moveT.text = $"공격기회:{p.moveC}";
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
