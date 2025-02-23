using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

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
                    "If you don't mind, could you tell me where-" // it would be nice if this line got cut off extra fast, rather than playing at normal speed (if easy to do)
            },
            new DialogueEntry
            {
                characterName = char2Name,
                dialogue = $"Great, then let's begin at once! TODO:Name Incorporated is counting on you, {char1Name}." //TODO name corp
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

            // Second tutorial text, stays on screen when enemies first spawn until the player attacks once
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
                dialogue = "Fascinating... it appears the subject is making use of its own cytoplasm to create projectiles! It seems to harden upon contact with the atmosphere!"
            },
            new DialogueEntry
            {
                characterName = char2Name,
                dialogue = "Yes, we've seen this before. The problem is that its regenerative abilities aren't great enough to keep up the pace we need."
            },
            new DialogueEntry
            {
                characterName = char2Name,
                dialogue = "Once it starts to shrink, prepare the first injection."
            },
            new DialogueEntry
            {
                characterName = char1Name,
                dialogue = "Got it."
            },
            // End of event dialogue

            // Collection of events: Dialogue for the start of each phase
            // Dialogue for the virus phase is already included in the intro and respawn dialogue, so we only need this for the bacteria and nanobot phases

            // Start of event dialogue: Start of bacteria phase (Triggered when x amount of viruses have been defeated?)
            new DialogueEntry
            {
                characterName = char1Name,
                dialogue = "Subject has successfully neutralized the target number of viral pathogens."
            },
            new DialogueEntry
            {
                characterName = char2Name,
                dialogue = $"Excellent progress, {char1Name}! With this, our Zlorps are practically gauranteed to be raking in the profits already!"
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
            },
            // Bacteria start spawning now
            // End of event dialogue

            // Start of event dialogue: Start of nanobot phase (Triggered when x number of bacteria are defeated?)
            new DialogueEntry
            {
                characterName = char1Name,
                dialogue = "Doctor, the subject has successfully neutralized the target numbers of all biological specimens!"
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
            // Nanobots start spawning now
            // End of event dialogue
            
            // End of collection of events


            // Start of event dialogue: Health injection
            new DialogueEntry
            {
                characterName = char1Name,
                dialogue = "Subject mass has reduced considerably. Preparing to administer regenerative solution injection."
            },
            // If the injection is immediately successful:
            new DialogueEntry
            {
                characterName = char1Name,
                dialogue = "Subject is cooperative."
            },
            // If the needle can't reach the player fast enough:
            new DialogueEntry
            {
                characterName = char1Name,
                dialogue = "Doctor, the subject is exhibiting clear signs of distress. I think we should pursue an alternative delivery method for the solution."
            },
            new DialogueEntry
            {
                characterName = char2Name,
                dialogue = $"{char1Name}, do you have any idea how much a setback like that would cost us?! Just zap it if it won't stay still!"
            },
            new DialogueEntry
            {
                characterName = char1Name,
                dialogue = "Understood..."
            },
            // If the injection still isn't successful fast enough:
            new DialogueEntry
            {
                characterName = char1Name,
                dialogue = "Subject is remaining uncooperative. Administering electric shock."
            },
            // Player takes damage after this until they either get the health injection or die
            // End of event dialogue
            
            // Event dialogue: Health injection is successful
            new DialogueEntry
            {
                characterName = char1Name,
                dialogue = "Regenerative solution successfully administered. Subject mass is increasing."
            },
            new DialogueEntry
            {
                characterName = char2Name,
                dialogue = $"Right along with the mass of our future profits. Great job, {char1Name}!"
            },
            // End of event dialogue

            // Start of event dialogue: First time player dies during the virus phase (Phase 1 of 3, when only viruses are spawning)
            new DialogueEntry
            {
                characterName = char1Name,
                dialogue = "Oh, it failed already..."
            },
            new DialogueEntry
            {
                characterName = char1Name,
                dialogue = $"Subject Z-{life_number}, failure to evade or neutralize pathogen {enemy_type}." // TODO add actual variables here. Hopefully this can be in a format with zeroes if its less than 3 digits, like 001, 002, 023, etc.
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
            },
            // Show scientific report scorecard.
            // End of event dialogue

            // Start of event dialogue: First time player dies during the bacteria phase (Phase 2 of 3, when both viruses and bacteria are spawning)
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
                dialogue = $"Subject Z-{life_number}, failure to evade or neutralize pathogen {enemy_type}." // TODO add actual variables here. Hopefully the number can be in a format with zeroes if its less than 3 digits, like 001, 002, 023, etc.
            },
            new DialogueEntry
            {
                characterName = char1Name,
                dialogue = $"Preparing subject Z-{life_number+1} for testing." // TODO add the proper variable and math here (not sure if what I did works in C#)
            },
            // Show scientific report scorecard.
            // End of event dialogue

            // Start of event dialogue: First time player dies during the nanobot phase (Final of 3 Phases, when all three enemy types are spawning)
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
            },
            // Show scientific report scorecard. Perhaps with a personal note from char1?
            // End of event dialogue

            // Start of event dialogue: New player life (after the intro has already happened)
            new DialogueEntry
            {
                characterName = char1Name,
                dialogue = "Restarting experiment."
            },
            // As player spawns?
            new DialogueEntry
            {
                characterName = char1Name,
                dialogue = $"Introducing test subject Z-{life_number}."
            },
            // As viruses spawn?
            new DialogueEntry
            {
                characterName = char1Name,
                dialogue = "Re-introducing viral pathogens."
            },
            // End of event dialogue

            // Start of event dialogue: Player defeats a virus for the first time
            // This will probably happen very soon after or even during the dialogue reacting to Zlorp's attack method, so this dialogue likely needs to be queued up somehow
            new DialogueEntry
            {
                characterName = char1Name,
                dialogue = "Subject has neutralized a specimen of the Covid-19 virus."
            },
            new DialogueEntry
            {
                characterName = char2Name,
                dialogue = "As expected of a Zlorp! It's too bad we didn't have any back in 2020, just imagine the profits..." // Salivating
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
            },
            // End of event dialogue

            // Start of event dialogue: Player defeats a bacteria for the first time
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
            },
            // End of event dialogue

            // Start of event dialogue: Player defeats a nanobot for the first time
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
            },
            // Maybe spawns increase after this? They don't have to though
            // End of event dialogue


        };
    }
}