using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{
    [SerializeField]
    private DamageReceiver _dr;

    private bool hasFired = false;

    [SerializeField]
    private PlayerFire _playerFire;

    private void OnEnable()
    {
        _dr.OnDeath += HandleDeath;
        _playerFire.OnFire += HandleFire;
    }

    private void OnDisable()
    {
        _dr.OnDeath -= HandleDeath;
        _playerFire.OnFire -= HandleFire;
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
}