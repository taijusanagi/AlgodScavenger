using UnityEngine;

public class GameManager : MonoBehaviour
{
    private LocalStorage localStorge;
    private AlgodManager algodManager;

    private string storageKeyForPrivateKey = "privateKey";

    void Start()
    {
        localStorge = GetComponent<LocalStorage>();
        algodManager = GetComponent<AlgodManager>();
        InitAccount();
        string address = algodManager.GetAddress();
        Debug.Log(address);
    }

    public void InitAccount()
    {
        string privateKey = localStorge.LoadFromLocal(storageKeyForPrivateKey);
        if (privateKey == "")
        {
            CreateAndSaveAccount();
        }
        else
        {
            algodManager.SetAccountFromPrivateKey(privateKey);
        }
    }

    public void CreateAndSaveAccount()
    {
        string privateKey = algodManager.CreateAccount();
        localStorge.SaveToLocal(storageKeyForPrivateKey, privateKey.ToString());
    }

}
