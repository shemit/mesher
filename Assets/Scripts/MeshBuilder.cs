using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class MeshBuilder : MonoBehaviour {

    public List<LineStream> lineStreams;

    void Awake()
    {

    }
   
    public void BuildMesh()
    {

        int meshResolution = lineStreams[0].points.Count;
        float uvSize = 1f / meshResolution;
        Vector3[] vertices = new Vector3[meshResolution * 2];
        int[] triangles = new int[meshResolution * 2 * 3];
        Vector3[] normals = new Vector3[meshResolution * 2];
        Vector2[] uvs = new Vector2[meshResolution * 2];

        for (int j = 0; j < lineStreams.Count; j++)
        {
            LineStream lineStream = lineStreams[j];
            for (int i = 0; i < lineStream.points.Count; i++)
            {
                // Determine how to find the most appropriate point to connect
                Vector3 point = lineStream.points[i];
                vertices[j * meshResolution + i] = point;
                uvs[j * meshResolution + i] = new Vector2(i * uvSize, j);
            }
        }

        for (int i = 0; i < meshResolution; i++)
        {
            int vtx1 = i;
            int vtx2 = (i + 1) % meshResolution;
            int vtx3 = meshResolution + i;
            int vtx4 = meshResolution + ((i + 1) % meshResolution);

            triangles[6 * i] = vtx1;
            triangles[6 * i + 1] = vtx3;
            triangles[6 * i + 2] = vtx4;


            Plane plane = new Plane(
                vertices[vtx1],
                vertices[vtx3],
                vertices[vtx4]
            );

            normals[2 * i] = plane.normal;

            triangles[6 * i + 3] = vtx1;
            triangles[6 * i + 4] = vtx4;
            triangles[6 * i + 5] = vtx2;

            plane = new Plane(
                vertices[vtx1],
                vertices[vtx4],
                vertices[vtx2]
            );

            normals[2 * i + 1] = plane.normal;
        }


        Mesh mesh = GetComponent<MeshFilter>().sharedMesh;
        mesh.Clear();
        
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.normals = normals;
        mesh.uv = uvs;
        mesh.name = "Custom Mesh";
    }

    // Use this for initialization
    void Start () {
    }
	
	// Update is called once per frame
	void Update () {

        BuildMesh();
    }
}
