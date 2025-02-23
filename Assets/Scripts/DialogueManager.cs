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

    [Header("Character configs")] [SerializeField]
    private Color char1Color;

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
            new DialogueEntry
            {
                characterName = char1Name,
                dialogue = "Incredible..."
            },
            new DialogueEntry
            {
                characterName = char2Name,
                dialogue = "It really is, isn't it?"
            },
            new DialogueEntry
            {
                characterName = char1Name,
                dialogue = "Yes! Honestly, if I hadn't seen it myself, I'm not even sure I could believe it."
            },
            new DialogueEntry
            {
                characterName = char1Name,
                dialogue =
                    "If you don't mind, could you tell me where?" // it would be nice if this line got cut off extra fast, rather than playing at normal speed (if easy to do)
            },
            new DialogueEntry
            {
                characterName = char2Name,
                dialogue = "Great, then let's begin at once!"
            },
            new DialogueEntry
            {
                characterName = char1Name,
                dialogue = "...Right. Commencing test on subject Z-001."
            },
            // End of intro

            // First tutorial text, stays on the screen until the player starts moving
            new DialogueEntry
            {
                characterName =
                    " ", // I wasn't sure if null would work here, but basically we just want the name to be empty for this
                dialogue = "(Use WASD to move!)" // This should use a variable for keybinds if they are rebindable
            },
            //End of first tutorial text

            // Start of event dialogue: Player starts moving
            new DialogueEntry
            {
                characterName = char2Name,
                dialogue = "Oh, it's a lively one! Excellent!"
            },
            new DialogueEntry
            {
                characterName = char1Name,
                dialogue = "It's interesting how it moves so... glorpily."
            },
            new DialogueEntry
            {
                characterName = char2Name,
                dialogue =
                    $"Hah! Glorpily?! I thought you were a scientist, {char1Name}!" // I think this works but double check for me I never use C#
            },
            new DialogueEntry
            {
                characterName = char1Name,
                dialogue = "I-" // Another line that should be cut off fast if possible
            },
            new DialogueEntry
            {
                characterName = char2Name,
                dialogue = "But you know what? I like it."
            },
            new DialogueEntry
            {
                characterName = char2Name,
                dialogue = "From now on, all Z-type specimens shall be referred to as Zlorp!"
            },
            new DialogueEntry
            {
                characterName = char2Name,
                dialogue = $"And now, {char1Name}, it's time to introduce the first wave of pathogens."
            },
            new DialogueEntry
            {
                characterName = char1Name,
                dialogue = "R-Right, introducing viral samples to the test enviroment..."
            },
            // End of event dialogue

            // Virus enemies start spawning

            // Second tutorial text, stays on screen until the player attacks once
            new DialogueEntry
            {
                characterName =
                    " ", // I wasn't sure if null would work here, but basically we just want the name to be empty for this
                dialogue =
                    "(Click and hold to prepare your attack. Use your mouse to aim before releasing it!)" // This should use a variable if there are different control methods
            },
            // End of second tutorial text

            // Start of dialogue event: Player attacks for the first time
            new DialogueEntry
            {
                characterName = char1Name,
                dialogue = "Fascinating... it appears the subject makes use of its own cytoplasm to create projectiles!"
            },
            new DialogueEntry
            {
                characterName = char1Name,
                dialogue = "It seems to harden considerably upon contact with the atmosphere."
            },
            new DialogueEntry
            {
                characterName = char2Name,
                dialogue =
                    "The problem is that its regenerative abilities aren't great enough to keep up the pace we need."
            },
            new DialogueEntry
            {
                characterName = char2Name,
                dialogue = "Once it starts to shrink, ready the first injection."
            },
            new DialogueEntry
            {
                characterName = char1Name,
                dialogue = "Got it."
            },
            // End of event dialogue
        };
    }
}