using UnityEngine;
using System.Collections.Generic;

interface IPathFinder
{
    IEnumerable<Vector2> GetPath(Vector2 A, Vector2 C, IEnumerable<Edge> edges);
}

