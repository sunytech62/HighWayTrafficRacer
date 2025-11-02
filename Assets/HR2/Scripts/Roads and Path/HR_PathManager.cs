using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DefaultExecutionOrder(-20)]
public class HR_PathManager : MonoBehaviour
{

    #region SINGLETON PATTERN
    private static HR_PathManager instance;
    public static HR_PathManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindFirstObjectByType<HR_PathManager>();

            // If still null, create a new GameObject with this component
            if (instance == null)
                instance = new GameObject("HR_PathManager").AddComponent<HR_PathManager>();

            return instance;
        }
    }
    #endregion

    public HR_Player player;

    public List<Transform> pathPoints = new List<Transform>();

    public List<Transform> closestPathPointsToPlayer = new List<Transform>();

    public Transform closestPathPointToPlayer;

    [Tooltip("Update path processing every X seconds.")]
    public float interval = 0.5f;
    private float nextTime = 0f;

    /// <summary>
    /// Maximum distance used by CheckPathPoints() to decide if a path point is "close enough."
    /// Points farther than this get removed from closestPathPointsToPlayer.
    /// </summary>
    [Tooltip("Distance threshold for path points to be considered 'close' to the player.")]
    public float maxCloseDistance = 125f;

    /// <summary>
    /// Minimum distance used by IsInFront() check. If the point is extremely close, 
    /// we treat it as not "in front" to avoid re-adding duplicates, etc.
    /// </summary>
    [Tooltip("Distance below which we won't treat a point as 'in front' to avoid duplications.")]
    public float minFrontDistance = 20f;

    /// <summary>
    /// Called when the object becomes enabled and active.
    /// </summary>
    private void OnEnable()
    {

        HR_CurvedRoadManager.OnAllRoadsAligned += HR_CurvedRoadManager_OnAllRoadsAligned;
        HR_CurvedRoadManager.OnRoadAligned += HR_CurvedRoadManager_OnRoadAligned;
        HR_Events.OnPlayerSpawned += HR_GamePlayHandler_OnPlayerSpawned;

    }

    private void HR_CurvedRoadManager_OnRoadAligned(HR_CurvedRoad road)
    {
        // Re-check path after a single road is realigned
        ProcessPath();
    }

    private void HR_CurvedRoadManager_OnAllRoadsAligned(List<HR_CurvedRoad> allRoads)
    {
        // After all roads are aligned, add their bones to pathPoints
        for (int i = 0; i < allRoads.Count; i++)
            AddPath(allRoads[i]);
    }

    /// <summary>
    /// Called when the object becomes disabled.
    /// </summary>
    private void OnDisable()
    {

        HR_CurvedRoadManager.OnAllRoadsAligned -= HR_CurvedRoadManager_OnAllRoadsAligned;
        HR_CurvedRoadManager.OnRoadAligned -= HR_CurvedRoadManager_OnRoadAligned;
        HR_Events.OnPlayerSpawned -= HR_GamePlayHandler_OnPlayerSpawned;

    }

    /// <summary>
    /// Called once per frame to update the closest path point to the player, 
    /// but only if Time.time >= nextTime.
    /// </summary>
    private void Update()
    {

        if (Time.time >= nextTime)
        {
            nextTime += interval;
            ProcessPath();
        }

    }

    /// <summary>
    /// Main logic for updating the path data. Checks path points near the player,
    /// finds the truly closest path point, and adjusts this transform's forward direction.
    /// </summary>
    public void ProcessPath()
    {

        if (pathPoints != null && pathPoints.Count > 2)
            CheckPathPoints();

        if (player != null)
        {
            // Attempt to find the single closest path point
            closestPathPointToPlayer = FindClosestPointOnPathWithTransform(
                player.transform.position,
                out Vector3 direction
            );
        }

        // Re-orient this manager's transform to reflect the path's angle
        transform.forward = GetPathAngle();

    }

    /// <summary>
    /// Adds the path points (bones) from the specified curved road to the path manager,
    /// skipping duplicates.
    /// </summary>
    /// <param name="curvedRoad">The curved road containing path points.</param>
    public void AddPath(HR_CurvedRoad curvedRoad)
    {

        // Null-check
        if (!curvedRoad || curvedRoad.bones == null)
            return;

        for (int i = 0; i < curvedRoad.bones.Length; i++)
        {
            Transform bone = curvedRoad.bones[i];
            if (bone != null && !pathPoints.Contains(bone))
            {
                pathPoints.Add(bone);
            }
        }

        // Optionally sort after every AddPath if needed:
        // pathPoints.Sort(...);

    }

    /// <summary>
    /// Gets the angle of the path based on the "first" and "middle" points 
    /// in closestPathPointsToPlayer.
    /// </summary>
    /// <returns>A direction vector from first to middle. If insufficient points, returns Vector3.zero.</returns>
    public Vector3 GetPathAngle()
    {

        Transform first = GetFirstClosestPoint();
        Transform middle = GetMiddleClosestPoint(first);

        if (!first || !middle)
            return Vector3.zero;

        return middle.position - first.position;
    }

    /// <summary>
    /// Event handler for when the player is spawned. Store reference to newly spawned player.
    /// </summary>
    /// <param name="spawnedPlayer">The spawned player.</param>
    private void HR_GamePlayHandler_OnPlayerSpawned(HR_Player spawnedPlayer)
    {
        player = spawnedPlayer;
    }

    /// <summary>
    /// Finds the position on the path that is closest to the given targetPosition.
    /// Returns that position as a Vector3. Also sets pathDirection for the segment's direction.
    /// </summary>
    public Vector3 FindClosestPointOnPath(Vector3 targetPosition, out Vector3 pathDirection)
    {

        // If path is invalid or has < 2 points, just return zero
        if (pathPoints == null || pathPoints.Count < 2)
        {
            pathDirection = Vector3.forward;
            return Vector3.zero;
        }

        // Start by comparing to the first path point
        Vector3 closestPoint = pathPoints[0].position;
        pathDirection = pathPoints[1].position - pathPoints[0].position;

        float minDistance = Vector3.Distance(targetPosition, closestPoint);

        // Iterate through each pair of points
        for (int i = 0; i < pathPoints.Count - 1; i++)
        {

            Vector3 pointA = pathPoints[i].position;
            Vector3 pointB = pathPoints[i + 1].position;
            Vector3 projectedPoint = ProjectPointOnLineSegment(pointA, pointB, targetPosition);

            float distance = Vector3.Distance(targetPosition, projectedPoint);

            if (distance < minDistance)
            {
                closestPoint = projectedPoint;
                pathDirection = pointB - pointA; // direction of that segment
                minDistance = distance;
            }
        }

        return closestPoint;
    }

    /// <summary>
    /// Similar to FindClosestPointOnPath, but returns the actual Transform 
    /// from pathPoints rather than a projected position on a segment.
    /// </summary>
    public Transform FindClosestPointOnPathWithTransform(Vector3 targetPosition, out Vector3 pathDirection)
    {

        // If path is invalid or has < 2 points, just return null
        if (pathPoints == null || pathPoints.Count < 2)
        {
            pathDirection = Vector3.forward;
            return null;
        }

        Transform closestPoint = pathPoints[0];
        pathDirection = pathPoints[1].position - pathPoints[0].position;

        float minDistance = Vector3.Distance(targetPosition, closestPoint.position);

        for (int i = 0; i < pathPoints.Count - 1; i++)
        {

            Vector3 pointA = pathPoints[i].position;
            Vector3 pointB = pathPoints[i + 1].position;
            Vector3 projectedPoint = ProjectPointOnLineSegment(pointA, pointB, targetPosition);

            float distance = Vector3.Distance(targetPosition, projectedPoint);

            if (distance < minDistance)
            {
                // we update 'closestPoint' to be the actual Transform 
                // at index i (the start of that segment).
                closestPoint = pathPoints[i];
                pathDirection = pointB - pointA;
                minDistance = distance;
            }
        }

        return closestPoint;
    }

    /// <summary>
    /// Projects a point onto a line segment (A-B).
    /// </summary>
    public Vector3 ProjectPointOnLineSegment(Vector3 pointA, Vector3 pointB, Vector3 point)
    {

        Vector3 AB = pointB - pointA;
        float t = Vector3.Dot(point - pointA, AB) / Vector3.Dot(AB, AB);
        t = Mathf.Clamp01(t);
        return pointA + t * AB;
    }

    /// <summary>
    /// Gets the first item from closestPathPointsToPlayer, if any exist.
    /// </summary>
    public Transform GetFirstClosestPoint()
    {

        // We'll return the first valid transform in the list
        for (int i = 0; i < closestPathPointsToPlayer.Count; i++)
        {
            if (closestPathPointsToPlayer[i] != null)
                return closestPathPointsToPlayer[i];
        }

        return null;
    }

    /// <summary>
    /// Tries to get a 'middle' point from closestPathPointsToPlayer, 
    /// using the provided 'target' to find its index and offset a bit.
    /// </summary>
    public Transform GetMiddleClosestPoint(Transform target)
    {

        if (target == null)
            return null;

        // Index of 'target' inside our closest list
        int targetIndex = closestPathPointsToPlayer.IndexOf(target);
        if (targetIndex == -1)
            return null;

        // Example attempt to pick a point a third of the way from the end
        // This logic is from original code. We'll clamp it so we don't get out-of-range
        int offset = (int)(((closestPathPointsToPlayer.Count - 1) - targetIndex) / 3f);
        int middleIndex = targetIndex + offset;

        if (middleIndex < 0 || middleIndex >= closestPathPointsToPlayer.Count)
            return null;

        return closestPathPointsToPlayer[middleIndex];
    }

    /// <summary>
    /// Gets the last valid item in closestPathPointsToPlayer, if it exists.
    /// </summary>
    public Transform GetLastClosestPoint()
    {

        for (int i = closestPathPointsToPlayer.Count - 1; i >= 0; i--)
        {
            if (closestPathPointsToPlayer[i] != null)
                return closestPathPointsToPlayer[i];
        }

        return null;
    }

    /// <summary>
    /// Scans pathPoints to see which are "close enough" to the player. 
    /// Maintains the closestPathPointsToPlayer list accordingly.
    /// </summary>
    private void CheckPathPoints()
    {

        if (!player)
            return;

        Vector3 playerPos = player.transform.position;

        // We'll iterate once over pathPoints
        for (int i = 0; i < pathPoints.Count; i++)
        {

            if (!pathPoints[i])
                continue;  // skip null entries

            float distance = Vector3.Distance(pathPoints[i].position, playerPos);

            // If within maxCloseDistance, ensure it's in the list
            if (distance < maxCloseDistance)
            {
                if (!closestPathPointsToPlayer.Contains(pathPoints[i]))
                {
                    closestPathPointsToPlayer.Add(pathPoints[i]);
                }

                // Optionally check if it's "in front" of the player as well.
                // The existing code uses an approach that if it's in front 
                // AND not in the list, add it. We'll do the same:
                // But if it's very close (< minFrontDistance), skip the "in front" check.
                if (distance > minFrontDistance && IsInFront(player.gameObject, pathPoints[i].gameObject))
                {
                    if (!closestPathPointsToPlayer.Contains(pathPoints[i]))
                    {
                        closestPathPointsToPlayer.Add(pathPoints[i]);
                    }
                }

            }
            else
            {
                // If it's in the list but now too far, remove it
                if (closestPathPointsToPlayer.Contains(pathPoints[i]))
                {
                    closestPathPointsToPlayer.Remove(pathPoints[i]);
                }
            }

            // If it's behind the player, also remove it from the list
            // (Same as the original logic)
            if (!IsInFront(player.gameObject, pathPoints[i].gameObject))
            {
                if (closestPathPointsToPlayer.Contains(pathPoints[i]))
                {
                    closestPathPointsToPlayer.Remove(pathPoints[i]);
                }
            }
        }

        // Sort by Z after the updates
        SortByZPosition();
    }

    /// <summary>
    /// Finds the road that contains the specified transform (point) 
    /// by checking if point.IsChildOf(...) each spawned HR_CurvedRoad.
    /// </summary>
    public HR_CurvedRoad FindRoadByTransform(Transform point)
    {

        if (!point)
            return null;

        HR_CurvedRoadManager roadsInstance = HR_CurvedRoadManager.Instance;
        if (!roadsInstance || roadsInstance.spawnedRoads == null)
            return null;

        for (int i = 0; i < roadsInstance.spawnedRoads.Count; i++)
        {
            if (roadsInstance.spawnedRoads[i] != null)
            {
                if (point.IsChildOf(roadsInstance.spawnedRoads[i].transform))
                {
                    return roadsInstance.spawnedRoads[i];
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Finds the road that contains closestPathPointToPlayer, 
    /// or null if none is found.
    /// </summary>
    public HR_CurvedRoad FindRoadByPlayer()
    {

        if (!closestPathPointToPlayer)
            return null;

        HR_CurvedRoadManager roadsInstance = HR_CurvedRoadManager.Instance;
        if (!roadsInstance || roadsInstance.spawnedRoads == null || roadsInstance.spawnedRoads.Count < 1)
            return null;

        for (int i = 0; i < roadsInstance.spawnedRoads.Count; i++)
        {
            if (roadsInstance.spawnedRoads[i] != null)
            {
                if (closestPathPointToPlayer.IsChildOf(roadsInstance.spawnedRoads[i].transform))
                {
                    return roadsInstance.spawnedRoads[i];
                }
            }
        }

        return null;
    }

    /// <summary>
    /// Checks if 'target' is in front of 'other' using a dot product test,
    /// but also disregards if distance < minFrontDistance (by original code).
    /// </summary>
    private bool IsInFront(GameObject target, GameObject other)
    {

        // If they're too close, original code returns true automatically
        if (Vector3.Distance(target.transform.position, other.transform.position) < minFrontDistance)
            return true;

        Vector3 directionToOther = (other.transform.position - target.transform.position).normalized;
        float dotProduct = Vector3.Dot(target.transform.forward, directionToOther);

        return dotProduct > 0;
    }

    /// <summary>
    /// Sorts the entire pathPoints list by their Z position in world space,
    /// ignoring any that are null. 
    /// Then reassign the pathPoints list in sorted order.
    /// </summary>
    private void SortByZPosition()
    {
        // Filter out any nulls that might appear
        pathPoints = pathPoints.Where(item => item != null).ToList();
        // Sort
        pathPoints.Sort((a, b) => a.transform.position.z.CompareTo(b.transform.position.z));
    }

}
