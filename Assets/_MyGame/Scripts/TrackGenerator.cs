using System.Linq;
using System.Collections.Generic;
using UnityEngine;

// expecting: 赛道障碍物出现效果: 可以看到形成赛道的障碍物是从水下由近及远依次浮起来的,当远离玩家由依次落入水中

public class TrackGenerator : MonoBehaviour
{
    [SerializeField]
    Transform _myboat;

    [SerializeField]
    GameObject[] _prefabBlocks;
    [Header("Track Width")]
    [SerializeField]
    float _trackWidthMin = 0.5f;
    [SerializeField]
    float _trackWidthMax = 1.0f;
    [Header("Block Random Offset In Horizonal")]
    [SerializeField]
    float _offsetInXMin = -3.0f;
    [SerializeField]
    float _offsetInXMax = 3.0f;
    [Header("Track Length")]
    [SerializeField]
    float _trackLength = 20.0f;
    [SerializeField]
    float _trackOffsetInZ = 5.0f;

    List<GameObject> _cachedGameObjects = new List<GameObject>();
    List<GameObject> _presentTrackBlocks = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var myboatPos = _myboat.position;

        var trackInZMin = myboatPos.z - _trackOffsetInZ;
        var trackInZMax = trackInZMin + _trackLength;

        var trackEnd = trackInZMin;

        // destroy block
        foreach (var block in _presentTrackBlocks.ToArray())
        {
            var boxCollider = block.GetComponent<BoxCollider>();
            var blockBegin = block.transform.localPosition.z - boxCollider.bounds.extents.z;
            var blockEnd = block.transform.localPosition.z + boxCollider.bounds.extents.z;
            if (blockBegin < trackInZMin)
            {
                _presentTrackBlocks.Remove(block);
                block.SetActive(false);
                _cachedGameObjects.Add(block);
            }
            else if (blockEnd > trackEnd)
                trackEnd = blockEnd;
        }
        while (trackEnd < trackInZMax)
        {
            var block = RandomGenerateBlock();
            _presentTrackBlocks.Add(block);
            var boxCollider = block.GetComponent<BoxCollider>();
            block.transform.localPosition = new Vector3(Random.Range(_offsetInXMin, _offsetInXMax), 0.0f, trackEnd + boxCollider.bounds.extents.z);
            trackEnd = block.transform.localPosition.z + boxCollider.bounds.extents.z;
        }
    }

    GameObject RandomGenerateBlock()
    {
        GameObject gameObject = null;
        if (_cachedGameObjects.Count > 0)
        {
            var fetchIndex = Random.Range(0, _cachedGameObjects.Count);
            gameObject = _cachedGameObjects[fetchIndex];
            gameObject.SetActive(true);
            _cachedGameObjects.RemoveAt(fetchIndex);
        }
        else
        {
            gameObject = Instantiate(_prefabBlocks[Random.Range(0, _prefabBlocks.Length)], transform);
            gameObject.layer = LayerMask.NameToLayer("TrackBlock");
        }
        return gameObject;
    }
}
