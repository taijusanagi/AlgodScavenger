using Algorand.Unity;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;
public class AlgodManager : MonoBehaviour
{
    class AccountDataFromAPI
    {
        public double amount;
    }

    private string apiBaseURL = "https://testnet-api.algonode.cloud/";

    // I could not find testnet API works fine with the Algorand.Unity package
    // So I decided using API directly for some logic
    AlgodClient algod;
    private Account _account;

    void Start()
    {

    }

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

    // I could not find testnet API works fine with the Algorand.Unity package
    // So I decided taking amount valud from API directly
    public async UniTask<double> CheckBalanceByAddress(string address)
    {
        {
            using (UnityWebRequest webRequest = UnityWebRequest.Get(apiBaseURL + "v2/accounts/" + address))
            {
                await webRequest.SendWebRequest().WithCancellation(this.GetCancellationTokenOnDestroy());
                if (webRequest.result == UnityWebRequest.Result.ConnectionError || webRequest.result == UnityWebRequest.Result.ProtocolError)
                {
                    Debug.LogError($"Error: {webRequest.error}");
                    return 0;
                }
                else
                {
                    string jsonResponse = webRequest.downloadHandler.text;
                    AccountDataFromAPI account = JsonUtility.FromJson<AccountDataFromAPI>(jsonResponse);
                    return account.amount;
                }
            }
        }
    }

}