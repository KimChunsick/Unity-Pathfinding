using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private Point _index;
    public Point index { set { _index = value; } get { return _index; } }

    public Tile endPoint { set; get; }
    public Tile[,] mapData { set; get; }

    private List<Tile> _openList;
    private List<Tile> _closeList;

    private int _width = 0;
    private int _height = 0;

    private void Awake()
    {
        _openList = new List<Tile>();
        _closeList = new List<Tile>();

        _width = FindObjectOfType<MapGenerator>().mapWidth;
        _height = FindObjectOfType<MapGenerator>().mapHeight;
    }

    private void Start()
    {
        AddNearTile();
    }

    private void AddNearTile()
    {
        //R
        if (_index.x < _width && !mapData[_index.x + 1, _index.y].type.Equals(TILE_TYPE.WALL))
            _openList.Add(mapData[_index.x + 1, _index.y]);
        //RU
        if (_index.x < _width && _index.y < _height && !mapData[_index.x + 1, _index.y + 1].type.Equals(TILE_TYPE.WALL))
            _openList.Add(mapData[_index.x - 1, _index.y + 1]);
        //RD
        if (_index.x < _width && _index.y > 0 && !mapData[_index.x + 1, _index.y - 1].type.Equals(TILE_TYPE.WALL))
            _openList.Add(mapData[_index.x - 1, _index.y - 1]);

        //L
        if (_index.x > 0 && !mapData[_index.x - 1, _index.y].type.Equals(TILE_TYPE.WALL))
            _openList.Add(mapData[_index.x - 1, _index.y]);
        //LU
        if (_index.x > 0 && _index.y < _height && !mapData[_index.x - 1, _index.y + 1].type.Equals(TILE_TYPE.WALL))
            _openList.Add(mapData[_index.x - 1, _index.y + 1]);
        //LD
        if (_index.x > 0 && _index.y > 0 && !mapData[_index.x - 1, _index.y - 1].type.Equals(TILE_TYPE.WALL))
            _openList.Add(mapData[_index.x - 1, _index.y - 1]);

        //D
        if (_index.y > 0 && !mapData[_index.x, _index.y - 1].type.Equals(TILE_TYPE.WALL))
            _openList.Add(mapData[_index.x, _index.y - 1]);

        //U
        if (_index.y < _height && !mapData[_index.x, _index.y + 1].type.Equals(TILE_TYPE.WALL))
            _openList.Add(mapData[_index.x, _index.y + 1]);
    }
}