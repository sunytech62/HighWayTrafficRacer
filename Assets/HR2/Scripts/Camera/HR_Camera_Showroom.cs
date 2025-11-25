using UnityEngine;
using UnityEngine.EventSystems;

public class HR_Camera_Showroom : MonoBehaviour
{
    public Transform target;
    public float distance = 8f;

    [Space]
    public bool orbitingNow = true;
    public float orbitSpeed = 5f;
    public float smoothSpeed = 5f;

    [Space]
    public float minY = 5f;
    public float maxY = 35f;

    [Space]
    public float dragSpeed = 10f;
    public float orbitX = 0f;
    public float orbitY = 0f;

    private void OnEnable()
    {
        Quaternion desiredRotation = Quaternion.Euler(orbitY, orbitX, 0);
        transform.rotation = desiredRotation;
    }
    private void LateUpdate()
    {
        if (!target)
            return;

        if (orbitingNow)
            orbitX += Time.deltaTime * orbitSpeed;

        orbitY = ClampAngle(orbitY, minY, maxY);

        Quaternion desiredRotation = Quaternion.Euler(orbitY, orbitX, 0);

        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, Time.deltaTime * smoothSpeed);

        Vector3 direction = transform.rotation * Vector3.back;
        Vector3 desiredPosition = target.transform.position + direction * distance;

        transform.position = desiredPosition;
    }
    private float ClampAngle(float angle, float min, float max)
    {

        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;

        return Mathf.Clamp(angle, min, max);

    }
    public void ToggleAutoRotation(bool state)
    {
        orbitingNow = state;
    }
    public void OnDrag(PointerEventData pointerData)
    {
        float x = pointerData.delta.x * dragSpeed * .04f;

        if (x > 10f)
            x = 10f;
        if (x < -10f)
            x = -10f;

        orbitX += x;
        orbitY -= pointerData.delta.y * dragSpeed * .04f;
    }

    public void Reset()
    {
        HR_MainMenuManager mainMenuManager = FindFirstObjectByType<HR_MainMenuManager>();

        if (!mainMenuManager)
            return;

        Transform spawnPoint = mainMenuManager.carSpawnLocation;

        if (!spawnPoint)
            return;
        target = spawnPoint;
    }
}
