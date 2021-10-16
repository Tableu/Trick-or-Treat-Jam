using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private PlayerInputActions _playerInputActions;
    private Vector2 _mouseScreenPosition;
    private Vector2 _mouseWorldPosition;
    public bool LockInput { get; set; }

    private void Awake()
    {
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.UI.Click.started += OnClick;
        LockInput = false;
    }

    private void OnEnable()
    {
        _playerInputActions.Enable();
    }

    private void OnDisable()
    {
        _playerInputActions.Disable();
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    // Update is called once per frame
    void Update()
    {
        _mouseScreenPosition = _playerInputActions.UI.Point.ReadValue<Vector2>();
        _mouseWorldPosition = Camera.main.ScreenToWorldPoint(_mouseScreenPosition);
    }
    
    private void OnClick(InputAction.CallbackContext context)
    {
        RaycastHit2D hit = Physics2D.Raycast(_mouseWorldPosition, Vector2.zero);
        if (hit.collider != null)
        {
            var layer = hit.collider.gameObject.layer;
            
            if (layer.Equals(LayerMask.NameToLayer("Objects")))
            {
                if(!LockInput)
                    ClickedOnObject();
            }else if (layer.Equals(LayerMask.NameToLayer("Exits")))
            {
                if(!LockInput)
                    ClickedOnExit();
            }else if (layer.Equals(LayerMask.NameToLayer("UI")))
            {
                if(hit.collider.CompareTag("UI"))
                    ClickedOnTextBox();
            }
        }
    }

    private void ClickedOnObject()
    {
        Debug.Log("object");
    }

    private void ClickedOnExit()
    {
        
    }
    private void ClickedOnTextBox()
    {
        
    }
}
