using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenuHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject Menu;
    [SerializeField]
    private GameObject Choices;

    private void Awake()
    {
        Choices.gameObject.SetActive(false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Tamagotchi");
    }

    public void ChoicesTrigger()
    {
        Menu.gameObject.SetActive(!Menu.gameObject.activeInHierarchy);
        Choices.gameObject.SetActive(!Choices.gameObject.activeInHierarchy);
        Debug.Log("Menu and choices have swapped active");
    }

    public void DeleteSave()
    {
        File.Delete(Application.persistentDataPath + "/gamesave.save");
        ChoicesTrigger();
        Debug.Log("File has been deleted");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
