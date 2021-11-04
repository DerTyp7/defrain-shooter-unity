using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveData : MonoBehaviour
{
    [SerializeField] PlayerData _PlayerData = new PlayerData();

    private void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(SavePlayerToJson);
    }

    public void SavePlayerToJson()
    {
        string playerData = JsonUtility.ToJson(_PlayerData);

        System.IO.File.WriteAllText(Application.persistentDataPath + "/PlayerData.json", playerData);
        Debug.Log(Application.persistentDataPath);
    }

}

[System.Serializable]
public class PlayerData
{
    public string username;
}