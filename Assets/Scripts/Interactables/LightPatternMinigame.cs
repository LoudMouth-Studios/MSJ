using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LightPatternMinigame : MonoBehaviour
{
    public static LightPatternMinigame Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private GameObject minigamePanel;
    [SerializeField] private Transform switchContainer;
    [SerializeField] private TMP_Text attemptsText;
    [SerializeField] private Button closeButton;

    [Header("Switch")]
    [SerializeField] private GameObject switchPrefab;

    [Header("Switch Visuals")]
    [SerializeField] private Sprite switchOnSprite;
    [SerializeField] private Sprite switchOffSprite;

    [Header("Pattern")]
    [SerializeField] private int patternLength = 5;

    [Header("Defeat")]
    [SerializeField] private string defeatSceneName = "Defeat";

    private bool[] correctPattern;
    private bool[] playerPattern;

    private int attempts;

    private JunctionBox currentJunctionBox;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        minigamePanel.SetActive(false);
		GenerateRandomPattern();
        
        if (closeButton != null)
        {
            closeButton.onClick.RemoveAllListeners();
            closeButton.onClick.AddListener(Close);
        }
    }

    public void Open(JunctionBox junctionBox)
    {
        if (junctionBox == null)
            return;

        currentJunctionBox = junctionBox;

        attempts = 0;

        minigamePanel.SetActive(true);

        CreateSwitches();

        UpdateAttemptsText();
    }

    private void GenerateRandomPattern()
    {
        correctPattern = new bool[patternLength];

        for (int i = 0; i < patternLength; i++)
        {
            correctPattern[i] = Random.Range(0, 2) == 1;
        }

        // Make sure the pattern isn't completely OFF
        bool hasOnSwitch = false;

        for (int i = 0; i < correctPattern.Length; i++)
        {
            if (correctPattern[i])
            {
                hasOnSwitch = true;
                break;
            }
        }

        if (!hasOnSwitch)
        {
            int randomIndex = Random.Range(0, correctPattern.Length);
            correctPattern[randomIndex] = true;
        }
    }

    public bool[] GetCurrentPattern()
    {
        return correctPattern;
    }

    private void CreateSwitches()
    {
        ClearSwitches();

        playerPattern = new bool[correctPattern.Length];

        for (int i = 0; i < correctPattern.Length; i++)
        {
            CreateSwitch(i);
        }
    }

    private void CreateSwitch(int index)
    {
        GameObject switchObject = Instantiate(
            switchPrefab,
            switchContainer
        );

        Button button = switchObject.GetComponent<Button>();
        Image image = switchObject.GetComponent<Image>();

        SetSwitchVisual(image, false);

        if (button != null)
        {
            button.onClick.AddListener(() =>
            {
                ToggleSwitch(index, image);
            });
        }
    }

    private void ToggleSwitch(int index, Image image)
    {
        playerPattern[index] = !playerPattern[index];

        SetSwitchVisual(image, playerPattern[index]);
    }

    private void SetSwitchVisual(Image image, bool isOn)
    {
        if (image == null)
            return;

        image.sprite = isOn
            ? switchOnSprite
            : switchOffSprite;
    }

    public void CheckPattern()
    {
        if (playerPattern == null)
            return;

        if (IsCorrectPattern())
        {
            Success();
        }
        else
        {
            WrongPattern();
        }
    }

    private bool IsCorrectPattern()
    {
        if (playerPattern.Length != correctPattern.Length)
            return false;

        for (int i = 0; i < playerPattern.Length; i++)
        {
            if (playerPattern[i] != correctPattern[i])
                return false;
        }

        return true;
    }

    private void WrongPattern()
    {
        attempts++;

        UpdateAttemptsText();

        if (attempts >= 3)
        {
            Defeat();
            return;
        }

        ResetSwitches();
    }

    private void ResetSwitches()
    {
        for (int i = 0; i < playerPattern.Length; i++)
        {
            playerPattern[i] = false;
        }

        foreach (Transform child in switchContainer)
        {
            Image image = child.GetComponent<Image>();

            if (image != null)
            {
                SetSwitchVisual(image, false);
            }
        }
    }

    private void Success()
    {
        ClearSwitches();

        minigamePanel.SetActive(false);

        if (currentJunctionBox != null)
        {
            currentJunctionBox.MinigameCompleted();
        }

        currentJunctionBox = null;
    }

    private void Defeat()
    {
        if (GameManager.CheatsEnabled)
        {
            attempts = 0;          // reset mistakes and let the player try again
            UpdateAttemptsText();
            ResetSwitches();
            return;
        }

        currentJunctionBox = null;
        SceneManager.LoadScene(defeatSceneName);
    }

    private void UpdateAttemptsText()
    {
        if (attemptsText != null)
        {
            attemptsText.text = "Mistakes: " + attempts + " / 3";
        }
    }

    private void ClearSwitches()
    {
        foreach (Transform child in switchContainer)
        {
            Destroy(child.gameObject);
        }
    }
    public void Close()
    {
        ClearSwitches();

        minigamePanel.SetActive(false);

        currentJunctionBox = null;
    }
   
}