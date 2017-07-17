using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LineStream : MonoBehaviour {
    
    public List<Vector3> points;
    private Vector3 center = new Vector3();

    public Plane getPlaneFromPoints(List<Vector3> _points)
    {
        int n = points.Count;
        Vector3 sum = new Vector3();
        foreach (Vector3 point in points)
        {
            sum += point;
        }

        Vector3 centroid = sum * (1.0f / ((float)n));

        float xx = 0, xy = 0, xz = 0;
        float yy = 0, yz = 0, zz = 0;

        foreach (Vector3 point in points)
        {
            Vector3 r = point - centroid;
            xx += r.x * r.x;
            xy += r.x * r.y;
            xz += r.x * r.z;
            yy += r.y * r.y;
            yz += r.y * r.z;
            zz += r.z * r.z;
        }

        float det_x = yy * zz - yz * yz;
        float det_y = xx * zz - xz * xz;
        float det_z = xx * yy - xy * xy;

        float det_max = Mathf.Max(det_x, det_y, det_z);

        Vector3 normal = new Vector3();

        if (det_max == det_x)
        {
            float a = (xz * yz - xy * zz) / det_x;
            float b = (xy * yz - xz * yy) / det_x;

            normal = new Vector3(1.0f, a, b);
        }
        else if (det_max == det_y)
        {
            float a = (yz * xz - xy * zz) / det_y;
            float b = (xy * yz - xz * yy) / det_y;

            normal = new Vector3(x: a, y: 1.0f, z: b);
        }
        else
        {
            float a = (yz * xy - xz * yy) / det_z;
            float b = (xz * xy - yz * xx) / det_z;

            normal = new Vector3(x: a, y: b, z: 1.0f);
        }

        Plane plane = new Plane(normal.normalized, centroid);
        center = centroid;

        return plane;
    }

    public void OnDrawGizmos()
    {
        Plane plane = getPlaneFromPoints(points);
        Gizmos.DrawRay(new Ray(center, plane.normal));
        //Gizmos.DrawCube(-1 * plane.distance * plane.normal, new Vector3(1.0f, 1.0f, .01f));
    }

}