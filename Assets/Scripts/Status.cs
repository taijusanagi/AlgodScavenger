using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Status : MonoBehaviour
{

    public Text addressText;

    void Start()
    {
        Debug.Log("Status Start");
        string address = AccountManager.instance.GetAddress();
        addressText.text = "Address: " + address;
        Debug.Log("Address updated");
    }

    public void OnclickStart()
    {
        SceneManager.LoadScene(2);
    }

    public void OnclickBack()
    {
        SceneManager.LoadScene(0);
    }
}
