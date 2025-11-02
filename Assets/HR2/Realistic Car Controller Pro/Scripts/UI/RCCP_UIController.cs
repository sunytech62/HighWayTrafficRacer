using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[AddComponentMenu("BoneCracker Games/Realistic Car Controller Pro/UI/Mobile/RCCP UI Controller")]
public class RCCP_UIController : RCCP_UIComponent, IPointerDownHandler, IPointerUpHandler
{
    private Button button;

    [Range(0f, 1f)] public float input = 0f;

    [Min(0f)] public float sensitivity = 5f;

    [Min(0f)] public float gravity = 5f;

    public bool isPressing = false;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        input = 0f;
        isPressing = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressing = true;
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        isPressing = false;
    }
    private void Update()
    {
        if (button && !button.interactable)
        {
            isPressing = false;
            input = 0f;
            return;
        }

        if (isPressing)
            input += Time.deltaTime * sensitivity;
        else
            input -= Time.deltaTime * gravity;

        if (input < 0f)
            input = 0f;

        if (input > 1f)
            input = 1f;
    }

    private void OnDisable()
    {

        input = 0f;
        isPressing = false;
    }
}
