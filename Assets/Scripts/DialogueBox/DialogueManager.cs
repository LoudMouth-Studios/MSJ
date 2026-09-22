using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private Image profilePicture;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private Button dialogueButton;

    [Header("Dialogue")]
    [SerializeField] private DialogueLine[] dialogueLines;

    [Header("Level")]
    [SerializeField] private LevelManager levelManager;

    [Header("Typewriter Settings")]
    [SerializeField] private float typingSpeed = 0.03f;

    private int currentLine = 0;

    private Coroutine typingCoroutine;
    private bool isTyping = false;

    private void Start()
    {
        dialogueButton.onClick.AddListener(NextLine);
    }

    public void StartDialogue()
    {
        if (dialogueLines.Length == 0)
            return;

        currentLine = 0;

        dialogueBox.SetActive(true);

        ShowLine();
    }

    private void ShowLine()
    {
        DialogueLine line = dialogueLines[currentLine];

        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
        }

        typingCoroutine = StartCoroutine(TypeText(line.text));
    }

    private IEnumerator TypeText(string text)
    {
        isTyping = true;

        dialogueText.text = "";

        foreach (char letter in text)
        {
            dialogueText.text += letter;

            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    public void NextLine()
    {
        if (isTyping)
        {
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
            }

            dialogueText.text = dialogueLines[currentLine].text;

            isTyping = false;

            return;
        }
        
        currentLine++;
        
        if (currentLine >= dialogueLines.Length)
        {
            dialogueBox.SetActive(false);
            
            levelManager.StartLevelTimer();

            return;
        }
        
        ShowLine();
    }
}