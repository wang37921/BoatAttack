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
    float _trackWidthMin = 25.0f;
    [SerializeField]
    float _trackWidthMax = 30.0f;
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
            var boxColliders = block.GetComponentsInChildren<BoxCollider>();
            var blockHalfLength = boxColliders.Max(e => e.bounds.extents.z);
            var blockBegin = block.transform.localPosition.z - blockHalfLength;
            var blockEnd = block.transform.localPosition.z + blockHalfLength;
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
            var boxColliders = block.GetComponentsInChildren<BoxCollider>();
            var blockHalfLength = boxColliders.Max(e => e.bounds.extents.z);
            block.transform.localPosition = new Vector3(Random.Range(_offsetInXMin, _offsetInXMax), 0.0f, trackEnd + blockHalfLength);
            trackEnd = block.transform.localPosition.z + blockHalfLength;
        }
    }

    GameObject RandomGenerateBlock()
    {
        GameObject blockObject = null;
        if (_cachedGameObjects.Count > 0)
        {
            var fetchIndex = Random.Range(0, _cachedGameObjects.Count);
            blockObject = _cachedGameObjects[fetchIndex];
            blockObject.SetActive(true);
            _cachedGameObjects.RemoveAt(fetchIndex);
        }
        else
        {
            var trackWidth = Random.Range(_trackWidthMin, _trackWidthMax);
            var halfTrackWidth = trackWidth / 2.0f;

            blockObject = new GameObject("track block");
            blockObject.transform.SetParent(transform);

            var leftBlock = Instantiate(_prefabBlocks[Random.Range(0, _prefabBlocks.Length)], blockObject.transform);
            var leftBlockCollider = leftBlock.GetComponent<BoxCollider>();
            leftBlock.layer = LayerMask.NameToLayer("TrackBlock");
            leftBlock.transform.localPosition = new Vector3(-halfTrackWidth - leftBlockCollider.bounds.extents.x, 0.0f, 0.0f);

            var rightBlock = Instantiate(_prefabBlocks[Random.Range(0, _prefabBlocks.Length)], blockObject.transform);
            var rightBlockCollider = rightBlock.GetComponent<BoxCollider>();
            rightBlock.layer = LayerMask.NameToLayer("TrackBlock");
            rightBlock.transform.localPosition = new Vector3(halfTrackWidth + rightBlockCollider.bounds.extents.x, 0.0f, 0.0f);
        }
        return blockObject;
    }
}
