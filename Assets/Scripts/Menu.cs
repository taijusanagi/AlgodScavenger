using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public void OnclickContinue()
    {
        SceneManager.LoadScene(1);
    }

    public void OnclickNewGame()
    {
        SceneManager.LoadScene(1);
    }
}
