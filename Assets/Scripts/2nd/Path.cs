using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Для отображения пути используется компонент Line Renderer. Что бы отобразить изменения в пути можно двигать точки A или C.
/// </summary>

class Path : MonoBehaviour, IPathFinder
{
    public LineRenderer renderer;

    public Transform A, C;
    public List<Edge> _edges = new List<Edge>();

    public IEnumerable<Vector2> GetPath(Vector2 A, Vector2 C, IEnumerable<Edge> edges)
    {
        var path = new List<Vector2>();

        if(A == C) return path;
        if(edges == null) return path;

        var enumerator = edges.GetEnumerator();
        var currentPoint = A;
        var targetPoint = C;

        path.Add(A);

        while(enumerator.MoveNext())
        {
            var edge = enumerator.Current;

            if(edge.Start == null || edge.End == null) return new List<Vector2>();
            if(edge.Second.Min == null || edge.Second.Max == null) return new List<Vector2>();

            var mid = GetMidPoint(edge.Start.position, edge.End.position);

            var directionMid = FindDirection(currentPoint, mid);
            var targetDirectionMid = FindDirection(targetPoint, mid + GetRectCenter(edge.Second) / 2f);

            if(IsLineIntersectLine(out var pointIntersection, currentPoint, directionMid, targetPoint, targetDirectionMid)
                && IsPointInsideRectangle(pointIntersection, edge.Second))
            {
                if(LineSegmentsIntersection(currentPoint, targetPoint, edge.Start.position, edge.End.position, out var edgeIntersection))
                {
                    path.Add(targetPoint);
                    break;
                }
                else
                {
                    path.Add(pointIntersection);
                    currentPoint = pointIntersection;
                }

            }
            else
            {
                currentPoint = mid;
                path.Add(mid);
            }
        }

        if(!path.Contains(C)) path.Add(C);

        return path;
    }

    private Vector3 GetRectCenter(Rectangle r) => (r.Max.position - r.Min.position) / 2f;

    private Vector3 FindDirection(Vector3 point, Vector3 target)
        => (point - target) / (point - target).magnitude;

    private bool IsPointInsideRectangle(Vector2 point, Rectangle rectangle)
        => point.x >= rectangle.Min.position.x && point.y >= rectangle.Min.position.y
        && point.x <= rectangle.Max.position.x && point.y <= rectangle.Max.position.y;

    private Vector3 GetMidPoint(Vector3 A, Vector3 B)
        => Vector3.Lerp(A, B, 0.5f);

    private void OnDrawGizmos()
    {
        var path = GetPath(A.position, C.position, _edges);

        var index = 0;

        foreach(var item in path)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(item, .1f);

            renderer.positionCount = index + 1;
            renderer.SetPosition(index, item);

            index++;
        }
    }

    public bool IsLineIntersectLine(out Vector3 intersection, Vector3 lp1, Vector3 ld1, Vector3 lp2, Vector3 ld2)
    {
        Vector3 lineVec3 = lp2 - lp1;
        Vector3 crossVec1and2 = Vector3.Cross(ld1, ld2);
        Vector3 crossVec3and2 = Vector3.Cross(lineVec3, ld2);

        float planarFactor = Vector3.Dot(lineVec3, crossVec1and2);

        if(Mathf.Abs(planarFactor) < 0.0001f && crossVec1and2.sqrMagnitude > 0.0001f)
        {
            float s = Vector3.Dot(crossVec3and2, crossVec1and2) / crossVec1and2.sqrMagnitude;
            intersection = lp1 + (ld1 * s);
            return true;
        }
        else
        {
            intersection = Vector3.zero;
            return false;
        }
    }

    public bool LineSegmentsIntersection(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4, out Vector2 intersection)
    {
        intersection = Vector2.zero;

        var d = (p2.x - p1.x) * (p4.y - p3.y) - (p2.y - p1.y) * (p4.x - p3.x);

        if(d == 0.0f) return false;

        var u = ((p3.x - p1.x) * (p4.y - p3.y) - (p3.y - p1.y) * (p4.x - p3.x)) / d;
        var v = ((p3.x - p1.x) * (p2.y - p1.y) - (p3.y - p1.y) * (p2.x - p1.x)) / d;

        if(u < 0.0f || u > 1.0f || v < 0.0f || v > 1.0f) return false;


        intersection.x = p1.x + u * (p2.x - p1.x);
        intersection.y = p1.y + u * (p2.y - p1.y);

        return true;
    }
}
