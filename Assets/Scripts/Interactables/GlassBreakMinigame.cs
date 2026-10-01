using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GlassBreakMinigame : MonoBehaviour
{
    public static GlassBreakMinigame Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private GameObject popup;
    [SerializeField] private Button glassButton;
    [SerializeField] private Button closeButton;
    [SerializeField] private TMP_Text tapCounter;
    
    [Header("Glass Images")]
    [SerializeField] private GameObject glass1;
    [SerializeField] private GameObject glass2;
    [SerializeField] private GameObject glass3;
    
    [Header("Crack Thresholds")]
   // [SerializeField] private int glass1Threshold = 5;
    [SerializeField] private int glass2Threshold = 2;
    [SerializeField] private int glass3Threshold = 4;

    [Header("Glass Breaking")]
    [SerializeField] private int requiredTaps = 25;

    private int currentTaps;

    private Diamond currentDiamond;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        popup.SetActive(false);

        glassButton.onClick.RemoveAllListeners();
        glassButton.onClick.AddListener(TapGlass);

        closeButton.onClick.RemoveAllListeners();
        closeButton.onClick.AddListener(Close);
    }

    public void Open(Diamond diamond)
    {
        if (diamond == null)
            return;

        currentDiamond = diamond;

        currentTaps = 0;

        popup.SetActive(true);

        UpdateTapCounter();
    }

    private void TapGlass()
    {
        currentTaps++;

        UpdateTapCounter();
        UpdateGlassImage();

        if (currentTaps >= requiredTaps)
        {
            BreakGlass();
        }
    }
    private void UpdateGlassImage()
    {
        if (glass1 != null)
            glass1.SetActive(false);

        if (glass2 != null)
            glass2.SetActive(false);

        if (glass3 != null)
            glass3.SetActive(false);

        if (currentTaps < glass2Threshold)
        {
            if (glass1 != null)
                glass1.SetActive(true);
        }
        else if (currentTaps < glass3Threshold)
        {
            if (glass2 != null)
                glass2.SetActive(true);
        }
        else
        {
            if (glass3 != null)
                glass3.SetActive(true);
        }
    }

    private void UpdateTapCounter()
    {
        if (tapCounter != null)
        {
            tapCounter.text = currentTaps + " / " + requiredTaps;
        }
    }

    private void BreakGlass()
    {
        Debug.Log("Glass broken!");

        if (currentDiamond != null)
        {
            currentDiamond.Collect();
        }

        Close();
    }

    public void Close()
    {
        popup.SetActive(false);

        currentDiamond = null;
        currentTaps = 0;
    }
}