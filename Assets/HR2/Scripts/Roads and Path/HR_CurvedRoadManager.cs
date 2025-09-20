//----------------------------------------------
//                   Highway Racer 2
//
//        Curved Road Manager (Revised)
//----------------------------------------------

using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Pooling the road with a given amount. Calculates total length of the pool, and translates previous roads to the next position.
/// </summary>
public class HR_CurvedRoadManager : MonoBehaviour {

    #region SINGLETON PATTERN
    private static HR_CurvedRoadManager instance;
    public static HR_CurvedRoadManager Instance {
        get {
            if (instance == null)
                instance = FindFirstObjectByType<HR_CurvedRoadManager>();
            return instance;
        }
    }
    #endregion

    [System.Serializable]
    public class RoadObjects {
        public HR_CurvedRoad road;
    }

    /// <summary>
    /// Array of road objects to be managed.
    /// </summary>
    public RoadObjects[] roads;

    /// <summary>
    /// List of spawned roads.
    /// </summary>
    public List<HR_CurvedRoad> spawnedRoads = new List<HR_CurvedRoad>();

    /// <summary>
    /// Keep track of the last road we repositioned.
    /// </summary>
    private HR_CurvedRoad lastRoad;

    /// <summary>
    /// Container for spawned roads.
    /// </summary>
    public GameObject spawnedRoadsContainer;

    /// <summary>
    /// Delegate for the event triggered when all roads are aligned.
    /// </summary>
    /// <param name="allRoads">List of all aligned HR_CurvedRoad objects.</param>
    public delegate void onAllRoadsAligned(List<HR_CurvedRoad> allRoads);
    public static event onAllRoadsAligned OnAllRoadsAligned;

    /// <summary>
    /// Delegate for the event triggered when a single road is aligned.
    /// </summary>
    /// <param name="road">The aligned HR_CurvedRoad object.</param>
    public delegate void onRoadAligned(HR_CurvedRoad road);
    public static event onRoadAligned OnRoadAligned;

    private void Awake() {

        // Deactivate all road prefabs in the scene to avoid duplicates.
        for (int i = 0; i < roads.Length; i++) {
            if (roads[i] != null && roads[i].road && roads[i].road.gameObject.scene != null)
                roads[i].road.gameObject.SetActive(false);
        }

        // Creating the roads.
        CreateRoads();

    }

    /// <summary>
    /// Creates all roads.
    /// </summary>
    private void CreateRoads() {

        // Quick validation
        if (roads == null || roads.Length == 0) {
            Debug.LogError("No roads assigned. Please assign at least one road prefab in the inspector.");
            return;
        }

        // Try to find or create a container for the spawned roads.
        spawnedRoadsContainer = GameObject.Find("HR_CurvedRoads");
        if (!spawnedRoadsContainer)
            spawnedRoadsContainer = new GameObject("HR_CurvedRoads");
        spawnedRoadsContainer.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);

        // Initial spawn of roads (one for each entry in the roads array).
        for (int k = 0; k < roads.Length; k++) {

            if (!roads[k].road) {
                Debug.LogWarning("A road in the array is missing a reference. Skipping.");
                continue;
            }

            // Make sure it's not marked static at runtime to avoid issues
            roads[k].road.gameObject.isStatic = false;
            foreach (Transform item in roads[k].road.transform)
                item.gameObject.isStatic = false;

            // Instantiate
            HR_CurvedRoad newRoad = Instantiate(
                roads[k].road.gameObject,
                roads[k].road.transform.position,
                roads[k].road.transform.rotation
            ).GetComponent<HR_CurvedRoad>();

            newRoad.gameObject.SetActive(true);

            // Optional: Randomize curves on creation
            newRoad.RandomizeCurve();

            newRoad.transform.SetParent(spawnedRoadsContainer.transform);

            // Add to the spawned list
            spawnedRoads.Add(newRoad);
        }

        // Align them in a chain
        for (int i = 0; i < spawnedRoads.Count; i++) {
            if (i != 0) {
                // Snap this road to the end of the previous road
                spawnedRoads[i].transform.position = spawnedRoads[i - 1].endPoint.position;
                spawnedRoads[i].transform.rotation = spawnedRoads[i - 1].endPoint.rotation;
            }
        }

