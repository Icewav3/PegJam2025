using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayCharacterPortrait : MonoBehaviour
{
    [SerializeField]
    private DialogueManager _dm;

    [SerializeField]
    private Image _portrait;

    [SerializeField]
    private Sprite _char1Sprite;
    [SerializeField]
    private Sprite _char2Sprite;

    private void Update()
    {
        if (_dm.CurrentEntry == null) return;

        DialogueEntry entry = (DialogueEntry)_dm.CurrentEntry;
        if(entry.characterName == _dm.Char1Name)
        {
            _portrait.sprite = _char1Sprite;
        }
        else
        {
            _portrait.sprite = _char2Sprite;
        }
    }
}
