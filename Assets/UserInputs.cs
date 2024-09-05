using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UserInputs : MonoBehaviour
{
    public static UserInputs Instance;

    public Vector2 MoveInput { get;private set; }   

    public bool JumpInput { get; private set; }

    public bool AttackInput { get; private set; }
    public bool DownAttackInput { get; private set; }
    public bool UpAttackInput { get; private set; }
    public bool ChannelInput { get; private set; }
    public bool ThrowInput { get; private set; }
    public bool HeavyInput { get; private set; }
    public bool MenuInput { get; private set; }
    private PlayerInput _playerInput;
    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _attackAction;
    private InputAction _downattackAction;
    private InputAction _heavyattackAction;
    private InputAction _throwAction;
    private InputAction _upattackAction;
    private InputAction _channelAction;
    private InputAction _escapeAction;
    private void Awake()
    {
        if(Instance==null)
        {
            Instance = this;
        }
        _playerInput=GetComponent<PlayerInput>();
        SetInputActions();
    }
    private void SetInputActions()
    {
        _moveAction = _playerInput.actions["Move"];
        
        _jumpAction = _playerInput.actions["Jump"];
        _attackAction = _playerInput.actions["Attack"];
        _downattackAction = _playerInput.actions["DownAttack"];
        _upattackAction = _playerInput.actions["UpAttack"];
        _channelAction = _playerInput.actions["Channel"];
        _escapeAction = _playerInput.actions["Menu"];
        _heavyattackAction = _playerInput.actions["HeavyAttack"];
        _throwAction = _playerInput.actions["ThrowOrShoot"];

    }
    private void Update()
    {
       UpdateInputs();
    }
    private void UpdateInputs()
    {
        if(_moveAction.WasPressedThisFrame())
        {
            MoveInput = _moveAction.ReadValue<Vector2>();
        }
        
        AttackInput = _attackAction.WasPressedThisFrame();
        DownAttackInput=_downattackAction.WasPressedThisFrame();
        UpAttackInput=_upattackAction.WasPressedThisFrame();
        JumpInput=_jumpAction.WasPressedThisFrame();
        ChannelInput=_channelAction.WasPressedThisFrame();
        HeavyInput=_heavyattackAction.WasPressedThisFrame();
        ThrowInput=_throwAction.WasPressedThisFrame();
        MenuInput=_escapeAction.WasPressedThisFrame();
    }
}
