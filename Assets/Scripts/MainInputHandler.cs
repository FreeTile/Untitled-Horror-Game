using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainInputHandler : MonoBehaviour
{
    [SerializeField] private InputActionAsset playerControls;

    [SerializeField] private string actionMapName = "PlayerGame";


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

        inventoryAction = playerControls.FindActionMap(actionMapName).FindAction(inventory);
        journalAction = playerControls.FindActionMap(actionMapName).FindAction(journal);
        menuAction = playerControls.FindActionMap(actionMapName).FindAction(menu);
        RegisterInputActions();
    }

    void RegisterInputActions()
    {
        inventoryAction.performed += context =>
        {
            InventoryDown = true;
            GameManager.Instance.OpenInventory();
        };
        journalAction.performed += context =>
        {
            JournalDown = true;
            GameManager.Instance.OpenJournal();
        };
        menuAction.performed += context =>
        {
            MenuDown = true;
            GameManager.Instance.ProceedEsc();
        };
    }
    private void LateUpdate()
    {
        MenuDown = false;
        JournalDown = false;
        InventoryDown = false;
    }

    private void OnEnable()
    {
        inventoryAction.Enable();
        journalAction.Enable();
        menuAction.Enable();
    }

    private void OnDisable()
    {
        inventoryAction.Disable();
        journalAction.Disable();
        menuAction.Disable();
    }
}
