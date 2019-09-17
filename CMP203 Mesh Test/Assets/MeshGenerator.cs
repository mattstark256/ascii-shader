using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator : MonoBehaviour
{
    [SerializeField]
    private int subdivisions = 8;
    [SerializeField]
    private int loops = 2;
    [SerializeField]
    private float thickness = 0.5f;
    [SerializeField]
    private float radiusA = 2;
    [SerializeField]
    private float radiusB = 1;

    [SerializeField]
    private float rotateSpeed = 30;
    private float angleOffset = 0;

    private Vector3[] crossSectionPoints = {
        new Vector3(-1, -1, 0),
        new Vector3(1, -1, 0),
        new Vector3(1, 1, 0),
        new Vector3(-1, 1, 0),
    };

    void Update()
    {
        angleOffset += Time.deltaTime * rotateSpeed;

        Vector3[] vertices = new Vector3[subdivisions * 4 * 3]; // 4 per loop and 3 per corner for sharp edges
        int[] triangles = new int[subdivisions * 8 * 3];
        for (int i = 0; i < subdivisions; i++)
        {
            float angleA = 360 * loops * (float)i / subdivisions;
            float angleB = 360 * (float)i / subdivisions + angleOffset;

            for (int j = 0; j < 4; j++)
            {
                Vector3 vertex = crossSectionPoints[j] * thickness + new Vector3(radiusB * Mathf.Cos(Mathf.Deg2Rad * angleB) + radiusA, radiusB * Mathf.Sin(Mathf.Deg2Rad * angleB), 0);
                vertex.x *= 1.414f;
                vertex.z = Mathf.Sin(Mathf.Deg2Rad * angleA) * vertex.x;
                vertex.x = Mathf.Cos(Mathf.Deg2Rad * angleA) * vertex.x;
                vertices[i * 12 + j * 3] = vertex;
                vertices[i * 12 + j * 3 + 1] = vertex;
                vertices[i * 12 + j * 3 + 2] = vertex;
            }

            // Bottom
            triangles[i * 24] = i * 12;
            triangles[i * 24 + 1] = i * 12 + 3;
            triangles[i * 24 + 2] = ((i + 1) * 12) % vertices.Length;
            triangles[i * 24 + 3] = i * 12 + 3;
            triangles[i * 24 + 4] = ((i + 1) * 12 + 3) % vertices.Length;
            triangles[i * 24 + 5] = ((i + 1) * 12) % vertices.Length;

            // Top
            triangles[i * 24 + 6] = i * 12 + 6;
            triangles[i * 24 + 7] = i * 12 + 9;
            triangles[i * 24 + 8] = ((i + 1) * 12 + 6) % vertices.Length;
            triangles[i * 24 + 9] = i * 12 + 9;
            triangles[i * 24 + 10] = ((i + 1) * 12 + 9) % vertices.Length;
            triangles[i * 24 + 11] = ((i + 1) * 12 + 6) % vertices.Length;

            // Outside
            triangles[i * 24 + 12] = i * 12 + 4;
            triangles[i * 24 + 13] = i * 12 + 7;
            triangles[i * 24 + 14] = ((i + 1) * 12 + 5) % vertices.Length;
            triangles[i * 24 + 15] = i * 12 + 7;
            triangles[i * 24 + 16] = ((i + 1) * 12 + 8) % vertices.Length;
            triangles[i * 24 + 17] = ((i + 1) * 12 + 5) % vertices.Length;

            // Inside
            triangles[i * 24 + 18] = i * 12 + 10;
            triangles[i * 24 + 19] = i * 12 + 1;
            triangles[i * 24 + 20] = ((i + 1) * 12 + 11) % vertices.Length;
            triangles[i * 24 + 21] = i * 12 + 1;
            triangles[i * 24 + 22] = ((i + 1) * 12 + 2) % vertices.Length;
            triangles[i * 24 + 23] = ((i + 1) * 12 + 11) % vertices.Length;
        }

        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    //private void OnDrawGizmos()
    //{
    //    foreach (Vector3 vertex in GetComponent<MeshFilter>().mesh.vertices)
    //    {
    //        Gizmos.DrawSphere(vertex, 0.1f);
    //    }
    //}
}
