using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private float typingSpeed = 0.05f;
    [Header("Character configs")]
    [SerializeField] private Color char1Color;
    [SerializeField] private Color char2Color;
    [SerializeField] private string char1Name;
    [SerializeField] private string char2Name;
    [SerializeField] private Sprite char1Portrait;
    [SerializeField] private Sprite char2Portrait;
    [SerializeField] private PlayerInput _playerInput;
    //private vars
    private DialogueEntry[] dialogueEntries;
    private int _currentDialogueIndex = -1;
    private bool isTyping = false;
    private Coroutine typingCoroutine;
    //Events
    public UnityEvent onDialogueComplete;

    // Dialogue entry struct to hold both name and dialogue
    [System.Serializable]
    public struct DialogueEntry
    {
        public string characterName;
        public string dialogue;
    }

    private void OnEnable()
    {
        _playerInput.actions.FindAction("SkipDialog").performed += HandleSkipDialog;
    }

    private void OnDisable()
    {
        _playerInput.actions.FindAction("SkipDialog").performed -= HandleSkipDialog;
    }

    private void HandleSkipDialog(InputAction.CallbackContext ctx)
    {
        SkipTyping();
    }

    // Call this from Unity Events to show next dialogue
    public void ShowNextDialogue()
    {
        _currentDialogueIndex++;
        
        // Disable if we reach the end
        if (_currentDialogueIndex >= dialogueEntries.Length)
        {
            this.gameObject.SetActive(false);
        }

        ShowDialogue(dialogueEntries[_currentDialogueIndex]);
    }

    // Call this to show a specific dialogue entry
    public void ShowDialogueEntry(int index)
    {
        if (index >= 0 && index < dialogueEntries.Length)
        {
            _currentDialogueIndex = index;
            ShowDialogue(dialogueEntries[index]);
        }
    }

    private void ShowDialogue(DialogueEntry entry)
    {
        // Update name immediately
        nameText.text = entry.characterName;

        // Stop any existing typing
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
        }

        // Start typing new text
        typingCoroutine = StartCoroutine(TypeText(entry.dialogue));
    }

    public void ResetDialogue()
    {
        _currentDialogueIndex = -1;
    }

    private IEnumerator TypeText(string text)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char c in text)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        onDialogueComplete.Invoke();
    }

    //TODO cate can u make this go when pressing space??

    // i did yw <3
    public void SkipTyping()
    {
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            dialogueText.text = dialogueEntries[_currentDialogueIndex].dialogue;
            isTyping = false;
            onDialogueComplete.Invoke();
        }
    }
    
    // Dialogue entries
    public void Awake()
    {
        dialogueEntries = new DialogueEntry[]
        {
            // Start of intro
            new DialogueEntry { 
                characterName = char1Name,
                dialogue = "Incredible..."
            },
            new DialogueEntry {
                characterName = char2Name,
                dialogue = "It really is, isn't it?"
            },
            new DialogueEntry {
                characterName = char1Name,
                dialogue = "Yes! Honestly, if I hadn't seen it myself, I'm not sure I could believe it."
            },
            new DialogueEntry {
                characterName = char2Name,
                dialogue = "If you don't mind, could you tell me where—" // it would be nice if this line got cut off extra fast, rather than playing at normal speed
            },
            // End of intro
        };
    }
}