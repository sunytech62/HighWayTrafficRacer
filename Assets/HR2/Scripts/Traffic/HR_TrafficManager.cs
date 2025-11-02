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
/// Manages the traffic cars in the game.
/// </summary>
public class HR_TrafficManager : MonoBehaviour
{

    #region SINGLETON PATTERN
    private static HR_TrafficManager instance;
    public static HR_TrafficManager Instance
    {
        get
        {
            if (instance == null)
                instance = FindFirstObjectByType<HR_TrafficManager>();
            return instance;
        }
    }
    #endregion

    #region HR_GamePlayManager Instance
    // Cache reference to the gameplay manager for quick access.
    private HR_GamePlayManager _GameplayManager;
    public HR_GamePlayManager GameplayManager
    {
        get
        {
            if (_GameplayManager == null)
                _GameplayManager = HR_GamePlayManager.Instance;
            return _GameplayManager;
        }
    }
    #endregion

    /// <summary>
    /// Reference to the lane manager (populated lazily).
    /// </summary>
    private HR_LaneManager laneManager;
    public HR_LaneManager LaneManager
    {
        get
        {
            if (laneManager == null)
                laneManager = HR_LaneManager.Instance;
            return laneManager;
        }
    }

    /// <summary>
    /// Contains prefabs and spawn amounts for traffic cars.
    /// </summary>
    [System.Serializable]
    public class TrafficCars
    {
        [Tooltip("The traffic car prefab.")]
        public HR_TrafficCar trafficCar;

        [Tooltip("How many of this prefab to spawn.")]
        public int amount = 1;
    }

    /// <summary>
    /// Array of traffic cars to spawn, each with a prefab and an amount.
    /// </summary>
    [Tooltip("List of different traffic car prefabs and how many to spawn for each.")]
    public TrafficCars[] trafficCars;

    [Space(5)]
    [Header("Spawn Distance Settings")]

    /// <summary>
    /// Minimum distance from the camera at which traffic cars can spawn.
    /// </summary>
    [Tooltip("Minimum spawn distance in front of the camera.")]
    public float minDistance = 300f;

    /// <summary>
    /// Maximum distance from the camera at which traffic cars can spawn.
    /// </summary>
    [Tooltip("Maximum spawn distance in front of the camera.")]
    public float maxDistance = 600f;

    /// <summary>
    /// List of spawned traffic cars for easy management.
    /// </summary>
    private List<HR_TrafficCar> spawnedTrafficCars = new List<HR_TrafficCar>();

    /// <summary>
    /// Container for the spawned traffic cars (parent GameObject).
    /// </summary>
    [HideInInspector] public GameObject spawnedTrafficCarsContainer;

    /// <summary>
    /// Called when the script instance is being loaded (start of play).
    /// </summary>
    private void Start()
    {
        CreateTraffic();
    }

    /// <summary>
    /// Called once per frame; if the game is started, we animate traffic.
    /// </summary>
    private void Update()
    {
        if (GameplayManager != null && GameplayManager.gameStarted)
            AnimateTraffic();
    }

    /// <summary>
    /// Instantiates all traffic cars and disables them initially, storing them in a container object.
    /// </summary>
    private void CreateTraffic()
    {

        // Create container for the spawned traffic cars.
        spawnedTrafficCarsContainer = new GameObject("HR_TrafficContainer");

        // Loop through each trafficCar config
        for (int i = 0; i < trafficCars.Length; i++)
        {
            if (!trafficCars[i].trafficCar)
            {
                // If no prefab reference, skip
                continue;
            }

            // Spawn 'amount' copies of this trafficCar
            for (int k = 0; k < trafficCars[i].amount; k++)
            {
                GameObject go = Instantiate(
                    trafficCars[i].trafficCar.gameObject,
                    Vector3.zero,
                    Quaternion.identity
                );

                HR_TrafficCar trafficCarComp = go.GetComponent<HR_TrafficCar>();
                // If no HR_TrafficCar component, skip
                if (!trafficCarComp)
                {
                    Destroy(go);
                    continue;
                }

                spawnedTrafficCars.Add(trafficCarComp);

                // Set inactive, place under container
                go.SetActive(false);
                go.transform.SetParent(spawnedTrafficCarsContainer.transform, true);

                // Move behind the camera or out of view initially
                go.transform.position -= Vector3.forward * 100f;
            }
        }

        // Slight delay before populating traffic
        Invoke(nameof(Populate), 0.1f);
    }

