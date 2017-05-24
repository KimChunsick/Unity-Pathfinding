using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour {

    [SerializeField]
    private string _fileName;

    [SerializeField]
    private int _mapWidth;

    [SerializeField]
    private int _mapHeight;

    private char[,] _mapArr;

    private void Awake()
    {
        _mapArr = new char[_mapWidth, _mapHeight];
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
            if (arr[i].Contains("\r\n"))
            {
                _mapArr[width, height] = arr[i].ToCharArray()[0];
                _mapArr[0, ++height] = arr[i].ToCharArray()[3];
                width = 1;
            }
            else
            {
                _mapArr[width, height] = arr[i].ToCharArray()[0];
                ++width;
            }
        }
    }

    private void Start()
    {
        GenerateMap();
    }

    private void GenerateMap()
    {
        for (int x = 0; x < _mapWidth; ++x)
        {
            for (int y = 0; y < _mapHeight; ++y)
            {
                switch(_mapArr[x, y])
                {
                    case '0':
                        {
                            GameObject temp = Instantiate(Resources.Load<GameObject>("Rand"), new Vector3(x, 0, y), Quaternion.identity, transform);
                        }
                        break;

                    case 'W':
                        {
                            GameObject temp = Instantiate(Resources.Load<GameObject>("Rand"), new Vector3(x, 0, y), Quaternion.identity, transform);
                            GameObject temp2 = Instantiate(Resources.Load<GameObject>("Wall"), new Vector3(x, 1, y), Quaternion.identity, transform);
                        }
                        break;

                    case 'S':
                        {
                            GameObject temp = Instantiate(Resources.Load<GameObject>("StartPoint"), new Vector3(x, 0, y), Quaternion.identity, transform);
                        }
                        break;

                    case 'E':
                        {
                            GameObject temp = Instantiate(Resources.Load<GameObject>("EndPoint"), new Vector3(x, 0, y), Quaternion.identity, transform);
                        }
                        break;
                }
            }
        }
    }
}
