using UnityEngine;

public class GameManager : MonoBehaviour
{
    private LocalStorage localStorge;
    private AlgodManager algodManager;


    private string storageKeyForPrivateKey = "privateKey";

    async void Start()
    {
        localStorge = GetComponent<LocalStorage>();
        algodManager = GetComponent<AlgodManager>();
        InitAccount();
        string address = algodManager.GetAddress();
        Debug.Log(address);
        double balance = await algodManager.CheckBalanceByAddress(address);
        Debug.Log(balance);
        // string txId = await algodManager.MakePayment("ZRVP5276H7PWMI5VIQVLFGICYEOAUVD467FJ2Z72UUGDETF6K7LXBRHQ4E", 1);
        // Debug.Log(txId);
        // string scavengeTxId = await algodManager.Scavenge();
        // Debug.Log(scavengeTxId);
        // string optInTxId = await algodManager.AcceptAlgodStone();
        // Debug.Log(optInTxId);
        // string transferTxId = await algodManager.MintAlgodStone(1);
        // Debug.Log(transferTxId);
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