        // Ensure we have a minimum number of roads (e.g., 3).
        int minimumRequiredRoads = 3;

        // If you need more, keep instantiating from the road array until we reach the minimum.
        // This is safer than recursion:
        while (spawnedRoads.Count < minimumRequiredRoads) {

            for (int k = 0; k < roads.Length; k++) {

                if (!roads[k].road)
                    continue;

                HR_CurvedRoad extraRoad = Instantiate(
                    roads[k].road.gameObject,
                    roads[k].road.transform.position,
                    roads[k].road.transform.rotation
                ).GetComponent<HR_CurvedRoad>();

                extraRoad.gameObject.SetActive(true);
                extraRoad.RandomizeCurve();
                extraRoad.transform.SetParent(spawnedRoadsContainer.transform);

                // Position it at the end of the last spawned road
                if (spawnedRoads.Count > 0) {
                    int lastIndex = spawnedRoads.Count - 1;
                    extraRoad.transform.position = spawnedRoads[lastIndex].endPoint.position;
                    extraRoad.transform.rotation = spawnedRoads[lastIndex].endPoint.rotation;
                }

                spawnedRoads.Add(extraRoad);
            }

        }

        // Invoke event if roads are aligned
        if (OnAllRoadsAligned != null && spawnedRoads.Count > 0) {
            OnAllRoadsAligned(spawnedRoads);
        }
    }

    private void Update() {
        AnimateRoads();
    }

    /// <summary>
    /// Detects if each road is behind the camera and repositions it if so.
    /// </summary>
    private void AnimateRoads() {

        Camera mainCamera = Camera.main;
        if (!mainCamera)
            return;

        // Realign only one road (or none) per frame to avoid jitter.
        for (int i = 0; i < spawnedRoads.Count; i++) {

            if (HasCameraPassedRoad(spawnedRoads[i], mainCamera.transform)) {
                // Reposition this road at the end of the last road
                ReAlignRoad(spawnedRoads[i]);
                // After re-aligning one road, break to avoid multiple moves this frame
                break;
            }

        }
    }

    /// <summary>
    /// Checks if the camera has passed the road's endpoint (meaning road is behind camera).
    /// </summary>
    /// <remarks>
    /// Adjust distance threshold or remove if not desired.
    /// </remarks>
    private bool HasCameraPassedRoad(HR_CurvedRoad road, Transform cameraTransform, float distanceThreshold = 50f) {

        // 1) If the camera is too close, don't reposition yet (optional).
        float distance = Vector3.Distance(cameraTransform.position, road.endPoint.position);
        if (distance < distanceThreshold)
            return false;

        // 2) Dot product with the camera's forward. If dot < 0 => road is behind camera.
        Vector3 directionToRoad = (road.endPoint.position - cameraTransform.position).normalized;
        float dotProduct = Vector3.Dot(cameraTransform.forward, directionToRoad);

        return (dotProduct < 0f);
    }

    /// <summary>
    /// Repositions the given road behind the last road in the chain and randomizes its curve.
    /// </summary>
    private void ReAlignRoad(HR_CurvedRoad road) {

        // If we haven't aligned any road before, just snap to the last item in the list.
        if (!lastRoad && spawnedRoads.Count > 0) {
            road.transform.position = spawnedRoads[spawnedRoads.Count - 1].endPoint.position;
            road.transform.rotation = spawnedRoads[spawnedRoads.Count - 1].endPoint.rotation;
        }
        // Otherwise, align to the endPoint of the last aligned road.
        else if (lastRoad) {
            road.transform.position = lastRoad.endPoint.position;
            road.transform.rotation = lastRoad.endPoint.rotation;
        }

        // Update the lastRoad reference
        lastRoad = road;
        // Place this road at the bottom of the hierarchy so it's "last"
        lastRoad.transform.SetAsLastSibling();

        // Randomize the curve if desired
        lastRoad.RandomizeCurve();

        // Trigger the event
        if (OnRoadAligned != null)
            OnRoadAligned(lastRoad);
    }

}
