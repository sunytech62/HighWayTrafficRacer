using Unity.Cinemachine;
using UnityEngine;

public class GarageCam : MonoBehaviour
{
    [SerializeField] float movingSpeed = 5;
    private float actualMovingSpeed;
    CinemachineOrbitalFollow orbitalFollow;
    CinemachineInputAxisController inputAxisController;

    private float verticalAxisValue;
    bool isDraging = false;
    private float dragingDelay;

    private void Start()
    {
        if (!orbitalFollow) orbitalFollow = GetComponent<CinemachineOrbitalFollow>();
        if (!inputAxisController) inputAxisController = GetComponent<CinemachineInputAxisController>();
        actualMovingSpeed = movingSpeed;
        verticalAxisValue = orbitalFollow.VerticalAxis.Value;
    }

    void Update()
    {
        if (isDraging)
        {
            movingSpeed = 0;
            dragingDelay = 3f;
            return;
        }
        if (dragingDelay > 0)
        {
            dragingDelay -= Time.deltaTime;
            return;
        }

        if (movingSpeed < actualMovingSpeed)
            movingSpeed += Time.deltaTime * 2;

        orbitalFollow.HorizontalAxis.Value += movingSpeed * Time.deltaTime;
        orbitalFollow.VerticalAxis.Value = Mathf.Lerp(orbitalFollow.VerticalAxis.Value, verticalAxisValue, Time.deltaTime);
    }

    public void OnPointerDown()
    {
        isDraging = true;
        inputAxisController.Controllers.ForEach((i) =>
        {
            if (i.Name == "Look Orbit X")
            {
                i.Enabled = true;
            }
            if (i.Name == "Look Orbit Y")
            {
                i.Enabled = true;
            }
        });
    }

    public void OnPointerUp()
    {
        isDraging = false;
        inputAxisController.Controllers.ForEach((i) =>
        {
            if (i.Name == "Look Orbit X")
            {
                i.Enabled = false;
            }
            if (i.Name == "Look Orbit Y")
            {
                i.Enabled = false;
            }
        });
    }
}
