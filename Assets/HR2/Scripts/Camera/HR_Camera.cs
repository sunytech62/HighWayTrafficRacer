//----------------------------------------------
//                   Highway Racer
//
// Copyright © 2014 - 2025 BoneCracker Games
// https://www.bonecrackergames.com
//----------------------------------------------

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the camera functionalities for the game.
/// </summary>
public class HR_Camera : MonoBehaviour {

    /// <summary>
    /// The target the camera should follow.
    /// </summary>
    public HR_Player player;

    /// <summary>
    /// Actual camera component.
    /// </summary>
    private Camera actualCamera;

    /// <summary>
    /// Enum for different camera modes.
    /// </summary>
    public enum CameraMode { Top, TPS, TPS_Fixed, FPS }

    [Space()]
    public CameraMode cameraMode = CameraMode.Top;

    /// <summary>
    /// Camera mode index.
    /// </summary>
    public int CameraModeIndex {

        get {

            switch (cameraMode) {

                case CameraMode.Top:
                    return 0;

                case CameraMode.TPS:
                    return 1;

                case CameraMode.TPS_Fixed:
                    return 2;

                case CameraMode.FPS:
                    return 3;

                default:
                    break;

            }

            return 0;

        }

    }

    /// <summary>
    /// // The height from the target to the camera
    /// </summary>
    public float height_Top = 2.5f;

    /// <summary>
    /// The distance from the target to the camera
    /// </summary>
    public float distance_Top = 8.5f;
    public Quaternion rotation_Top = Quaternion.identity;

    /// <summary>
    /// The height from the target to the camera
    /// </summary>
    [Space()] public float height_TPS = 2.5f;

    /// <summary>
    /// The distance from the target to the camera
    /// </summary>
    public float distance_TPS = 8.5f;
    public Quaternion rotation_TPS = Quaternion.identity;

    /// <summary>
    /// Speed of rotation
    /// </summary>
    [Space()] public float rotationSpeed = 2f;

    /// <summary>
    /// Tilt the camera related to curve angle of the road.
    /// </summary>
    [Space()] public bool tilt = true;
    public float tiltMultiplier = 4f;

    private Vector3 targetPosition = Vector3.zero;
    private Quaternion targetRotation = Quaternion.identity;

    private void Awake() {

        actualCamera = GetComponentInChildren<Camera>();

    }

    /// <summary>
    /// Called when the script instance is being loaded.
    /// </summary>
    private void OnEnable() {

        HR_Events.OnPlayerSpawned += HR_GamePlayHandler_OnPlayerSpawned;
        RCCP_InputManager.OnChangedCamera += ChangeCameraMode;

    }

    /// <summary>
    /// Changes the camera mode.
    /// </summary>
    public void ChangeCameraMode() {

        switch (CameraModeIndex) {

            case 0:

                cameraMode = CameraMode.TPS;
                break;

            case 1:

                cameraMode = CameraMode.TPS_Fixed;
                break;

            case 2:

                cameraMode = CameraMode.FPS;
                break;

            case 3:

                cameraMode = CameraMode.Top;
                break;

        }

    }

    /// <summary>
    /// Called when the script instance is being disabled.
    /// </summary>
    private void OnDisable() {

        HR_Events.OnPlayerSpawned -= HR_GamePlayHandler_OnPlayerSpawned;
        RCCP_InputManager.OnChangedCamera -= ChangeCameraMode;

    }

    /// <summary>
    /// Called when the player is spawned.
    /// </summary>
    /// <param name="spawnedPlayer">The spawned player.</param>
    private void HR_GamePlayHandler_OnPlayerSpawned(HR_Player spawnedPlayer) {

        player = spawnedPlayer;

    }

