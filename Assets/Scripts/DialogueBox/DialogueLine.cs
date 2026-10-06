using UnityEngine;

// One line of dialogue, filled in on the DialogueManager in the Inspector.
[System.Serializable]
public class DialogueLine
{
    [TextArea(2, 5)]
    public string text;
}