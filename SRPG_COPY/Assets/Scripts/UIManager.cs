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
    public static UIManager instance;

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
    }
    public void ShowTile(Tile t)
    {
        tile.enabled = true;
        tile.text = $"[{t.getX()},{t.getY()}]\n비용:{t.moveCost}";
    }
    void Showturn()
    {
        switch (TurnManager.instance.turn)
        {
            case TurnManager.TurnState.start:
                turn.text = "시작";
                break;
            case TurnManager.TurnState.pAttackTurn:
                turn.text = "플레이어 공격";
                tile.enabled=false;
                break;
            case TurnManager.TurnState.pMoveTurn:
                turn.text = "플레이어 이동";
                break;
            case TurnManager.TurnState.end:
                turn.text = "종료";
                break;
            case TurnManager.TurnState.enemyTurn:
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
