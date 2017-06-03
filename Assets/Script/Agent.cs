using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AL.ALUtil;

[System.Serializable]
public struct Point
{
    public int x;
    public int y;

    public Point(int x, int y)
    {
        this.x = x;
        this.y = y;
    }
}

public class Agent : MonoBehaviour {

    [SerializeField]
    private float _weight = 1f;
    public float weight { get { return _weight; } }

    [SerializeField]
    private bool _isMove = true;

    public Tile startPoint { set; get; }
    public Tile endPoint { set; get; }
    public Tile[,] mapData { set; get; }

    private List<Tile> _openList;
    private List<Tile> _closeList;
    private List<Tile> _path;

    private int _width = 0;
    private int _height = 0;
    private Point[] _directs =
    {
        new Point(1, 0), new Point(-1, 0), new Point(0, 1), new Point(0, -1),
        new Point(1, 1), new Point(-1, 1), new Point(1, -1), new Point(-1, -1)
    };

    private void Awake()
    {
        _openList = new List<Tile>();
        _closeList = new List<Tile>();
        _path = new List<Tile>();

        _width = FindObjectOfType<MapGenerator>().mapWidth;
        _height = FindObjectOfType<MapGenerator>().mapHeight;
    }

    private void Start()
    {
        SetTile();
        FindPath();
        PathHighligh();

        if (_isMove)
            StartCoroutine("Move");
    }

    void SetTile()
    {
        Tile init = mapData[startPoint.index.x, startPoint.index.y];
        init.g = 0;
        init.h = Mathf.Abs(endPoint.index.x - startPoint.index.x) + Mathf.Abs(endPoint.index.y - startPoint.index.y);
        init.f = init.g + init.h;
        mapData[startPoint.index.x, startPoint.index.y] = init;

        _openList.Add(init);
        while(_openList.Count > 0)
        {
            Tile tile = _openList[0];
            for (int i = 0; i < _openList.Count; ++i)
            {
                if (_openList[i].f < tile.f)
                    tile = _openList[i];
            }

            if (tile.type.Equals(TILE_TYPE.END))
                break;

            _openList.Remove(tile);
            _closeList.Add(tile);
            AddNearTile(tile);
        }
    }

    private void AddNearTile(Tile centerTile)
    {
        for (int i = 0; i < _directs.Length; ++i)
        {
            Point point = new Point(centerTile.index.x + _directs[i].x, centerTile.index.y + _directs[i].y);

            if ((point.x < 0 || point.x >= _width || point.y < 0 || point.y >= _height) || mapData[point.x, point.y].type.Equals(TILE_TYPE.WALL))
                continue;

            Tile tile = mapData[point.x, point.y];
            if (_closeList.Contains(tile))
                continue;

            if (!_openList.Contains(tile))
            {
                tile.g = centerTile.g + _weight;
                tile.h = Mathf.Abs(endPoint.index.x - point.x) + Mathf.Abs(endPoint.index.y - point.y);
                tile.f = tile.g + tile.h;
                tile.nextTile = centerTile;
                mapData[point.x, point.y] = tile;
                _openList.Add(tile);
            }
            else if (mapData[point.x, point.y].g > centerTile.g + 1)
            {
                tile.g = centerTile.g + _weight;
                tile.f = tile.g + tile.h;
                tile.nextTile = centerTile;
            }
        }
    }

    private void FindPath()
    {
        Tile tile = mapData[endPoint.index.x, endPoint.index.y];
        while (tile != null)
        {
            _path.Add(tile);

            if (tile.nextTile.Equals(startPoint))
                break;

            tile = tile.nextTile;
        }
        _path.Reverse();
    }

    private void PathHighligh()
    {
        for (int i = 0; i < _path.Count; ++i)
        {
            Debug.Log(_path[i].name);
            _path[i].SetPathColor();
        }
    }

    private IEnumerator Move()
    {
        int indexCount = 0;
        float timer = 0f;
        Tile currentTile = _path[indexCount];
        while(true)
        {
            Vector3 targetPosition = new Vector3(currentTile.transform.position.x, 1f, currentTile.transform.position.z);

            if (targetPosition.Equals(transform.position))
            {
                if (indexCount > _path.Count)
                    break;

                currentTile = _path[(indexCount + 1)];
                timer = 0f;
            }
            timer += Time.deltaTime;
            Debug.Log(timer);
            transform.position = ALLerp.Lerp(transform.position, targetPosition, timer);
            yield return null;
        }
    }
}