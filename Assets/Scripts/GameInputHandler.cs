using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInputHandler : MonoBehaviour
{
    [SerializeField] private InputActionAsset playerControls;

    [SerializeField] private string actionMapName = "PlayerGame";

    [SerializeField] private string movement = "Movement";
    [SerializeField] private string look = "Look";
    [SerializeField] private string flashlight = "Flashlight";
    [SerializeField] private string interact = "Interact";
    [SerializeField] private string throwAct = "Throw";
    [SerializeField] private string sprint = "Sprint";
    [SerializeField] private string lightangle = "LightAngle";

    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction flashlightAction;
    private InputAction interactAction;
    private InputAction throwAction;
    private InputAction sprintAction;
    private InputAction lightangleAction;

    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public float WheelInput { get; private set; }
    public bool FlashlightDown { get; private set; }
    public bool SprintInput { get; private set; }
    public bool InteractDown { get; private set; }
    public bool InteractHold { get; private set; }
    public bool ThrowDown { get; private set; }
    public static GameInputHandler Instance { get; private set; }


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        moveAction = playerControls.FindActionMap(actionMapName).FindAction(movement);
        lookAction = playerControls.FindActionMap(actionMapName).FindAction(look);
        flashlightAction = playerControls.FindActionMap(actionMapName).FindAction(flashlight);
        interactAction = playerControls.FindActionMap(actionMapName).FindAction(interact);
        throwAction = playerControls.FindActionMap(actionMapName).FindAction(throwAct);
        sprintAction = playerControls.FindActionMap(actionMapName).FindAction(sprint);
        lightangleAction = playerControls.FindActionMap(actionMapName).FindAction(lightangle);
        RegisterInputActions();
    }

    void RegisterInputActions()
    {
        moveAction.performed += context => MoveInput = context.ReadValue<Vector2>();
        lookAction.performed += context => LookInput = context.ReadValue<Vector2>();

        moveAction.canceled += context => MoveInput = Vector2.zero;
        lookAction.canceled += context => LookInput = Vector2.zero;

        lightangleAction.performed += context => WheelInput = context.ReadValue<float>();

        lightangleAction.canceled += context => WheelInput = 0f;

        flashlightAction.performed += context => FlashlightDown = true;
        sprintAction.performed += context => SprintInput = true;
        throwAction.performed += context => ThrowDown = true;
        interactAction.performed += context => {
            InteractDown = true;
            InteractHold = true;
        };

        interactAction.canceled += context => InteractHold = false;
        sprintAction.canceled += context => SprintInput = false;
    }
    private void LateUpdate()
    {
        InteractDown = false;
        ThrowDown = false;
        FlashlightDown = false;

    }

    private void OnEnable()
    {
        moveAction.Enable();
        lookAction.Enable();    
        flashlightAction.Enable();        
        interactAction.Enable();
        throwAction.Enable();
        sprintAction.Enable();
        lightangleAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable(); 
        lookAction.Disable();
        flashlightAction.Disable();
        interactAction.Disable();
        throwAction.Disable();
        sprintAction.Disable();
        lightangleAction.Disable();
    }
}
