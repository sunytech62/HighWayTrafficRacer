using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(RCCP_CarController))]
[RequireComponent(typeof(HR_PlayerStabilizer))]
public class HR_Player : MonoBehaviour
{
    public static HR_Player Instance;

    // Events
    public static UnityAction<HR_Player> OnPlayerSpawned;
    // public static UnityAction<HR_PlayerHandler, int,int> OnGameOver;
    // public static UnityAction</*HR_PlayerHandler, */int, HR_UI_DynamicScoreDisplayer.Side> OnNearMiss;
    public static UnityAction OnBikeCrashed;
    public static UnityAction<HR_Player> OnGameOver;
    private RCCP_CarController carController;
    public RCCP_CarController CarController
    {
        get
        {
            if (!carController) carController = GetComponent<RCCP_CarController>();
            return carController;
        }
    }

    private Rigidbody rigid;
    public Rigidbody Rigid
    {
        get
        {
            if (!rigid) rigid = GetComponent<Rigidbody>();
            return rigid;
        }
    }

    private HR_Settings settings;
    public HR_Settings Settings
    {
        get
        {
            if (!settings) settings = HR_Settings.Instance;
            return settings;
        }
    }

    private HR_GamePlayManager gamePlayManager;
    public HR_GamePlayManager GameplayManager
    {
        get
        {
            if (!gamePlayManager) gamePlayManager = HR_GamePlayManager.Instance;
            return gamePlayManager;
        }
    }

    private HR_PathManager pathManager;
    public HR_PathManager PathManager
    {
        get
        {
            if (!pathManager) pathManager = HR_PathManager.Instance;
            return pathManager;
        }
    }

    private HR_LaneManager laneManager;
    public HR_LaneManager LaneManager
    {
        get
        {
            if (!laneManager) laneManager = HR_LaneManager.Instance;
            return laneManager;
        }
    }

    public bool canCrash = true;

    public float damage = 0f;       // Current damage.
    public bool crashed = false;    // Is the game over?

    public float score;             // Current score
    public float timeLeft = 100f;   // Time left.
    public int combo;               // Current near miss combo.
    public int maxCombo;            // Highest combo count.

    public float speed = 0f;        // Current speed.
    public float distance = 0f;     // Total distance traveled.
    public float highSpeedCurrent = 0f; // Current high speed time.
    public float highSpeedTotal = 0f;   // Total high speed time.
    public float opposideDirectionCurrent = 0f; // Current opposite direction time.
    public float opposideDirectionTotal = 0f;   // Total opposite direction time.
    public int nearMisses;          // Total near misses.
    private float comboTime;        // Combo time for near misses.
    private Vector3 previousPosition; // Previous position used to calculate total traveled distance.

    private int MinimumSpeedToScore
    {
        get
        {
            return Settings.minimumSpeedForGainScore;
        }
    }
    private int MinimumSpeedToHighSpeed
    {
        get
        {
            return Settings.minimumSpeedForHighSpeed;
        }
    }

    public int TotalDistanceMoneyMP
    {
        get
        {
            return Settings.totalDistanceMoneyMP;
        }
    }
    public int TotalNearMissMoneyMP
    {
        get
        {
            return Settings.totalNearMissMoneyMP;
        }
    }
    public int TotalOverspeedMoneyMP
    {
        get
        {
            return Settings.totalOverspeedMoneyMP;
        }
    }
    public int TotalOppositeDirectionMP
    {
        get
        {
            return Settings.totalOppositeDirectionMP;
        }
    }

    private string currentTrafficCarNameLeft = null;
    private string currentTrafficCarNameRight = null;

    private bool oppositeDirection = false;

    public bool bombTriggered = false;
    public float bombHealth = 100f;

    private AudioSource hornSource;

    private float timerLeftIndicator, timerRightIndicator;

    public delegate void onNearMiss(HR_Player player, int score, HR_UI_DynamicScoreDisplayer.Side side);
    public static event onNearMiss OnNearMiss;

    private void Awake()
    {
        Instance = this;
        // Creating horn audio source.
        hornSource = HR_CreateAudioSource.NewAudioSource(gameObject, "Horn", 10f, 100f, 1f, Settings.hornClip, true, false, false);
    }
    private void OnDestroy()
    {
        Instance = null;
    }

