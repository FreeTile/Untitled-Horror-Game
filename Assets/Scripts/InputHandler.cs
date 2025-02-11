using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [SerializeField] private InputActionAsset playerControls;

    [SerializeField] private string actionMapName = "PlayerGame";

    [SerializeField] private string movement = "Movement";
    [SerializeField] private string look = "Look";
    [SerializeField] private string flashlight = "Flashlight";
    [SerializeField] private string interact = "Interact";
    [SerializeField] private string throwAct = "Throw";
    [SerializeField] private string inventory = "Inventory";
    [SerializeField] private string sprint = "Sprint";
    [SerializeField] private string journal = "Journal";
    [SerializeField] private string lightangle = "LightAngle";

    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction flashlightAction;
    private InputAction interactAction;
    private InputAction throwAction;
    private InputAction inventoryAction;    
    private InputAction sprintAction;
    private InputAction journalAction;
    private InputAction lightangleAction;

    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public float WheelInput { get; private set; }
    public bool FlashlightInput { get; private set; }
    public bool InventoryInput { get; private set; }
    public bool SprintInput { get; private set; }
    public bool JournalInput { get; private set; }
    public bool InteractDown { get; private set; }
    public bool InteractHold { get; private set; }
    public bool ThrowDown { get; private set; }
    public static InputHandler Instance { get; private set; }


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
        inventoryAction = playerControls.FindActionMap(actionMapName).FindAction(inventory);
        sprintAction = playerControls.FindActionMap(actionMapName).FindAction(sprint);
        journalAction = playerControls.FindActionMap(actionMapName).FindAction(journal);
        lightangleAction = playerControls.FindActionMap(actionMapName).FindAction(lightangle);
        RegisterInputActions();
    }

    void RegisterInputActions()
    {
        moveAction.performed += context => MoveInput = context.ReadValue<Vector2>();
        lookAction.performed += context => LookInput = context.ReadValue<Vector2>();
        lightangleAction.performed += context => WheelInput = context.ReadValue<float>();

        moveAction.canceled += context => MoveInput = Vector2.zero;
        lookAction.canceled += context => LookInput = Vector2.zero;
        lightangleAction.canceled += context => WheelInput = 0f;

        flashlightAction.performed += context => FlashlightInput = true;
        inventoryAction.performed += context => InventoryInput = true;
        sprintAction.performed += context => SprintInput = true;
        journalAction.performed += context => JournalInput = true;
        interactAction.performed += context => {
            InteractDown = true;
            InteractHold = true;
        };
        throwAction.performed += context => ThrowDown = true;

        interactAction.canceled += context => InteractHold = false;
        inventoryAction.canceled += context => InventoryInput = false;
        sprintAction.canceled += context => SprintInput = false;
        journalAction.canceled += context => JournalInput = false;

    }


    private void OnEnable()
    {
        moveAction.Enable();
        lookAction.Enable();    
        flashlightAction.Enable();        
        interactAction.Enable();
        throwAction.Enable();
        inventoryAction.Enable();
        sprintAction.Enable();
        journalAction.Enable();
        lightangleAction.Enable();
    }

    private void OnDisable()
    {
        moveAction.Disable(); 
        lookAction.Disable();
        flashlightAction.Disable();
        interactAction.Disable();
        throwAction.Disable();
        inventoryAction.Disable();
        sprintAction.Disable();
        journalAction.Disable();
        lightangleAction.Disable();
    }

    private void LateUpdate()
    {
        InteractDown = false;
        ThrowDown = false;
        FlashlightInput = false;
    }

}
