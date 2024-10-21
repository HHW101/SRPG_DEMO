using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static MapControl;
public class InputManager : MonoBehaviour
{
    private MapControl _controls;  // PlayerControls 객체 선언
    InputAction moveaction;
    InputAction clickaction;
    InputAction cancelaction;
    // Start is called before the first frame update
    InputManager _inputManager;
    Vector2 inputM;

  
    private void Awake()
    {
        
        _controls = new MapControl();
        moveaction = _controls.SelectTile.Move;
        clickaction = _controls.SelectTile.Click;
        cancelaction = _controls.SelectTile.Cancel;
       
    }

    void Start()
    {
        
    }
    void OnDisable()
    {
        moveaction.Disable();
        clickaction.Disable();
    }
    void OnEnable()
    {
        moveaction.Enable();
        moveaction.performed += OnMove;
        clickaction.Enable();
        clickaction.started += OnClick;
        cancelaction.Enable();
        cancelaction.started += OnCancel;
       // selectaction.performed += OnCancel;
        //selectaction.performed += OnOpenMenu;
    }
    private void FixedUpdate()
    {
        
    }
    public void selectTIleON()
    {
        
    }
    
    public void OnMove(InputAction.CallbackContext context)
    {
        inputM = context.ReadValue<Vector2>();
        switch (GameManager.instance.inputmode)
        {
            case GameManager.InputMode.Player:
                if (inputM != null)
                {
                  
                    GameManager.instance.ShiftSelect((int)inputM.x, (int)inputM.y,1);
                }
                break;
            case GameManager.InputMode.Map:
                if (inputM != null)
                {
               
                    GameManager.instance.ShiftSelect((int)inputM.x, (int)inputM.y,0);
                }
                break;
            case GameManager.InputMode.block:
                break;
        }
    
        
    }
  
    public void OnClick(InputAction.CallbackContext context)
    {
       // GameManager.instance.ClickTile();

        switch (GameManager.instance.inputmode)
        {
            case GameManager.InputMode.Player:
             if (context.started) //처음 눌린 순만. performed- :계속 canceled 떨어졌을 때
             {
                    //ChangeInputMode(InputMode.Map);
                    GameManager.instance.getClick();
              }
               break;
            case GameManager.InputMode.Map:
                if (context.started)
                {
                    GameManager.instance.SelectPlayer();
                    UnityEngine.Debug.Log("클릭");
                }
                break;
            case GameManager.InputMode.block:

                break;

        }
    }
    public void OnCancel(InputAction.CallbackContext context) {
        if (context.started) //처음 눌린 순만. performed- :계속 canceled 떨어졌을 때
        {
            UnityEngine.Debug.Log("취소");
            //ChangeInputMode(InputMode.Map);
            GameManager.instance.getCancel();
        }
       
    }
    public void OnOpenMenu(InputAction.CallbackContext context)
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) // 스페이스바를 눌렀을 때
        {
            Time.timeScale = (Time.timeScale == 1.0f) ? 2.0f : 1.0f; // 속도 토글
        }
    }
}
