using System;
using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[System.Serializable]
public struct DialogueEntry
{
    public string characterName;
    public string dialogue;
}

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private float typingSpeed = 0.05f;

    [Header("Character configs")] [SerializeField]
    private Color char1Color;

    [SerializeField] private Color char2Color;
    private string char1Name = "Dr. Cecil";
    public string Char1Name => char1Name;
    private string char2Name = "Dr. Zorbist";
    public string Char2Name => char2Name;
    [SerializeField] private Sprite char1Portrait;
    [SerializeField] private Sprite char2Portrait;

    [SerializeField] private PlayerInput _playerInput;

    private Dictionary<string, List<DialogueEntry>> _dialogueSequences;
    private List<DialogueEntry> _currentSequence;

    public DialogueEntry? CurrentEntry
    {
        get
        {
            if (_currentSequence == null) return null;
            if(_currentDialogueIndex >= _currentSequence.Count) return null;
            return _currentSequence[_currentDialogueIndex];
        }
    }

    private int _currentDialogueIndex = -1;
    private bool isTyping = false;
    private Coroutine typingCoroutine;

    private object life_number;
    public event Action onDialogueComplete;

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
        if (isTyping)
            SkipTyping();
        else
            ShowNextDialogue();
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
                dialogue = $"Great, then let's begin at once! ZorBio Incorporated is counting on you, {char1Name}."
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
                characterName = char2Name, dialogue = "From now on, Z-type specimens shall be referred to as Zlorps!"
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
            new()
            {
                characterName = char1Name,
                dialogue = "Fascinating... it appears the subject makes use of its own cytoplasm to create projectiles!"
            },
            new()
            {
                characterName = char1Name,
                dialogue = "It seems to harden considerably upon contact with the atmosphere."
            },
            new()
            {
                characterName = char2Name,
                dialogue =
                    "The problem is that its regenerative abilities aren't great enough to keep up the pace we need."
            },
            new()
            {
                characterName = char2Name,
                dialogue = "Once it starts to shrink, prepare to release more of the regenerative solution."
            },
            new DialogueEntry { characterName = char1Name, dialogue = "Got it." }
        };

        // Start of bacteria stage reaction (when bacteria start spawning, triggered by certain score?)
        _dialogueSequences["bacteria_spawn"] = new List<DialogueEntry>
        {
            new DialogueEntry
            {
                characterName = char1Name,
                dialogue = "Subject has successfully neutralized the target number of viral pathogens."
            },
            new DialogueEntry
            {
                characterName = char2Name,
                dialogue =
                    $"Excellent progress, {char1Name}! With this, our Zlorps are practically gauranteed to be raking in the profits already!"
            },
            new DialogueEntry
            {
                characterName = char2Name,
                dialogue = "Now, let's see how the little guy fares against bacteria."
            },
            new DialogueEntry
            {
                characterName = char1Name,
                dialogue = "Understood. Now introducing bacterial samples to the test environment."
            }
            // Bacteria start spawning after this dialogue, if possible
        };

        // Start of nanobot stage reaction (when nanobots start spawning, triggered by certain score?)
        _dialogueSequences["nanobots_spawn"] = new List<DialogueEntry>
        {
            new DialogueEntry
            {
                characterName = char1Name,
                dialogue =
                    "Doctor, the subject has successfully neutralized the target numbers of all biological specimens!"
            },
            new DialogueEntry
            {
                characterName = char2Name,
                dialogue = "Great! Now we can push it even further with those deadly Chinese nanobots!"
            },
            new DialogueEntry
            {
                characterName = char1Name,
                dialogue = "O-Oh, right..."
            },
            new DialogueEntry
            {
                characterName = char1Name,
                dialogue = "I, umm... I'm introducing them now..."
            },
            // Nanobots start spawning after this dialogue, if possible
        };

        // First (Virus) stage death
        // First stage death (Virus stage) reaction
        _dialogueSequences["death_stage1"] = new List<DialogueEntry>
        {
            new DialogueEntry
            {
                characterName = char1Name,
                dialogue = "Oh, it failed already..."
            },
            new DialogueEntry
            {
                characterName = char1Name,
                dialogue =
                    $"Subject Z-{life_number}, failure to evade or neutralize a pathogen." // TODO add actual variables here. Hopefully this can be in a format with zeroes if its less than 3 digits, like 001, 002, 023, etc.
            },
            new DialogueEntry
            {
                characterName = char1Name,
                dialogue = "Shall I prepare the next test subject?"
            },
            new DialogueEntry
            {
                characterName = char1Name,
                dialogue = "Yes, let's keep this moving."
            }
        };

        // Second (Bacteria) stage death
        // Second stage death (Bacteria stage)
        _dialogueSequences["death_stage2"] = new List<DialogueEntry>
        {
            new DialogueEntry
            {
                characterName = char1Name,
                dialogue = "Damn, it was doing so well!"
            },
            new DialogueEntry
            {
                characterName = char2Name,
                dialogue = "Forget about it. We have plenty more subjects to get through."
            },
            new DialogueEntry
            {
                characterName = char1Name,
                dialogue = "*Sigh*. Well, I guess you're right."
            },
            new DialogueEntry
            {
                characterName = char1Name,
                dialogue =
                    $"Subject Z-{life_number}, failure to evade or neutralize a pathogen." // TODO add actual variables here. Hopefully the number can be in a format with zeroes if its less than 3 digits, like 001, 002, 023, etc.
            },
            new DialogueEntry
            {
                characterName = char1Name,
                dialogue =
                    $"Preparing subject Z-{life_number} for testing." // TODO add the proper variable and math here (not sure if what I did works in C#)
            }
        };

        // Third (Nanobot) stage death
        // Third stage death (Nanobot stage)
        _dialogueSequences["death_stage3"] = new List<DialogueEntry>
        {
            new DialogueEntry
            {
                characterName = char1Name,
                dialogue = "Zlorp, no!"
            },
            new DialogueEntry
            {
                characterName = char1Name,
                dialogue = "How could you let this happen?! You were doing so well!"
            },
            new DialogueEntry
            {
                characterName = char2Name,
                dialogue = $"Hey, {char1Name}, let's keep it moving!"
            },
            new DialogueEntry
            {
                characterName = char2Name,
                dialogue = "We still have hundreds more Zlorps to get through today."
            },
            new DialogueEntry
            {
                characterName = char1Name,
                dialogue = "I- I'm not so sure I can keep doing this..."
            },
            new DialogueEntry
            {
                characterName = char2Name,
                dialogue = "You damn well better keep doing it! Remember that contract you signed?"
            },
            new DialogueEntry
            {
                characterName = char1Name,
                dialogue = "R-Right... I'm sorry doctor."
            }
        };

        // Player respawn 
        _dialogueSequences["respawn"] = new List<DialogueEntry>
        {
            new DialogueEntry
            {
                characterName = char1Name,
                dialogue = "Restarting experiment."
            },
            // As player spawns?
            new DialogueEntry
            {
                characterName = char1Name,
                dialogue = $"Introducing test subject Z-{life_number}." // TODO verify this variable
            },
            // As viruses spawn?
            new DialogueEntry
            {
                characterName = char1Name,
                dialogue = "Re-introducing viral pathogens."
            }
        };

        // Player defeats a virus for the first time
        // This will probably happen very soon after or even during the dialogue reacting to Zlorp's attack method, so this dialogue likely needs to be queued up somehow. This should go for any dialogue that could play at a random time, they could easily overlap
        _dialogueSequences["virus_defeated"] = new List<DialogueEntry>
        {
            new DialogueEntry
            {
                characterName = char1Name,
                dialogue = "Subject has neutralized a specimen of the Covid-19 virus."
            },
            new DialogueEntry
            {
                characterName = char2Name,
                dialogue =
                    "As expected of a Zlorp! It's too bad we didn't have any back in 2020, just imagine the profits..." // Salivating
            },
            new DialogueEntry
            {
                characterName = char1Name,
                dialogue = "Are you seriously calling them Zlorps?"
            },
            new DialogueEntry
            {
                characterName = char2Name,
                dialogue = "What, you don't like it?"
            },
            new DialogueEntry
            {
                characterName = char1Name,
                dialogue = "..."
            }
        };

        // PLayer defeats a bacteria for the first time
        _dialogueSequences["bacteria_defeated"] = new List<DialogueEntry>
        {
            new DialogueEntry
            {
                characterName = char1Name,
                dialogue = "Subject has neutralized a bacterial specimen."
            },
            new DialogueEntry
            {
                characterName = char2Name,
                dialogue = "Hmm... I wonder if there would be a market for Zlorps in sewage treatment facilities..."
            },
            new DialogueEntry
            {
                characterName = char1Name,
                dialogue = "I... I'm not really sure that would be ethical..."
            },
            new DialogueEntry
            {
                characterName = char2Name,
                dialogue = $"I'm sorry, what was that {char1Name}?"
            },
            new DialogueEntry
            {
                characterName = char1Name,
                dialogue = "...Nevermind."
            }
        };

        // Player defeats a nanobot for the first time
        _dialogueSequences["nanobot_defeated"] = new List<DialogueEntry>
        {
            new DialogueEntry
            {
                characterName = char1Name,
                dialogue = "T-The subject destroyed one of the nanobots!"
            },
            new DialogueEntry
            {
                characterName = char2Name,
                dialogue = "WHat?! That's fantastic!"
            },
            new DialogueEntry
            {
                characterName = char2Name,
                dialogue = "Keep adding more! Let's see exactly what this Glorp is capable of!"
            },
            new DialogueEntry
            {
                characterName = char1Name,
                dialogue = "More?! But doctor, it's-"
            },
            new DialogueEntry
            {
                characterName = char2Name,
                dialogue = $"Just add the damn nanobots, {char1Name}! Need I remind you of your contract again?!"
            },
            new DialogueEntry
            {
                characterName = char1Name,
                dialogue = "N-No sir... I'll add them."
            }
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
            if (SceneGod.SInstance._currentState == SceneGod.GameState.Intro)
                SceneGod.SInstance.EnterGameState();
            else
                SceneGod.SInstance.dialogue.SetActive(false);
        }
        else
        {
            SceneGod.SInstance.dialogue.SetActive(true);
            ShowDialogue(_currentSequence[_currentDialogueIndex]);
        }
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