    private void OnEnable()
    {
        CheckGroundGap();

        WheelCollider[] wheelColliders = GetComponentsInChildren<WheelCollider>(true);

        foreach (WheelCollider item in wheelColliders)
            item.forceAppPointDistance = .15f;

        OnPlayerSpawned?.Invoke(this);
    }

    private void Update()
    {
        // If scene doesn't include gameplay manager, return.
        if (!GameplayManager)
        {
            speed = 0f;
            previousPosition = transform.position;
            highSpeedCurrent = 0f;
            opposideDirectionCurrent = 0f;
            combo = 0;
            comboTime = 2f;
            return;
        }

        // If game is not started yet or if crashed, return.
        if (crashed || !GameplayManager.gameStarted)
        {
            speed = 0f;
            previousPosition = transform.position;
            highSpeedCurrent = 0f;
            opposideDirectionCurrent = 0f;
            combo = 0;
            comboTime = 2f;
            return;
        }

        // Total distance traveled.
        distance += Vector3.Distance(previousPosition, transform.position) / 1000f;
        previousPosition = transform.position;

        // Is speed high enough, gain score.
        if (speed >= MinimumSpeedToScore)
            score += CarController.speed * (Time.deltaTime * .05f);

        // If speed is higher than high speed threshold, gain “high speed” time.
        if (speed >= MinimumSpeedToHighSpeed)
        {
            highSpeedCurrent += Time.deltaTime;
            highSpeedTotal += Time.deltaTime;
        }
        else
        {
            highSpeedCurrent = 0f;
        }

        // 1) Try to find the closest lane from HR_LaneManager
        HR_Lane lane = FindClosestLane(transform.position);

        if (lane && lane.leftSide)
            oppositeDirection = true;
        else
            oppositeDirection = false;

        // If car is at opposite direction in TwoWay mode, gain “opposite direction” time.
        if (speed >= MinimumSpeedToHighSpeed && oppositeDirection)
        {
            opposideDirectionCurrent += Time.deltaTime;
            opposideDirectionTotal += Time.deltaTime;
        }
        else
        {
            opposideDirectionCurrent = 0f;
        }

        // If mode is time attack, reduce the timer.
        if (GameManager.SelectedMode == GameMode.TimeTrial)
        {
            timeLeft -= Time.deltaTime;

            // If timer hits 0, game over.
            if (timeLeft < 0)
            {
                timeLeft = 0;
                GameOver();
            }
        }

        comboTime += Time.deltaTime;

        // If game mode is bomb...
        if (GameManager.SelectedMode == GameMode.LowSpeedBomb)
        {
            // Bomb will be triggered below 80 km/h.
            if (speed > 80f)
            {
                if (!bombTriggered)
                    bombTriggered = true;
                else
                    bombHealth += Time.deltaTime * 5f;
            }
            else if (bombTriggered)
            {
                bombHealth -= Time.deltaTime * 10f;
            }

            bombHealth = Mathf.Clamp(bombHealth, 0f, 100f);

            // If bomb health hits 0, blow and game over.
            if (bombHealth <= 0f)
            {
                GameObject explosion = Instantiate(Settings.explosionEffect, transform.position, transform.rotation);
                explosion.transform.SetParent(null);

                Rigid.AddForce(Vector3.up * 10000f, ForceMode.Impulse);
                Rigid.AddTorque(Vector3.up * 10000f, ForceMode.Impulse);
                Rigid.AddTorque(Vector3.forward * 10000f, ForceMode.Impulse);

                HR_Bomb bomb = GetComponentInChildren<HR_Bomb>();

                if (bomb)
                    Destroy(bomb.gameObject);

                GameOver();

                for (int i = 0; i < CarController.AllWheelColliders.Length; i++)
                {
                    if (CarController.AllWheelColliders[i] == null)
                        continue;

                    CarController.AllWheelColliders[i].DetachWheel();
                }
            }
        }

        // Reset combo if no near-miss for a while.
        if (comboTime >= 2)
            combo = 0;

        IndicatorLights();
    }

