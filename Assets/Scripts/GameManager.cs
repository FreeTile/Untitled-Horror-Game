using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    HealthManager healthManager;
    SanityManager sanityManager;
    GameInputHandler inputHandler;

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
        if (Instance == null)
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
