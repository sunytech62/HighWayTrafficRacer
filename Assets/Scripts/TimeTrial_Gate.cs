using System;
using UnityEngine;
using UnityEngine.UI;

public class TimeTrial_Gate : MonoBehaviour
{
    public float carMaxDistance = 1000f;

    [SerializeField] Slider progressBar;

    Collider collider;

    Vector3 carStartPos;
    float timerCheckDirection;

    private void OnEnable()
    {
        collider = GetComponent<Collider>();
        SetGatePosition(true);
    }

    private void LateUpdate()
    {
        if (!HR_Player.Instance) return;

        progressBar.value = Vector3.Distance(HR_Player.Instance.transform.position, carStartPos);

        timerCheckDirection += Time.deltaTime;
        if (timerCheckDirection > 1)
        {
            timerCheckDirection = 0;
            CheckDirectionToCar();
        }
    }

    void CheckDirectionToCar()
    {
        Vector3 toTarget = (HR_Player.Instance.transform.position - transform.position).normalized;
        float distanceToCar = Vector3.Distance(HR_Player.Instance.transform.position, transform.position);
        if (distanceToCar > carMaxDistance || Vector3.Dot(transform.forward, toTarget) > 0)
        {
            SetGatePosition();
        }
    }

    public void SetGatePosition(bool isAddTime = false)
    {
        collider.enabled = false;
        Debug.LogError("Gate Pos Set");
        var beforePos = transform.position;
        var carPos = HR_Player.Instance.transform.position;
        carStartPos = carPos;
        transform.position = new Vector3(carPos.x, carPos.y, carPos.z + (carMaxDistance - 10));
        transform.rotation = Quaternion.identity;
        var wpPos = HR_PathManager.Instance.FindClosestPointOnPathWithTransform(transform.position, out var fff).position;
        transform.position = new Vector3(wpPos.x, wpPos.y + 2.5f, wpPos.z);
        if (isAddTime)
        {
            float timeToAdd = 0f;
            if (HR_Player.Instance.timeLeft < 5)
                timeToAdd = 40f;
            else if (HR_Player.Instance.timeLeft < 20)
                timeToAdd = 25;
            else if (HR_Player.Instance.timeLeft < 40)
                timeToAdd = 10;
            else
                timeToAdd = 5;
            //   timeToAdd = Vector3.Distance(beforePos, transform.position) / (carMaxDistance / HR_Player.Instance.timeLeft);
            HR_Player.Instance.AddTime(timeToAdd);
        }
        progressBar.maxValue = Vector3.Distance(HR_Player.Instance.transform.position, transform.position);
        Invoke(nameof(EnableCollider), 3f);
    }

    private void EnableCollider()
    {
        collider.enabled = true;
    }
}
