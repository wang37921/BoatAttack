using Sirenix.OdinInspector;
using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu]
public sealed class LevelDatas : SerializedScriptableObject
{
    [ListDrawerSettings(OnBeginListElementGUI = "OnBeginListElementGUI")]
    public List<LevelData> levels;

    void OnBeginListElementGUI(int index)
    {
        GUILayout.Label("Level Index:" + index.ToString());
    }
}

[SerializeField]
public class LevelData
{
    [Range(10, 200)]
    public int Time = 30;

    [Range(0, 10)]
    public int Hit = 2;

    [Range(0.1f, 1f)]
    public float Star = 0.7f;
}
