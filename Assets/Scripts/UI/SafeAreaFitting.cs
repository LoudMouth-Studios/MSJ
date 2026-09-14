using UnityEngine;

// Production SafeAreaFitter (Rotation & Notch Aware):
[RequireComponent(typeof(RectTransform))]
public class SafeAreaFitter : MonoBehaviour {
    private RectTransform _rt;
    private Rect _lastSafe;
    void Awake() { _rt = GetComponent<RectTransform>(); ApplySafeArea(); }
    void Update() {
        if (Screen.safeArea != _lastSafe) ApplySafeArea();
    }
    void ApplySafeArea() {
        _lastSafe = Screen.safeArea;
        Vector2 min = _lastSafe.position, max = min + _lastSafe.size;
        min.x /= Screen.width; min.y /= Screen.height;
        max.x /= Screen.width; max.y /= Screen.height;
        _rt.anchorMin = min; _rt.anchorMax = max;
    }
}