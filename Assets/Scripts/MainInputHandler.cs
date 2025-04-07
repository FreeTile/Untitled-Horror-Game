using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//Same as GameInputHandler class
//Used to handle opening UI such as menu, inventory and journal.
public class MainInputHandler : MonoBehaviour
{
    [SerializeField] private InputActionAsset playerControls;

    [SerializeField] private string actionMapName = "PlayerGame";
    private InputActionMap ActionMap;

    [SerializeField] private string inventory = "Inventory";
    [SerializeField] private string journal = "Journal";
    [SerializeField] private string menu = "Menu";

    private InputAction inventoryAction;
    private InputAction journalAction;
    private InputAction menuAction;


    public bool InventoryDown { get; private set; }
    public bool JournalDown { get; private set; }
    public bool MenuDown { get; private set; }
    public static MainInputHandler Instance { get; private set; }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        ActionMap = playerControls.FindActionMap(actionMapName);

        inventoryAction = ActionMap.FindAction(inventory);
        journalAction = ActionMap.FindAction(journal);
        menuAction = ActionMap.FindAction(menu);
    }

    void RegisterInputActions()
    {
        inventoryAction.performed += OnInventoryPerformed;
        journalAction.performed += OnJournalPerformed;
        menuAction.performed += OnMenuPerformed;
    }
    private void OnInventoryPerformed(InputAction.CallbackContext context)
    {
        InventoryDown = true;
        GameManager.Instance.OpenInventory();
    }
    private void OnJournalPerformed(InputAction.CallbackContext context)
    {
        JournalDown = true;
        GameManager.Instance.OpenJournal();
    }
    private void OnMenuPerformed(InputAction.CallbackContext context)
    {
        MenuDown = true;
        GameManager.Instance.ProceedEsc();
    }

    private void LateUpdate()
    {
        MenuDown = false;
        JournalDown = false;
        InventoryDown = false;
    }

    private void OnEnable()
    {
        RegisterInputActions();
        inventoryAction.Enable();
        journalAction.Enable();
        menuAction.Enable();
    }

    private void OnDisable()
    {
        UnregisterInputActions();
        inventoryAction.Disable();
        journalAction.Disable();
        menuAction.Disable();
    }

    void UnregisterInputActions()
    {
        inventoryAction.performed -= OnInventoryPerformed;
        journalAction.performed -= OnJournalPerformed;
        menuAction.performed -= OnMenuPerformed;
    }
}
