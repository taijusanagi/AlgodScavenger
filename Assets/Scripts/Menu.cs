using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public Button continueButton;
    void Awake()
    {
        Debug.Log("Menu.awake");
        bool interactable = AccountManager.instance.LoadAccount();
        continueButton.interactable = interactable;
    }

    public void OnclickContinue()
    {
        MusicManager.instance.PlayEffect();
        SceneManager.LoadScene(1);
    }

    public void OnclickNewGame()
    {
        MusicManager.instance.PlayEffect();
        AccountManager.instance.DeleteAccount();
        AccountManager.instance.CreateAccount();
        // wait for account creation
        while (!AccountManager.instance.LoadAccount())
        {
            Debug.Log("waiting for account creation");
        }
        SceneManager.LoadScene(1);
    }
}
