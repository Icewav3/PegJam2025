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

        _portrait.enabled = true;
        DialogueEntry entry = (DialogueEntry)_dm.CurrentEntry;
        Debug.Log(entry.dialogue);
        Debug.Log(entry.characterName);
        if (entry.characterName == _dm.Char1Name)
        {
            _portrait.sprite = _char1Sprite;
        }
        else if(entry.characterName == _dm.Char2Name) 
        {
            _portrait.sprite = _char2Sprite;
        }
        else
        {
            _portrait.enabled = false;
        }
    }
}
