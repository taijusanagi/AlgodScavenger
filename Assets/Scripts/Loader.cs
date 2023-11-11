using UnityEngine;

public class Loader : MonoBehaviour
{

    public GameObject gameManger;
    void Awake()
    {
        if (GameManager.instance == null)
        {
            Instantiate(gameManger);
        }
    }

}
