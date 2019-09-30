using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{
    [Header("下一场景ID(默认下一关)")]
    public int NextLevel = -1;
    
    [Header("奖杯所需时间")]
    public float time = 30;
    
    [Header("奖杯所需撞击帆船")]
    public int hit = 1;

    [Header("奖杯所需吃星比例")]
    [Range(0.1f, 1f)]
    public float star = 0.7f;

    static LevelEnd _instance;
    public static LevelEnd Instance {
        get{
            if(_instance == null)
                _instance = FindObjectOfType<LevelEnd>();
            return _instance;
        }
    }

}
