using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;
using TMPro;

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

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
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
    }
    void Showturn()
    {
        switch (GameManager.instance.turn)
        {
            case GameManager.TurnState.start:
                turn.text = "시작";
               
                break;
            case GameManager.TurnState.playerTurn:
                turn.text = "플레이어 이동";
                break;
            case GameManager.TurnState.end:
                turn.text = "종료";
                break;
            case GameManager.TurnState.enemyTurn:
                turn.text = "적 턴";
                break;
        }
    }
    // Update is called once per frame
    void Update()
    {
        Showturn();
    }
}
