using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;

public class PlayerData
{
    public int ID;
    public int group;
    public bool cupTime;
    public bool cupStar;
    public bool cupHit;
}

public class PlayerModel
{
    List<PlayerData> _playerData;
    public List<PlayerData> playerDatas
    {
        get
        {
            if (_playerData == null)
            {
                _playerData = ReadPlayerData();
            }
            return _playerData;
        }
        set
        {
            _playerData = value;
        }
    }

    static PlayerModel _instance;
    public static PlayerModel Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new PlayerModel();
            }
            return _instance;
        }
    }
    PlayerModel() { }

    public List<PlayerData> ReadPlayerData()
    {
        var str = PlayerPrefs.GetString("PlayerData");
        List<PlayerData> data = null;
        if (str != null)
        {
            data = JsonMapper.ToObject<List<PlayerData>>(str);
        }
        else
        {
            data = new List<PlayerData>();
            data.Add(new PlayerData { ID = 1, group = 0, cupTime = false, cupStar = false, cupHit = false });
        }
        return data;
    }

    public void SavePlayerData()
    {
        var str = JsonMapper.ToJson(_playerData);
        PlayerPrefs.SetString("PlayerData", str);
    }

    public void AddNewLevelData(int nextLevelID)
    {
        _playerData.Add(new PlayerData { ID = nextLevelID, group = 0, cupTime = false, cupStar = false, cupHit = false });
    }
}
