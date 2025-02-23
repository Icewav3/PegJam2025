using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandlePlayerDeath : MonoBehaviour
{
    [SerializeField]
    private DamageReceiver _dr;

    [SerializeField]
    private SpriteRenderer _spr;

    [SerializeField]
    private Rigidbody2D _rb;

    [SerializeField]
    private PlayerInputHandler _playerInputHandler;

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
        _spr.enabled = false;
        _playerInputHandler.enabled = false;
        _rb.constraints = RigidbodyConstraints2D.FreezePosition;

        SceneGod.SInstance.DialogueManager.OnSequenceComplete += HandleDialogueComplete;
    }

    private void HandleDialogueComplete(DialogueManager dm)
    {
        SceneGod.SInstance.DialogueManager.OnSequenceComplete -= HandleDialogueComplete;

        StartCoroutine(DeathRoutine());
    }

    private IEnumerator DeathRoutine()
    {
        yield return new WaitForSeconds(1.5f);

        SceneGod.SInstance.IncrementDeaths();
    }
}
