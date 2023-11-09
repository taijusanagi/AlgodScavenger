using System;
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

    AlgodClient algod;
    private Account _account;

    void Start()
    {
        algod = new AlgodClient(apiBaseURL);
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

    // AlgodClient Account not working so get balance from API directly
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

    // this function is not used in prod, just for testing
    public async UniTask<string> MakePayment(string reciever, ulong amount)
    {
        var (txnParamsError, txnParams) = await algod.TransactionParams();
        txnParamsError.ThrowIfError();
        Debug.Log($"Successfully created params!");

        var paymentTxn = Transaction.Payment(
            sender: _account.Address,
            txnParams: txnParams,
            receiver: Address.FromString(reciever),
            amount: amount
        );
        var signedTxn = _account.SignTxn(paymentTxn);
        var (sendTxnError, txid) = await algod.SendTransaction(signedTxn);
        sendTxnError.ThrowIfError();
        Debug.Log($"Successfully sent tx!");
        var (confirmErr, confirmed) = await algod.WaitForConfirmation(txid.TxId);
        confirmErr.ThrowIfError();
        Debug.Log($"Successfully made payment! Confirmed on round {confirmed.ConfirmedRound}");
        return txid.TxId;
    }

}