using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuHandler : MonoBehaviour
{
    [SerializeField]
    GameObject Menu;
    [SerializeField]
    GameObject Buttons;
    [SerializeField]
    GameObject Stats;
    [SerializeField]
    GameObject Background;
    [SerializeField]
    GameObject Confirm;
    [SerializeField]
    GameObject Restart;
    [SerializeField]
    GameObject Slime;

    private int _days;

    private void Awake()
    {
        Menu.SetActive(false);
        Confirm.SetActive(false);
        Restart.SetActive(false);
    }

    public void changeMenuState()
    {
        Menu.gameObject.SetActive(!Menu.activeInHierarchy);
    }

    public void saveBeforeQuit()
    {
        Menu.SetActive(false);
        Buttons.SetActive(false);
        Stats.SetActive(false);
        Background.SetActive(false);
        Confirm.SetActive(true);
        Slime.SetActive(false);
    }

    public void quitGame()
    {
        Debug.Log("Game exited");
        Application.Quit();
    }

    public void getDays(int days)
    {
        _days = days;
    }

    public void StartReset(string reason)
    {
        Text text = Restart.GetComponentInChildren<Text>();

        switch (reason)
        {
            case "Hunger":
                text.text = $"Thanks to your incompotence in taking care of the slime, it died from hunger after being in your care for {_days} days";
                break;
            case "Cleanliness":
                text.text = $"Thanks to your incompotence in taking care of the slime, it left after being in your care for {_days} days due to not being cleaned often enough";
                break;
            case "Happiness":
                text.text = $"Thanks to your incompotence in taking care of the slime, it left after being in your care for {_days} days in hopes of finding someone else that could make it happy";
                break;
            default:
                text.text = "Something went wrong, and I do not know why";
                break;
        }
        Menu.SetActive(false);
        Buttons.SetActive(false);
        Stats.SetActive(false);
        Background.SetActive(false);
        Slime.SetActive(false);
        Restart.SetActive(true);
    }

    public void ResetGame()
    {
        Menu.SetActive(true);
        Buttons.SetActive(true);
        Stats.SetActive(true);
        Background.SetActive(true);
        Slime.SetActive(true);
        Restart.SetActive(false);
    }
}
