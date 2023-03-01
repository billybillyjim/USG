using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleMeshGenerator : MonoBehaviour
{
    public float radius = 1f;
    public int segments = 32;
    public Vector2 center1;
    public Vector2 center2;
    public Material material;

    void Start()
    {
        // Create meshes for each circle
        Mesh circleMesh1 = CreateCircleMesh(center1, radius, segments);
        Mesh circleMesh2 = CreateCircleMesh(center2, radius, segments);

        // Combine the two circle meshes into one
        CombineInstance[] combine = new CombineInstance[2];
        combine[0].mesh = circleMesh1;
        combine[0].transform = transform.localToWorldMatrix;
        combine[1].mesh = circleMesh2;
        combine[1].transform = transform.localToWorldMatrix;

        Mesh combinedMesh = new Mesh();
        combinedMesh.CombineMeshes(combine, true, true);

        // Fill in the mesh between the circles
        Vector3[] vertices = combinedMesh.vertices;
        int[] triangles = new int[(segments + 1) * 6];
        int triangleIndex = 0;

        for (int i = 0; i < segments; i++)
        {
            int currentIndex = i * 2;
            int nextIndex = (i + 1) * 2 % (segments * 2);

            triangles[triangleIndex++] = currentIndex;
            triangles[triangleIndex++] = currentIndex + 1;
            triangles[triangleIndex++] = nextIndex;

            triangles[triangleIndex++] = currentIndex + 1;
            triangles[triangleIndex++] = nextIndex + 1;
            triangles[triangleIndex++] = nextIndex;
        }

        combinedMesh.triangles = triangles;

        // Create a game object to display the combined mesh
        GameObject circleObject = new GameObject("CircleMesh");
        circleObject.AddComponent<MeshFilter>().mesh = combinedMesh;
        circleObject.AddComponent<MeshRenderer>().material = material;
    }

    Mesh CreateCircleMesh(Vector2 center, float radius, int segments)
    {
        Mesh mesh = new Mesh();

        Vector3[] vertices = new Vector3[segments * 2];
        int[] triangles = new int[segments * 3 * 2];

        for (int i = 0; i < segments; i++)
        {
            float angle = i * Mathf.PI * 2f / segments;

            float x = center.x + radius * Mathf.Cos(angle);
            float y = center.y + radius * Mathf.Sin(angle);

            vertices[i * 2] = new Vector3(x, y, 0f);
            vertices[i * 2 + 1] = new Vector3(x, y, 1f);

            if (i < segments - 1)
            {
                triangles[i * 6] = i * 2;
                triangles[i * 6 + 1] = i * 2 + 1;
                triangles[i * 6 + 2] = i * 2 + 2;

                triangles[i * 6 + 3] = i * 2 + 2;
                triangles[i * 6 + 4] = i * 2 + 1;
                triangles[i * 6 + 5] = i * 2 + 3;
            }
            else
            {
                triangles[i * 6] = i * 2;
                triangles[i * 6 + 1] = i * 2 + 1;
                triangles[i * 6 + 2] = 0;

                triangles[i * 6 + 3] = 0;
                triangles[i * 6 + 4] = i * 2 + 1;
                triangles[i * 6 + 5] = 1;
            }
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;

        return mesh;
    }
}
