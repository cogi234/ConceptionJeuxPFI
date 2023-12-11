using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreMesh : MonoBehaviour
{
    [SerializeField] Vector2Int numVertices;
    [Header("Waves")]
    [SerializeField] float amplitude;
    [SerializeField] Vector3 timeFrequency;
    [SerializeField] Vector3 frequency;
    [SerializeField] Vector3 offset;

    Mesh mesh;
    const float radius = 0.5f;

    private void Awake()
    {
        Generate();
    }

    private void Update()
    {
        mesh.vertices = GetVertices();
        mesh.RecalculateNormals();
    }

    public void Generate()
    {
        mesh = new Mesh();
        mesh.name = "Generated Core";
        mesh.vertices = GetVertices();
        mesh.triangles = GetTriangles();
        //mesh.uv = GetUvs();
        mesh.RecalculateNormals();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    Vector3[] GetVertices()
    {
        Vector3[] vertices = new Vector3[numVertices.x * numVertices.y + 2];

        float bottomDeformation = 1;
        bottomDeformation += amplitude * Mathf.Sin(offset.x + (Time.time * timeFrequency.x) + (0 * frequency.x)) / 3;
        bottomDeformation += amplitude * Mathf.Sin(offset.y + (Time.time * timeFrequency.y) + (-radius * frequency.y)) / 3;
        bottomDeformation += amplitude * Mathf.Sin(offset.z + (Time.time * timeFrequency.z) + (0 * frequency.z)) / 3;
        vertices[0] = new Vector3(0, -radius, 0) * bottomDeformation;// Bottom

        //Angles are in radians
        //v for vertical and h for horizontal
        for (int vIndex = 1; vIndex <= numVertices.y; vIndex++)
        {
            float vAngle = (Mathf.PI * (vIndex / ((float)numVertices.y + 1))) - (Mathf.PI / 2);
            for (int hIndex = 0; hIndex < numVertices.x; hIndex++)
            {
                int index = GetIndex(hIndex, vIndex - 1);

                float hAngle = (2 * Mathf.PI) * (hIndex / (float)numVertices.x);
                float x = Mathf.Cos(vAngle) * Mathf.Cos(hAngle);
                float y = Mathf.Sin(vAngle);
                float z = Mathf.Cos(vAngle) * Mathf.Sin(hAngle);

                float deformation = 1;
                deformation += amplitude * Mathf.Sin(offset.x + (Time.time * timeFrequency.x) + (x * frequency.x)) / 3;
                deformation += amplitude * Mathf.Sin(offset.y + (Time.time * timeFrequency.y) + (y * frequency.y)) / 3;
                deformation += amplitude * Mathf.Sin(offset.z + (Time.time * timeFrequency.z) + (z * frequency.z)) / 3;

                Vector3 position = new Vector3(x, y, z) * radius * deformation;

                vertices[index] = position;
            }
        }

        float topDeformation = 1;
        topDeformation += amplitude * Mathf.Sin(offset.x + (Time.time * timeFrequency.x) + (0 * frequency.x)) / 3;
        topDeformation += amplitude * Mathf.Sin(offset.y + (Time.time * timeFrequency.y) + (radius * frequency.y)) / 3;
        topDeformation += amplitude * Mathf.Sin(offset.z + (Time.time * timeFrequency.z) + (0 * frequency.z)) / 3;
        vertices[numVertices.x * numVertices.y + 1] = new Vector3(0, radius, 0) * topDeformation;// Top

        return vertices;
    }

    int[] GetTriangles()
    {
        List<int> triangles = new List<int>();

        //Bottom
        for (int i = 1; i <= numVertices.x; i++)
        {
            triangles.AddRange(new int[]
            {
                0, i, (i % numVertices.x) + 1
            });
        }
        
        //Middle
        for (int vIndex = 0; vIndex < numVertices.y - 1; vIndex++)
        {
            for (int hIndex = 0; hIndex < numVertices.x; hIndex++)
            {
                int bottomLeft = GetIndex(hIndex, vIndex);
                int bottomRight = GetIndex((hIndex + 1) % numVertices.x, vIndex);
                int topLeft = GetIndex(hIndex, (vIndex + 1) % numVertices.y);
                int topRight = GetIndex((hIndex + 1) % numVertices.x, (vIndex + 1) % numVertices.y);

                triangles.AddRange(new int[]
                {
                    bottomLeft, topLeft, topRight,
                    bottomLeft, topRight, bottomRight
                });
            }
        }
        
        //Top
        int topIndex = numVertices.x * numVertices.y + 1;
        for (int i = 1; i <= numVertices.x; i++)
        {
            triangles.AddRange(new int[]
            {
                topIndex, topIndex - i, topIndex - ((i % numVertices.x) + 1)
            });
        }

        return triangles.ToArray();
    }

    Vector2[] GetUvs()
    {
        return new Vector2[numVertices.x * numVertices.y + 2];
    }

    int GetIndex(int hIndex, int vIndex)
    {
        return vIndex * numVertices.x + hIndex + 1;
    }
}
