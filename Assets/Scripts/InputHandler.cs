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
    [SerializeField] private string inventory = "Inventory";
    [SerializeField] private string sprint = "Sprint";
    [SerializeField] private string journal = "Journal";

    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction flashlightAction;
    private InputAction interactAction;
    private InputAction inventoryAction;    
    private InputAction sprintAction;
    private InputAction journalAction;

    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public bool FlashlightInput { get; private set; }
    public bool InteractInput { get; private set; }
    public bool InventoryInput { get; private set; }
    public bool SprintInput { get; private set; }
    public bool JournalInput { get; private set; }

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
        inventoryAction = playerControls.FindActionMap(actionMapName).FindAction(inventory);
        sprintAction = playerControls.FindActionMap(actionMapName).FindAction(sprint);
        journalAction = playerControls.FindActionMap(actionMapName).FindAction(journal);

        RegisterInputActions();
    }

    void RegisterInputActions()
    {
        moveAction.performed += context => MoveInput = context.ReadValue<Vector2>();
        lookAction.performed += context => LookInput = context.ReadValue<Vector2>();

        moveAction.canceled += context => MoveInput = Vector2.zero;
        lookAction.canceled += context => LookInput = Vector2.zero;

        flashlightAction.performed += context => FlashlightInput = true;
        interactAction.performed += context => InteractInput = true;
        inventoryAction.performed += context => InventoryInput = true;
        sprintAction.performed += context => SprintInput = true;
        journalAction.performed += context => JournalInput = true;

        flashlightAction.canceled += context => FlashlightInput = false;
        interactAction.canceled += context => InteractInput = false;
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
        inventoryAction.Enable();
        sprintAction.Enable();
        journalAction.Enable();

    }

    private void OnDisable()
    {
        moveAction.Disable(); 
        lookAction.Disable();
        flashlightAction.Disable();
        interactAction.Disable();
        inventoryAction.Disable();
        sprintAction.Disable();
        journalAction.Disable();
    }

}
