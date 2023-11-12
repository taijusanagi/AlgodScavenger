using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
public class AccountManager : MonoBehaviour
{


    public static AccountManager instance = null;

    public string address;

    private LocalStorage localStorge;
    private AlgodManager algodManager;
    private string hash;

    private string storageKeyForPrivateKey = "privateKey";

    void Awake()
    {

        Debug.Log("AccountManager.awake");
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        localStorge = GetComponent<LocalStorage>();
        algodManager = GetComponent<AlgodManager>();
    }

    public bool LoadAccount()
    {
        string privateKey = localStorge.LoadFromLocal(storageKeyForPrivateKey);
        if (privateKey != "")
        {
            algodManager.SetAccountFromPrivateKey(privateKey);
        }
        return privateKey != "";
    }

    public void CreateAccount()
    {
        Debug.Log("CreateAccount start");
        CreateAndSaveAccount();
        Debug.Log("CreateAccount end");
    }

    public void DeleteAccount()
    {
        localStorge.DeleteFromLocal(storageKeyForPrivateKey);
    }

    public void CreateAndSaveAccount()
    {
        string privateKey = algodManager.CreateAccount();
        instance.localStorge.SaveToLocal(storageKeyForPrivateKey, privateKey.ToString());
    }

    void Start()
    {
        Debug.Log("start");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public string GetAddress()
    {
        return algodManager.GetAddress();
    }

    public async UniTask<double> GetBalance(string address)
    {
        return await algodManager.CheckBalanceByAddress(address);
    }

    public async UniTask<double> GetStoneBalance(string address)
    {
        return await algodManager.CheckStoneBalanceByAddress(address);
    }

    public async UniTask<string> GenerateHash()
    {
        hash = await algodManager.Scavenge();
        UnityEngine.Random.InitState(hash.GetHashCode());
        return hash;
    }

    public async UniTask<string> AcceptTx()
    {
        return await algodManager.AcceptAlgodStone();
    }

    public async UniTask<string> GetAlgodStone()
    {
        return await algodManager.MintAlgodStone(1);
    }


}
