using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using LitJson;

[SerializeField]
public class PlayerData
{
    public int map;
    public int index;
    public bool cupTime;
    public bool cupStar;
    public bool cupHit;
}

public class PlayerModel
{
    Dictionary<string, PlayerData> _playerData;
    public Dictionary<string, PlayerData> playerDatas
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

    public UnityAction onInitDone;
    public LevelDatas levelDatas;
    public PlayerData currentLevel;
    public LevelData CurrentLevelData
    {
        get
        {
            if(currentLevel == null)
                return null;
            return levelDatas.levels[currentLevel.index];
        }
    }

    public PlayerModel Init()
    {
        Addressables.LoadAssetAsync<LevelDatas>("Level Datas").Completed += handle =>
        {
            levelDatas = handle.Result;
            onInitDone();
        };
        return this;
    }

    public Dictionary<string, PlayerData> ReadPlayerData()
    {
        var str = PlayerPrefs.GetString("PlayerData");
        Dictionary<string, PlayerData> data = null;
        if (string.IsNullOrEmpty(str))
        {
            data = new Dictionary<string, PlayerData>();
            data["0"] = new PlayerData { index = 0, map = 0, cupTime = false, cupStar = false, cupHit = false };
        }
        else
        {
            data = JsonMapper.ToObject<Dictionary<string, PlayerData>>(str);
        }
        return data;
    }

    // 保存当前关卡并返回下一个关卡
    public PlayerData SavePlayerData()
    {
        PlayerData nextLevelData = null;
        if (currentLevel != null)
        {
            playerDatas[currentLevel.index.ToString()] = currentLevel;

            // 添加下一关 : 任意奖杯 仍有关卡
            if ((currentLevel.cupHit || currentLevel.cupStar || currentLevel.cupTime) && currentLevel.index + 1 < levelDatas.levels.Count)
            {
                // 若有记录则加载，无则新建
                if (playerDatas.ContainsKey((currentLevel.index + 1).ToString()))
                {
                    nextLevelData = playerDatas[(currentLevel.index + 1).ToString()];
                }
                else
                {
                    nextLevelData = new PlayerData { index = currentLevel.index + 1, map = 0, cupTime = false, cupStar = false, cupHit = false };
                    playerDatas[nextLevelData.index.ToString()] = nextLevelData;
                }
            }
        }

        var str = JsonMapper.ToJson(playerDatas);
        PlayerPrefs.SetString("PlayerData", str);

#if UNITY_EDITOR
        Debug.Log(str);
#endif
        return nextLevelData;
    }




    public void AddNewLevelData(int nextLevelID, int _map, bool _cupTime, bool _cupStar, bool _cupHit)
    {
        playerDatas[nextLevelID.ToString()] = new PlayerData { index = nextLevelID, map = _map, cupTime = _cupTime, cupStar = _cupStar, cupHit = _cupHit };

        // if (WindowLevel.levelsData.Levels.Count > int.Parse(nextLevelID) && (_cupHit || _cupStar || _cupTime))
        // {
        //     playerDatas[(int.Parse(nextLevelID) + 1).ToString()] = new PlayerData { ID = nextLevelID, group = _group, cupTime = false, cupStar = false, cupHit = false };
        // }

    }
}
