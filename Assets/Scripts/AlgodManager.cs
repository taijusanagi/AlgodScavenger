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
    private ulong algodStoneId = 475835119;
    private ulong appId = 475848475;

    AlgodClient algod;

    // this is hackathon product, so keep it simple
    private string ownerMnemonic = "nest skill piano place typical resemble staff identify proof urban mail birth range pass fly gossip apple easily exhibit angle cream critic comic ability true";
    private Account _ownerAccount;
    private Account _userAccount;


    void Start()
    {
        algod = new AlgodClient(apiBaseURL);
        _ownerAccount = new Account(PrivateKey.FromMnemonic(ownerMnemonic));
    }

    public string CreateAccount()
    {
        var (privateKey, address) = Account.GenerateAccount();
        return privateKey.ToString();
    }

    public void SetAccountFromPrivateKey(string privateKey)
    {
        Account account = new Account(PrivateKey.FromString(privateKey));
        _userAccount = account;
    }


    public string GetAddress()
    {
        return _userAccount.Address.ToString();
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
            sender: _userAccount.Address,
            txnParams: txnParams,
            receiver: Address.FromString(reciever),
            amount: amount
        );
        var signedTxn = _userAccount.SignTxn(paymentTxn);
        var (sendTxnError, txid) = await algod.SendTransaction(signedTxn);
        sendTxnError.ThrowIfError();
        Debug.Log($"Successfully sent tx!");
        var (confirmErr, confirmed) = await algod.WaitForConfirmation(txid.TxId);
        confirmErr.ThrowIfError();
        Debug.Log($"Successfully made payment! Confirmed on round {confirmed.ConfirmedRound}");
        return txid.TxId;
    }

    public async UniTask<string> Scavenge()
    {
        var (txnParamsError, txnParams) = await algod.TransactionParams();
        txnParamsError.ThrowIfError();
        Debug.Log($"Successfully created params!");

        string base64String = "iC0e8A==";
        byte[] bytes = Convert.FromBase64String(base64String);
        CompiledTeal compiledTeal = bytes;
        CompiledTeal[] appArguments = new CompiledTeal[] { compiledTeal };

        var paymentTxn = Transaction.AppCall(_userAccount.Address, txnParams, appId, OnCompletion.NoOp, appArguments);
        var signedTxn = _userAccount.SignTxn(paymentTxn);

        var (sendTxnError, txid) = await algod.SendTransaction(signedTxn);
        sendTxnError.ThrowIfError();
        Debug.Log($"Successfully sent tx!");
        var (confirmErr, confirmed) = await algod.WaitForConfirmation(txid.TxId);
        confirmErr.ThrowIfError();
        Debug.Log($"Successfully scavenged! Confirmed on round {confirmed.ConfirmedRound}");
        return txid.TxId;
    }

    public async UniTask<string> AcceptAlgodStone()
    {
        var (txnParamsError, txnParams) = await algod.TransactionParams();
        txnParamsError.ThrowIfError();
        Debug.Log($"Successfully created params!");
        var txn = Transaction.AssetAccept(_userAccount.Address, txnParams, algodStoneId);
        var signedTxn = _userAccount.SignTxn(txn);
        var (sendTxnError, txid) = await algod.SendTransaction(signedTxn);
        sendTxnError.ThrowIfError();
        Debug.Log($"Successfully sent tx!");
        var (confirmErr, confirmed) = await algod.WaitForConfirmation(txid.TxId);
        confirmErr.ThrowIfError();
        Debug.Log($"Successfully optin! Confirmed on round {confirmed.ConfirmedRound}");
        return txid.TxId;
    }

    public async UniTask<string> MintAlgodStone(ulong amount)
    {
        var (txnParamsError, txnParams) = await algod.TransactionParams();
        txnParamsError.ThrowIfError();
        Debug.Log($"Successfully created params!");
        var txn = Transaction.AssetTransfer(_ownerAccount.Address, txnParams, algodStoneId, amount, _userAccount.Address);
        var signedTxn = _ownerAccount.SignTxn(txn);
        var (sendTxnError, txid) = await algod.SendTransaction(signedTxn);
        sendTxnError.ThrowIfError();
        Debug.Log($"Successfully sent tx!");
        var (confirmErr, confirmed) = await algod.WaitForConfirmation(txid.TxId);
        confirmErr.ThrowIfError();
        Debug.Log($"Successfully sent asset! Confirmed on round {confirmed.ConfirmedRound}");
        return txid.TxId;
    }

}