using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;

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



    public EventReference pauseEnterSound;
    public EventReference journalEnterSound;
    private EventInstance pauseSound;
    private EventInstance journalSound;

    public SanityManager sanityManager;

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

    public void PauseGameplay()
    {
        if (state == State.Game)
        {
            state = State.Esc;
            switchControlSystem();
        }
    }

    public void ResumeGameplay()
    {
        if (state == State.Esc)
        {
            state = State.Game;
            switchControlSystem();
        }
    }

    public void GameOver()
    {
        Time.timeScale = 0f;
        Debug.Log("Game Over");
    }

    public void ProceedEsc()
    {
        Debug.Log("Proceeding Esc");
        if (state == State.Game)
        {
            state = State.Esc;
            pauseSound.start();
            SanityManager.ambientInstance.setVolume(0.2f);
            pauseMenuUI.SetActive(true);
            hudUI.SetActive(false); // Hide HUD
        }
        else
        {
            pauseSound.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            state = State.Game;
            SanityManager.ambientInstance.setVolume(1f);
            pauseMenuUI.SetActive(false);
            settingsMenuUI.SetActive(false);
            hudUI.SetActive(true); // Show HUD again
        }
        switchControlSystem();
    }


    public void OpenInventory()
    {
        if (state == State.Inventory)
        {
            SanityManager.ambientInstance.setVolume(1f);
            state = State.Game;            
            InventoryUI.SetActive(false);

        }
        else if (state != State.Esc)
        {
            SanityManager.ambientInstance.setVolume(0.2f);
            state = State.Inventory;
            InventoryUI.SetActive(true);
        }
        switchControlSystem();
    }

    public void OpenJournal()
    {
        if (state == State.Journal)
        {
            SanityManager.ambientInstance.setVolume(1f);
            state = State.Game;
        }
        else if (state != State.Esc)
        {
            state = State.Journal;
            //Open the journal
            SanityManager.ambientInstance.setVolume(0.2f);
            RuntimeManager.PlayOneShot(journalEnterSound);
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
            cam.transform.localPosition = originalPos + Random.insideUnitSphere * magnitude;
            elapsed += Time.deltaTime;
            yield return null;
        }

        cam.transform.localPosition = originalPos;
    }
}
