using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Persistent singleton that plays a camera-zoom + fade-to-black transition before loading a scene.
public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance { get; private set; }

    CanvasGroup fadeCanvasGroup;
    float lastFadeDuration = 0.3f;

    public static SceneTransitionManager GetInstance()
    {
        if (Instance == null)
        {
            var go = new GameObject("SceneTransitionManager");
            Instance = go.AddComponent<SceneTransitionManager>();
        }
        return Instance;
    }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        CreateFadeOverlay();
    }

    void CreateFadeOverlay()
    {
        var canvasGO = new GameObject("FadeCanvas", typeof(Canvas), typeof(CanvasScaler), typeof(GraphicRaycaster));
        canvasGO.transform.SetParent(transform);
        var canvas = canvasGO.GetComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 32760;

        var imageGO = new GameObject("FadeImage", typeof(Image));
        imageGO.transform.SetParent(canvasGO.transform, false);
        var image = imageGO.GetComponent<Image>();
        image.color = Color.black;
        var rt = image.rectTransform;
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        fadeCanvasGroup = imageGO.AddComponent<CanvasGroup>();
        fadeCanvasGroup.alpha = 0f;
        fadeCanvasGroup.blocksRaycasts = false;
        fadeCanvasGroup.interactable = false;
    }

    public void LoadSceneWithZoom(string sceneName, RectTransform zoomTarget, float zoomScale = 4f, float zoomDuration = 0.45f, float fadeDuration = 0.3f)
    {
        StartCoroutine(TransitionRoutine(sceneName, zoomTarget, zoomScale, zoomDuration, fadeDuration));
    }

    IEnumerator TransitionRoutine(string sceneName, RectTransform zoomTarget, float zoomScale, float zoomDuration, float fadeDuration)
    {
        lastFadeDuration = fadeDuration;
        fadeCanvasGroup.blocksRaycasts = true;

        if (zoomTarget != null)
        {
            yield return ZoomIn(zoomTarget, zoomScale, zoomDuration);
        }

        yield return Fade(0f, 1f, fadeDuration);

        SceneManager.sceneLoaded += OnSceneLoadedFadeIn;
        SceneManager.LoadScene(sceneName);
    }

    // Scales the target's canvas content around the target's screen position, so the clicked button stays put while everything else zooms in around it.
    IEnumerator ZoomIn(RectTransform target, float zoomScale, float duration)
    {
        Transform zoomRoot = GetOrCreateZoomRoot(target);
        Vector3 targetWorldPos = target.position;
        Vector3 offset = zoomRoot.position - targetWorldPos;
        Vector3 initialScale = zoomRoot.localScale;

        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            float t = Mathf.Clamp01(elapsed / duration);
            float eased = t * t * (3f - 2f * t);
            float scale = Mathf.Lerp(1f, zoomScale, eased);
            zoomRoot.localScale = initialScale * scale;
            zoomRoot.position = targetWorldPos + offset * scale;
            yield return null;
        }
    }

    Transform GetOrCreateZoomRoot(RectTransform anyChild)
    {
        Canvas canvas = anyChild.GetComponentInParent<Canvas>();
        Transform canvasTransform = canvas.transform;
        Transform existing = canvasTransform.Find("ZoomRoot");
        if (existing != null)
        {
            return existing;
        }

        var go = new GameObject("ZoomRoot", typeof(RectTransform));
        RectTransform rt = go.GetComponent<RectTransform>();
        rt.SetParent(canvasTransform, false);
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;

        int childCount = canvasTransform.childCount;
        for (int i = childCount - 1; i >= 0; i--)
        {
            Transform child = canvasTransform.GetChild(i);
            if (child == rt)
            {
                continue;
            }
            child.SetParent(rt, true);
        }

        return rt;
    }

    IEnumerator Fade(float from, float to, float duration)
    {
        float elapsed = 0f;
        fadeCanvasGroup.alpha = from;
        while (elapsed < duration)
        {
            elapsed += Time.unscaledDeltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(from, to, elapsed / duration);
            yield return null;
        }
        fadeCanvasGroup.alpha = to;
    }

    void OnSceneLoadedFadeIn(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoadedFadeIn;
        StartCoroutine(FadeInAfterLoad());
    }

    IEnumerator FadeInAfterLoad()
    {
        yield return Fade(1f, 0f, lastFadeDuration);
        fadeCanvasGroup.blocksRaycasts = false;
    }
}
