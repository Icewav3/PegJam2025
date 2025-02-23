using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    [SerializeField] private DamageReceiver _dr;

    private bool hasFired = false;
    private bool hasMoved = false;

    [SerializeField] private PlayerFire _playerFire;

    [SerializeField] private PlayerMove _playerMove;

    private void OnEnable()
    {
        _dr.OnDeath += HandleDeath;
        _playerFire.OnFire += HandleFire;
        _playerMove.OnMove += HandleMove;
    }

    private void OnDisable()
    {
        _dr.OnDeath -= HandleDeath;
        _playerFire.OnFire -= HandleFire;
        _playerMove.OnMove -= HandleMove;
    }

    private void HandleDeath(DamageReceiver dr, DamageEvent dmgEvent)
    {
        SceneGod.SInstance.DialogueManager.StartDialogueSequence("death_stage1");
    }

    private void HandleFire(PlayerFire playerFire)
    {
        if (hasFired) return;
        hasFired = true;
        SceneGod.SInstance.DialogueManager.StartDialogueSequence("first_attack");
    }

    private void HandleMove(PlayerMove playerMove)
    {
        if (hasMoved) return;
        hasMoved = true;
        SceneGod.SInstance.DialogueManager.ShowNextDialogue();
    }
}