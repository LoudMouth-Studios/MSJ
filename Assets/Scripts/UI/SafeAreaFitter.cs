using UnityEngine;

// Fits this RectTransform inside the device's safe area (notches, punch-hole cameras, rounded
// corners) and updates it when the safe area changes, for example after rotating the device.
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