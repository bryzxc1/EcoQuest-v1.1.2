using UnityEngine;
using TMPro;

public class NPCInteraction : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject interactPrompt; 
    [SerializeField] private GameObject dialogueBox;    
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private GameObject questCanvas;

    [Header("Trash Management")]
    [SerializeField] private GameObject trashContainer; 

    [Header("Settings")]
    [SerializeField] private string[] dialogueLines;

    private int currentLineIndex = 0;
    private bool isPlayerNearby = false;
    private bool isTalking = false;
    private QuestManager[] allQuests;

    private void Start()
    {
        interactPrompt.SetActive(false);
        dialogueBox.SetActive(false);
        
        if (questCanvas != null) questCanvas.SetActive(false);
        if (trashContainer != null) trashContainer.SetActive(false);

        allQuests = FindObjectsOfType<QuestManager>();
    }

    private void Update()
    {
        bool isQuestActive = questCanvas != null && questCanvas.activeInHierarchy;

        HandleInteractionPrompt(isQuestActive);

        if (isPlayerNearby && !isTalking && !isQuestActive && Input.GetKeyDown(KeyCode.E))
        {
            StartDialogue();
        }

        if (isTalking && Input.GetMouseButtonDown(0))
        {
            DisplayNextLine();
        }
    }

    private void HandleInteractionPrompt(bool isQuestActive)
    {
        bool shouldShowPrompt = isPlayerNearby && !isTalking && !isQuestActive;
        
        if (interactPrompt.activeSelf != shouldShowPrompt)
        {
            interactPrompt.SetActive(shouldShowPrompt);
        }
    }

    private void StartDialogue()
    {
        isTalking = true;
        currentLineIndex = 0;
        interactPrompt.SetActive(false);
        dialogueBox.SetActive(true);
        UpdateDialogueText();
    }

    private void DisplayNextLine()
    {
        currentLineIndex++;
        if (currentLineIndex < dialogueLines.Length)
        {
            UpdateDialogueText();
        }
        else
        {
            EndDialogue();
        }
    }

    private void UpdateDialogueText()
    {
        if (dialogueLines.Length > 0)
        {
            dialogueText.text = dialogueLines[currentLineIndex];
        }
    }

    private void EndDialogue()
    {
        isTalking = false;
        dialogueBox.SetActive(false);

        foreach (QuestManager qm in allQuests)
        {
            qm.ResetQuest(); 
        }
        
        if (trashContainer != null) trashContainer.SetActive(true);
        if (questCanvas != null) questCanvas.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) isPlayerNearby = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNearby = false;
            interactPrompt.SetActive(false);
            dialogueBox.SetActive(false);
            isTalking = false; 
        }
    }
}