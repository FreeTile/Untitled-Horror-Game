using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameInputHandler : MonoBehaviour
{
    [SerializeField] private InputActionAsset playerControls;
    
    private static bool isSubscribed = false;

    public bool isCrouching;
    //Names for action map and actions from the action asset
    [SerializeField] private string actionMapName = "PlayerGame";
    private InputActionMap ActionMap;

    [SerializeField] private string movement = "Movement";
    [SerializeField] private string look = "Look";
    [SerializeField] private string flashlight = "Flashlight";
    [SerializeField] private string interact = "Interact";
    [SerializeField] private string throwAct = "Throw";
    [SerializeField] private string sprint = "Sprint";
    [SerializeField] private string lightangle = "LightAngle";
    [SerializeField] private string crouch = "Crouch";

    //Variables for actions from action asset
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction flashlightAction;
    private InputAction interactAction;
    private InputAction throwAction;
    private InputAction sprintAction;
    private InputAction lightangleAction;
    private InputAction crouchAction;

    //Setting up variables that are available in other scripts
    public Vector2 MoveInput { get; private set; }
    public Vector2 LookInput { get; private set; }
    public float WheelInput { get; private set; }
    public bool FlashlightDown { get; private set; }
    public bool SprintInput { get; private set; }
    public bool InteractDown { get; private set; }
    public bool InteractHold { get; private set; }
    public bool ThrowDown { get; private set; }
    public bool CrouchDown { get; private set; }
    public static GameInputHandler Instance { get; private set; }


    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        ActionMap = playerControls.FindActionMap(actionMapName);

        //Setting up actions
        moveAction = ActionMap.FindAction(movement);
        lookAction = ActionMap.FindAction(look);
        flashlightAction = ActionMap.FindAction(flashlight);
        interactAction = ActionMap.FindAction(interact);
        throwAction = ActionMap.FindAction(throwAct);
        sprintAction = ActionMap.FindAction(sprint);
        lightangleAction = ActionMap.FindAction(lightangle);
        crouchAction = ActionMap.FindAction(crouch);
        RegisterInputActions();
    }

    //Function for changing public variables depends on pressed keys
    //.performed when key pressed, .canceled when key released
    void RegisterInputActions()
    {
        if (isSubscribed) return;
        moveAction.performed += context => MoveInput = context.ReadValue<Vector2>();
        lookAction.performed += context => LookInput = context.ReadValue<Vector2>();

        moveAction.canceled += context => MoveInput = Vector2.zero;
        lookAction.canceled += context => LookInput = Vector2.zero;

        lightangleAction.performed += context => WheelInput = context.ReadValue<float>();

        lightangleAction.canceled += context => WheelInput = 0f;

        flashlightAction.performed += context => FlashlightDown = true;
        crouchAction.performed += context => CrouchDown = true;
        sprintAction.performed += context => SprintInput = true;
        throwAction.performed += context => ThrowDown = true;
        interactAction.performed += context => {
            InteractDown = true;
            InteractHold = true;
        };

        interactAction.canceled += context => InteractHold = false;
        sprintAction.canceled += context => SprintInput = false;

        isSubscribed = true;
    }
    private void LateUpdate()
    {
        InteractDown = false;
        ThrowDown = false;
        FlashlightDown = false;
        CrouchDown = false;
    }

    // IMPORTANT 
    //Enables input reading upon enabling component
    private void OnEnable()
    {
        Debug.Log("Enabled");
        moveAction.Enable();
        lookAction.Enable();    
        flashlightAction.Enable();        
        interactAction.Enable();
        throwAction.Enable();
        sprintAction.Enable();
        lightangleAction.Enable();
        crouchAction.Enable();
    }

    // IMPORTANT 
    //Disables input reading upon enabling component
    private void OnDisable()
    {
        Debug.Log("Disabled");

        moveAction.Disable(); 
        lookAction.Disable();
        flashlightAction.Disable();
        interactAction.Disable();
        throwAction.Disable();
        sprintAction.Disable();
        lightangleAction.Disable();
        crouchAction.Disable();
    }
    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

}
