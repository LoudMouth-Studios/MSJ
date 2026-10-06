using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;

// Shows dialogue lines one by one with a typewriter effect. The dialogue button
// finishes the line that is being typed, or goes to the next line.
public class DialogueManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private Button dialogueButton;

    [Header("Dialogue")]
    [SerializeField] private DialogueLine[] dialogueLines;

    [Header("Typewriter Settings")]
    [SerializeField] private float typingSpeed = 0.03f;

    private int currentLine = 0;

    private Coroutine typingCoroutine;
    private bool isTyping = false;

    // Cached so the typewriter doesn't allocate a new WaitForSeconds for every letter.
    private WaitForSeconds typingWait;
    private float typingWaitDuration = -1f;

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

            yield return GetTypingWait();
        }

        isTyping = false;
    }

    // Rebuilds the cached wait only when typingSpeed changed (e.g. tweaked in the Inspector during Play).
    private WaitForSeconds GetTypingWait()
    {
        if (typingWait == null || typingWaitDuration != typingSpeed)
        {
            typingWait = new WaitForSeconds(typingSpeed);
            typingWaitDuration = typingSpeed;
        }

        return typingWait;
    }

    public void NextLine()
    {
        // If the text is still typing,
        // instantly show the complete sentence.
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
            return;
        }

        ShowLine();
    }
}