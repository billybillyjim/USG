using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class CircleGenerator : MonoBehaviour
{
    public int xSize, ySize;

    public float step = 0.1f;
    private List<Vector3> vertices;
    private List<List<Vector3>> circleVerts = new List<List<Vector3>>();
    private List<int> triangles;

    private Mesh mesh;
    public int circleFidelity = 36;
    public float circleSize = 3f;
    public int verticesCount = 0;
    public int circleCount = 0;
    public int offsetModifier = 0;
    private int anglePerVert;

    public void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        mesh.name = "Procedural Grid";
        vertices = new List<Vector3>();
        vertices.Capacity = 1000;
        triangles = new List<int>();
        triangles.Capacity = 1000;
        anglePerVert = 360 / (circleFidelity);
    }
    public void GenerateCircle(Vector3 startPos)
    {

        List<Vector3> verts = new List<Vector3>();
        List<int> tris = new List<int>();
        int startIndex = vertices.Count;
        // Add the center vertex
        verts.Add(startPos);

        // Add the vertices around the circumference of the circle
        for (int i = 0; i < circleFidelity; i++)
        {
            float angle = i * 2 * Mathf.PI / circleFidelity;
            float x = Mathf.Cos(angle) * circleSize;
            float y = Mathf.Sin(angle) * circleSize;
            verts.Add(new Vector3(x, y, 0) + startPos);
        }

        // Add the triangles
        for (int i = 1; i < circleFidelity; i++)
        {
            tris.Add(startIndex);
            tris.Add(i + 1 + startIndex);
            tris.Add(i + startIndex);
        }

        // Close the circle
        tris.Add(startIndex);
        tris.Add(1 + startIndex);
        tris.Add(circleFidelity + startIndex);

        vertices.AddRange(verts);
        triangles.AddRange(tris);

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();

        circleCount++;
    }

    private int nfmod(int k, int n) { return ((k %= n) < 0) ? k + n : k; }

    public void GenerateLineSegment(Vector3 startPos, Vector3 endPos)
    {
        int verts = vertices.Count;

        GenerateCircle(endPos);

        var angle = nfmod((int)(Mathf.Rad2Deg * -Mathf.Atan2(endPos.x - startPos.x, endPos.y - startPos.y)) + 90 , 360);

        //Debug.Log("__________________");

        var cf = (circleFidelity / 4);
        var s1 = Mathf.FloorToInt(Mathf.Min(360 - anglePerVert, (angle + anglePerVert / 2)) / anglePerVert) + verts;
        Debug.Log((angle + anglePerVert / 2));
        if (s1 >= circleFidelity * (circleCount - 1))
        {
            s1 -= circleFidelity;
        }
        
        lastClosest = s1;
        var s1Start = s1 + cf;
        var s1End = s1 - cf;

        if(s1Start >= (circleFidelity + 1) * (circleCount - 1))
        {
            s1Start -= circleFidelity;
        }

        if(s1End <= (circleFidelity + 1) * (circleCount - 2))
        {
            s1End += circleFidelity;
        }

        lastLeft = s1Start;
        lastRight = s1End;

        var s2 = s1 + (circleFidelity / 2);
        var s2Start = s1Start + circleFidelity + 1;
        var s2End = s1End + circleFidelity + 1;

        List<int> tris = new List<int>();

        for(int i = 0; i < circleFidelity / 2; i++)
        {
            int start = s1Start - i;
            int next = start - 1;

            int secondStart = s1Start + circleFidelity + 1 + i;
            int secondNext = secondStart + 1;

            if (start <= (circleFidelity + 1) * (circleCount - 2))
            {
                start += circleFidelity;
            }
            if(next <= (circleFidelity + 1) * (circleCount - 2))
            {
                next += circleFidelity;
            }
            if(secondStart >= (circleFidelity + 1) * (circleCount))
            {
                secondStart -= circleFidelity;
            }
            if(secondNext >= (circleFidelity + 1) * (circleCount))
            {
                secondNext -= circleFidelity;
            }

            tris.Add(start);
            tris.Add(secondStart);
            tris.Add(next);

            tris.Add(secondStart);
            tris.Add(next);
            tris.Add(secondNext);
        }

        triangles.AddRange(tris);
        mesh.triangles = triangles.ToArray();

    }

    int lastClosest = -1;
    int lastLeft = -1;
    int lastRight = -1;

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;
        Gizmos.color = Color.black;
        for (int i = 0; i < vertices.Count; i++)
        {
            if (i == lastClosest)
            {
                Gizmos.color = Color.white;
            }
            else if(i == lastLeft)
            {
                Gizmos.color = new Color(0.6f, 0.6f, 0.6f);
            }
            else if(i == lastRight)
            {
                Gizmos.color = new Color(0.9f, 0.9f, 0.9f);
            }
            else if(i == 0)
            {
                Gizmos.color = Color.black;
            }
            else if(i == 1)
            {
                Gizmos.color = Color.blue;
            }
            else if(i == 2)
            {
                Gizmos.color = Color.red;
            }
            else if(i == 3)
            {
                Gizmos.color = Color.green;
            }
            else if (i == 4)
            {
                Gizmos.color = Color.cyan;
            }
            else if (i == 5)
            {
                Gizmos.color = new Color(0.8f, 0.1f, 0.8f);
            }
            else if (i == 6)
            {
                Gizmos.color = Color.magenta;
            }
            else if (i == 7)
            {
                Gizmos.color = new Color(0.8f, 0.8f, 0.1f);
            }
            else if (i == 8)
            {
                Gizmos.color = Color.yellow;
            }
            else if (i == 9)
            {
                Gizmos.color = new Color(0.8f, 0.5f, 0);
            }
            else
            {
                Gizmos.color = new Color(0.3f, 0.3f, 0.7f);
            }


            Gizmos.DrawSphere(vertices[i], 0.01f);

        }

        
    }
}
