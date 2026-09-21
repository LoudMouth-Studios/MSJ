using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    // public string characterName;
    //
    // public Sprite portrait;

    [TextArea(2, 5)]
    public string text;
}