    /// <summary>
    /// Performs an initial placement (Populate) of the traffic cars.
    /// </summary>
    private void Populate()
    {

        // If no main camera, we can't position cars meaningfully
        if (!Camera.main)
            return;

        // For each traffic car, realign it
        for (int i = 0; i < spawnedTrafficCars.Count; i++)
        {
            // We pass 'true' to ignoreDistanceToRef, meaning we might place them closer than minDistance
            ReAlignTraffic(spawnedTrafficCars[i], ignoreDistanceToRef: true);
        }
    }

    /// <summary>
    /// Animates the traffic each frame. If the traffic car is too far behind or ahead of the camera, we reposition it.
    /// </summary>
    private void AnimateTraffic()
    {

        if (!Camera.main)
            return;

        for (int i = 0; i < spawnedTrafficCars.Count; i++)
        {
            HR_TrafficCar car = spawnedTrafficCars[i];
            // If camera has gone beyond the car's z + 100, or if the car is beyond camera's z - maxDistance, realign it
            float cameraZ = Camera.main.transform.position.z;
            float carZ = car.transform.position.z;

            bool behindCamTooFar = cameraZ > (carZ + 100f);
            bool aheadCamTooFar = cameraZ < (carZ - maxDistance);

            if (behindCamTooFar || aheadCamTooFar)
            {
                ReAlignTraffic(car, ignoreDistanceToRef: false);
            }
        }
    }

    /// <summary>
    /// Realigns (teleports) a traffic car to a new position ahead of or behind the camera, along a lane.
    /// </summary>
    private void ReAlignTraffic(HR_TrafficCar realignableObject, bool ignoreDistanceToRef)
    {

        // 1) If no lane manager or lanes array is empty, we can't place this properly
        if (!LaneManager || LaneManager.lanes == null || LaneManager.lanes.Length == 0)
        {
            realignableObject.gameObject.SetActive(false);
            return;
        }

        // 2) Pick a random lane
        HR_LaneManager.Lane[] lanes = LaneManager.lanes;
        HR_LaneManager.Lane randomLane = lanes[Random.Range(0, lanes.Length)];

        // 3) Determine the distance to spawn the car from the camera
        float spawnDistance = Random.Range(minDistance, maxDistance);
        if (ignoreDistanceToRef)
        {
            // If ignoring distance, spawn somewhat closer
            spawnDistance = Random.Range(60f, minDistance);
        }

        // 4) Compute the new position for the car
        // We add (Vector3.forward * spawnDistance) to the camera's position
        Vector3 cameraPos = Camera.main.transform.position;
        Vector3 pos = randomLane.lane.FindClosestPointOnPath(cameraPos + (Vector3.forward * spawnDistance), out Vector3 dir);

        // 5) Set the car's lane reference
        realignableObject.currentLane = randomLane.lane;

        // 6) Move the car to the computed position
        realignableObject.transform.position = pos;

        // 7) Adjust orientation based on the game mode
        switch (PlayerPrefs.GetInt("SelectedTraffic"))
        {
            case 0/*HR_GamePlayManager.Mode.OneWay*/:
                realignableObject.transform.forward = dir;
                break;

            case 1/*HR_GamePlayManager.Mode.TwoWay*/:
                // If this lane is leftSide, invert direction
                realignableObject.transform.forward = randomLane.lane.leftSide ? -dir : dir;
                break;

            default:
                // TimeAttack and Bomb also use 'dir'
                realignableObject.transform.forward = dir;
                break;
        }

        // 8) Slight vertical offset if needed
        realignableObject.transform.position += realignableObject.transform.up * realignableObject.spawnHeight;

        // 9) Reactivate the car and reset it
        realignableObject.gameObject.SetActive(true);
        realignableObject.OnEnable();

        // 10) Check if this newly placed car overlaps with another
        if (CheckIfClipping(realignableObject.triggerCollider))
        {
            // If clipping, disable it
            realignableObject.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// Checks if the bounding box of 'trafficCarBound' overlaps with another active traffic car.
    /// </summary>
    /// <param name="trafficCarBound">The bounding box collider of the newly placed car.</param>
    /// <returns>True if it is clipping with another traffic car, otherwise false.</returns>
    private bool CheckIfClipping(BoxCollider trafficCarBound)
    {

        if (!trafficCarBound)
            return false;

        for (int i = 0; i < spawnedTrafficCars.Count; i++)
        {
            HR_TrafficCar otherCar = spawnedTrafficCars[i];
            // Skip if same car or if other is inactive
            if (!otherCar.gameObject.activeSelf
                || trafficCarBound.transform.IsChildOf(otherCar.transform))
            {
                continue;
            }

            // If bounding boxes overlap via HR_BoundsExtension.ContainBounds
            if (HR_BoundsExtension.ContainBounds(
                trafficCarBound.transform,
                trafficCarBound.bounds,
                otherCar.triggerCollider.bounds))
            {
                return true;
            }
        }

        return false;
    }

}
