using UnityEngine;

public class GameManager : MonoBehaviour
{
    private LocalStorage localStorge;
    // private RestClient restClient;
    private AlgodManager algodManager;


    private string storageKeyForPrivateKey = "privateKey";

    async void Start()
    {
        localStorge = GetComponent<LocalStorage>();
        // restClient = GetComponent<RestClient>();
        algodManager = GetComponent<AlgodManager>();
        InitAccount();
        string address = algodManager.GetAddress();
        Debug.Log(address);
        double balance = await algodManager.CheckBalanceByAddress(address);
        Debug.Log(balance);
        // algodManager.MakePayment("ZRVP5276H7PWMI5VIQVLFGICYEOAUVD467FJ2Z72UUGDETF6K7LXBRHQ4E", 1).Forget();

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
