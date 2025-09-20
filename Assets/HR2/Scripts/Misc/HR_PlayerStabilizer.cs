//----------------------------------------------
//                   Highway Racer
//
// Copyright © 2014 - 2025 BoneCracker Games
// https://www.bonecrackergames.com
//----------------------------------------------

using UnityEngine;

/// <summary>
/// Finds the closest HR_Lane automatically from HR_LaneManager, then applies
/// Rigidbody force and torque to keep the vehicle centered on that lane. 
/// 
/// This version dynamically adjusts the look-ahead distance based on speed.
/// </summary>
[RequireComponent(typeof(Rigidbody))]
[DefaultExecutionOrder(10)]
public class HR_PlayerStabilizer : MonoBehaviour {

    private RCCP_CarController CarController {

        get {

            if (!carController)
                carController = GetComponent<RCCP_CarController>();

            return carController;

        }

    }
    private RCCP_CarController carController;

    [Tooltip("Minimum look-ahead distance (at low speed)")]
    private float minLookAheadDistance = 1f;

    [Tooltip("Maximum look-ahead distance (at high speed)")]
    private float maxLookAheadDistance = 5f;

    [Tooltip("Vehicle speed (m/s) at which we will use the max look-ahead distance. If your car is often going 20 m/s (~72 km/h), adjust as needed.")]
    private float speedForMaxLookAhead = 320f;

    [Header("Steering & Centering Strength")]
    [Tooltip("Torque factor for turning the vehicle to match the lane's direction.")]
    private float torqueStrength = .002f;

    [Tooltip("Sideways force factor to pull the vehicle back to center of lane.")]
    private float lateralForce = .002f;

    [Tooltip("Force mode used when applying torque and lateral forces.")]
    private ForceMode forceMode = ForceMode.VelocityChange;

    private void FixedUpdate() {

        if (!CarController.canControl)
            return;

        // 1) Try to find the closest lane from HR_LaneManager
        HR_Lane lane = FindClosestLane(transform.position);

        if (!lane)
            return; // no lane found, do nothing

        // 2) Compute a dynamic look-ahead distance based on speed
        float speed = CarController.absoluteSpeed;

        // Interpolate between min and max look-ahead as speed goes from 0 to speedForMaxLookAhead
        float lookAheadDist = Mathf.Lerp(minLookAheadDistance, maxLookAheadDistance, speed / speedForMaxLookAhead);
        // Optionally clamp so it never exceeds the chosen range
        lookAheadDist = Mathf.Clamp(lookAheadDist, minLookAheadDistance, maxLookAheadDistance);

        // 3) Look "lookAheadDist" meters ahead in the vehicle's forward direction
        Vector3 lookAheadPos = transform.position + transform.forward * lookAheadDist;

        // 4) Get the nearest point & direction on that lane
        Vector3 laneForward;
        Vector3 lanePoint = lane.FindClosestPointOnPath(lookAheadPos, out laneForward);

        // If the lane's direction is basically zero, skip
        if (laneForward.sqrMagnitude < 0.0001f)
            return;

        // A) HEADING CORRECTION (Yaw Torque)
        Vector3 carFwdFlat = new Vector3(transform.forward.x, 0f, transform.forward.z).normalized;
        Vector3 laneFwdFlat = new Vector3(laneForward.x, 0f, laneForward.z).normalized;
        float angle = Vector3.SignedAngle(carFwdFlat, laneFwdFlat, Vector3.up);

        // Convert angle (degrees) to torque
        float yawTorque = angle * torqueStrength;
        CarController.Rigid.AddTorque(Vector3.up * yawTorque, forceMode);

        // B) LATERAL CORRECTION (Sideways Force)
        Vector3 offset = lanePoint - transform.position;
        offset.x *= 10f;
        offset.y = 0f;  // ignore vertical differences
        offset.z = 0f;  // ignore vertical differences
        Vector3 lateralPush = offset * lateralForce;
        CarController.Rigid.AddForce(lateralPush, forceMode);

        Vector3 localVelocity = transform.InverseTransformDirection(CarController.Rigid.angularVelocity);
        localVelocity.y *= .925f;
        CarController.Rigid.angularVelocity = Vector3.Lerp(CarController.Rigid.angularVelocity, transform.TransformDirection(localVelocity), 1f);

    }

    /// <summary>
    /// Finds the lane from HR_LaneManager that is closest to a given position.
    /// </summary>
    private HR_Lane FindClosestLane(Vector3 position) {

        if (!HR_LaneManager.Instance)
            return null;

        HR_Lane closestLane = null;
        float closestDistSq = float.PositiveInfinity;

        var allLaneWrappers = HR_LaneManager.Instance.lanes;

        if (allLaneWrappers == null || allLaneWrappers.Length == 0)
            return null;

        for (int i = 0; i < allLaneWrappers.Length; i++) {

            HR_Lane candidateLane = allLaneWrappers[i].lane;
            if (!candidateLane)
                continue;

            Vector3 dummyForward;
            Vector3 lanePoint = candidateLane.FindClosestPointOnPath(position, out dummyForward);

            float distSq = (lanePoint - position).sqrMagnitude;

            if (distSq < closestDistSq) {

                closestDistSq = distSq;
                closestLane = candidateLane;

            }

        }

        return closestLane;

    }

}
