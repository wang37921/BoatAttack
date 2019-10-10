using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;
using TMPro;


public class WindowLevel : MonoBehaviour
{
    [SerializeField]
    Transform _content;
    [SerializeField]
    Color _Color = new Color(0.7f, 0.7f, 0.7f, 1f);

    [SerializeField]
    GameObject _windowStart;

    void Start()
    {
        // PlayerPrefs.DeleteAll();
        // PlayerModel.Instance.SavePlayerData();

        // 读取存档
        PlayerModel.Instance.Init().onInitDone += () =>
        {
            // 根据读出的关卡加载按钮
            for (int i = 0; i < PlayerModel.Instance.levelDatas.levels.Count; i++)
            {
                var levelIndex = i;
                // 实例化
                Addressables.InstantiateAsync("ButtonLevel", _content).Completed += handle =>
                {
                    var transf = handle.Result.transform;

                    transf.GetChild(0).GetComponent<TextMeshProUGUI>().text = levelIndex.ToString();
                    if (PlayerModel.Instance.playerDatas.ContainsKey(levelIndex.ToString()))
                    {
                        var playerDatas = PlayerModel.Instance.playerDatas[levelIndex.ToString()];

                        transf.GetChild(1).GetComponent<Image>().color = playerDatas.cupTime ? Color.white : _Color;
                        transf.GetChild(2).GetComponent<Image>().color = playerDatas.cupStar ? Color.white : _Color;
                        transf.GetChild(3).GetComponent<Image>().color = playerDatas.cupHit ? Color.white : _Color;

                        transf.GetComponent<Button>().onClick.AddListener(() =>
                        {
                            GameController.Instance.LoadGameAddressable(playerDatas);
                        });
                    }
                    else
                    {
                        foreach (var img in transf.GetComponentsInChildren<Image>())
                        {
                            img.color = _Color;
                        }
                    }
                };
            }


        };
    }

#if UNITY_EDITOR

    private void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            PlayerPrefs.DeleteAll();
            Debug.LogError("已删除存档");
        }
    }
#endif

    public void BackMainMenu()
    {
        gameObject.SetActive(false);
        _windowStart.SetActive(true);
    }
}
