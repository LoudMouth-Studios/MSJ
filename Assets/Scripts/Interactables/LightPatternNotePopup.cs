using UnityEngine;
using UnityEngine.UI;

// Popup that draws the light pattern from the note as on/off switches.
public class LightPatternNotePopup : MonoBehaviour
{
    public static LightPatternNotePopup Instance { get; private set; }

    [SerializeField] private GameObject popup;
    [SerializeField] private GameObject[] onSwitches;
    [SerializeField] private GameObject[] offSwitches;
    [SerializeField] private Button closeButton;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (closeButton != null)
        {
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(Close);
        }

        popup.SetActive(false);
    }

    public void Open(bool[] pattern)
    {
        popup.SetActive(true);

        for (int i = 0; i < pattern.Length; i++)
        {
            if (i >= onSwitches.Length || i >= offSwitches.Length)
                continue;

            if (pattern[i])
            {
                onSwitches[i].SetActive(true);
                offSwitches[i].SetActive(false);
            }
            else
            {
                onSwitches[i].SetActive(false);
                offSwitches[i].SetActive(true);
            }
        }
    }

    public void Close()
    {
        popup.SetActive(false);
    }
}