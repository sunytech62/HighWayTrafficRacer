//----------------------------------------------
//                   Highway Racer
//
// Copyright © 2014 - 2025 BoneCracker Games
// https://www.bonecrackergames.com
//----------------------------------------------

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Traffic car controller.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class HR_TrafficCar : MonoBehaviour {

    #region TRAFFIC MANAGER REFERENCE

    private HR_TrafficManager trafficManager;
    /// <summary>
    /// Quick access to HR_TrafficManager singleton.
    /// </summary>
    public HR_TrafficManager TrafficManager {
        get {
            if (trafficManager == null)
                trafficManager = HR_TrafficManager.Instance;
            return trafficManager;
        }
    }

    #endregion

    #region RIGIDBODY REFERENCE

    private Rigidbody rigid;
    /// <summary>
    /// Cached reference to this object's Rigidbody.
    /// </summary>
    private Rigidbody Rigid {
        get {
            if (rigid == null)
                rigid = GetComponent<Rigidbody>();
            return rigid;
        }
    }

    #endregion
   
    /// <summary>
    /// Vertical offset added to the traffic car when it is spawned/re-aligned.
    /// </summary>
    [Tooltip("Vertical offset from the ground when spawning/re-aligning.")]
    public float spawnHeight = 0f;

    /// <summary>
    /// A trigger volume used to detect collisions with the vehicle's front.
    /// </summary>
    [HideInInspector]
    public BoxCollider triggerCollider;

    /// <summary>
    /// Whether this car has crashed.
    /// </summary>
    [HideInInspector]
    public bool crashed = false;

    /// <summary>
    /// Enum representing the direction of lane change in progress.
    /// </summary>
    public enum ChangingLines { Straight, Right, Left }

    /// <summary>
    /// Current direction of lane change.
    /// </summary>
    [HideInInspector]
    public ChangingLines changingLines = ChangingLines.Straight;

    /// <summary>
    /// Current lane in which this traffic car is driving.
    /// </summary>
    [Tooltip("The lane this traffic car is assigned to.")]
    public HR_Lane currentLane;

    /// <summary>
    /// Is the car driving in the opposite direction (for Two-Way logic).
    /// </summary>
    [Tooltip("If true, car is going opposite to 'forward' direction of the main path.")]
    public bool oppositeDirection = false;

    /// <summary>
    /// Maximum speed of the car (km/h).
    /// </summary>
    [Tooltip("Maximum speed (in km/h). This will be randomized slightly on OnEnable.")]
    public float maximumSpeed = 10f;
    private float _maximumSpeed = 10f;

    /// <summary>
    /// The current target speed (km/h) of this vehicle.
    /// </summary>
    private float desiredSpeed;

    /// <summary>
    /// Distance to the next car ahead in the trigger volume. Used to slow down or brake.
    /// </summary>
    private float distance = 100f;

    /// <summary>
    /// Steering angle for the vehicle's orientation (smoothly rotated).
    /// </summary>
    private Quaternion steeringAngle = Quaternion.identity;

    /// <summary>
    /// The forward direction of the path. Updated by lane logic (GetClosestPointOnPath).
    /// </summary>
    private Vector3 pathDirection = Vector3.forward;

    [Space(10)]
    /// <summary>
    /// The wheel models for visual rotation.
    /// </summary>
    public Transform[] wheelModels;
    private float wheelRotation = 0f;

    // Headlights, braking states
    private bool headlightsOn = false;
    private bool brakingOn = false;

    /// <summary>
    /// Enum representing the state of the turn signals.
    /// </summary>
    private enum SignalsOn { Off, Right, Left, All }
    private SignalsOn signalsOn = SignalsOn.Off;

    private float signalTimer = 0f;

    /// <summary>
    /// Time after spawn in which collisions are ignored to prevent immediate crashes.
    /// </summary>
    private float spawnProtection = 0f;

    [Space(10)]
    /// <summary>
    /// Layer mask for collision detection.
    /// </summary>
    [Tooltip("Used in OnCollisionEnter to detect collisions with vehicles/objects on these layers.")]
    public LayerMask collisionLayer = 1;

    /// <summary>
    /// Layer mask for detecting other vehicles in the OnTriggerStay logic.
    /// </summary>
    [Tooltip("Used in OnTriggerStay to detect distance to cars on these layers.")]
    public LayerMask detectionLayer = 1;

    [Space(10)]
    /// <summary>
    /// Engine sound clip for this traffic car.
    /// </summary>
    [Tooltip("Engine sound clip played by an AudioSource created in Awake.")]
    public AudioClip engineSound;
    private AudioSource engineSoundSource;

    [Space(10)]
    /// <summary>
    /// Lights for headlights.
    /// </summary>
    [Tooltip("Lights for the car's headlights.")]
    public Light[] headLights;

    /// <summary>
    /// Lights for brakes.
    /// </summary>
    [Tooltip("Lights for the car's brake lights.")]
    public Light[] brakeLights;

    /// <summary>
    /// Lights for turn signals.
    /// </summary>
    [Tooltip("Lights for the car's turn signals.")]
    public Light[] signalLights;

    // The waypoint considered "closest" in the lane path
    private Vector3 closestWaypoint = Vector3.zero;

    // Interval for periodically re-checking path
    private float interval = 0.25f;
    private float nextTime = 0f;

    /// <summary>
    /// Called when the script instance is being loaded (before Start).
    /// </summary>
    private void Awake() {

        // Force all child lights to use vertex rendering, presumably for performance
        Light[] allLights = GetComponentsInChildren<Light>();
        foreach (Light l in allLights)
            l.renderMode = LightRenderMode.ForceVertex;

        distance = 100f;
        CreateTriggerVolume();

        // Create engine sound source
        engineSoundSource = HR_CreateAudioSource.NewAudioSource(
            gameObject, "Engine Sound", 2f, 5f, 1f, engineSound, true, true, false
        );
        engineSoundSource.pitch = 1.5f;

        // Backup copy of maximumSpeed
        _maximumSpeed = maximumSpeed;

        // Mark child objects to trafficCarsLayer if not "Ignore Raycast"
        foreach (Transform t in transform) {
            if (t.gameObject.layer != LayerMask.NameToLayer("Ignore Raycast"))
                t.gameObject.layer = LayerMask.NameToLayer(HR_Settings.Instance.trafficCarsLayer);
        }
    }

    /// <summary>
    /// Called when the object becomes enabled (after Awake).
    /// </summary>
    private void Start() {
        // Randomly change lanes at intervals
        InvokeRepeating(nameof(ChangeLines), Random.Range(15, 45), Random.Range(15, 45));
    }

    /// <summary>
    /// Called each time the object is enabled.
    /// This might be after re-alignment in HR_TrafficManager.
    /// </summary>
    public void OnEnable() {

        // Reset velocity and constraints
        Rigid.linearVelocity = Vector3.zero;
        Rigid.angularVelocity = Vector3.zero;
        Rigid.constraints = RigidbodyConstraints.FreezeRotationX |
                            RigidbodyConstraints.FreezeRotationZ |
                            RigidbodyConstraints.FreezePositionY;

        // Reset flags
        crashed = false;
        spawnProtection = 0f;
        distance = 100f;
        steeringAngle = transform.rotation;

        // Slightly randomize max speed
        maximumSpeed = Random.Range(_maximumSpeed * 0.85f, _maximumSpeed);

        // Check direction
        oppositeDirection = Vector3.Dot(transform.forward, Vector3.forward) < 0;

        // Re-compute path
        GetClosestPointOnPath();

        // Signals off initially
        signalsOn = SignalsOn.Off;
        changingLines = ChangingLines.Straight;

        // If it's nighttime in the gameplay manager, turn on headlights
        if (TrafficManager.GameplayManager != null)
            headlightsOn = (TrafficManager.GameplayManager.dayOrNight == HR_GamePlayManager.DayOrNight.Night);
        else
            headlightsOn = false;

    }

    /// <summary>
    /// Creates a trigger volume (BoxCollider) in front of the vehicle to detect close cars.
    /// </summary>
    private void CreateTriggerVolume() {

        // Calculate combined bounds for the entire object
        Bounds bounds = HR_GetBounds.GetBounds(transform);

        // Create a separate child game object for the trigger
        GameObject triggerColliderGO = new GameObject("HR_TriggerVolume");
        triggerColliderGO.gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
        triggerColliderGO.transform.position = bounds.center;
        triggerColliderGO.transform.rotation = transform.rotation;
        triggerColliderGO.transform.SetParent(transform, true);
        triggerColliderGO.transform.localScale = transform.localScale;

        // Add a box collider as the trigger
        BoxCollider boxCollider = triggerColliderGO.AddComponent<BoxCollider>();
        boxCollider.isTrigger = true;
        boxCollider.size = bounds.size;
        boxCollider.center = bounds.center;

        // Slightly enlarge the bounding box to detect front vehicles
        triggerCollider = boxCollider;
        triggerCollider.size = new Vector3(
            bounds.size.x * 1.05f,
            bounds.size.y,
            bounds.size.z + (bounds.size.z * 3f)
        );
        triggerCollider.center = new Vector3(
            bounds.center.x,
            0f,
            bounds.center.z + (triggerCollider.size.z / 2f) - (bounds.size.z / 3f)
        );

    }

    /// <summary>
    /// Update is called every frame.
    /// - Increments spawn protection timer.
    /// - Updates lights and wheels visually.
    /// </summary>
    private void Update() {

        // Gradually remove spawn protection after 1 second
        spawnProtection += Time.deltaTime;
        if (spawnProtection > 1f)
            spawnProtection = 1f;

        Lights();
        Wheels();
    }

    /// <summary>
    /// Finds the closest waypoint in currentLane (if any) to this transform.
    /// </summary>
    private void GetClosestPointOnPath() {

        if (currentLane != null && TrafficManager.LaneManager) {
            closestWaypoint = TrafficManager.LaneManager.FindClosestPointOnLane(
                currentLane,
                transform.position + transform.forward * 1f,
                out pathDirection
            );

            // If driving opposite direction, flip path direction
            if (oppositeDirection)
                pathDirection = -pathDirection;
        } else {
            closestWaypoint = Vector3.zero;
            pathDirection = Vector3.forward;
        }
    }
   
    /// <summary>
    /// Manages the car's movement logic every FixedUpdate for stable physics.
    /// </summary>
    private void Navigation() {

        // If no LaneManager, can't navigate properly
        if (!TrafficManager.LaneManager) {
            changingLines = ChangingLines.Straight;
            desiredSpeed = 0f;
            brakingOn = true;
            pathDirection = Vector3.forward;
            return;
        }

        // Periodically recalc closest path
        if (Time.time >= nextTime) {
            nextTime += interval;
            GetClosestPointOnPath();
        }

        // If there's no valid waypoint, car can't navigate
        if (closestWaypoint == Vector3.zero) {
            changingLines = ChangingLines.Straight;
            desiredSpeed = 0f;
            brakingOn = true;
            pathDirection = Vector3.forward;
            return;
        }

        // Smoothly steer towards pathDirection
        Quaternion targetRotation = Quaternion.LookRotation(pathDirection);
        steeringAngle = Quaternion.Slerp(steeringAngle, targetRotation, Time.fixedDeltaTime * 5f);

        // Decide speed: if crashed, fade to zero
        if (crashed) {
            desiredSpeed = Mathf.Lerp(desiredSpeed, 0f, Time.fixedDeltaTime);
        } else {
            // distance is updated in OnTriggerStay
            float factor = Mathf.InverseLerp(10f, 30f, distance);
            desiredSpeed = factor * maximumSpeed;
        }

        // If any car is too close, brake
        brakingOn = (distance < 25f);

        // If not crashed, rotate the transform
        if (!crashed)
            transform.rotation = steeringAngle;

        // Smoothly set velocity
        Vector3 targetVelocity = transform.forward * (desiredSpeed / 3.6f);
        Rigid.linearVelocity = Vector3.Slerp(Rigid.linearVelocity, targetVelocity, Time.fixedDeltaTime * 3f);
        Rigid.angularVelocity = Vector3.Slerp(Rigid.angularVelocity, Vector3.zero, Time.fixedDeltaTime * 5f);

        if (!crashed) {
            // Attempt to nudge horizontally if needed
            // offsetDirection is effectively the difference in x between the car and the "closestWaypoint"
            Vector3 offsetDirection = new Vector3(closestWaypoint.x, 0f, 0f) - new Vector3(transform.position.x, 0f, 0f);

            // clamp offset to [-1,1]
            offsetDirection.x = Mathf.Clamp(offsetDirection.x, -1f, 1f);
            offsetDirection.y = 0f;
            offsetDirection.z = 0f;

            // If offset is large enough, decide left/right changing lines
            changingLines = offsetDirection.magnitude > 0.45f
                ? (offsetDirection.x > 0f ? ChangingLines.Right : ChangingLines.Left)
                : ChangingLines.Straight;

            // Apply a mild force sideways to correct lane
            Rigid.AddForce(
                offsetDirection * 0.005f * Mathf.Clamp(Rigid.linearVelocity.magnitude, 0f, 20f),
                ForceMode.VelocityChange
            );
        }
    }

    /// <summary>
    /// Quick check if the car is facing "backward" relative to an arbitrary zero direction 
    /// (though the original code's logic is somewhat incomplete).
    /// </summary>
    private bool IsFacingBackward() {
        float dotProduct = Vector3.Dot(transform.forward, Vector3.zero);
        return dotProduct < 0;
    }

    /// <summary>
    /// Manages signals, headlights, brake lights, etc.
    /// </summary>
    private void Lights() {

        // If not crashed, determine signals from changingLines
        if (!crashed) {
            signalsOn = SignalsOn.Off;
            if (changingLines == ChangingLines.Right)
                signalsOn = SignalsOn.Right;
            else if (changingLines == ChangingLines.Left)
                signalsOn = SignalsOn.Left;
        } else {
            // If crashed, hazard signals
            signalsOn = SignalsOn.All;
        }

        // Flicker the signals every 0.5s
        signalTimer += Time.deltaTime;

        for (int i = 0; i < signalLights.Length; i++) {
            signalLights[i].intensity = signalsOn switch {
                SignalsOn.Off => 0f,
                SignalsOn.Left when signalLights[i].transform.localPosition.x < 0f
                    => (signalTimer >= 0.5f ? 0f : 1f),
                SignalsOn.Right when signalLights[i].transform.localPosition.x > 0f
                    => (signalTimer >= 0.5f ? 0f : 1f),
                SignalsOn.All => (signalTimer >= 0.5f ? 0f : 1f),
                _ => signalLights[i].intensity
            };

            if (signalTimer >= 1f)
                signalTimer = 0f;
        }

        // Headlights on/off
        for (int i = 0; i < headLights.Length; i++)
            headLights[i].intensity = headlightsOn ? 1f : 0f;

        // Brake lights: fully on if braking, partially on if headlights are on
        for (int i = 0; i < brakeLights.Length; i++)
            brakeLights[i].intensity = brakingOn ? 1f : (headlightsOn ? 0.6f : 0f);

    }

    /// <summary>
    /// Visually rotate wheel models based on speed.
    /// </summary>
    private void Wheels() {

        // Increase rotation rate by desiredSpeed * 20 (arbitrary scaling)
        wheelRotation += desiredSpeed * 20f * Time.deltaTime;

        // Apply local rotation to each wheel
        for (int i = 0; i < wheelModels.Length; i++) {
            wheelModels[i].localRotation = Quaternion.Euler(wheelRotation, 0f, 0f);
        }
    }

    /// <summary>
    /// Physics update loop.
    /// </summary>
    private void FixedUpdate() {
        Navigation();
    }

    /// <summary>
    /// Called when another collider stays within this object's trigger (the front volume).
    /// Used to measure distance to vehicles ahead on 'detectionLayer'.
    /// </summary>
    private void OnTriggerStay(Collider col) {
        if ((detectionLayer.value & (1 << col.gameObject.layer)) > 0) {
            // Check if that object is behind us or not
            if (!IsFacingBackward(col.transform)) {
                distance = Vector3.Distance(transform.position, col.transform.position);
            }
        }
    }

    /// <summary>
    /// If an object on detectionLayer leaves, reset distance to default (20).
    /// </summary>
    private void OnTriggerExit(Collider col) {
        if ((detectionLayer.value & (1 << col.gameObject.layer)) > 0) {
            distance = 100f;
        }
    }

    /// <summary>
    /// Called when a collision occurs with objects on 'collisionLayer'.
    /// If impulse is large enough and spawnProtection has worn off, mark crashed = true.
    /// </summary>
    private void OnCollisionEnter(Collision col) {
        if ((collisionLayer.value & (1 << col.gameObject.layer)) > 0) {

            // If not a big enough collision, or already crashed, or still protected, ignore
            if (col.impulse.magnitude < 1000f || crashed || spawnProtection < 0.5f)
                return;

            // Mark crashed
            crashed = true;
            signalsOn = SignalsOn.All;
        }
    }

    /// <summary>
    /// Checks if the transform is facing backward relative to 'target'.
    /// e.g. if dot < 0, we consider it behind us.
    /// </summary>
    private bool IsFacingBackward(Transform target) {
        Vector3 directionToTarget = (target.position - transform.position).normalized;
        float dotProduct = Vector3.Dot(transform.forward, directionToTarget);
        return (dotProduct < 0);
    }

    /// <summary>
    /// Changes the lane (Left, Right) randomly if we are currently straight.
    /// (Used in Start() with repeating invoke to periodically attempt lane changes.)
    /// </summary>
    /*
    private void ChangeLines() {

        
        // If currently turning or there's no lane manager, do nothing
        if (changingLines == ChangingLines.Left || changingLines == ChangingLines.Right)
            return;

        if (!HR_LaneManager.Instance || HR_LaneManager.Instance.lanes == null)
            return;

        int maxLanes = HR_LaneManager.Instance.lanes.Length;
        if (maxLanes < 1)
            return;

        // Pick a random lane index that matches our direction logic:
        int randomNumber = Random.Range(0, maxLanes);

        // If lane is leftSide != oppositeDirection, keep picking
        while (HR_LaneManager.Instance.lanes[randomNumber].lane.leftSide != oppositeDirection) {
            randomNumber = Random.Range(0, maxLanes);
        }

        // Switch lane
        currentLane = HR_LaneManager.Instance.lanes[randomNumber].lane;
    }
    */

    private bool IsLaneClear(HR_Lane newLane, float safeDistance = 10f) {
        foreach (var car in FindObjectsOfType<HR_TrafficCar>()) {
            if (car == this) continue;

            if (car.currentLane == newLane) {
                float distance = Mathf.Abs(car.transform.position.z - transform.position.z);
                if (distance < safeDistance) {
                    return false; // Someone is too close on this lane
                }
            }
        }
        return true;
    }

    
    private void ChangeLines() {
        if (changingLines == ChangingLines.Left || changingLines == ChangingLines.Right)
            return;

        if (!HR_LaneManager.Instance || HR_LaneManager.Instance.lanes == null)
            return;

        int maxLanes = HR_LaneManager.Instance.lanes.Length;
        if (maxLanes < 1)
            return;

        int attempts = 0;
        const int maxAttempts = 10;

        int randomNumber = Random.Range(0, maxLanes);
        while ((HR_LaneManager.Instance.lanes[randomNumber].lane.leftSide != oppositeDirection 
                || !IsLaneClear(HR_LaneManager.Instance.lanes[randomNumber].lane)) 
               && attempts < maxAttempts) 
        {
            randomNumber = Random.Range(0, maxLanes);
            attempts++;
        }

        if (attempts < maxAttempts) {
            currentLane = HR_LaneManager.Instance.lanes[randomNumber].lane;
        }
    }

    
    
    
    
    
    
    
    
    /// <summary>
    /// Applies default rigidbody settings for traffic cars.
    /// Called from Reset() (editor-time) or can be used at runtime if needed.
    /// </summary>
    private void SetRigidbodySettings() {

        // Ensure we have a Rigidbody
        if (!Rigid) {
            gameObject.AddComponent<Rigidbody>();
        }

        Rigid.interpolation = RigidbodyInterpolation.Interpolate;
        Rigid.mass = 1500f;
        Rigid.linearDamping = 1f;
        Rigid.angularDamping = 4f;
        Rigid.maxAngularVelocity = 2.5f;
        Rigid.constraints = RigidbodyConstraints.FreezeRotationX |
                            RigidbodyConstraints.FreezeRotationZ |
                            RigidbodyConstraints.FreezePositionY;
    }

    /// <summary>
    /// Called when component is reset in the Editor; sets up default rigidbody configuration.
    /// </summary>
    private void Reset() {
        SetRigidbodySettings();
    }

}
