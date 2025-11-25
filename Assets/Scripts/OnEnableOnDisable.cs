using UnityEngine;
using UnityEngine.Events;

public class OnEnableOnDisable : MonoBehaviour
{
    [SerializeField] UnityEvent onEnableEvent;
    [SerializeField] UnityEvent onDisableEvent;

    private void OnEnable()
    {
        onEnableEvent?.Invoke();
    }
    private void OnDisable()
    {
        onDisableEvent?.Invoke();
    }
}
