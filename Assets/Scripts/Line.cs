using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer), typeof(EdgeCollider2D))]
public class Line : MonoBehaviour
{
    private LineRenderer lineRenderer;
    private EdgeCollider2D edgeCollider;

    private List<Vector2> points;

    private void Awake()
    {
        // Get external components
        if (!lineRenderer) lineRenderer = GetComponent<LineRenderer>();
        if (!edgeCollider) edgeCollider = GetComponent<EdgeCollider2D>();
    }

    public void UpdateLine(Vector2 mousePos, float minDistance)
    {
        // Check if there are no points
        // If yes, create new points starting at mousePos
        if (points == null)
        {
            points = new List<Vector2>();
            SetPoint(mousePos);
            return;
        }

        // Check if mouse has moved enough to insert new point
        // If yes, set point
        float distance = Vector2.Distance(mousePos, points[^1]);
        if (distance > minDistance)
            SetPoint(mousePos);
    }

    private void SetPoint(Vector2 point)
    {
        points.Add(point);

        // Update external components
        lineRenderer.positionCount = points.Count;
        lineRenderer.SetPosition(points.Count - 1, point);
        if (points.Count > 1) edgeCollider.points = points.ToArray();
    }
}
