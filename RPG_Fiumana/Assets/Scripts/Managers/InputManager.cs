using System;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    #region SINGLETON
    private static InputManager _instance;
    public static InputManager Instance
    {
        get
        {
            if(_instance)
                return _instance;

            _instance = GameObject.FindFirstObjectByType<InputManager>();
            return _instance;
        }
    }
    #endregion

    public PlayerInputs playerControls;
    public InputAction leftClick;

    private void Awake()
    {
        playerControls = new PlayerInputs();
    }

    private void OnEnable()
    {
        playerControls.Enable();
        foreach(var action in playerControls.asset.actionMaps)
        {
            action.Enable();
        }

        leftClick = playerControls.GameplayInput.LeftButtonClick;
    }

    private void OnDisable()
    {
        playerControls.Disable();
        foreach(InputActionMap action in playerControls.asset.actionMaps)
        {
            action.Disable();
        }
    }
}
