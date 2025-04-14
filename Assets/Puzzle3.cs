using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using UnityEngine.Rendering;
using FMODUnity;

public class Puzzle3 : MonoBehaviour
{
    [SerializeField]
    public Button[] oliverButtons;
    [SerializeField]
    public Button[] wendyButtons;
    [SerializeField]
    public Door BasementDoor;
    [SerializeField]
    public GameObject ExtendedBasement;

    public EventReference DoorUnlocked;


    public string correctOliver = "OLIVER";
    public string correctWendy = "WENDY";

    private Button selectedButton = null;

    private void Start()
    {

        foreach (Button button in oliverButtons)
        {
            button.onClick.AddListener(() => OnButtonClicked(button));
        }
        foreach (Button button in wendyButtons)
        {
            button.onClick.AddListener(() => OnButtonClicked(button));
        }
    }

    private void Update()
    {
        var cameraRay = Camera.main.ScreenPointToRay(new Vector2(Screen.width/2, Screen.height/2));
        Debug.DrawRay(cameraRay.origin, cameraRay.direction * 10000, Color.magenta);

        RaycastHit hit;
        if (Physics.Raycast(cameraRay, out hit, 10000f))
        {
            if (Input.GetMouseButtonDown(0))
            {
                Button btn = hit.transform.gameObject.GetComponent<Button>();
                if (btn != null)
                {
                    OnButtonClicked(btn);
                }
            }
        }
    }

    public void OnButtonClicked(Button button)
    {

        if (selectedButton == null)
        {
            selectedButton = button;
            button.GetComponent<Image>().color = Color.green;
        }
        else
        {
            if (selectedButton == button)
            {
                selectedButton.GetComponent<Image>().color = Color.white;
                selectedButton = null;
            }
            else
            {
                if (IsSameRow(button, selectedButton))
                {
                    SwapLetters(button, selectedButton);

                    CheckPuzzleSolved();
                    selectedButton.GetComponent<Image>().color = Color.white;
                    button.GetComponent<Image>().color = Color.white;
                    selectedButton = null;
                }
                else
                {
                    selectedButton.GetComponent<Image>().color = Color.green;
                    selectedButton = button;
                    button.GetComponent<Image>().color = Color.green;
                }
            }
        }
    }

    private bool IsSameRow(Button button1, Button button2)
    {
        bool button1InOliver = System.Array.Exists(oliverButtons, b => b == button1);
        bool button2InOliver = System.Array.Exists(oliverButtons, b => b == button2);

        bool button1InWendy = System.Array.Exists(wendyButtons, b => b == button1);
        bool button2InWendy = System.Array.Exists(wendyButtons, b => b == button2);

        return (button1InOliver && button2InOliver) || (button1InWendy && button2InWendy);
    }

    private void SwapLetters(Button button1, Button button2)
    {
        TextMeshProUGUI text1 = button1.GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI text2 = button2.GetComponentInChildren<TextMeshProUGUI>();

        if (text1 != null && text2 != null)
        {
            string temp = text1.text;
            text1.text = text2.text;
            text2.text = temp;

        }
    }

    private string GetConcatenatedText(Button[] buttons)
    {
        var sortedButtons = buttons.OrderBy(b => b.transform.GetSiblingIndex()).ToArray();

        string result = "";
        foreach(Button btn in sortedButtons)
        {
            TextMeshProUGUI textComp = btn.GetComponentInChildren<TextMeshProUGUI>();
            if (textComp != null)
            {
                result += textComp.text;
            }
        }
        return result;
    }

    private void CheckPuzzleSolved()
    {
        string currentOliver = GetConcatenatedText(oliverButtons);
        string currentWendy = GetConcatenatedText(wendyButtons);
        Debug.Log("Checking PuzzleSolved..." + currentOliver + currentWendy);



        if (currentOliver == correctOliver && currentWendy == correctWendy)
        {
            Debug.Log("Solved Puzzle3");
            BasementDoor.isLocked = false;
            ExtendedBasement.SetActive(true);
            RuntimeManager.PlayOneShot(DoorUnlocked);
        }
    }
}
