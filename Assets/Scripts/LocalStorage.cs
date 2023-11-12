using System.IO;
using UnityEngine;

public class LocalStorage : MonoBehaviour
{
    void Start()
    {
        LoadLocalStageData();
    }

    private void LoadLocalStageData()
    {
        string path = GetPath();
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

    public void DeleteFromLocal(string key)
    {
        string path = GetPath();
        File.Delete(path + key);
    }

    private string GetPath()
    {
        string path = Application.persistentDataPath + "/AppData/";
        return path;
    }

    public void SaveToLocal(string key, string value)
    {
        string path = GetPath();
        File.WriteAllText(path + key, value);
    }

    public string LoadFromLocal(string key)
    {
        string path = GetPath();
        try
        {
            return File.ReadAllText(path + key);
        }
        catch
        {
            return "";
        }
    }
}
