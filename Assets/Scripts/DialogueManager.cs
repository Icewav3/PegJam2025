using System;
using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private string char2Name = "Dr. Zorbist";
    [SerializeField] private Sprite char1Portrait;
    [SerializeField] private Sprite char2Portrait;

    [SerializeField] private PlayerInput _playerInput;

    private Dictionary<string, List<DialogueEntry>> _dialogueSequences;
    private List<DialogueEntry> _currentSequence;
    private int _currentDialogueIndex = -1;
    private bool isTyping = false;
    private Coroutine typingCoroutine;

    private object life_number;
    public event Action onDialogueComplete;

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

    private void Start()
    {
        InitializeDialogueSequences();
    }

    private void InitializeDialogueSequences()
    {
        _dialogueSequences = new Dictionary<string, List<DialogueEntry>>();

        // Introduction sequence
        _dialogueSequences["intro"] = new List<DialogueEntry>
        {
            new DialogueEntry { characterName = char1Name, dialogue = "Incredible..." },
            new DialogueEntry { characterName = char2Name, dialogue = "It really is, isn't it?" },
            new()
            {
                characterName = char1Name,
                dialogue = "Yes! Honestly, if I hadn't seen it myself, I'm not even sure I could believe it."
            },
            new DialogueEntry { characterName = char1Name, dialogue = "If you don't mind, could you tell me where-" },
            new()
            {
                characterName = char2Name,
                dialogue = $"Great, then let's begin at once! Zorbist Biotics Incorporated is counting on you, {char1Name}."
            },
            new DialogueEntry { characterName = char1Name, dialogue = "...Right. Commencing test on subject Z-001." }
        };

        // Movement tutorial
        _dialogueSequences["movement_tutorial"] = new List<DialogueEntry>
        {
            new DialogueEntry { characterName = " ", dialogue = "(Use WASD to move!)" }
        };

        // Movement reaction
        _dialogueSequences["movement_reaction"] = new List<DialogueEntry>
        {
            new DialogueEntry { characterName = char2Name, dialogue = "Oh, it's a lively one! Excellent!" },
            new DialogueEntry { characterName = char1Name, dialogue = "It's interesting how it moves so... glorpily." },
            new()
            {
                characterName = char2Name, dialogue = $"Hah! Glorpily?! I thought you were a scientist, {char1Name}!"
            },
            new DialogueEntry { characterName = char1Name, dialogue = "I-" },
            new DialogueEntry { characterName = char2Name, dialogue = "But you know what? I like it." },
            new()
            {
                characterName = char2Name, dialogue = "From now on, all Z-type specimens shall be referred to as Zlorps!"
            },
            new()
            {
                characterName = char2Name,
                dialogue = $"And now, {char1Name}, it's time to introduce the first wave of pathogens."
            },
            new()
            {
                characterName = char1Name, dialogue = "R-Right, introducing viral samples to the test enviroment..."
            }
        };

        // Attack tutorial
        _dialogueSequences["attack_tutorial"] = new List<DialogueEntry>
        {
            new()
            {
                characterName = " ",
                dialogue = "(Click and hold to prepare your attack. Use your mouse to aim before releasing it!)"
            }
        };

        // First attack reaction
        _dialogueSequences["first_attack"] = new List<DialogueEntry>
        {
            new DialogueEntry { characterName = char1Name, dialogue = "Fascinating... it appears the subject makes use of its own cytoplasm to create projectiles!" },
            new DialogueEntry { characterName = char1Name, dialogue = "It seems to harden considerably upon contact with the atmosphere." },
            new DialogueEntry { characterName = char2Name, dialogue = "The problem is that its regenerative abilities aren't great enough to keep up the pace we need." },
            new DialogueEntry { characterName = char2Name, dialogue = "Once it starts to shrink, prepare the first injection." },
            new DialogueEntry { characterName = char1Name, dialogue = "Got it." }
        };
    }

    // Public method to start a dialogue sequence by key
    public void StartDialogueSequence(string sequenceKey)
    {
        if (_dialogueSequences.TryGetValue(sequenceKey, out List<DialogueEntry> sequence))
        {
            _currentSequence = sequence;
            _currentDialogueIndex = -1;
            ShowNextDialogue();
        }
        else
        {
            Debug.LogWarning($"Dialogue sequence '{sequenceKey}' not found!");
        }
    }

    public void ShowNextDialogue()
    {
        _currentDialogueIndex++;

        if (_currentDialogueIndex >= _currentSequence.Count)
        {
            this.gameObject.SetActive(false);
            return;
        }

        ShowDialogue(_currentSequence[_currentDialogueIndex]);
    }

    public void ShowDialogueEntry(int index)
    {
        if (_currentSequence != null && index >= 0 && index < _currentSequence.Count)
        {
            _currentDialogueIndex = index;
            ShowDialogue(_currentSequence[index]);
        }
    }

    private void ShowDialogue(DialogueEntry entry)
    {
        nameText.text = entry.characterName;

        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
        }

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
        onDialogueComplete?.Invoke();
    }

    public void SkipTyping()
    {
        if (isTyping)
        {
            StopCoroutine(typingCoroutine);
            dialogueText.text = _currentSequence[_currentDialogueIndex].dialogue;
            isTyping = false;
            onDialogueComplete?.Invoke();
        }
    }
}