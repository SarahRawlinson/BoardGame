using System.Collections.Generic;
using BoardGame.UI;
using UnityEngine;
using UnityEngine.Serialization;
// ReSharper disable Unity.PreferNonAllocApi

namespace BoardGame.Board
{
    public class RandomPositions : MonoBehaviour
{
    class Position
    {
        public readonly int Index;
        public readonly GameObject PositionGameObject;
        public readonly Transform TransformPos;

        public Position(int i, GameObject gameObject, Transform transform)
        {
            Index = i;
            PositionGameObject = gameObject;
            TransformPos = transform;
        }
    }
    [SerializeField] private GameObject startPosition;

    [FormerlySerializedAs("endPositions")] [SerializeField] private GameObject endPosition;

    [SerializeField] private int numberOfPositions = 10;
    [SerializeField] private GameObject positionPrefab;
    [SerializeField] private float padding = 10f;
    [SerializeField] private int tryAttemptsToCreate = 50;
    private int _actualPositionsLength;
    private readonly List<Position> _positionsList = new List<Position>();
    private readonly List<GameObject> _positions = new List<GameObject>();
    private Transform[] _transformsPositions;
    private LineRenderer _lineRenderer;

    public Vector3 NextPosition(int moves)
    {
        return _transformsPositions[moves].position;
    }

    void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        CreatePositions();
        SortPositions();
        CreateLineTransforms();
        SetUpLine(_transformsPositions);
    }

    private void CreateLineTransforms()
    {
        _transformsPositions = new Transform[_actualPositionsLength];
        foreach (Position position in _positionsList)
        {
            position.PositionGameObject.GetComponent<PositionUI>().SetLabel((position.Index + 1).ToString());
            //Debug.Log($"position {position.index.ToString()}");
            _transformsPositions[position.Index] = position.TransformPos;
        }
    }

    private void SortPositions()
    {
        
        List<GameObject> positionsToAllocate = _positions;
        List<GameObject> positionsAllocated = new List<GameObject>();
        GameObject startPos = startPosition;
        _positionsList.Add(new Position(0, startPos, startPos.transform));
        positionsAllocated.Add(startPos);
        GameObject endPos = endPosition;
        _positionsList.Add(new Position(_actualPositionsLength-1, endPos, endPos.transform));
        positionsAllocated.Add(endPos);
        bool allocationComplete = false;
        int startIndex = 0;
        int endIndex = _actualPositionsLength-1;
        while (!allocationComplete)
        {
            GameObject closestToStart = positionsToAllocate[0];
            GameObject closestToEnd = positionsToAllocate[^1];
            foreach (GameObject pos in positionsToAllocate)
            {
                if (Vector3.Distance(pos.transform.position, startPos.transform.position) <
                    Vector3.Distance(closestToStart.transform.position, startPos.transform.position)
                    && pos != closestToEnd)
                {
                    closestToStart = pos;
                    continue;
                }
                if (Vector3.Distance(pos.transform.position, endPos.transform.position) <
                    Vector3.Distance(closestToEnd.transform.position, endPos.transform.position)
                    && pos != closestToStart)
                {
                    closestToEnd = pos;
                }
                
            }
            startPos = closestToStart;
            endPos = closestToEnd;
            positionsAllocated.Add(closestToStart);
            positionsAllocated.Add(closestToEnd);
            positionsToAllocate.Remove(closestToStart);
            positionsToAllocate.Remove(closestToEnd);
            startIndex += 1;
            endIndex -= 1;
            _positionsList.Add(new Position(startIndex,closestToStart,closestToStart.transform));
            _positionsList.Add(new Position(endIndex,closestToEnd,closestToEnd.transform));

            if (positionsAllocated.Count == _actualPositionsLength) allocationComplete = true;
            if (positionsAllocated.Count == _actualPositionsLength - 1)
            {
                _positionsList.Add(new Position(startIndex + 1,positionsToAllocate[0],positionsToAllocate[0].transform));
                allocationComplete = true;
            }
        }
    }


    void SetUpLine(Transform[] points)
    {
        _lineRenderer.positionCount = points.Length;
        DrawLines(points);
        
    }

    void DrawLines(Transform[] points)
    {
        for (int i = 0; i < points.Length; i++)
        {
            Vector3 pos = points[i].position;
            pos.y += 1;
            _lineRenderer.SetPosition(i,pos);
        }
    }

    private void CreatePositions()
    {
        Terrain terrain = GetComponent<Terrain>();
        Vector3 position = terrain.transform.position;
        var terrainData = terrain.terrainData;
        float minX = position.x + terrainData.size.x;
        float maxX = position.x - ((position.x) + terrainData.size.x);
        float minZ = position.z + terrainData.size.z;
        float maxZ = position.z - ((position.z) + terrainData.size.z);
        startPosition.transform.position =  RandomTerrainPosition(minX, maxX, minZ, maxZ, terrain);
        endPosition.transform.position =  RandomTerrainPosition(minX, maxX, minZ, maxZ, terrain);
        int i;
        for (i = 0; i < (numberOfPositions - 2); i++)
        {
            bool noCollisions = false;
            int countTry = 0;
            while (!noCollisions && countTry < tryAttemptsToCreate)
            {
                Vector3 vector3 = RandomTerrainPosition(minX, maxX, minZ, maxZ, terrain);
                Collider[] colliders = Physics.OverlapSphere(vector3, padding * 2);
                noCollisions = true;
                foreach (Collider colliderCollision in colliders)
                {
                    if (!colliderCollision.TryGetComponent(out Terrain _))
                    {
                        noCollisions = false;
                        break;
                    }
                }
                if (noCollisions)
                {
                    GameObject newPosition = Instantiate(positionPrefab, vector3, Quaternion.identity);
                    _positions.Add(newPosition);
                    Debug.Log("position added");
                }
                else
                {
                    //Debug.Log($"interaction on position {i.ToString()} try {countTry.ToString()}");
                }
                countTry += 1;
            }
        }
        _actualPositionsLength = _positions.Count + 2;
        Debug.Log((_actualPositionsLength).ToString());
    }

    private static Vector3 RandomTerrainPosition(float minX, float maxX, float minZ, float maxZ, Terrain terrain)
    {
        float randX = Random.Range(minX, minX + maxX);
        float randZ = Random.Range(minZ, minZ + maxZ);
        Vector2 vector2 = new Vector2(randX, randZ);
        Vector3 vector3 = new Vector3(randX, GetHeightWorldCoords(terrain.terrainData, vector2), randZ);
        return vector3;
    }

    private static float GetHeightWorldCoords(TerrainData terrainData,Vector2 point)
    {
        Vector3 scale=terrainData.heightmapScale;
        return terrainData.GetHeight((int)(point.x/scale.x),(int)(point.y/scale.z));
    }

    public int GetPosition(int playerPosition)
    {
        if (playerPosition >= _transformsPositions.Length) return _transformsPositions.Length - 1;
        return playerPosition;
    }
}
}