    private void FixedUpdate()
    {
        // If scene doesn't include gameplay manager, return.
        if (!GameplayManager)
            return;

        // Speed of the car.
        speed = CarController.speed;

        // >>> ADDED <<< Make sure game is started and not crashed before auto-aligning
        if (!crashed && GameplayManager.gameStarted)
        {
            AlignToRoad();
            CheckNearMiss();
            Stability();
            IndicatorLights();
        }

        CheckPosition();
        CheckOutOfRoad();
    }

    // >>> ADDED <<<
    /// <summary>
    /// Slightly nudges the player’s steering toward the road’s forward direction.
    /// </summary>
    private void AlignToRoad()
    {

        // If there's no path manager or no car controller, do nothing
        if (!PathManager || !CarController)
            return;

        // Get the road direction from PathManager
        Vector3 pathDirection;
        Vector3 closestPoint = PathManager.FindClosestPointOnPath(transform.position, out pathDirection);

        // Signed angle between car's forward and the path direction
        float angleDiff = Vector3.SignedAngle(transform.forward, pathDirection, Vector3.up);

        // Some constants to tune how strong the auto-steer is
        float correctionFactor = 0.02f;   // Higher = more forceful steering
        float maxAutoSteer = 0.20f;   // Caps how much correction we apply

        // The final “auto” steer amount we add on top of user steering
        float autoSteer = Mathf.Clamp(angleDiff * correctionFactor, -maxAutoSteer, maxAutoSteer);

        // Combine auto-steering with the user's current steering input
        float userSteer = CarController.Inputs.steerInput;
        CarController.Inputs.steerInput = Mathf.Clamp(userSteer + autoSteer, -1f, 1f);
    }
    // >>> ADDED <<<


    /// <summary>
    /// Checks near vehicles by drawing raycasts to the left and right sides.
    /// </summary>
    private void CheckNearMiss()
    {

        Debug.DrawRay(CarController.AeroDynamics.COM.position, (-transform.right * 2f), Color.white);
        Debug.DrawRay(CarController.AeroDynamics.COM.position, (transform.right * 2f), Color.white);
        Debug.DrawRay(CarController.AeroDynamics.COM.position, (transform.forward * 50f), Color.white);

        int layerMaskCombined = (1 << LayerMask.NameToLayer(HR_Settings.Instance.trafficCarsLayer));

        // Raycasting to the left side.
        RaycastHit[] leftHits = Physics.RaycastAll(CarController.AeroDynamics.COM.position, (-transform.right), 2f, layerMaskCombined);

        if (leftHits != null && leftHits.Length > 0)
        {

            Transform trafficCar = null;

            for (int i = 0; i < leftHits.Length; i++)
            {

                if (leftHits[i].collider.attachedRigidbody)
                {

                    trafficCar = leftHits[i].collider.attachedRigidbody.transform;
                    break;

                }

            }

            if (trafficCar)
            {

                // If hits, get its name.
                currentTrafficCarNameLeft = trafficCar.name;

            }

        }
        else
        {

            if (currentTrafficCarNameLeft != null && speed > Settings.minimumSpeedForGainScore)
            {

                nearMisses++;
                combo++;
                comboTime = 0;

                if (maxCombo <= combo)
                    maxCombo = combo;

                score += 100f * Mathf.Clamp(combo / 1.5f, 1f, 20f);
                OnNearMiss?.Invoke(this, (int)(100f * Mathf.Clamp(combo / 1.5f, 1f, 20f)), HR_UI_DynamicScoreDisplayer.Side.Left);

            }

            currentTrafficCarNameLeft = null;

        }

        // Raycasting to the right side.
        RaycastHit[] rightHits = Physics.RaycastAll(CarController.AeroDynamics.COM.position, (transform.right), 2f, layerMaskCombined);

        if (rightHits != null && rightHits.Length > 0)
        {

            Transform trafficCar = null;

            for (int i = 0; i < rightHits.Length; i++)
            {

                if (rightHits[i].collider.attachedRigidbody)
                {

                    trafficCar = rightHits[i].collider.attachedRigidbody.transform;
                    break;

                }

            }

            if (trafficCar)
            {

                // If hits, get its name.
                currentTrafficCarNameRight = trafficCar.name;

            }

        }
        else
        {

            if (currentTrafficCarNameRight != null && speed > Settings.minimumSpeedForGainScore)
            {

                nearMisses++;
                combo++;
                comboTime = 0;

                if (maxCombo <= combo)
                    maxCombo = combo;

                score += 100f * Mathf.Clamp(combo / 1.5f, 1f, 20f);
                OnNearMiss?.Invoke(this, (int)(100f * Mathf.Clamp(combo / 1.5f, 1f, 20f)), HR_UI_DynamicScoreDisplayer.Side.Right);

            }

            currentTrafficCarNameRight = null;

        }

        // Raycasting to the front.
        RaycastHit[] frontHits = Physics.RaycastAll(CarController.AeroDynamics.COM.position, (transform.forward), 50f);

        if (frontHits != null && frontHits.Length > 0)
        {

            Transform trafficCar = null;

            for (int i = 0; i < frontHits.Length; i++)
            {

                if (frontHits[i].collider.gameObject.layer == LayerMask.NameToLayer(Settings.trafficCarsLayer))
                {

                    trafficCar = frontHits[i].collider.gameObject.transform;
                    break;

                }

            }

            if (trafficCar && CarController.Lights && CarController.Lights.highBeamHeadlights)
                trafficCar.SendMessageUpwards("ChangeLines", SendMessageOptions.DontRequireReceiver);

        }

        // Horn and siren.
        if (CarController.Lights && hornSource)
        {

            hornSource.volume = Mathf.Lerp(hornSource.volume, CarController.Lights.highBeamHeadlights ? 1f : 0f, Time.deltaTime * 25f);

            if (CarController.Lights.highBeamHeadlights)
            {

                RCCP_VehicleUpgrade_Siren upgradeSiren = GetComponentInChildren<RCCP_VehicleUpgrade_Siren>();

                if (upgradeSiren && upgradeSiren.isActiveAndEnabled)
                    hornSource.clip = Settings.sirenAudioClip;

                if (!hornSource.isPlaying)
                    hornSource.Play();

            }
            else
            {

                hornSource.Stop();

            }

        }

    }

