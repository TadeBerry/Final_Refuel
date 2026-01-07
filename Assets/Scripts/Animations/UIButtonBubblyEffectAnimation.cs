using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

public class UIButtonBubblyEffectAnimation : MonoBehaviour
{
    private float hoverScaleMultiplier = 1.10f;
    private float duration = 0.1f;
    private float clickScaleMultiplier = 0.9f;

    private Dictionary<Transform, Vector3> originalScales = new Dictionary<Transform, Vector3>();
    private Dictionary<Transform, Coroutine> activeCoroutines = new Dictionary<Transform, Coroutine>();

    private void OnEnable()
    {
        originalScales.Clear();

        Button[] allButtons = GetComponentsInChildren<Button>(true);

        foreach (Button btn in allButtons)
        {
            Transform buttonTransform = btn.transform;
            
            if (!originalScales.ContainsKey(buttonTransform))
            {
                originalScales.Add(buttonTransform, buttonTransform.localScale);
            }

            EventTrigger trigger = btn.gameObject.GetComponent<EventTrigger>() ?? btn.gameObject.AddComponent<EventTrigger>();
            trigger.triggers.Clear();

            //hover
            AddEvent(trigger, EventTriggerType.PointerEnter, () => StartScaling(buttonTransform, true));
            
            //exit
            AddEvent(trigger, EventTriggerType.PointerExit, () => StartScaling(buttonTransform, false));
            
            //selection
            AddEvent(trigger, EventTriggerType.Select, () => StartScaling(buttonTransform, true));
            AddEvent(trigger, EventTriggerType.Deselect, () => StartScaling(buttonTransform, false));
            
            //click
            AddEvent(trigger, EventTriggerType.PointerDown, () => {
                ResetButtonRoutine(buttonTransform);
                activeCoroutines[buttonTransform] = StartCoroutine(ClickEffect(buttonTransform));
            });
        }
    }

    private void StartScaling(Transform buttonTransform, bool isExpanding)
    {
        ResetButtonRoutine(buttonTransform);
        if (!originalScales.ContainsKey(buttonTransform)) return;

        Vector3 target = isExpanding ? originalScales[buttonTransform] * hoverScaleMultiplier : originalScales[buttonTransform];
        activeCoroutines[buttonTransform] = StartCoroutine(ScaleTo(buttonTransform, target));
    }

    private void ResetButtonRoutine(Transform buttonTransform)
    {
        if (activeCoroutines.ContainsKey(buttonTransform) && activeCoroutines[buttonTransform] != null)
        {
            StopCoroutine(activeCoroutines[buttonTransform]);
            activeCoroutines[buttonTransform] = null;
        }
    }

    private void AddEvent(EventTrigger trigger, EventTriggerType type, System.Action action)
    {
        EventTrigger.Entry entry = new EventTrigger.Entry { eventID = type };
        entry.callback.AddListener((data) => action());
        trigger.triggers.Add(entry);
    }

    private IEnumerator ScaleTo(Transform buttonTransform, Vector3 targetScale)
    {
        float time = 0;
        Vector3 startScale = buttonTransform.localScale;
        while (time < duration)
        {
            if (buttonTransform == null) yield break;
            buttonTransform.localScale = Vector3.Lerp(startScale, targetScale, time / duration);
            time += Time.unscaledDeltaTime; 
            yield return null;
        }
        if (buttonTransform != null) buttonTransform.localScale = targetScale;
    }

    private IEnumerator ClickEffect(Transform buttonTransform)
    {
        Vector3 original = originalScales[buttonTransform];
        yield return StartCoroutine(ScaleTo(buttonTransform, original * clickScaleMultiplier));
        yield return StartCoroutine(ScaleTo(buttonTransform, original * hoverScaleMultiplier));
    }
}