    /// <summary>
    /// Called once per frame, after all Update functions have been called.
    /// </summary>
    private void LateUpdate() {

        if (player == null)
            return;

        if (player.crashed) {

            CrashCamera();
            return;

        }

        // Find the closest point on the path to the target
        Vector3 closestPoint = HR_PathManager.Instance.FindClosestPointOnPath(player.transform.position, out Vector3 pathDirection);

        if (closestPoint == Vector3.zero)
            return;

        switch (cameraMode) {

            case CameraMode.Top:

                // Set the camera's position to the closest point on the path
                targetPosition = closestPoint - pathDirection.normalized * distance_Top;
                targetPosition.y = player.transform.position.y + height_Top;

                // Calculate the target rotation based on the path direction
                targetRotation = Quaternion.LookRotation(pathDirection) * rotation_Top;

                actualCamera.fieldOfView = 60f;

                break;

            case CameraMode.TPS:

                // Set the camera's position to the closest point on the path
                targetPosition = closestPoint - pathDirection.normalized * distance_TPS;
                targetPosition.y = player.transform.position.y + height_TPS;

                // Calculate the target rotation based on the path direction
                targetRotation = Quaternion.LookRotation(pathDirection) * rotation_TPS;

                actualCamera.fieldOfView = 60f;

                break;

            case CameraMode.TPS_Fixed:

                // Set the camera's position to the closest point on the path
                targetPosition = closestPoint - pathDirection.normalized * distance_TPS;
                targetPosition.y = player.transform.position.y + height_TPS;
                targetPosition.x = player.transform.position.x;

                // Calculate the target rotation based on the path direction
                targetRotation = Quaternion.LookRotation(pathDirection) * rotation_TPS;

                actualCamera.fieldOfView = 60f;

                break;

            case CameraMode.FPS:

                // Set the camera's position to the closest point on the path
                targetPosition = player.transform.position;
                targetPosition += Vector3.up * .6f;
                targetPosition += player.transform.forward * 1.5f;

                float tiltAmount = player.transform.InverseTransformDirection(player.CarController.Rigid.angularVelocity).y * 3f;
                Quaternion tilt = Quaternion.Euler(0f, tiltAmount * 2f, tiltAmount * -2f);

                // Calculate the target rotation based on the path direction
                targetRotation = Quaternion.LookRotation(pathDirection);
                targetRotation *= tilt;

                actualCamera.fieldOfView = 70f;

                break;

        }

        // Convert the rotations to forward vectors
        Vector3 forwardA = Quaternion.Euler(0f, transform.eulerAngles.y, 0f) * Vector3.forward;
        Vector3 forwardB = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f) * Vector3.forward;

        // Calculate the signed angle around the up axis (Y axis)
        float signedAngle = Vector3.SignedAngle(forwardA, forwardB, Vector3.up);

        if (!tilt)
            signedAngle = 0f;

        Quaternion tiltAngle = Quaternion.LookRotation(Vector3.forward) * Quaternion.Euler(0f, 0f, -signedAngle * tiltMultiplier);

        if (cameraMode != CameraMode.FPS) {

            transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * 5f);
            transform.position = new Vector3(transform.position.x, targetPosition.y, targetPosition.z);

        } else {

            transform.position = targetPosition;

        }

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation * tiltAngle, rotationSpeed * Time.deltaTime);

        actualCamera.transform.localPosition = Vector3.zero;
        actualCamera.transform.localRotation = Quaternion.identity;

        ApplySpeedBasedShake(player.CarController.absoluteSpeed);

    }

    private void CrashCamera() {

        transform.LookAt(player.transform);
        transform.Rotate(Vector3.forward, -10f);

        float distance = Vector3.Distance(transform.position, player.transform.position);

        actualCamera.fieldOfView = Mathf.Lerp(65f, 5f, Mathf.InverseLerp(-50f, 50f, distance));

    }

    /// <summary>
    /// Applies a speed-based shake to the camera by offsetting its local position/rotation 
    /// with mild Perlin noise. Call this every frame from LateUpdate().
    /// </summary>
    /// <param name="speed">Vehicle speed (or some fraction of it) that drives the shake amount.</param>
    private void ApplySpeedBasedShake(float speed) {

        // 1) Adjust these constants to tune the overall “feel” of the shake.
        float maxShakeAmplitude = 0.15f;     // Maximum offset in local space
        float frequency = 2.5f;              // How quickly the noise changes
        float speedFactor = .0015f;         // How much speed influences the amplitude

        // 2) Calculate final amplitude based on speed. Clamped to avoid extreme values.
        float amplitude = Mathf.Clamp(speed * speedFactor, 0f, maxShakeAmplitude);

        // 3) Get Perlin noise values in range [0..1], then subtract 0.5f so it centers at 0.
        //    Multiplying by amplitude for the final offset.
        float shakeX = (Mathf.PerlinNoise(Time.time * frequency, 0f) - 0.5f) * amplitude;
        float shakeY = (Mathf.PerlinNoise(0f, Time.time * frequency) - 0.5f) * amplitude;

        // 4) Option A: Offset the local position. 
        //    Usually you store your "base" position, then add noise each frame.
        Vector3 baseLocalPos = Vector3.zero;  // Could store in a field if you want a reference point
        Vector3 localPositionOffset = new Vector3(shakeX, shakeY, 0f);

        actualCamera.transform.localPosition = baseLocalPos + localPositionOffset;

        // 5) Option B: If you want rotational shake (like the camera “shuddering”):
        //    We’ll do a small roll (Z) or pitch (X). Adjust amplitude & frequency as needed.
        float rollAngle = shakeX * 5f;      // 5 degrees max, for example
        float pitchAngle = shakeY * 5f;     // 5 degrees max, for example

        // Combine rotation with your existing orientation. 
        // For example, if you already have a “targetRotation,” you could do:
        actualCamera.transform.localRotation *= Quaternion.Euler(pitchAngle, 0f, rollAngle);

    }

}
