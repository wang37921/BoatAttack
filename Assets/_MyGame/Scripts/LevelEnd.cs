using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelEnd : MonoBehaviour
{
    [Header("下一关ID")]
    public int NextLevel = 0;
    
    [Header("奖杯所需时间")]
    public float time = 30;
    
    [Header("奖杯所需撞击帆船")]
    public int hit = 1;

    [Header("奖杯所需吃星比例")]
    [Range(0.1f, 1f)]
    public float star = 0.7f;

}
