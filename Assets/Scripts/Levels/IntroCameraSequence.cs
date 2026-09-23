using UnityEngine;

public class IntroCameraSequence : MonoBehaviour
{
    [Header("This object's own camera (auto-filled)")]
    [SerializeField] private Camera introCamera;
    [SerializeField] private AudioListener introListener;

    [Header("Gameplay camera (Main Camera, holds the Cinemachine Brain)")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private AudioListener mainListener;

    [Header("UI to hide until gameplay starts")]
    [SerializeField] private GameObject gameplayCanvas;
    
    [Header("Dialogue to start once gameplay begins")]
    [SerializeField] private DialogueManager dialogueManager;


    void Reset()
    {
        introCamera = GetComponent<Camera>();
        introListener = GetComponent<AudioListener>();
    }

    void Awake()
    {
        introCamera.enabled = true;
        introListener.enabled = true;

        mainCamera.enabled = false;
        mainListener.enabled = false;

        if (gameplayCanvas != null)
            gameplayCanvas.SetActive(false);

        GameManager.IsPaused = true;
    }

    // Hooked up as an Animation Event on the last frame of the pan clip
    public void OnIntroFinished()
    {
        introCamera.enabled = false;
        introListener.enabled = false;

        mainCamera.enabled = true;
        mainListener.enabled = true;

        if (gameplayCanvas != null)
            gameplayCanvas.SetActive(true);

        GameManager.IsPaused = false;
        
        if (dialogueManager != null)
            dialogueManager.StartDialogue();
    }
}