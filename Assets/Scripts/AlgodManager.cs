using Algorand.Unity;
using UnityEngine;

public class AlgodManager : MonoBehaviour
{
    // AlgodClient algod;
    private Account _account;

    public string CreateAccount()
    {
        var (privateKey, address) = Account.GenerateAccount();
        return privateKey.ToString();
    }

    public void SetAccountFromPrivateKey(string privateKey)
    {
        Account account = new Account(PrivateKey.FromString(privateKey));
        _account = account;
    }

    public string GetAddress()
    {
        return _account.Address.ToString();
    }

}