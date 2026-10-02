using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class WireCutMinigame : MonoBehaviour
{
    public static WireCutMinigame Instance { get; private set; }

    [Header("UI")]
    [SerializeField] private GameObject minigamePanel;
    [SerializeField] private Transform wireContainer;

    private int triesLeft;
    
    [Header("Text")]
    [SerializeField] private TMP_Text wireText;

    private string correctWire;

    [Header("Wire")]
    [SerializeField] private GameObject wireButtonPrefab;

    [Header("Wire Sprites")]
    [SerializeField] private Sprite blueWire;
    [SerializeField] private Sprite greenWire;
    [SerializeField] private Sprite redWire;
    [SerializeField] private Sprite yellowWire;

    [Header("Defeat")]
    [SerializeField] private string defeatSceneName = "Defeat";

    private List<WireData> wires = new List<WireData>();

    private PowerInteractable currentPower;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        minigamePanel.SetActive(false);
    }
    
    public void Open(PowerInteractable power)
    {
        if (power == null)
            return;

        currentPower = power;

        minigamePanel.SetActive(true);

        triesLeft = 2;
        GenerateWireText();
        CreateWires();
    }

    private void GenerateWireText()
    {
        string[] colors = { "RED", "YELLOW", "GREEN", "BLUE" };

        // Randomly select the correct wire.
        int wireIndex = Random.Range(0, colors.Length);
        correctWire = colors[wireIndex];

        // Randomly select a different text color.
        int textColorIndex = Random.Range(0, colors.Length - 1);

        if (textColorIndex >= wireIndex)
        {
            textColorIndex++;
        }

        string textColor = colors[textColorIndex].ToLower();

        // Update the displayed text.
        wireText.text = $"CUT THE <color={textColor}>{correctWire}</color> WIRE";
    }

    private void CreateWires()
    {
        ClearWires();

        wires.Clear();
        
        wires.Add(new WireData("Blue", blueWire));
        wires.Add(new WireData("Green", greenWire));
        wires.Add(new WireData("Red", redWire));
        wires.Add(new WireData("Yellow", yellowWire));

        Shuffle(wires);
        
        foreach (WireData wire in wires)
        {
            CreateWireButton(wire);
        }
    }

    private void CreateWireButton(WireData wire)
    {
        GameObject wireObject = Instantiate(
            wireButtonPrefab,
            wireContainer
        );

        Image image = wireObject.GetComponent<Image>();

        if (image != null)
        {
            image.sprite = wire.sprite;
            image.preserveAspect = true;
        }

        Button button = wireObject.GetComponent<Button>();

        if (button != null)
        {
            string wireColor = wire.colorName;

            button.onClick.AddListener(() => CutWire(wireColor));
        }
    }

    private void CutWire(string selectedWire)
    {
        if (selectedWire.ToUpper() == correctWire)
        {
            Success();
        }
        else
        {
            Defeat();
        }
    }

    private void Success()
    {
        ClearWires();

        minigamePanel.SetActive(false);

        if (currentPower != null)
        {
            currentPower.MinigameCompleted();
        }

        currentPower = null;
    }

    private void Defeat()
    {
        if (triesLeft == 0)
        {
            if (GameManager.CheatsEnabled)
                return;            // out of tries, but with cheats on you can keep trying

            currentPower = null;
            SceneManager.LoadScene(defeatSceneName);
        }
        else
        {
            triesLeft--;
        }
    }

    private void ClearWires()
    {
        foreach (Transform child in wireContainer)
        {
            Destroy(child.gameObject);
        }
    }

    private void Shuffle(List<WireData> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);

            WireData temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    private class WireData
    {
        public string colorName;
        public Sprite sprite;

        public WireData(string colorName, Sprite sprite)
        {
            this.colorName = colorName;
            this.sprite = sprite;
        }
    }
}