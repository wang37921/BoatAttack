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
    [SerializeField]
    float _blockDisappearSinkDepth = 2.0f;
    [SerializeField]
    float _blockSinkDistDepthRatio = 5.0f;
    [SerializeField]
    float _blockSpaceMin = 0.5f;
    [SerializeField]
    float _blockSpaceMax = 1.0f;
    [SerializeField]
    bool LeftSide = false;

    float TrackRange => _blockDisappearSinkDepth * _blockSinkDistDepthRatio;

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

        var expectedTrackForwardBoundary = myboatPos.z + TrackRange;
        var expectedTrackBackwardBoundary = myboatPos.z - TrackRange;

        // destroy block
        foreach (var block in _presentTrackBlocks.ToArray())
        {
            var distInZ = block.transform.localPosition.z - myboatPos.z;
            if (Mathf.Abs(distInZ) > TrackRange + _blockSpaceMax)
            {
                _presentTrackBlocks.Remove(block);
                block.SetActive(false);
                _cachedGameObjects.Add(block);
            }
        }
        // calc current boundary
        var trackForwardBoundary = 0.0f;
        var trackBackwardBoundary = 0.0f;
        if (_presentTrackBlocks.Count != 0)
        {
            trackForwardBoundary = _presentTrackBlocks.Max(e => e.transform.localPosition.z + e.GetComponent<BoxCollider>().bounds.extents.z);
            trackBackwardBoundary = _presentTrackBlocks.Min(e => e.transform.localPosition.z - e.GetComponent<BoxCollider>().bounds.extents.z);
        }
        else if (TrackRange > 0.0f)
        {
            var block = RandomGenerateBlock();
            var boxCollider = block.GetComponent<BoxCollider>();
            block.transform.localPosition = CalcInitalTrackBlockPosition(boxCollider);
            _presentTrackBlocks.Add(block);
            trackForwardBoundary = _myboat.localPosition.z + boxCollider.bounds.extents.z;
            trackBackwardBoundary = _myboat.localPosition.z - boxCollider.bounds.extents.z;
        }

        // generate block
        while (trackForwardBoundary < expectedTrackForwardBoundary)
        {
            var block = RandomGenerateBlock();
            var boxCollider = block.GetComponent<BoxCollider>();
            var space = Random.Range(_blockSpaceMin, _blockSpaceMax);
            block.transform.localPosition = CalcTrackBlockPosition(boxCollider, trackForwardBoundary, true);
            trackForwardBoundary = block.transform.localPosition.z + boxCollider.bounds.extents.z;
            _presentTrackBlocks.Add(block);
        }
        while (trackBackwardBoundary > expectedTrackBackwardBoundary)
        {
            var block = RandomGenerateBlock();
            var boxCollider = block.GetComponent<BoxCollider>();
            var space = Random.Range(_blockSpaceMin, _blockSpaceMax);
            block.transform.localPosition = CalcTrackBlockPosition(boxCollider, trackBackwardBoundary, false);
            trackBackwardBoundary = block.transform.localPosition.z - boxCollider.bounds.extents.z;
            _presentTrackBlocks.Add(block);
        }

        // adjust block sink depth
        foreach (var block in _presentTrackBlocks)
        {
            var distInZ = block.transform.localPosition.z - myboatPos.z;
            distInZ = Mathf.Abs(distInZ);
            var sinkDepth = distInZ / _blockSinkDistDepthRatio;
            var pos = block.transform.localPosition;
            pos.y = -sinkDepth;
            block.transform.localPosition = pos;
        }
    }

    Vector3 CalcInitalTrackBlockPosition(BoxCollider boxCollider)
    {
        return new Vector3(
            LeftSide ? -boxCollider.bounds.extents.x : boxCollider.bounds.extents.x,
            0.0f,
            _myboat.localPosition.z);
    }

    Vector3 CalcTrackBlockPosition(BoxCollider boxCollider, float boundary, bool isForward)
    {
        var space = Random.Range(_blockSpaceMin, _blockSpaceMax);
        return new Vector3(
            LeftSide ? -boxCollider.bounds.extents.x : boxCollider.bounds.extents.x,
            0.0f,
            isForward ? boundary + boxCollider.bounds.extents.z + space : boundary - boxCollider.bounds.extents.z - space);
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
