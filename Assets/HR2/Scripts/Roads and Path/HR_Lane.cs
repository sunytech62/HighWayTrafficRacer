//----------------------------------------------
//                   Highway Racer
//
// Copyright © 2014 - 2025 BoneCracker Games
// https://www.bonecrackergames.com
//----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Manages lanes for the path the vehicles follow.
/// </summary>
public class HR_Lane : MonoBehaviour {

    /// <summary>
    /// List of points that make up the lane (typically generated in CreateWaypoints()).
    /// </summary>
    [Tooltip("Waypoints that define this lane's path.")]
    public List<Transform> points = new List<Transform>();

    /// <summary>
    /// Initial horizontal offset from the original path. If -1, we auto-set it using transform.position.x on CreateWaypoints().
    /// </summary>
    [Tooltip("Horizontal offset for the lane. If -1, it will be set to the lane's current x-position on first usage.")]
    public float initialDistance = -1f;

    /// <summary>
    /// Indicates if the lane is on the left side (used by certain mode logic, e.g. Two-Way).
    /// </summary>
    [Tooltip("If true, this lane is considered 'left side' in a two-way scenario.")]
    public bool leftSide = false;

    /// <summary>
    /// Creates waypoints for the lane based on a list of path points (e.g. from HR_PathManager).
    /// </summary>
    /// <param name="pathPoints">Global path points to copy.</param>
    public void CreateWaypoints(List<Transform> pathPoints) {

        // Safety check in case pathPoints is null or too short.
        if (pathPoints == null || pathPoints.Count < 1)
            return;

        for (int i = 0; i < pathPoints.Count; i++) {
            if (pathPoints[i] != null) {

                // If initialDistance hasn't been set yet, use this lane's current X position.
                if (initialDistance == -1f)
                    initialDistance = transform.position.x;

                // Create a new waypoint object
                GameObject newWP = new GameObject("Waypoint_" + i);
                newWP.transform.SetParent(transform);

                // Copy position & rotation from the corresponding path point
                newWP.transform.position = pathPoints[i].position;
                newWP.transform.rotation = pathPoints[i].rotation;

                // Apply horizontal offset
                newWP.transform.position += Vector3.right * initialDistance;

                // Track it in our points list
                points.Add(newWP.transform);
            }
        }

        //SmoothWaypoints(subdivisions: 4);

    }

    public List<Vector3> GetSmoothedPoints(int subdivisions) {
        var raw = points.Select(t => t.position).ToList();
        var smooth = new List<Vector3>();

        // for closed loops you’d wrap around; here we’ll just duplicate end points
        for (int i = 0; i < raw.Count - 1; i++) {
            // p0…p3 for this span
            Vector3 p0 = i == 0 ? raw[i] : raw[i - 1];
            Vector3 p1 = raw[i];
            Vector3 p2 = raw[i + 1];
            Vector3 p3 = (i + 2 < raw.Count) ? raw[i + 2] : raw[i + 1];

            // always include the start of the segment
            smooth.Add(p1);
            for (int step = 1; step < subdivisions; step++) {
                float t = step / (float)subdivisions;
                smooth.Add(CatmullRom(p0, p1, p2, p3, t));
            }
        }
        // finally include the very last point
        smooth.Add(raw[raw.Count - 1]);
        return smooth;
    }

    public void SmoothWaypoints(int subdivisions) {

        // sample the spline
        List<Vector3> sm = GetSmoothedPoints(subdivisions);

        // destroy old waypoint objects
        foreach (var t in points) DestroyImmediate(t.gameObject);
        points.Clear();

        // recreate
        for (int i = 0; i < sm.Count; i++) {
            var go = new GameObject("WP_smooth_" + i);
            go.transform.SetParent(transform);
            go.transform.position = sm[i];
            points.Add(go.transform);
        }
    }


    /// <summary>
    /// Updates existing waypoints of this lane by re-aligning them to the current path points 
    /// (from HR_PathManager), while preserving the same horizontal offset.
    /// </summary>
    public void UpdateWaypoints() {

        // Grab the main path manager
        HR_PathManager pathManager = HR_PathManager.Instance;

        if (!pathManager || pathManager.pathPoints == null || pathManager.pathPoints.Count < 1)
            return;

        // We only update up to the min of both lists to avoid out-of-range issues.
        int maxCount = Mathf.Min(pathManager.pathPoints.Count, points.Count);

        for (int i = 0; i < maxCount; i++) {
            Transform src = pathManager.pathPoints[i];
            if (!src)
                continue;

            // Re-align
            points[i].position = src.position;
            points[i].rotation = src.rotation;

            // Re-apply horizontal offset
            points[i].position += Vector3.right * initialDistance;
        }

    }

    /// <summary>
    /// Finds the closest point on this lane to the given target position,
    /// returning that point's position in world space and the path segment direction.
    /// </summary>
    public Vector3 FindClosestPointOnPath(Vector3 targetPosition, out Vector3 pathDirection) {

        // If lane is empty or has < 2 points, cannot form a path
        if (points.Count < 2) {
            pathDirection = Vector3.forward;
            return Vector3.zero;
        }

        // Initialize with the first segment
        Vector3 closestPoint = points[0].position;
        pathDirection = points[1].position - points[0].position;
        float minDistance = Vector3.Distance(targetPosition, closestPoint);

        // Check every segment
        for (int i = 0; i < points.Count - 1; i++) {

            Vector3 pointA = points[i].position;
            Vector3 pointB = points[i + 1].position;

            Vector3 projectedPoint = ProjectPointOnLineSegment(pointA, pointB, targetPosition);
            float distance = Vector3.Distance(targetPosition, projectedPoint);

            if (distance < minDistance) {
                closestPoint = projectedPoint;
                pathDirection = pointB - pointA;
                minDistance = distance;
            }
        }

        return closestPoint;
    }

    /// <summary>
    /// Projects a point onto a line segment from pointA to pointB.
    /// </summary>
    private Vector3 ProjectPointOnLineSegment(Vector3 pointA, Vector3 pointB, Vector3 point) {

        Vector3 AB = pointB - pointA;
        float dotABAB = Vector3.Dot(AB, AB);

        // If A and B are essentially the same, no segment to project onto
        if (dotABAB < Mathf.Epsilon)
            return pointA;

        float t = Vector3.Dot(point - pointA, AB) / dotABAB;
        t = Mathf.Clamp01(t);
        return pointA + t * AB;
    }

    private Vector3 CatmullRom(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t) {
        // standard Catmull–Rom basis
        float t2 = t * t;
        float t3 = t2 * t;
        return 0.5f * (
            (2f * p1) +
            (-p0 + p2) * t +
            (2f * p0 - 5f * p1 + 4f * p2 - p3) * t2 +
            (-p0 + 3f * p1 - 3f * p2 + p3) * t3
        );
    }

    /// <summary>
    /// Draws gizmos in the editor for visualization (lane points & lines).
    /// </summary>
    private void OnDrawGizmos() {

        for (int i = 0; i < points.Count; i++) {

            // Draw sphere at each point
            Color defColor = Gizmos.color;
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(points[i].position, 0.4f);

            // Draw line to next point
            if (i < points.Count - 1) {
                Gizmos.DrawLine(points[i].position, points[i + 1].position);
            }

            Gizmos.color = defColor;
        }

    }

}
