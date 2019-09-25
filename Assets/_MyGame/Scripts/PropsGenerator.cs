using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct PropsGenerateData
{
    public GameObject Prefab;
    public float Probability;
}


public class PropsGenerator : MonoBehaviour
{
    [SerializeField]
    GameObject[] Props;

    [SerializeField]
    float _generateInterval = 1.0f;
    [SerializeField]
    [ReadOnly]
    float _generateTick = 0.0f;

    [SerializeField]
    float _generateDistMax = 8.0f;
    [SerializeField]
    float _generateDistMin = 3.0f;
    [SerializeField]
    float _generateRadius = 1.5f;

    [SerializeField]
    float _disappearSqrDist = 5.0f;

    List<GameObject> _propsInstList = new List<GameObject>();
    MyBoatController _myBoat;

    // Start is called before the first frame update
    void Start()
    {
        _myBoat = FindObjectOfType<MyBoatController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameController.Instance.IsGaming)
        {
            if (_myBoat.HasFuel)
            {
                if (_propsInstList.Count != 0)
                {
                    foreach (var prop in _propsInstList.ToArray())
                    {
                        var sqrDist = Vector3.SqrMagnitude(_myBoat.transform.position - prop.transform.position);
                        if (sqrDist > _disappearSqrDist || !prop.activeSelf)
                        {
                            _propsInstList.Remove(prop);
                            Destroy(prop);
                        }
                    }
                }

                if (_generateTick != 0.0f)
                    _generateTick = Mathf.Max(_generateTick - Time.deltaTime, 0.0f);
                else
                {
                    _generateTick = _generateInterval;
                    var tmp = Props[Random.Range(0, Props.Length)];
                    var dist = Random.Range(_generateDistMin, _generateDistMax);
                    var spawnPoint = _myBoat.transform.forward * dist + _myBoat.transform.position;
                    var offsetInPlane = Random.insideUnitCircle * _generateRadius;
                    spawnPoint += new Vector3(offsetInPlane.x, 0.0f, offsetInPlane.y);
                    spawnPoint.y = 0.0f;
                    var propInst = Instantiate(tmp, spawnPoint, Quaternion.Euler(0.0f, Random.Range(0.0f, 360.0f), 0.0f), transform);
                    _propsInstList.Add(propInst);
                }
            }
        }
    }

    private void OnDisable()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}
