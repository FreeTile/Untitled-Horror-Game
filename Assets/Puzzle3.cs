using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Puzzle3 : MonoBehaviour
{
    public Button[] oliverButtons;
    public Button[] wendyButtons;

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

    private void OnButtonClicked(Button button)
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
                }
                selectedButton.GetComponent<Image>().color = Color.white;
                selectedButton = null;
            }
        }
    }

    private bool IsSameRow(Button button1, Button button2)
    {
        bool inOliverRow = System.Array.Exists(oliverButtons, b => b == button1 || b == button2);
        bool inWendyRow = System.Array.Exists(wendyButtons, b => b == button1 || b == button2);

        return inOliverRow || inWendyRow;
    }

    private void SwapLetters(Button button1, Button button2)
    {
        TextMeshProUGUI text1 = button1.GetComponentInChildren<TextMeshProUGUI>();
        TextMeshProUGUI text2 = button2.GetComponentInChildren<TextMeshProUGUI>();

        string temp = text1.text;
        text1.text = text2.text;
        text2.text = temp;
    }
}
