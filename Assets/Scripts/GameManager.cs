using System.Collections;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System;
using NoteSystem;

[RequireComponent(typeof(GameInputHandler))]
[RequireComponent(typeof(MainInputHandler))]
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    GameInputHandler inputHandler;
    [SerializeField] private GameObject InventoryUI;
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject settingsMenuUI;
    [SerializeField] private GameObject hudUI;
    [SerializeField] private DeathScreen deathScreen;
    [SerializeField] private GameObject backScreen;


    public EventReference pauseEnterSound;
    public EventReference journalEnterSound;
    private EventInstance pauseSound;
    private EventInstance journalSound;

    public SanityManager sanityManager;

    //Game states affect player's controls
    public enum State
    {
        Esc,
        Settings,
        Game,
        Inventory,
        Journal,
        Note,
        GameOver
    }

    public State state;

    void Start()
    {
        if (Instance == null) //Creating singleton instance at the beginning 
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);

        }
        else
        {
            Destroy(gameObject);
        }
        pauseSound = RuntimeManager.CreateInstance(pauseEnterSound);
        journalSound = RuntimeManager.CreateInstance(journalEnterSound);

        inputHandler = GetComponent<GameInputHandler>();

        GameStart();


    }

    //Switching controls between UI and main game control systems
    public void switchControlSystem()
    {
        
        if (state != State.Game)
        {
            PauseGameplay();
            inputHandler.enabled = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            ResumeGameplay();
            inputHandler.enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    private void GameStart()
    {
        state = State.Game;
        switchControlSystem();
        Debug.Log("Started Game");
    }

    public void PauseGameplay()
    {
       Time.timeScale = 0f;
    }

    public void ResumeGameplay()
    {
        Time.timeScale = 1f;
    }

    public void GameOver()
    {
        state = State.GameOver;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        hudUI.SetActive(false);
        MainInputHandler.Instance.enabled = false;
        GameInputHandler.Instance.enabled = false;
        deathScreen.gameObject.SetActive(true);
        deathScreen.Death();
    }

    public void ProceedEsc()
    {
        Debug.Log("Proceeding Esc");
        switch (state)
        {
            case State.Game:
                backScreen.SetActive(true);
                state = State.Esc;
                pauseSound.start();
                sanityManager.SetMusicVolume(0.2f);
                pauseMenuUI.SetActive(true);
                hudUI.SetActive(false);
                switchControlSystem();
                break;

            case State.Esc:
                pauseSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                state = State.Game;
                sanityManager.SetMusicVolume(1f);
                pauseMenuUI.SetActive(false);
                settingsMenuUI.SetActive(false);
                hudUI.SetActive(true); // Show HUD again
                backScreen.SetActive(false);
                switchControlSystem();
                break;

            case State.Settings:
                pauseMenuUI.SetActive(true);
                settingsMenuUI.SetActive(false);
                state = State.Esc;
                break;

            case State.Inventory:
                InventoryUI.SetActive(false);
                backScreen.SetActive(false);
                state = State.Game;
                sanityManager.SetMusicVolume(1f);
                hudUI.SetActive(true);
                switchControlSystem();
                break;

            case State.Journal:
                NoteUIManager.instance.CloseInventory();
                sanityManager.SetMusicVolume(1f);
                state = State.Game;
                switchControlSystem();
                break;
        }
    }


    public void OpenInventory()
    {
        if (state == State.Inventory)
        {
            sanityManager.SetMusicVolume(1f);
            state = State.Game;
            InventoryUI.SetActive(false);
            backScreen.SetActive(false);
            hudUI.SetActive(true);
        }
        else if (state != State.Esc && state != State.Settings)
        {
            sanityManager.SetMusicVolume(0.2f);
            state = State.Inventory;
            NoteUIManager.instance.CloseInventory();
            InventoryUI.SetActive(true);
            backScreen.SetActive(true);
            hudUI.SetActive(false);
        }
        switchControlSystem();
    }

    public void OpenJournal()
    {
        if (state == State.Journal)
        {
            sanityManager.SetMusicVolume(1f);
            state = State.Game;
            hudUI.SetActive(true);
        }
        else if (state != State.Esc && state != State.Settings)
        {
            state = State.Journal;
            sanityManager.SetMusicVolume(0.2f);
            RuntimeManager.PlayOneShot(journalEnterSound);
            InventoryUI.SetActive(false);
            backScreen.SetActive(false);
            hudUI.SetActive(false);
        }
        switchControlSystem();
    }

    //Jumpscare Trigger
    public void TriggerJumpScare()
    {
        StartCoroutine(JumpScareRoutine());
    }

    private IEnumerator JumpScareRoutine()
    {
        //Sound
        RuntimeManager.PlayOneShot("event:/JumpScare");

        Camera cam = Camera.main;
        if (cam == null)
        {
            Debug.LogWarning("Main Camera not found for JumpScare!");
            yield break;
        }

        Vector3 originalPos = cam.transform.localPosition;
        float elapsed = 0f;
        float duration = 0.5f;
        float magnitude = 0.2f;

        //Camera shake
        while (elapsed < duration)
        {
            cam.transform.localPosition = originalPos + UnityEngine.Random.insideUnitSphere * magnitude;
            elapsed += Time.deltaTime;
            yield return null;
        }

        cam.transform.localPosition = originalPos;

    }

    public void ChangeState(string newState)
    {
        if (Enum.TryParse(newState, true, out State parsedState))
        {
            state = parsedState;
            switchControlSystem();
        }
    }
   

}
