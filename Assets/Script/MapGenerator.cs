using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{

    [SerializeField]
    private string _fileName = "";

    [SerializeField]
    private int _mapWidth = 0;
    public int mapWidth { get { return _mapWidth; } }

    [SerializeField]
    private int _mapHeight = 0;
    public int mapHeight { get { return _mapHeight; } }

    private Tile[,] _mapData = null;

    private Tile _wall = null;
    private Tile _rand = null;
    private Tile _endPoint = null;
    private Tile _startPoint = null;
    private Agent _agent = null;


    private void Awake()
    {
        _wall = Resources.Load<Tile>("Wall");
        _rand = Resources.Load<Tile>("Rand");
        _endPoint = Resources.Load<Tile>("EndPoint");
        _startPoint = Resources.Load<Tile>("StartPoint");
        _agent = Instantiate(Resources.Load<Agent>("Agent"), Vector3.zero, Quaternion.identity, transform);

        _mapData = new Tile[_mapWidth, _mapHeight];

        ParseMap();
    }

    private void ParseMap()
    {
        string[] separator = { " ", "\r\n", "\r", "\n" };

        TextAsset text = Resources.Load<TextAsset>(_fileName);
        string[] arr = text.text.Split(separator, StringSplitOptions.RemoveEmptyEntries);

        int width = _mapWidth - 1;
        int height = 0;

        for (int i = arr.Length - 1; i >= 0; --i)
        {
            if (width < 0)
            {
                width = _mapWidth - 1;
                ++height;
            }
            Tile tile = GenerateTile(arr[i].ToCharArray()[0], width, height, i);
            _mapData[width, height] = tile;
            --width;
        }

        FindObjectOfType<Agent>().mapData = _mapData;
    }

    private Tile GenerateTile(char tileTag, int x, int y, int i)
    {
        Tile tile = null;
        switch (tileTag)
        {
            case '0':
                tile = Instantiate(_rand, new Vector3(x, 0, y), Quaternion.identity, transform);
                tile.type = TILE_TYPE.RAND;
                break;

            case 'A':
                tile = Instantiate(_startPoint, new Vector3(x, 0, y), Quaternion.identity, transform);
                tile.name = string.Format("{0}_X:{1}_Y{2}", tile.type.ToString(), x, y);
                tile.type = TILE_TYPE.RAND;

                _agent.transform.position = new Vector3(x, 1, y);
                _agent.startPoint = tile;
                break;

            case 'W':
                tile = Instantiate(_rand, new Vector3(x, 0, y), Quaternion.identity, transform);
                tile.name = string.Format("{0}_X:{1}_Y{2}", tile.type.ToString(), x, y);
                tile.type = TILE_TYPE.RAND;

                tile = Instantiate(_wall, new Vector3(x, 1, y), Quaternion.identity, transform);
                tile.type = TILE_TYPE.WALL;
                break;

            case 'E':
                tile = Instantiate(_endPoint, new Vector3(x, 0, y), Quaternion.identity, transform);
                tile.type = TILE_TYPE.END;
                _agent.endPoint = tile;
                break;

            default:
                Debug.Log("Unknwon Tag!! : " + tileTag);
                break;
        }
        tile.index = new Point(x, y);
        tile.name = string.Format("{0}_X:{1}_Y{2}", tile.type.ToString(), x, y);
        return tile;
    }

    private void Start()
    {
        transform.position = new Vector3(transform.position.x - (float)_mapWidth * 0.5f, 0, transform.position.z - (float)_mapHeight * 0.5f);
    }
}