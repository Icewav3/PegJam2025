using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstPlayerDeathDialogue : MonoBehaviour
{
    [SerializeField]
    private DamageReceiver _dr;

    private void OnEnable()
    {
        _dr.OnDeath += HandleDeath;
    }

    private void OnDisable()
    {
        _dr.OnDeath -= HandleDeath;
    }

    private void HandleDeath(DamageReceiver dr, DamageEvent dmgEvent)
    {
        SceneGod.SInstance.DialogueManager.StartDialogueSequence("intro");
    }
}