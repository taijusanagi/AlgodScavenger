using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Status : MonoBehaviour
{
    public void OnclickStart()
    {
        SceneManager.LoadScene(2);
    }

    public void OnclickBack()
    {
        SceneManager.LoadScene(0);
    }
}
