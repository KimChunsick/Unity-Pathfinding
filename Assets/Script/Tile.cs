using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TILE_TYPE
{
    RAND,
    WALL,
    AGENT,
    END,
}

public class Tile : MonoBehaviour {

    [SerializeField]
    private TILE_TYPE _type;
    public TILE_TYPE type { set { _type = value; } get { return _type; } }

    [SerializeField]
    private Vector2 _index;
    public Vector2 index { set { _index = value; } get { return _index; } }

    [SerializeField]
    private float _weight;
    public float weight { get { return _weight; } }

    [SerializeField]
    TextMesh _text;
    public TextMesh text { get { return _text; } }

    public int f { set; get; }
    public int g { set; get; }
    public int h { set; get; }

    private MeshRenderer _renderer = null;

    private void Awake()
    {
        _renderer = GetComponent<MeshRenderer>();
    }

    public void AddedOpenList()
    {
        _renderer.material = Resources.Load<Material>("Open");
    }

    public void AddedCloseList()
    {
        _renderer.material = Resources.Load<Material>("Close");
    }
}
