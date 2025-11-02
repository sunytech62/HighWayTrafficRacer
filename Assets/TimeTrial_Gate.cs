using System;
using UnityEngine;

public class TimeTrial_Gate : MonoBehaviour
{

    Collider collider;

    private void OnEnable()
    {
        collider = GetComponent<Collider>();
        SetGatePosition();
        InvokeRepeating(nameof(CheckDirectionToCar), 0.5f, 1);
    }
    private void OnDisable()
    {
        CancelInvoke();
    }

    void CheckDirectionToCar()
    {
        if (!HR_Player.Instance) return;
        Vector3 toTarget = (HR_Player.Instance.transform.position - transform.position).normalized;
        if (Vector3.Dot(transform.forward, toTarget) > 0)
        {
            SetGatePosition();
        }
    }

    public void SetGatePosition(bool isAddTime = false)
    {
        collider.enabled = false;
        Debug.LogError("Gate Pos Set");
        var addedDist = transform.position;
        var carPos = HR_Player.Instance.transform.position;
        transform.position = new Vector3(carPos.x, carPos.y, carPos.z + 500);
        transform.rotation = Quaternion.identity;
        transform.position = HR_PathManager.Instance.FindClosestPointOnPathWithTransform(transform.position, out var fff).position;
        if (isAddTime) HR_Player.Instance.AddTime(Vector3.Distance(addedDist, transform.position) / 500);
        Invoke(nameof(EnableCollider), 3f);
    }

    private void EnableCollider()
    {
        collider.enabled = true;
    }
}
