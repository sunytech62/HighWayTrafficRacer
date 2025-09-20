//----------------------------------------------
//                   Highway Racer
//
// Copyright © 2014 - 2025 BoneCracker Games
// https://www.bonecrackergames.com
//----------------------------------------------

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages all the lanes for the path vehicles follow.
/// </summary>
[DefaultExecutionOrder(-10)]
public class HR_LaneManager : MonoBehaviour {

    #region SINGLETON PATTERN
    private static HR_LaneManager instance;
    public static HR_LaneManager Instance {
        get {
            if (instance == null)
                instance = FindFirstObjectByType<HR_LaneManager>();
            return instance;
        }
    }
    #endregion

    /// <summary>
    /// Class representing a lane with its properties.
    /// </summary>
    [System.Serializable]
    public class Lane {
        /// <summary>
        /// The HR_Lane object representing the lane.
        /// </summary>
        public HR_Lane lane;
    }

    /// <summary>
    /// Array of lanes used in the scene.
    /// Each element has a reference to a HR_Lane instance.
    /// </summary>
    [Tooltip("Each element represents one lane wrapper, containing an HR_Lane.")]
    public Lane[] lanes;

    /// <summary>
    /// Called when the object becomes enabled and active.
    /// Subscribes to the road alignment events.
    /// </summary>
    private void OnEnable() {

        HR_CurvedRoadManager.OnAllRoadsAligned += HR_CurvedRoadManager_OnAllRoadsAligned;
        HR_CurvedRoadManager.OnRoadAligned += HR_CurvedRoadManager_OnRoadAligned;
    }

    /// <summary>
    /// Called when a single road is aligned; updates all lane waypoints.
    /// </summary>
    private void HR_CurvedRoadManager_OnRoadAligned(HR_CurvedRoad road) {
        UpdateWaypoints();
    }

    /// <summary>
    /// Called when all roads are aligned; creates initial waypoints for all lanes.
    /// </summary>
    private void HR_CurvedRoadManager_OnAllRoadsAligned(List<HR_CurvedRoad> allRoads) {
        CreateWaypoints();
    }

    /// <summary>
    /// Called when the object becomes disabled.
    /// Unsubscribes from the road alignment events.
    /// </summary>
    private void OnDisable() {

        HR_CurvedRoadManager.OnAllRoadsAligned -= HR_CurvedRoadManager_OnAllRoadsAligned;
        HR_CurvedRoadManager.OnRoadAligned -= HR_CurvedRoadManager_OnRoadAligned;
    }

    /// <summary>
    /// Finds and assigns all HR_Lane components in the children of this game object.
    /// Stores them in the lanes array.
    /// </summary>
    [ContextMenu("Get All Lanes")]
    public void GetLanes() {

        // Find all HR_Lane objects in children
        HR_Lane[] foundLanes = GetComponentsInChildren<HR_Lane>(true);

        lanes = new Lane[foundLanes.Length];

        for (int i = 0; i < foundLanes.Length; i++) {
            lanes[i] = new Lane();
            lanes[i].lane = foundLanes[i];
        }
    }

    /// <summary>
    /// Creates waypoints for each lane based on the global path points from HR_PathManager.
    /// </summary>
    public void CreateWaypoints() {

        HR_PathManager pathManager = HR_PathManager.Instance;
        // If we can't get the path manager or no path points, do nothing
        if (!pathManager || pathManager.pathPoints == null || pathManager.pathPoints.Count < 1)
            return;

        // Create waypoints for each lane
        for (int i = 0; i < lanes.Length; i++) {
            if (lanes[i] == null || lanes[i].lane == null)
                continue;

            lanes[i].lane.CreateWaypoints(pathManager.pathPoints);
        }
    }

    /// <summary>
    /// Updates the waypoints for all lanes. 
    /// Typically called after a road has been repositioned or re-aligned.
    /// </summary>
    public void UpdateWaypoints() {

        for (int i = 0; i < lanes.Length; i++) {
            if (lanes[i] == null || lanes[i].lane == null)
                continue;

            lanes[i].lane.UpdateWaypoints();
        }
    }

    /// <summary>
    /// Gets the index of the specified lane in the lanes array.
    /// Returns 0 if not found.
    /// </summary>
    /// <param name="lane">The lane wrapper to find.</param>
    /// <returns>The index of the lane in the lanes array.</returns>
    public int GetLaneIndex(Lane lane) {

        for (int i = 0; i < lanes.Length; i++) {
            // Compare references
            if (Equals(lane, lanes[i]))
                return i;
        }

        return 0;
    }

