using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{

    [SerializeField]
    private string _fileName;

    [SerializeField]
    private int _mapWidth;
    public int mapWidth { get { return _mapWidth; } }

    [SerializeField]
    private int _mapHeight;
    public int mapHeight { get { return _mapHeight; } }

    private Tile[,] _mapData;
    private Vector2 _endPointIndex;

    private Tile _wall = null;
    private Tile _rand = null;
    private Tile _endPoint = null;
    private Agent _agent = null;


    private void Awake()
    {
        _wall = Resources.Load<Tile>("Wall");
        _rand = Resources.Load<Tile>("Rand");
        _endPoint = Resources.Load<Tile>("EndPoint");
        _agent = Resources.Load<Agent>("Agent");

        _mapData = new Tile[_mapWidth, _mapHeight];

        ParseMap();
    }

    private void ParseMap()
    {
        string[] separator = { " " };

        TextAsset text = Resources.Load<TextAsset>(_fileName);
        string[] arr = text.text.Split(separator, StringSplitOptions.RemoveEmptyEntries);

        int width = 0;
        int height = 0;
        for (int i = 0; i < arr.Length; i++)
        {
            Tile tile = GenerateTile(arr[i].ToCharArray()[0], width, height, i);
            _mapData[width, height] = tile;
            if (arr[i].Contains("\r\n"))
            {
                tile = GenerateTile(arr[i].ToCharArray()[0], 0, ++height, i);
                _mapData[0, height] = tile;
                width = 1;
            }
            else
            {
                ++width;
            }
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
                tile.text.text = i.ToString();
                break;

            case 'A':
                tile = Instantiate(_rand, new Vector3(x, 0, y), Quaternion.identity, transform);
                tile.text.text = i.ToString();
                tile.type = TILE_TYPE.RAND;

                Agent agent = Instantiate(_agent, new Vector3(x, 1, y), Quaternion.identity, transform);
                agent.index = new Point(x, y);
                break;

            case 'W':
                tile = Instantiate(_rand, new Vector3(x, 0, y), Quaternion.identity, transform);
                tile.type = TILE_TYPE.RAND;
                tile.text.text = i.ToString();

                tile = Instantiate(_wall, new Vector3(x, 1, y), Quaternion.identity, transform);
                tile.type = TILE_TYPE.WALL;
                break;

            case 'E':
                tile = Instantiate(_endPoint, new Vector3(x, 0, y), Quaternion.identity, transform);
                tile.type = TILE_TYPE.END;
                _endPointIndex = new Vector2(x, y);

                FindObjectOfType<Agent>().endPoint = tile;
                break;
        }
        tile.name = string.Format("{0}_X:{1}_Y{2}", tile.type.ToString(), x, y);
        return tile;
    }

    private void Start()
    {
        transform.position = new Vector3(transform.position.x - (float)_mapWidth * 0.5f, 0, transform.position.z - (float)_mapHeight * 0.5f);
    }
}