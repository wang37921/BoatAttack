using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CustomMenus
{
    [MenuItem("ResetRecord/All")]
    public static void ClearBestRecord()
    {
        ClearBestDistance();
        ClearBestHit();
    }
    [MenuItem("ResetRecord/Distance")]
    public static void ClearBestDistance()
    {
        PlayerPrefs.DeleteKey(GameController._kBestRecord);
    }
    [MenuItem("ResetRecord/Hit")]
    public static void ClearBestHit()
    {
        PlayerPrefs.DeleteKey(GameController._kBestHit);
    }
}