    /// <summary>
    /// Finds the closest point on the specified lane to the target position,
    /// along with the direction of that path segment.
    /// </summary>
    /// <param name="lane">The lane to search.</param>
    /// <param name="targetPosition">The target position.</param>
    /// <param name="pathDirection">The resulting direction from the closest segment.</param>
    /// <returns>The closest point (in world space) on the lane, or Vector3.zero if not found.</returns>
    public Vector3 FindClosestPointOnLane(HR_Lane lane, Vector3 targetPosition, out Vector3 pathDirection) {

        // 1) Attempt to locate the lane among our array
        HR_Lane foundLane = null;

        for (int i = 0; i < lanes.Length; i++) {
            if (lanes[i] != null && lanes[i].lane == lane) {
                foundLane = lanes[i].lane;
                break;
            }
        }

        // 2) If we can't find a matching lane, return zero
        if (!foundLane) {
            pathDirection = Vector3.forward;
            return Vector3.zero;
        }

        // 3) If the lane doesn't have enough points to form a segment, return zero
        List<Transform> pathPoints = foundLane.points;
        if (pathPoints == null || pathPoints.Count < 2) {
            pathDirection = Vector3.forward;
            return Vector3.zero;
        }

        // 4) Start by checking distance to the first point
        Vector3 closestPoint = pathPoints[0].position;
        pathDirection = pathPoints[1].position - pathPoints[0].position;
        float minDistance = Vector3.Distance(targetPosition, closestPoint);

        // 5) Loop over all consecutive pairs to project
        for (int i = 0; i < pathPoints.Count - 1; i++) {

            Vector3 pointA = pathPoints[i].position;
            Vector3 pointB = pathPoints[i + 1].position;
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
    /// Projects a point onto a line segment.
    /// </summary>
    /// <param name="pointA">The start of the segment.</param>
    /// <param name="pointB">The end of the segment.</param>
    /// <param name="point">The point to project.</param>
    /// <returns>A point on the segment [A,B] that is closest to 'point'.</returns>
    public Vector3 ProjectPointOnLineSegment(Vector3 pointA, Vector3 pointB, Vector3 point) {

        Vector3 AB = pointB - pointA;
        float dotABAB = Vector3.Dot(AB, AB);
        if (dotABAB < Mathf.Epsilon) {
            // A and B are effectively the same point
            return pointA;
        }

        float t = Vector3.Dot(point - pointA, AB) / dotABAB;
        t = Mathf.Clamp01(t);
        return pointA + t * AB;
    }

    /// <summary>
    /// Finds the lane that has a point closest to 'point'.
    /// This is done by scanning all lanes and all their points 
    /// to find the minimum distance to 'point'.
    /// </summary>
    public HR_Lane FindLaneByPoint(Transform point) {

        if (!point)
            return null;

        HR_Lane closestLane = null;
        float closestDistance = Mathf.Infinity;

        // Scan each lane
        for (int i = 0; i < lanes.Length; i++) {
            if (lanes[i] == null || lanes[i].lane == null)
                continue;

            List<Transform> allPoints = lanes[i].lane.points;
            if (allPoints != null && allPoints.Count >= 2) {

                // Check each point in the lane
                for (int k = 0; k < allPoints.Count; k++) {
                    float distance = Vector3.Distance(allPoints[k].position, point.position);

                    if (distance < closestDistance) {
                        closestDistance = distance;
                        closestLane = lanes[i].lane;
                    }
                }
            }
        }

        return closestLane;
    }

    /// <summary>
    /// Destroys all child HR_Lane objects and resets with a default of 4 lanes.
    /// This is an Editor-time function primarily for quick setup.
    /// </summary>
    private void Reset() {

        // Destroy existing child lanes
        HR_Lane[] allLanes = GetComponentsInChildren<HR_Lane>(true);
        for (int i = 0; i < allLanes.Length; i++)
            DestroyImmediate(allLanes[i].gameObject);

        // Recreate the array
        lanes = new Lane[4];

        // Initialize 4 lane gameobjects
        for (int i = 0; i < lanes.Length; i++) {

            // Create a new GameObject named Lane_i
            HR_Lane newLane = new GameObject("Lane_" + (i + 1)).AddComponent<HR_Lane>();
            newLane.transform.SetParent(transform);
            // Clear its local pos/rot
            newLane.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

            // Spread them out on X for a default layout: -4.8 ... +4.8
            float xPos = Mathf.Lerp(-4.8f, 4.8f, (float)i / 3f);
            newLane.transform.position += Vector3.right * xPos;

            // Create a Lane wrapper and store
            lanes[i] = new Lane();
            lanes[i].lane = newLane;

            // Mark if it's on the "left side"
            if (newLane.transform.localPosition.x < 0)
                lanes[i].lane.leftSide = true;
        }

    }
}