    /// <summary>
    /// Called when a collision occurs.
    /// </summary>
    /// <param name="col">The collision data associated with this collision.</param>
    private void OnCollisionEnter(Collision col)
    {
        // If scene doesn't include gameplay manager, return.
        if (!GameplayManager) return;

        // If scene doesn't include gameplay manager, return.
        if (!GameplayManager.gameStarted) return;

        if (!canCrash) return;

        if (crashed) return;

        // If hit is not a traffic car, return.
        if (col.collider.gameObject.layer != LayerMask.NameToLayer(Settings.trafficCarsLayer)) return;

        // Calculating collision impulse.
        float impulse = col.impulse.magnitude / 2000f;

        // If impulse is below the limit, return.
        if (impulse < Settings.minimumCollisionForGameOver) return;

        // Increasing damage.
        damage += impulse * 3f;

        // Resetting combo to 0.
        combo = 0;


        if (GameManager.SelectedMode == GameMode.TimeTrial) return;
        if (GameManager.SelectedMode == GameMode.Endless)
        {
            GameOver();
            return;
        }

        // If mode is bomb mode, reduce the bomb health.
        if (GameManager.SelectedMode == GameMode.LowSpeedBomb)
        {
            bombHealth -= impulse * 3f;
            return;
        }

        if (damage > 100f)
        {
            damage = 100f;
            // Game over.
            if (GameManager.SelectedMode == GameMode.Challenge) return;
            GameOver();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        Debug.LogError("trig " + other.tag);
        if (other.CompareTag("TimeTrialGate"))
        {
            Debug.LogError("timeGate");
            timeLeft += 30;
            if (other.TryGetComponent<TimeTrial_Gate>(out var gate))
            {
                gate.SetGatePosition(true);
            }
        }
    }
    /// <summary>
    /// Checks the position of the car. If it exceeds limits, respawns it.
    /// </summary>
    private void CheckPosition()
    {

        if (Rigid.isKinematic)
            return;

        if (!PathManager)
            return;

        HR_CurvedRoad currentRoad = PathManager.FindRoadByPlayer();

        if (!currentRoad)
            return;

        Transform currentPoint = PathManager.closestPathPointToPlayer;

        if (!currentPoint)
            return;

        if (Vector3.Distance(transform.position, currentPoint.position) > 100f)
            ResetVehicle();

        if (GameplayManager.gameStarted && !crashed && speed <= 10f)
            ResetVehicle();

    }

    private void CheckOutOfRoad()
    {
        if (Rigid.isKinematic) return;

        if (!CarController.IsGrounded) return;

        HR_PathManager path = HR_PathManager.Instance;

        if (!path) return;

        if (!path.closestPathPointToPlayer) return;

        HR_CurvedRoad currentRoad = path.FindRoadByTransform(path.closestPathPointToPlayer);

        if (!currentRoad) return;

        float maxRoadWidth = currentRoad.roadWidth;

        Vector3 locVel = (CarController.Rigid.linearVelocity);
        Vector3 locAngVel = CarController.Rigid.angularVelocity;

        Vector3 closestPoint = FindClosestPointOnPath(transform.position, out Vector3 dir);
        bool right = transform.position.x >= closestPoint.x ? true : false;

        if (Vector3.Distance(transform.position, closestPoint) > maxRoadWidth)
        {
            if (right)
            {
                CarController.Rigid.AddForce(-Vector3.right * 2f, ForceMode.VelocityChange);
                locVel.x = Mathf.Clamp(locVel.x, -10f, 0f);

                CarController.Rigid.AddTorque(-Vector3.up * .5f, ForceMode.VelocityChange);
                locAngVel.y = Mathf.Clamp(locAngVel.y, -.15f, 0f);
            }
            else
            {
                CarController.Rigid.AddForce(Vector3.right * 2f, ForceMode.VelocityChange);
                locVel.x = Mathf.Clamp(locVel.x, 0f, 10f);

                CarController.Rigid.AddTorque(Vector3.up * .5f, ForceMode.VelocityChange);
                locAngVel.y = Mathf.Clamp(locAngVel.y, 0f, .15f);
            }
        }

        CarController.Rigid.linearVelocity = (locVel);
        CarController.Rigid.angularVelocity = locAngVel;
    }

    private void Stability()
    {
        if (Rigid.isKinematic) return;

        if (!CarController.IsGrounded) return;

        float steerInput = CarController.Inputs.inputs.steerInput * 2f;
        steerInput = Mathf.Clamp(steerInput, -1.2f, 1.2f);

        CarController.Rigid.AddRelativeForce(Vector3.right * steerInput * 2f, ForceMode.Acceleration);
    }

    private void IndicatorLights()
    {
        if (Rigid.isKinematic) return;

        if (!CarController.canControl) return;

        if (CarController.Lights)
        {
            float steerInput = CarController.steerInput_P;

            if (steerInput >= .5f)
                timerRightIndicator += Time.deltaTime;
            else
                timerRightIndicator = 0f;

            timerRightIndicator = Mathf.Clamp01(timerRightIndicator);

            if (steerInput <= -.5f)
                timerLeftIndicator += Time.deltaTime;
            else
                timerLeftIndicator = 0f;

            timerLeftIndicator = Mathf.Clamp01(timerLeftIndicator);

            if (timerRightIndicator >= .49f)
                CarController.Lights.indicatorsRight = true;
            else
                CarController.Lights.indicatorsRight = false;

            if (timerLeftIndicator >= .49f)
                CarController.Lights.indicatorsLeft = true;
            else
                CarController.Lights.indicatorsLeft = false;
        }
    }

    public void GameOver()
    {
        Debug.LogError("GameOver");
        if (crashed) return;

        crashed = true;

        CarController.canControl = false;
        CarController.engineRunning = false;

        if (CarController.Lights)
            CarController.Lights.indicatorsAll = true;

        CarController.Inputs.applyBrakeOnDisable = true;
        CarController.Inputs.applyHandBrakeOnDisable = true;

        int[] scores = new int[4];
        scores[0] = Mathf.FloorToInt(distance * TotalDistanceMoneyMP);
        scores[1] = Mathf.FloorToInt(nearMisses * TotalNearMissMoneyMP);
        scores[2] = Mathf.FloorToInt(highSpeedTotal * TotalOverspeedMoneyMP);
        scores[3] = Mathf.FloorToInt(opposideDirectionTotal * TotalOppositeDirectionMP);

        for (int i = 0; i < scores.Length; i++)
            HR_API.AddCurrency(scores[i]);

        GameplayManager.CrashedPlayer(this, scores);

    }

    /// <summary>
    /// Eliminates ground gap distance when spawned.
    /// </summary>
    private void CheckGroundGap()
    {

        WheelCollider wheel = GetComponentInChildren<WheelCollider>();

        if (!wheel)
            return;

        float distancePivotBetweenWheel = Vector3.Distance(new Vector3(0f, transform.position.y, 0f), new Vector3(0f, wheel.transform.position.y, 0f));

        RaycastHit hit;

        if (Physics.Raycast(wheel.transform.position, -Vector3.up, out hit, 10f))
            transform.position = new Vector3(transform.position.x, hit.point.y + distancePivotBetweenWheel + (wheel.radius / 1f) + (wheel.suspensionDistance / 2f), transform.position.z);

    }

    /// <summary>
    /// Resets the vehicle to a safe position.
    /// </summary>
    private void ResetVehicle()
    {

        HR_PathManager path = HR_PathManager.Instance;

        if (!path)
            return;

        Transform currentPoint = path.closestPathPointToPlayer;

        if (!currentPoint)
            return;

        transform.position = currentPoint.position;
        transform.rotation = currentPoint.rotation;

        transform.position += transform.up * 2f;
        transform.position += transform.forward * 15f;

        Rigid.angularVelocity = Vector3.zero;
        Rigid.linearVelocity = new Vector3(0f, 0f, 12f);

        Physics.SyncTransforms();

        HR_ResetDecal resetDecal = HR_Settings.Instance.resetDecal;

        if (resetDecal)
        {

            GameObject decalRenderer = Instantiate(resetDecal.gameObject, transform, false);
            decalRenderer.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

        }

    }

    /// <summary>
    /// Finds the closest point on the path to a given position.
    /// </summary>
    /// <param name="targetPosition">The target position.</param>
    /// <param name="pathDirection">The direction of the path.</param>
    /// <returns>The closest point on the path.</returns>
    private Vector3 FindClosestPointOnPath(Vector3 targetPosition, out Vector3 pathDirection)
    {

        List<Transform> closestPathPoints = HR_PathManager.Instance.pathPoints;
        Vector3 closestPoint = closestPathPoints[0].position;

        pathDirection = closestPathPoints[1].position - closestPathPoints[0].position; // Initial direction

        float minDistance = Vector3.Distance(targetPosition, closestPoint);

        for (int i = 0; i < closestPathPoints.Count - 1; i++)
        {

            Vector3 pointA = closestPathPoints[i].position;
            Vector3 pointB = closestPathPoints[i + 1].position;
            Vector3 projectedPoint = ProjectPointOnLineSegment(pointA, pointB, targetPosition);

            float distance = Vector3.Distance(targetPosition, projectedPoint);

            if (distance < minDistance)
            {

                closestPoint = projectedPoint;
                pathDirection = pointB - pointA; // Update direction based on the segment
                minDistance = distance;

            }

        }

        return closestPoint;

    }

    /// <summary>
    /// Projects a point onto a line segment.
    /// </summary>
    /// <param name="pointA">The start point of the line segment.</param>
    /// <param name="pointB">The end point of the line segment.</param>
    /// <param name="point">The point to project.</param>
    /// <returns>The projected point on the line segment.</returns>
    private Vector3 ProjectPointOnLineSegment(Vector3 pointA, Vector3 pointB, Vector3 point)
    {

        Vector3 AB = pointB - pointA;
        float t = Vector3.Dot(point - pointA, AB) / Vector3.Dot(AB, AB);
        t = Mathf.Clamp01(t);
        return pointA + t * AB;

    }

#if UNITY_EDITOR
    public bool CheckVehicleSetup()
    {

        bool changed = false;

        RCCP_CarController cController = GetComponent<RCCP_CarController>();

        if (cController)
        {

            if (cController.ineffectiveBehavior)
            {

                changed = true;
                cController.ineffectiveBehavior = false;

            }

        }

        Rigidbody rig = GetComponent<Rigidbody>();

        if (rig)
        {

            if (rig.linearDamping != 0.005f)
            {

                changed = true;
                rig.linearDamping = 0.005f;

            }

            if (rig.angularDamping != 0.1f)
            {

                changed = true;
                rig.angularDamping = 0.1f;

            }

        }

        RCCP_Customizer customizer = GetComponentInChildren<RCCP_Customizer>(true);

        if (customizer)
        {

            if (customizer.autoSave)
            {

                changed = true;
                customizer.autoSave = false;

            }

            if (customizer.autoLoadLoadout)
            {

                changed = true;
                customizer.autoLoadLoadout = false;

            }

        }

        RCCP_Nos nos = GetComponentInChildren<RCCP_Nos>(true);

        if (nos)
        {

            if (nos.torqueMultiplier != 3.5f)
            {

                changed = true;
                nos.torqueMultiplier = 3.5f;

            }

        }

        RCCP_Input input = GetComponentInChildren<RCCP_Input>(true);

        if (input)
        {

            if (input.applyBrakeOnDisable)
            {

                changed = true;
                input.applyBrakeOnDisable = false;

            }

            if (input.applyHandBrakeOnDisable)
            {

                changed = true;
                input.applyHandBrakeOnDisable = false;

            }

        }

        RCCP_Clutch clutch = GetComponentInChildren<RCCP_Clutch>(true);
        RCCP_Engine engine = GetComponentInChildren<RCCP_Engine>(true);

        if (clutch)
        {

            if (clutch.clutchInputRaw != 0)
            {

                changed = true;
                clutch.clutchInputRaw = 0f;

            }

            if (clutch.clutchInput != 0)
            {

                changed = true;
                clutch.clutchInput = 0f;

            }

            if (engine && clutch.engageRPM != engine.minEngineRPM)
            {

                changed = true;
                clutch.engageRPM = engine.minEngineRPM;

            }

        }

        RCCP_AeroDynamics aeroDynamics = GetComponentInChildren<RCCP_AeroDynamics>(true);

        if (aeroDynamics)
        {

            if (aeroDynamics.autoReset)
            {

                changed = true;
                aeroDynamics.autoReset = false;

            }

            if (aeroDynamics.ignoreRigidbodyDragOnAccelerate)
            {

                changed = true;
                aeroDynamics.ignoreRigidbodyDragOnAccelerate = false;

            }

        }

        RCCP_Axle[] axles = GetComponentsInChildren<RCCP_Axle>();

        if (axles != null && axles.Length > 0)
        {

            for (int i = 0; i < axles.Length; i++)
            {

                if (axles[i].maxBrakeTorque != 15000f)
                {

                    changed = true;
                    axles[i].maxBrakeTorque = 15000f;

                }

            }

        }

        return changed;

    }
#endif

    /// <summary>
    /// Resets the Rigidbody settings.
    /// </summary>
    private void Reset()
    {

        HR_PlayerStabilizer stabilizer = GetComponent<HR_PlayerStabilizer>();

        if (!stabilizer)
            stabilizer = gameObject.AddComponent<HR_PlayerStabilizer>();

    }

    /// <summary>
    /// Finds the lane from HR_LaneManager that is closest to a given position.
    /// </summary>
    private HR_Lane FindClosestLane(Vector3 position)
    {

        if (!HR_LaneManager.Instance)
            return null;

        HR_Lane closestLane = null;
        float closestDistSq = float.PositiveInfinity;

        var allLaneWrappers = HR_LaneManager.Instance.lanes;

        if (allLaneWrappers == null || allLaneWrappers.Length == 0)
            return null;

        for (int i = 0; i < allLaneWrappers.Length; i++)
        {

            HR_Lane candidateLane = allLaneWrappers[i].lane;
            if (!candidateLane)
                continue;

            Vector3 dummyForward;
            Vector3 lanePoint = candidateLane.FindClosestPointOnPath(position, out dummyForward);

            float distSq = (lanePoint - position).sqrMagnitude;

            if (distSq < closestDistSq)
            {

                closestDistSq = distSq;
                closestLane = candidateLane;

            }

        }

        return closestLane;

    }

    internal void AddTime(float timeToAdd)
    {
        timeLeft += timeToAdd;
    }
}
