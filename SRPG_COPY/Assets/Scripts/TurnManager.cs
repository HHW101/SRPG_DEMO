using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    
    public enum TurnState
    {
        start, pMoveTurn,pAttackTurn, enemyTurn,end
    }
    public TurnState turn;
    public bool isProcessing = false; 
    public static TurnManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        turn = TurnState.start;

    }
    public void TurnChange(TurnState state)
    {
        turn = state;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
            TurnChange(TurnState.pMoveTurn);
        if (Input.GetKeyDown(KeyCode.X))
            TurnChange(TurnState.enemyTurn);
        if (Input.GetKeyDown(KeyCode.O))
            TurnChange(TurnState.pAttackTurn);
    }
}
