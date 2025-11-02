using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TextMeshProUGUI))]
public class HR_UI_TextPopperOnTextChange : MonoBehaviour
{
    private TextMeshProUGUI text;

    public string oldText = "";

    private float timer = 0f;

    private Vector3 defaultScale = Vector3.zero;

    public bool interacting = false;

    private void OnEnable()
    {
        timer = 0f;
        interacting = false;
    }

    private void OnDisable()
    {
        timer = 0f;
        interacting = false;
    }

    private void LateUpdate()
    {
        if (!text) text = GetComponent<TextMeshProUGUI>();

        if (!text) return;

        if (defaultScale == Vector3.zero) defaultScale = text.transform.localScale;

        if (text.text != oldText) timer = 1f;

        oldText = text.text;

        if (timer > 0)
        {
            timer -= Time.unscaledDeltaTime * 3f;

            if (!interacting)
                StartCoroutine(Pop());
        }
        else
        {
            timer = 0f;
            interacting = false;
            text.transform.localScale = Vector3.Lerp(text.transform.localScale, defaultScale, Time.unscaledDeltaTime * 5f);
        }
    }

    /// <summary>
    /// Applies the pop effect by changing the scale of the text.
    /// </summary>
    /// <returns>Coroutine for the pop effect.</returns>
    private IEnumerator Pop()
    {
        interacting = true;

        text.transform.localScale *= 1.2f;
        float time = 1f;

        while (time > 0f)
        {
            time -= Time.deltaTime;
            text.transform.localScale = Vector3.Lerp(text.transform.localScale, defaultScale, Time.unscaledDeltaTime * 5f);

            yield return null;
        }
        text.transform.localScale = defaultScale;
    }
}
