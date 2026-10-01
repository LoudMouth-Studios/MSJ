using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

// Fades a spot Light2D in on scene start and can fade it out on demand (used for scene transitions).
[RequireComponent(typeof(Light2D))]
public class Spotlight : MonoBehaviour
{
    [SerializeField] float fadeDuration = 0.5f;
    [SerializeField] float startDelay = 0f;
    [SerializeField] bool fadeInOnStart = true;

    Light2D light2D;
    float fullIntensity;
    Coroutine fadeRoutine;

    void Awake()
    {
        light2D = GetComponent<Light2D>();
        fullIntensity = light2D.intensity;
        light2D.intensity = 0f;
    }

    void Start()
    {
        if (fadeInOnStart)
        {
            fadeRoutine = StartCoroutine(DelayedFadeIn());
        }
    }

    IEnumerator DelayedFadeIn()
    {
        if (startDelay > 0f)
        {
            yield return new WaitForSecondsRealtime(startDelay);
        }
        yield return FadeRoutine(fullIntensity, null);
    }

    public void TurnOff(Action onComplete = null)
    {
        FadeTo(0f, onComplete);
    }

    public void TurnOn(Action onComplete = null)
    {
        FadeTo(fullIntensity, onComplete);
    }

    void FadeTo(float target, Action onComplete)
    {
        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
        }
        fadeRoutine = StartCoroutine(FadeRoutine(target, onComplete));
    }

    IEnumerator FadeRoutine(float target, Action onComplete)
    {
        float start = light2D.intensity;
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            light2D.intensity = Mathf.Lerp(start, target, elapsed / fadeDuration);
            yield return null;
        }
        light2D.intensity = target;
        fadeRoutine = null;
        onComplete?.Invoke();
    }
}
