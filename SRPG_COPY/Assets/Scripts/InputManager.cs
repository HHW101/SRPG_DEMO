using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using static MapControl;
public class InputManager : MonoBehaviour
{
    private MapControl _controls;  // PlayerControls 객체 선언
    InputAction selectaction;
    // Start is called before the first frame update
    InputManager _inputManager;
    Vector2 inputM;
    private void Awake()
    {
        _controls = new MapControl();
        selectaction = _controls.SelectTile.Move;
    }
    void Start()
    {
        
    }
    void OnDisable()
    {
        selectaction.Disable();
    }
    void OnEnable()
    {
        selectaction.Enable();
        selectaction.performed += OnMove;
        selectaction.performed += OnClick;
        selectaction.performed += OnCancel;
        selectaction.performed += OnOpenMenu;
    }
    private void FixedUpdate()
    {
        
    }
    public void selectTIleON()
    {
        
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        inputM =context.ReadValue<Vector2>();
        if (inputM != null) {
            Debug.Log($"{inputM.x}:{inputM.y}");
            GameManager.instance.ShiftSelect((int)inputM.x,(int)inputM.y);
        }
    }
    public void OnClick(InputAction.CallbackContext context)
    {

    }
    public void OnCancel(InputAction.CallbackContext context) { 
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
