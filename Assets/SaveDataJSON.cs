using UnityEngine;
using System.IO;

public class SaveDataJSON : MonoBehaviour
{
    public PlayerData playerData;

    void Awake()
    {
        playerData = PlayerData.Instance;
    }

    public void SaveData() {
        string json = JsonUtility.ToJson(playerData);
        Debug.Log(json);

        using (StreamWriter writer = new StreamWriter(Application.persistentDataPath + "/save.json")) {
            Debug.Log("Saved in " + Application.persistentDataPath + "/save.json");
            writer.Write(json);
        }
    }

    public void LoadData() {
        string json = JsonUtility.ToJson(playerData);
        using (StreamReader reader = new StreamReader(Application.persistentDataPath + "/save.json")) {
            json = reader.ReadToEnd();
        }

        JsonUtility.FromJsonOverwrite(json, playerData);
    }
}
