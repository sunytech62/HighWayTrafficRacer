using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HR_CurvedRoad : MonoBehaviour
{
    public Transform[] bones;

    public bool useRandomizedCurves = true;

    [Min(0f)] public float minimumCurveAngle = 30f;

    [Min(0f)] public float maximumCurveAngle = 90f;

    public Transform endPoint;

    public AnimationCurve curve = new AnimationCurve(new Keyframe(0f, 0f),
                                                     new Keyframe(.5f, 1f),
                                                     new Keyframe(1f, 0f));

    private float randomCurveInput = 1f;

    public Vector3 curveVector = Vector3.one;

    private Vector3 randomVector = Vector3.one;

    private Vector3[] initialPositions;

    private Quaternion[] initialRotations;

    [Min(1f)] public float roadWidth = 5.5f;

    [System.Serializable]
    public class SkinnedColliders
    {
        public SkinnedMeshRenderer skinnedMeshRenderer;
        public MeshCollider meshCollider;
        [HideInInspector] public Mesh bakedMesh;
    }

    public SkinnedColliders[] skinnedColliders;

    private void Awake()
    {
        // Store initial local positions and rotations of bones
        initialPositions = new Vector3[bones.Length];
        initialRotations = new Quaternion[bones.Length];

        for (int i = 0; i < bones.Length; i++)
        {
            initialPositions[i] = bones[i].localPosition;
            initialRotations[i] = bones[i].localRotation;
        }
    }

    public void RandomizeCurve()
    {
        // 1) Respect the toggle:
        if (!useRandomizedCurves)
            return;

        // 2) Randomize a Vector3 within a specific range for each component
        //    (Currently using direct +/- curveVector.x,y,z in world-y local space).
        randomVector = new Vector3(
            Random.Range(-curveVector.x, curveVector.x),
            Random.Range(-curveVector.y, curveVector.y),
            Random.Range(-curveVector.z, curveVector.z)
        );

        // 3) Random angle input
        float angle = Random.Range(minimumCurveAngle, maximumCurveAngle);

        // 4) Optionally flip sign to get left vs. right curve
        if (Random.Range(0, 2) == 1)
            angle = -angle;

        // If you want to limit just how negative it can go, you could clamp it here.
        // angle = Mathf.Clamp(angle, -someLimit, someLimit);

        randomCurveInput = angle;

        // Now apply everything:
        UpdateEverything();
    }

    public void UpdateEverything()
    {
        // 1) Reset each bone to its initial local pos/rot
        for (int i = 0; i < bones.Length; i++)
        {
            bones[i].localPosition = initialPositions[i];
            bones[i].localRotation = initialRotations[i];
        }

        // 2) Apply random offset and orientation changes
        for (int i = 0; i < bones.Length; i++)
        {
            if (i == 0)
                continue;

            // Use the animation curve to see how strong the offset is at this bone index
            float curveFactor = curve.Evaluate((float)i / ((float)bones.Length - 1));

            // Using direct randomVector in bone’s local space as given:
            bones[i].localPosition += randomVector * randomCurveInput * curveFactor;

            Vector3 direction = bones[i - 1].position - bones[i].position;
            bones[i].rotation = Quaternion.LookRotation(direction, transform.up);

            // If you truly need them reversed, do the 180:
            bones[i].Rotate(Vector3.up, 180f);
        }

        // 2) Apply random offset and orientation changes
        for (int i = 0; i < bones.Length; i++)
        {
            if (i == bones.Length - 1)
                continue;

            Vector3 direction = bones[i + 1].position - bones[i].position;
            bones[i].rotation = Quaternion.LookRotation(direction, transform.up);
        }

        // 3) Update colliders if needed (can be heavy if done every frame)
        UpdateColliders();
    }

    private void UpdateColliders()
    {
        for (int i = 0; i < skinnedColliders.Length; i++)
        {
            // Create a new mesh for baking
            skinnedColliders[i].bakedMesh = new Mesh();

            // Bake the current pose of the SkinnedMeshRenderer
            skinnedColliders[i].skinnedMeshRenderer.BakeMesh(skinnedColliders[i].bakedMesh);
            skinnedColliders[i].bakedMesh.RecalculateNormals();
            skinnedColliders[i].bakedMesh.RecalculateTangents(); // Only if your shader needs tangents

            // Reassign it to the mesh collider
            skinnedColliders[i].meshCollider.sharedMesh = null;
            skinnedColliders[i].meshCollider.sharedMesh = skinnedColliders[i].bakedMesh;
        }
    }

    private List<Transform> SortByZPosition(List<Transform> pathPoints)
    {
        pathPoints = pathPoints.Where(item => item != null).ToList();
        pathPoints.Sort((a, b) => a.transform.position.z.CompareTo(b.transform.position.z));
        return pathPoints;
    }

    [ContextMenu("Set EndPosition")]
    public void SetEndPosition()
    {
        endPoint = transform.Find("EndPoint");

        if (!endPoint)
        {
            endPoint = new GameObject("EndPoint").transform;
            endPoint.SetParent(transform, false);
        }

        endPoint.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

        // Get the bounding box size in world space
        Vector3 bounds = HR_GetBounds.GetBoundsSize(transform);

        // Move in local-forward direction by the road's length in z
        // If your road is oriented so that forward = +Z, transform.forward might be best:
        // If you are certain your road is aligned with +Z in world space, Vector3.forward * bounds.z is okay.
        endPoint.transform.position += transform.forward * bounds.z;
        // Or, if your road always spawns aligned with world Z, the existing approach is fine:
        // endPoint.transform.position += Vector3.forward * bounds.z;
    }

    /// <summary>
    /// Finds and sets the bones for the road automatically by searching for child objects named "Bone".
    /// </summary>
    [ContextMenu("Find Bones")]
    public void FindBones()
    {
        List<Transform> foundBones = new List<Transform>();

        foreach (Transform item in transform)
        {
            if (item.name.Contains("Bone"))
                foundBones.Add(item);
        }

        bones = foundBones.ToArray();
    }

    /// <summary>
    /// Checks and sorts the order of the bones based on their Z position.
    /// </summary>
    [ContextMenu("Check Bones Order")]
    public void CheckBonesOrder()
    {
        List<Transform> bonesOrder = bones.ToList();
        bonesOrder = SortByZPosition(bonesOrder);
        bones = bonesOrder.ToArray();
    }

    /// <summary>
    /// Smooths the bone positions by averaging each bone with its neighbors.
    /// </summary>
    [ContextMenu("Smooth Bones")]
    public void SmoothBones()
    {
        if (bones == null || bones.Length < 2)
            return;

        // We'll parent all new bones under the same parent as the first bone:
        Transform boneParent = bones[0].parent ?? transform;

        var newBones = new List<Transform>();

        for (int i = 0; i < bones.Length; i++)
        {
            // 1) keep the existing bone
            newBones.Add(bones[i]);

            // 2) if not the last one, insert midpoint bone
            if (i < bones.Length - 1)
            {
                var a = bones[i];
                var b = bones[i + 1];

                // world‐space midpoint
                Vector3 midPos = (a.position + b.position) * 0.5f;
                Quaternion midRot = Quaternion.Slerp(a.rotation, b.rotation, 0.5f);

                // create the new GameObject
                var go = new GameObject(a.name + "_sub");
                go.transform.SetParent(boneParent, false);

                // convert world→local
                go.transform.position = midPos;
                go.transform.rotation = midRot;

                newBones.Add(go.transform);
            }
        }

        // 3) replace your bone list
        bones = newBones.ToArray();
    }

    /// <summary>
    /// Draws gizmos in the editor for visualization.
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.magenta;

        if (endPoint)
            Gizmos.DrawSphere(endPoint.position, .6f);

        for (int i = 0; i < bones.Length; i++)
        {
            Gizmos.color = Color.green;

            if (bones[i])
                Gizmos.DrawSphere(bones[i].position, .6f);

            if (i < bones.Length - 1 && bones[i] && bones[i + 1])
                Gizmos.DrawLine(bones[i].position, bones[i + 1].position);
        }
    }

    /// <summary>
    /// Resets the end position of the road if needed.
    /// </summary>
    private void Reset()
    {
        SetEndPosition();
    }
}
