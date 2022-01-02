using System.Collections;
using System.Collections.Generic;
using BoardGame.UI;
using UnityEngine;
using UnityEngine.Serialization;

namespace BoardGame.Board
{
    public class RandomPositions : MonoBehaviour
{
    class Position
    {
        public int index;
        public GameObject positionGameObject;
        public Transform transformPos;

        public Position(int i, GameObject gameObject, Transform transform)
        {
            index = i;
            positionGameObject = gameObject;
            transformPos = transform;
        }
    }
    [SerializeField] private GameObject startPosition;

    [FormerlySerializedAs("endPositions")] [SerializeField] private GameObject endPosition;

    [SerializeField] private int numberOfPositions = 10;
    [SerializeField] private GameObject positionPrefab;
    [SerializeField] private float padding = 10f;
    [SerializeField] private int tryAttemptsToCreate = 50;
    private int actualPositionsLength = 0;
    private List<Position> positionsList = new List<Position>();
    private List<GameObject> positions = new List<GameObject>();
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
        _transformsPositions = new Transform[actualPositionsLength];
        foreach (Position position in positionsList)
        {
            position.positionGameObject.GetComponent<PositionUI>().SetLabel((position.index + 1).ToString());
            //Debug.Log($"position {position.index.ToString()}");
            _transformsPositions[position.index] = position.transformPos;
        }
    }

    private void SortPositions()
    {
        
        List<GameObject> positionsToAllocate = positions;
        List<GameObject> positionsAllocated = new List<GameObject>();
        GameObject startPos = startPosition;
        positionsList.Add(new Position(0, startPos, startPos.transform));
        positionsAllocated.Add(startPos);
        GameObject endPos = endPosition;
        positionsList.Add(new Position(actualPositionsLength-1, endPos, endPos.transform));
        positionsAllocated.Add(endPos);
        bool alocationComplete = false;
        int startIndex = 0;
        int endIndex = actualPositionsLength-1;
        while (!alocationComplete)
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
            positionsList.Add(new Position(startIndex,closestToStart,closestToStart.transform));
            positionsList.Add(new Position(endIndex,closestToEnd,closestToEnd.transform));

            if (positionsAllocated.Count == actualPositionsLength) alocationComplete = true;
            if (positionsAllocated.Count == actualPositionsLength - 1)
            {
                positionsList.Add(new Position(startIndex + 1,positionsToAllocate[0],positionsToAllocate[0].transform));
                alocationComplete = true;
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
        float MinX = position.x + terrain.terrainData.size.x;
        float MaxX = position.x - ((position.x) + terrain.terrainData.size.x);
        float MinZ = position.z + terrain.terrainData.size.z;
        float MaxZ = position.z - ((position.z) + terrain.terrainData.size.z);
        startPosition.transform.position =  RandomTerrainPosition(MinX, MaxX, MinZ, MaxZ, terrain);
        endPosition.transform.position =  RandomTerrainPosition(MinX, MaxX, MinZ, MaxZ, terrain);
        int i;
        for (i = 0; i < (numberOfPositions - 2); i++)
        {
            bool noCollisions = false;
            int countTry = 0;
            while (!noCollisions && countTry < tryAttemptsToCreate)
            {
                Vector3 vector3 = RandomTerrainPosition(MinX, MaxX, MinZ, MaxZ, terrain);
                Collider[] colliders = Physics.OverlapSphere(vector3, padding * 2);
                noCollisions = true;
                foreach (Collider collider in colliders)
                {
                    if (!collider.TryGetComponent(out Terrain hitTerrain))
                    {
                        noCollisions = false;
                        break;
                    }
                }
                if (noCollisions)
                {
                    GameObject newPosition = Instantiate(positionPrefab, vector3, Quaternion.identity);
                    positions.Add(newPosition);
                    Debug.Log("position added");
                }
                else
                {
                    //Debug.Log($"interaction on position {i.ToString()} try {countTry.ToString()}");
                }
                countTry += 1;
            }
        }
        actualPositionsLength = positions.Count + 2;
        Debug.Log((actualPositionsLength).ToString());
    }

    private static Vector3 RandomTerrainPosition(float MinX, float MaxX, float MinZ, float MaxZ, Terrain terrain)
    {
        float randX = UnityEngine.Random.Range(MinX, MinX + MaxX);
        float randZ = UnityEngine.Random.Range(MinZ, MinZ + MaxZ);
        Vector2 vector2 = new Vector2(randX, randZ);
        Vector3 vector3 = new Vector3(randX, GetHeightWorldCoords(terrain.terrainData, vector2), randZ);
        return vector3;
    }

    public static float GetHeightWorldCoords(TerrainData terrainData,Vector2 point)
    {
        Vector3 scale=terrainData.heightmapScale;
        return (float)terrainData.GetHeight((int)(point.x/scale.x),(int)(point.y/scale.z));
    }

    public int GetPosition(int playerPosition)
    {
        if (playerPosition >= _transformsPositions.Length) return _transformsPositions.Length - 1;
        return playerPosition;
    }
}
}

