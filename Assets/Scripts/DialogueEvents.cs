using UnityEngine;

public class DialogueEvents : MonoBehaviour
{
    private DialogueManager dialogueManager;
    private bool hasMovedDialoguePlayed = false;
    private bool hasAttackedDialoguePlayed = false;
    private bool hasFirstDeathDialoguePlayed = false;

    // Dialogue sequence indices (these would be replaced when we restructure DialogueManager)
    private const int FIRST_MOVE_DIALOGUE_START = 7;  // Index where movement dialogue starts
    private const int FIRST_ATTACK_DIALOGUE_START = 15; // Index where attack dialogue starts
    private const int FIRST_DEATH_DIALOGUE_START = 20; // You'll need to add death dialogue to the array

    private void Start()
    {
        dialogueManager = SceneGod.SInstance.dialogue.GetComponent<DialogueManager>();
    }

    // Call this when player first moves
    public void OnFirstMove()
    {
        if (!hasMovedDialoguePlayed)
        {
            hasMovedDialoguePlayed = true;
            dialogueManager.ShowDialogueEntry(FIRST_MOVE_DIALOGUE_START);
        }
    }

    // Call this when player first attacks
    public void OnFirstAttack()
    {
        if (!hasAttackedDialoguePlayed)
        {
            hasAttackedDialoguePlayed = true;
            dialogueManager.ShowDialogueEntry(FIRST_ATTACK_DIALOGUE_START);
        }
    }

    // Call this when player first dies
    public void OnFirstDeath()
    {
        if (!hasFirstDeathDialoguePlayed)
        {
            hasFirstDeathDialoguePlayed = true;
            dialogueManager.ShowDialogueEntry(FIRST_DEATH_DIALOGUE_START);
        }
    }

    // Call this to reset dialogue flags (e.g., when starting a new game)
    public void ResetDialogueFlags()
    {
        hasMovedDialoguePlayed = false;
        hasAttackedDialoguePlayed = false;
        hasFirstDeathDialoguePlayed = false;
    }
}