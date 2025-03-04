using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthManager))]
[RequireComponent(typeof(SanityManager))]
[RequireComponent(typeof(GameInputHandler))]
[RequireComponent(typeof(MainInputHandler))]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    HealthManager healthManager;
    SanityManager sanityManager;
    GameInputHandler inputHandler;

    //Game states affect player's controls
    public enum State 
    {
        Esc,
        Game,
        Inventory,
        Journal
    }

    public State state;

    void Start()
    {
        if (Instance == null) //Creating syngleton instance at the beginning 
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        healthManager = GetComponent<HealthManager>();
        sanityManager = GetComponent<SanityManager>();
        inputHandler = GetComponent<GameInputHandler>();

        GameStart();
    }

    //Switching controls between UI and main game control systems
    public void switchControlSystem()
    {
        if (state != State.Game)
        {
            inputHandler.enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            inputHandler.enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void GameStart()
    {
        state = State.Game;
        switchControlSystem();
    }

    public void GameOver()
    {
        //Game over code here
        //Animation + end screen
    }


    public void ProceedEsc()
    {
        Debug.Log("Proceeding Esc");
        if(state == State.Game)
        {
            state = State.Esc;
            //Turn on Menu on canvas
        }
        else
        {
            state = State.Game;
        }
        switchControlSystem();
    }

    public void OpenInventory()
    {
        if (state == State.Inventory)
        {
            state = State.Game;
        }
        else if(state != State.Esc)
        {
            state = State.Inventory;
            //Turn on Inventory on canvas
        }
        switchControlSystem();
    }

    public void OpenJournal()
    {
        if (state == State.Journal)
        {
            state = State.Game;
        }
        else if (state != State.Esc)
        {
            state = State.Journal;
            //Open the journal
        }
        switchControlSystem();
    }
}
