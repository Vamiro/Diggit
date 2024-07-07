using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class BaseHexagon : MonoBehaviour
{
    private Mesh _mesh;
    private MeshFilter _meshFilter;
    private MeshRenderer _meshRenderer;
    private List<Face> _faces;
    
    [SerializeField] private Material _material;
    [SerializeField, Range(0, 100)] private float _innerSize;
    [SerializeField, Range(0, 100)] private float _outerSize;
    [SerializeField, Range(0, 100)] private float _height;
    
    void Awake()
    {
        _meshFilter = GetComponent<MeshFilter>();
        _meshRenderer = GetComponent<MeshRenderer>();

        _mesh = new Mesh
        {
            name = "Hexagon"
        };

        _meshFilter.mesh = _mesh;
        _meshRenderer.material = _material;
    }

    private void OnEnable()
    {
        DrawMesh();
    }

    private void OnValidate()
    {
        if (Application.isPlaying)
        {
            DrawMesh();
        }
    }
    
    void DrawMesh()
    {
        DrawFaces();
        CombineFaces();
    }

    private void DrawFaces()
    {
        _faces = new List<Face>();
        
        //Top faces
        for (int point = 0; point < 6; point++)
        {
            _faces.Add(CreateFace(_innerSize, _outerSize, _height / 2f, _height / 2f, point));
        }
        //Bottom faces
        for (int point = 0; point < 6; point++)
        {
            _faces.Add(CreateFace(_innerSize, _outerSize, -_height / 2f, -_height / 2f, point, true));
        }
        //Outer faces
        for (int point = 0; point < 6; point++)
        {
            _faces.Add(CreateFace(_outerSize, _outerSize, _height / 2f, -_height / 2f, point, true));
        }
        //Inner faces
        for (int point = 0; point < 6; point++)
        {
            _faces.Add(CreateFace(_innerSize, _innerSize, _height / 2f, -_height / 2f, point));
        }
    }

    private void CombineFaces()
    {
        var vertices = new List<Vector3>();
        var triangles = new List<int>();
        var uvs = new List<Vector2>();
        
        for (int i = 0; i < _faces.Count; i++){
            //Add the vertices
            vertices.AddRange(_faces[i].Vertices);
            uvs.AddRange(_faces[i].UVs);
            
            //Offset the triangles
            var offset = (4 * i);
            triangles.AddRange(_faces[i].Triangles.Select(triangle => triangle + offset));
        }

        _mesh.vertices = vertices.ToArray();
        _mesh.triangles = triangles.ToArray();
        _mesh.uv = uvs.ToArray();
        _mesh. RecalculateNormals();
    }

    private Face CreateFace(float innerRadius, float outerRadius, float heightA, float heightB, int point, bool reverse = false)
    {
        Vector3 pointA = GetPoint(innerRadius, heightB, point);
        Vector3 pointB = GetPoint(innerRadius, heightB, (point < 5) ? point + 1 : 0);
        Vector3 pointC = GetPoint(outerRadius, heightA, (point < 5) ? point + 1 : 0);
        Vector3 pointD = GetPoint(outerRadius, heightA, point);
        List<Vector3> vertices = new List<Vector3>() { pointA, pointB, pointC, pointD };
        List<int> triangles = new List<int>() { 0, 1, 2, 2, 3, 0 };
        List<Vector2> uvs = new List<Vector2>() {
            new(0, 0),
            new(1, 0),
            new(1, 1),
            new(0, 1)
        };

        if (reverse)
        {
            vertices.Reverse();
        }
        
        return new Face(vertices, triangles, uvs);
    }

    private Vector3 GetPoint(float size, float height, int index)
    {
        float angleDegrees = 60 * index;
        float angleRadius = Mathf.PI / 180f * angleDegrees;
        return new Vector3(size * Mathf.Cos(angleRadius), height, size * Mathf.Sin(angleRadius));
    }
}

public struct Face
{
    public List<Vector3> Vertices { get; private set; }
    public List<int> Triangles { get; private set; }
    public List<Vector2> UVs { get; private set; }

    public Face(List<Vector3> vertices, List<int> triangles, List<Vector2> uVs)
    {
        Vertices = vertices;
        Triangles = triangles;
        UVs = uVs;
    }
}
