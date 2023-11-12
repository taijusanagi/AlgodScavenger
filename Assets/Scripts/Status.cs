using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Status : MonoBehaviour
{

    public Text addressText;
    private string _address;
    public Button startButton;

    void Awake()
    {

        Debug.Log("Status Start");
        startButton.interactable = false;
        _address = AccountManager.instance.GetAddress();
        // just set initial mock balance
        int balance = 0;
        addressText.text = "Address: " + _address + " / ALGO: " + balance + " ";
        Debug.Log("Address updated");
        InvokeRepeating("UpdateBalance", 0.0f, 5.0f);
    }

    private async void UpdateBalance()
    {
        Debug.Log("UpdateBalance");
        double balance = await AccountManager.instance.GetBalance(_address);
        if (balance > 0)
        {
            double formatedBalance = balance / 1000000;
            addressText.text = "Address: " + _address + " / ALGO: " + formatedBalance + " ";
            startButton.interactable = true;
        }
    }

    // private async IAsyncEnumerable<double> GetBalance()
    // {
    //     Debug.Log("GetBalance");
    //     string address = AccountManager.instance.GetAddress();
    //     Debug.Log("address: " + address);
    //     double balance = await AccountManager.instance.GetBalance(address);
    //     Debug.Log("balance: " + balance);
    //     addressText.text = "Address: " + address + " / ALGO: " + balance + " ";
    //     Debug.Log("Address updated");
    //     yield return balance;
    // }



    public void OnclickStart()
    {
        SceneManager.LoadScene(2);
    }

    public void OnclickBack()
    {
        SceneManager.LoadScene(0);
    }

    public void OnlickFund()
    {
        Application.OpenURL("https://dispenser.testnet.aws.algodev.network/?account=" + _address);
    }
}
