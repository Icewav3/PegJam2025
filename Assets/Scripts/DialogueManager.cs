using UnityEngine;
using TMPro;
using System.Collections;
using UnityEngine.Events;

public class DialogueManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private float typingSpeed = 0.05f;
    
    public UnityEvent onDialogueComplete;
    
    private int _currentDialogueIndex = -1;
    private bool isTyping = false;
    private Coroutine typingCoroutine;

    // Dialogue entry struct to hold both name and dialogue
    [System.Serializable]
    public struct DialogueEntry
    {
        public string characterName;
        public string dialogue;
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
    private DialogueEntry[] dialogueEntries = new DialogueEntry[]
    {
        new DialogueEntry { 
            characterName = "John",
            dialogue = "Hello there! This is entry 1."
        },
        new DialogueEntry {
            characterName = "Sarah",
            dialogue = "Welcome to our game! This is entry 2."
        },
        new DialogueEntry {
            characterName = "John",
            dialogue = "Thank you for playing! This is entry 3."
        },
        // Add more entries as needed
    };
}